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

namespace dhub.Controllers
{

    [Route("survey")]
    public class SurveyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SurveyController> _logger;

        public SurveyController(AppDbContext context, ILogger<SurveyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var surveys = await _context.Surveys
                .Include(s => s.Questions)
                .ToListAsync();
                _logger.LogInformation($"Survey Info: {JsonConvert.SerializeObject(surveys)}");
                return View(surveys);
            }
            catch (Exception ex) {
                _logger.LogError($"Error exception:{ex.Message}",ex);
                return NotFound(); }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyID == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpGet("create")]
        public async Task<IActionResult> Create([FromBody] Survey survey)
        {
            if(survey==null)
            {
                return BadRequest("Survey cannot be null.");
            }

            _logger.LogInformation("Received payload: {@Survey}", survey);

            if (survey.Questions == null)
            {
                survey.Questions = new List<Question>();
            }

            foreach (var question in survey.Questions)
            {
                if (!Enum.IsDefined(typeof(QuestionType), question.QuestionType))
                {
                    ModelState.AddModelError("Questions", $"Invalid QuestionType: {question.QuestionType}");
                }

                if (question.Options == null)
                {
                    question.Options = new List<string>();
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }

            survey.SurveyID = Guid.NewGuid();
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = survey.SurveyID }, survey);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyID == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Survey survey)
        {
            if (id != survey.SurveyID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(survey);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyExists(survey.SurveyID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var survey = await _context.Surveys
                .FirstOrDefaultAsync(s => s.SurveyID == id);

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
            var survey = await _context.Surveys.FindAsync(id);

            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool SurveyExists(Guid id)
        {
            return _context.Surveys.Any(e => e.SurveyID == id);
        }
    }
}
