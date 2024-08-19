using Microsoft.AspNetCore.Mvc;
using dhub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
//using dhub.Data;
using dhub.Models;

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

            // GET: question/index
            [HttpGet("index")]
            public async Task<IActionResult> Index()
            {
                var questions = await _context.Questions.Include(q => q.Survey).ToListAsync();
                return View(questions);
            }

            // GET: question/details/5
            [HttpGet("details/{id}")]
            public async Task<IActionResult> Details(Guid id)
            {
                var question = await _context.Questions.Include(q => q.Survey)
                    .FirstOrDefaultAsync(q => q.QuestionID == id);
                if (question == null)
                {
                    return NotFound();
                }
                return View(question);
            }

            // GET: question/create
            [HttpGet("create")]
            public IActionResult Create()
            {
                return View();
            }

            // POST: question/create
            [HttpPost("create")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("QuestionID,SurveyID,QuestionText,QuestionType,IsRequired,IsHidden,Options")] Question question)
            {
                if (ModelState.IsValid)
                {
                    question.QuestionID = Guid.NewGuid(); // Ensure the ID is set
                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }

            // GET: question/edit/5
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

            // POST: question/edit/5
            [HttpPost("edit/{id}")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id, [Bind("QuestionID,SurveyID,QuestionText,QuestionType,IsRequired,IsHidden,Options")] Question question)
            {
                if (id != question.QuestionID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
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
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }

            // GET: question/delete/5
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

            // POST: question/delete/5
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




