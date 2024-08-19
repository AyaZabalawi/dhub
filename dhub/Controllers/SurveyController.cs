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
        [Route("survey")]
        public class SurveyController : Controller
        {
            private readonly AppDbContext _context;

            public SurveyController(AppDbContext context)
            {
                _context = context;
            }

            // GET: survey/index
            [HttpGet("index")]
            public async Task<IActionResult> Index()
            {
                var surveys = await _context.Surveys.Include(s => s.Questions).ToListAsync();
                return View(surveys);
            }

            // GET: survey/details/5
            [HttpGet("details/{id}")]
            public async Task<IActionResult> Details(Guid id)
            {
                var survey = await _context.Surveys.Include(s => s.Questions)
                    .FirstOrDefaultAsync(s => s.SurveyID == id);
                if (survey == null)
                {
                    return NotFound();
                }
                return View(survey);
            }

            // GET: survey/create
            [HttpGet("create")]
            public IActionResult Create()
            {
                return View();
            }

        // POST: /survey/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                foreach (var question in survey.Questions)
                {
                    if (question.Options != null)
                    {
                        question.Options = question.Options.ToArray();
                    }
                }

                survey.SurveyID = Guid.NewGuid(); 
                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(survey);
        }

        // GET: survey/edit/5
        [HttpGet("edit/{id}")]
            public async Task<IActionResult> Edit(Guid id)
            {
                var survey = await _context.Surveys.FindAsync(id);
                if (survey == null)
                {
                    return NotFound();
                }
                return View(survey);
            }

            // POST: survey/edit/5
            [HttpPost("edit/{id}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id, [Bind("SurveyID,Name")] Survey survey)
            {
                if (id != survey.SurveyID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
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
                    return RedirectToAction(nameof(Index));
                }
                return View(survey);
            }

            // GET: survey/delete/5
            [HttpGet("delete/{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.SurveyID == id);
                if (survey == null)
                {
                    return NotFound();
                }
                return View(survey);
            }

            // POST: survey/delete/5
            [HttpPost("delete/{id}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(Guid id)
            {
                var survey = await _context.Surveys.FindAsync(id);
                if (survey != null)
                {
                    _context.Surveys.Remove(survey);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            [HttpPost("Survey/Submit")]
            public async Task<IActionResult> Submit(SurveyResponseViewModel viewModel)
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var response = new SurveyResponse
                {
                    SurveyID = viewModel.SurveyID,
                    SubmissionDate = DateTime.Now,
                    ResponsesJson = JsonConvert.SerializeObject(viewModel.Responses)
                };

                _context.SurveyResponses.Add(response);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
        private bool SurveyExists(Guid id)
            {
                return _context.Surveys.Any(e => e.SurveyID == id);
            }
        }
    }



