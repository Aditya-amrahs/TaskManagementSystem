using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskManagementContext _context;

        public HomeController(ILogger<HomeController> logger, TaskManagementContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            var tasks = _context.Tasks
                                .OrderBy(t => t.Priority) 
                                .ToList();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskManagementSystem.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"New Task:{task.Title} is Created Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.PriorityList = new SelectList(new[]
            {
                new { Value = "High", Text = "High" },
                new { Value = "Medium", Text = "Medium" },
                new { Value = "Low", Text = "Low" }
            }, "Value", "Text", task.Priority);
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskManagementSystem.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            TempData["DeleteMessage"] = $"{task.Title}: Task is Deleted!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
