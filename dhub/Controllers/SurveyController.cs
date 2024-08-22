using dhub.Data;
using dhub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using dhub.Users;
using Microsoft.AspNetCore.Authorization;
using dhub.Models.ViewModel;

namespace dhub.Controllers
{
    [Route("survey")]
    [Authorize(Policy ="ManageSurveysPolicy")] 
    public class SurveyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SurveyController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public SurveyController(AppDbContext context, ILogger<SurveyController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("The user ID was invalid.");
                return BadRequest("Invalid user ID.");
            }

            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            _logger.LogInformation("User roles: {Roles}", string.Join(", ", roles));

            var surveys = await _context.Surveys
                .Include(s => s.Questions)
                .Where(s => s.OwnerId == userId) 
                .ToListAsync();

            return View(surveys);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("The user ID was invalid.");
                return BadRequest("Invalid user ID.");
            }

            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyID == id && s.OwnerId == userId); 

            if (survey == null)
            {
                _logger.LogWarning("The user ID was invalid.");
                return NotFound();
            }

            return View(survey);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create(CreateSurveyVM survey)
        {
            if (survey is null)
            {
                _logger.LogWarning("Survey is null.");
                return BadRequest("Survey cannot be null.");
            }
                
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return Unauthorized(); }
            else
            {
                var createSurvey = new Survey()
                {
                    SurveyID  = Guid.NewGuid(),
                    Name = survey.Name,
                    Questions = survey.Questions,
                    FullName = user.FullName,
                    OwnerId = user.Id,
                };

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model State for survey creation is invalid.");
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    _logger.LogWarning("Survey creation failed with errors: {Errors}", string.Join(", ", errors));
                    return BadRequest(new { Errors = errors });
                }

                
                _context.Surveys.Add(createSurvey);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Survey successfully created.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (!Guid.TryParse(currentUserId, out var userId))
            {
                _logger.LogWarning("The user ID was invalid.");
                return BadRequest("Invalid user ID."); 
            }

            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyID == id && s.OwnerId == userId); 

            if (survey == null)
            {
                _logger.LogInformation("The user is forbidden to perform an edit action.");
                return Forbid();
            }

            return View(survey);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Survey survey)
        {
            if (id != survey.SurveyID)
            {
                _logger.LogWarning("The user ID was invalid.");
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var existingSurvey = _context.Surveys.Where(s => s.SurveyID == id && s.OwnerId.ToString() == userId); 
            if (existingSurvey == null)
            {
                _logger.LogInformation("The user is forbidden to perform an edit action.");
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model State for survey editing is invalid.");
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Edit Exception => {ex.Message}", ex);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("User ID is invalid");
                return BadRequest("Invalid user ID.");
            }

            var survey = await _context.Surveys
                .FirstOrDefaultAsync(s => s.SurveyID == id && s.OwnerId == userId); 

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost("deleteconfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userIdString = _userManager.GetUserId(User);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("User ID is invalid");
                return BadRequest("Invalid user ID.");
            }

            var survey = await _context.Surveys
                .FirstOrDefaultAsync(s => s.SurveyID == id && s.OwnerId == userId); 

            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Survey successfully deleted");
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyExists(Guid id)
        {
            return _context.Surveys.Any(e => e.SurveyID == id);
        }
    }
}
