using Microsoft.AspNetCore.Mvc;
using dhub.Models;
using dhub.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dhub.Controllers
{

    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var questions = await _context.Questions.Include(q => q.Survey).ToListAsync();
            return View(questions);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var question = await _context.Questions
                .Include(q => q.Survey)
                .FirstOrDefaultAsync(q => q.QuestionID == id);

            if (question == null)
            {
                return NotFound();
            }

            var responses = await _context.QuestionResponses
                .Where(r => r.QuestionID == id)
                .Include(r => r.SelectedOptions)
                .ToListAsync();

            var model = (Question: question, Responses: responses);

            return View(model);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.SurveyID == question.SurveyID);

                if (survey == null)
                {
                    return BadRequest(new { error = "Invalid SurveyID. The survey does not exist." });
                }

                question.Survey = survey;

                question.QuestionID = Guid.NewGuid();

                _context.Add(question);
                await _context.SaveChangesAsync();

                return Ok(question);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Question question)
        {
            if (id != question.QuestionID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.QuestionID))
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
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionID == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }
    }
}
