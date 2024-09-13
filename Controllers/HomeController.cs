using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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

        public IActionResult Index(string sortOrder, string searchterm, string[] Pfilter, string[] Sfilter)
        {
            //code before sort, filter

            //var tasks = _context.Tasks
            //                    .OrderBy(t => t.Priority) 
            //                    .ToList();

            //viewbag for sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PrioritySort = sortOrder == "priority_asc" ? "priority_desc" : "priority_asc";
            ViewBag.DueDateSort = sortOrder == "duedate_asc" ? "duedate_desc" : "duedate_asc";

            var tasks = from t in _context.Tasks
                        select t;

            switch (sortOrder)
            {
                case "priority_asc":
                    tasks = tasks.OrderBy(t => t.Priority);
                    break;
                case "priority_desc":
                    tasks = tasks.OrderByDescending(t => t.Priority);
                    break;
                case "duedate_asc":
                    tasks = tasks.OrderBy(t => t.DueDate);
                    break;
                case "duedate_desc":
                    tasks = tasks.OrderByDescending(t => t.DueDate);
                    break;
                default:
                    tasks = tasks.OrderBy(t => t.Priority); // Default sorting
                    break;
            }

            //search term logic
            if (!string.IsNullOrEmpty(searchterm))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchterm));
            }
            //ViewData["CurrentFilter"] = searchterm;

            //filter logic by Priority
            if(Pfilter != null && Pfilter.Any())
            {
                tasks = tasks.Where(t => Pfilter.Contains(t.Priority));
            }

            // by status
            if(Sfilter != null && Sfilter.Any())
            {
                bool iscompleted = Sfilter.Contains("Completed");
                tasks = tasks.Where(t => t.IsCompleted == iscompleted);
            }

            return View(tasks.ToList());
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
