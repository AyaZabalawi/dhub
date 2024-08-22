using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using dhub.Models;
using dhub.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dhub.Controllers
{
    [Route("SurveyResponse")]
    public class SurveyResponseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SurveyResponseController> _logger;

        public SurveyResponseController(AppDbContext context, ILogger<SurveyResponseController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("/Index")]
        public async Task<IActionResult> Index(Guid id)
        {
            var responses = await _context.SurveyResponses
                .Include(sr => sr.Survey) 
                .Where(x => x.SurveyID == id)
                .ToListAsync();

            return View(responses);
        }

        [HttpGet("Create/{surveyId}")]
        public async Task<IActionResult> Create(Guid surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.SurveyID == surveyId);

            if (survey == null)
            {
                return NotFound();
            }

            var model = new SurveyResponseViewModel
            {
                SurveyID = surveyId,
                Responses = survey.Questions.Select(q => new SurveyResponseViewModel.QuestionResponse
                {
                    QuestionID = q.QuestionID,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options
                }).ToList()
            };

            return View(model);
        }


        [HttpPost("Create/{surveyId}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create(Guid surveyId, [FromBody] SurveyResponseViewModel model)
        {
            if (!ModelState.IsValid || surveyId != model.SurveyID)
            {
                return BadRequest(ModelState);
            }

            var userName = User.Identity.Name;

            var surveyResponse = new SurveyResponse
            {
                SurveyID = model.SurveyID,
                SubmissionDate = DateTime.UtcNow,
                SubmittedBy = userName 
            };

            _context.SurveyResponses.Add(surveyResponse);
            await _context.SaveChangesAsync();

            var questionResponses = model.Responses.Select(r => new QuestionResponse
            {
                QuestionID = r.QuestionID,
                QuestionText = r.QuestionText,
                SurveyResponseID = surveyResponse.ResponseID,
                AnswerText = string.Join(", ", r.Response)
            }).ToList();

            _context.QuestionResponses.AddRange(questionResponses);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Survey submitted successfully!" });
        }


        [HttpGet("/Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _context.SurveyResponses
                .Include(sr => sr.QuestionResponses)
                    .ThenInclude(qr => qr.Question)
                .Include(sr => sr.Survey)
                .FirstOrDefaultAsync(sr => sr.ResponseID == id);

            if (response == null)
            {
                return NotFound();
            }

            var viewModel = new SurveyResponseDetailsViewModel
            {
                ResponseID = response.ResponseID,
                SurveyID = response.SurveyID,
                SubmissionDate = response.SubmissionDate,
                SurveyName = response.Survey.Name, 
                QuestionResponses = response.QuestionResponses.Select(qr => new QuestionResponseDetails
                {
                    QuestionResponseID = qr.QuestionResponseID,
                    QuestionID = qr.QuestionID,
                    QuestionText = qr.Question.QuestionText,
                    AnswerText = qr.AnswerText
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost("/Submit")]
        public async Task<IActionResult> Submit([FromBody] SurveyResponseViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("SurveyResponseViewModel cannot be null.");
            }

            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("User.Identity.Name is null or empty.");
                return BadRequest("User is not authenticated or the user name is not available.");
            }

            var surveyResponse = new SurveyResponse
            {
                ResponseID = Guid.NewGuid(),
                SurveyID = viewModel.SurveyID,
                SubmissionDate = DateTime.UtcNow,
                SubmittedBy = userName 
            };

            foreach (var response in viewModel.Responses)
            {
                var answerText = response.Response != null ? string.Join(", ", response.Response) : string.Empty;

                var questionResponse = new QuestionResponse
                {
                    QuestionResponseID = Guid.NewGuid(),
                    SurveyResponseID = surveyResponse.ResponseID,
                    QuestionID = response.QuestionID,
                    QuestionText = response.QuestionText,
                    AnswerText = answerText
                };

                _context.QuestionResponses.Add(questionResponse);
            }

            _context.SurveyResponses.Add(surveyResponse);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the survey response.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return Ok(new { message = "Survey submitted successfully!" });
        }




        [HttpPost("/Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _context.SurveyResponses
                .Include(sr => sr.QuestionResponses)
                .FirstOrDefaultAsync(sr => sr.ResponseID == id);

            if (response == null)
            {
                return NotFound();
            }

            _context.QuestionResponses.RemoveRange(response.QuestionResponses);

            _context.SurveyResponses.Remove(response);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = response.SurveyID });
        }
    }
}