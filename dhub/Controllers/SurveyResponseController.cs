using dhub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
//using dhub.Data;
using dhub.Models;
using Newtonsoft.Json;

namespace dhub.Controllers
{
        [Route("surveyresponse")]
        public class SurveyResponseController : Controller
        {
            private readonly AppDbContext _context;

            public SurveyResponseController(AppDbContext context)
            {
                _context = context;
            }

            // GET: surveyresponse/index
            [HttpGet("index")]
            public async Task<IActionResult> Index()
            {
                var responses = await _context.SurveyResponses.Include(sr => sr.Survey).ToListAsync();
                return View(responses);
            }

            // GET: surveyresponse/details/5
            [HttpGet("details/{id}")]
            public async Task<IActionResult> Details(Guid id)
            {
                var response = await _context.SurveyResponses.Include(sr => sr.Survey)
                    .FirstOrDefaultAsync(sr => sr.ResponseID == id);
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }

            // POST: surveyresponse/create
            [HttpPost("create")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("ResponseID,SurveyID,Answers")] SurveyResponse response)
            {
                if (ModelState.IsValid)
                {
                    response.ResponseID = Guid.NewGuid(); // Ensure the ID is set
                    _context.Add(response);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(response);
            }

            // GET: surveyresponse/survey/5
            [HttpGet("survey/{surveyId}")]
            public async Task<IActionResult> GetResponsesBySurvey(Guid surveyId)
            {
                var responses = await _context.SurveyResponses
                    .Where(sr => sr.SurveyID == surveyId)
                    .ToListAsync();
                return View(responses);
            }
        
        [HttpGet("Survey/Submit/{surveyId}")]
        public async Task<IActionResult> Submit(Guid surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.SurveyID == surveyId);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost("Survey/Submit")]
        public async Task<IActionResult> Submit(SurveyResponseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var serializedResponses = JsonConvert.SerializeObject(viewModel.Responses);

            var response = new SurveyResponse
            {
                SurveyID = viewModel.SurveyID,
                SubmissionDate = DateTime.Now,
                ResponsesJson = serializedResponses
            };

            _context.SurveyResponses.Add(response);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        private bool SurveyResponseExists(Guid id)
            {
                return _context.SurveyResponses.Any(e => e.ResponseID == id);
            }
        }
    }

