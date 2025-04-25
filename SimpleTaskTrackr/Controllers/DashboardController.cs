using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SimpleTaskTrackr.Data;
using SimpleTaskTrackr.Models.SimpleTaskTrackModel;

namespace SimpleTaskTrackr.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SimpleTaskTrackrDbContext simpleTaskTrackrDbContext;

        public DashboardController(SimpleTaskTrackrDbContext simpleTaskTrackrDbContext)
        {
            this.simpleTaskTrackrDbContext = simpleTaskTrackrDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var dataformDb = await simpleTaskTrackrDbContext.TasksProperties.ToListAsync();
            return View(dataformDb);
        }
        public async Task<IActionResult> My_Task()
        {
            var dataformDb = await simpleTaskTrackrDbContext.TasksProperties.ToListAsync();
            if (TempData["CurrentUser"] == null)
            {
                return View(dataformDb);
            }
            else
            {
                var TotalTask = await simpleTaskTrackrDbContext.TasksProperties.FirstOrDefaultAsync(x => x.UserId == (Guid)TempData["CurrentUser"]);
                if (TotalTask == null)
                {
                    return View();
                }
                else
                {
                    //var dataformDb = await simpleTaskTrackrDbContext.TasksProperties.Where(x => x.UserId == (Guid)TempData["CurrentUser"]).ToListAsync();
                    return View(TotalTask);
                }
            }
        }
    }
}
