using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Services;
using SimpleTaskTrackr.Data;
using SimpleTaskTrackr.Models;
using SimpleTaskTrackr.Models.SimpleTaskTrackModel;
using System.Diagnostics;

namespace SimpleTaskTrackr.Controllers
{
    public class HomeController : Controller
    {
        private readonly SimpleTaskTrackrDbContext simpleTaskTrackrDbContext;
        private readonly UserAuthenticationService userAuthenticationService;

        public HomeController(SimpleTaskTrackrDbContext simpleTaskTrackrDbContext, UserAuthenticationService userAuthenticationService)
        {
            this.simpleTaskTrackrDbContext = simpleTaskTrackrDbContext;
            this.userAuthenticationService = userAuthenticationService;
        }
        public async Task<IActionResult> Index()
        {

            var dataformDb = await simpleTaskTrackrDbContext.TasksProperties.ToListAsync();
            if(dataformDb== null || dataformDb.Count == 0)
            {
                return View(new List<TasksProperties>());
            }
            ViewBag.TotalTasks = simpleTaskTrackrDbContext.TasksProperties.Count();
            ViewBag.PendingTasks = simpleTaskTrackrDbContext.TasksProperties.Count(t => t.TaskStatus == "Pending");
            ViewBag.InProgressTasks = simpleTaskTrackrDbContext.TasksProperties.Count(t => t.TaskStatus == "In-Progress");
            ViewBag.CompletedTasks = simpleTaskTrackrDbContext.TasksProperties.Count(t => t.TaskStatus == "Completed");
            ViewBag.CancelledUserTasks = simpleTaskTrackrDbContext.TasksProperties.Count(t => t.TaskStatus == "Cancelled"); 
            ViewBag.OverdueUserTasks = simpleTaskTrackrDbContext.TasksProperties.Count(t => t.TaskDueDate < DateTime.Today);

            return View(dataformDb);

        }
        public async Task<IActionResult> My_Task()
        {
            var currentuserid = userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
            var tasks = await simpleTaskTrackrDbContext.TasksProperties.Where(x => x.UserId == Guid.Parse(currentuserid)).ToListAsync();

            if (tasks == null || tasks.Count == 0)
            {
                ViewBag.TotalUserTasks = tasks.Count();
                ViewBag.PendingUserTasks = tasks.Count(t => t.TaskStatus == "Pending");
                ViewBag.InProgressUserTasks = tasks.Count(t => t.TaskStatus == "In-Progress");
                ViewBag.CompletedUserTasks = tasks.Count(t => t.TaskStatus == "Completed");
                ViewBag.CancelledUserTasks = tasks.Count(t => t.TaskStatus == "Cancelled");
                ViewBag.OverdueUserTasks = tasks.Count(t => t.TaskDueDate < DateTime.Today);

                return View(new List<TasksProperties>());
            }
            else
            {
                ViewBag.TotalUserTasks = tasks.Count();
                ViewBag.PendingUserTasks = tasks.Count(t => t.TaskStatus == "Pending");
                ViewBag.InProgressUserTasks = tasks.Count(t => t.TaskStatus == "In-Progress");
                ViewBag.CompletedUserTasks = tasks.Count(t => t.TaskStatus == "Completed");
                ViewBag.CancelledUserTasks = tasks.Count(t => t.TaskStatus == "Cancelled");
                ViewBag.OverdueUserTasks = tasks.Count(t => t.TaskDueDate < DateTime.Today);

                return View(tasks);
            }

        }
    }
}
