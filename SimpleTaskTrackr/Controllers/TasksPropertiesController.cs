using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Services;
using SimpleTaskTrackr.Data;
using SimpleTaskTrackr.Models.SimpleTaskTrackModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleTaskTrackr.Controllers
{
    [Authorize]
    public class TasksPropertiesController : Controller
    {
        private readonly SimpleTaskTrackrDbContext _context;
        private readonly UserAuthenticationService userAuthenticationService;

        public TasksPropertiesController(SimpleTaskTrackrDbContext context, UserAuthenticationService userAuthenticationService)
        {
            _context = context;            
            this.userAuthenticationService = userAuthenticationService;
        }

        // GET: TasksProperties
        public async Task<IActionResult> Index()
        {
            return View(await _context.TasksProperties.ToListAsync());
        }

        // GET: TasksProperties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksProperties = await _context.TasksProperties.FirstOrDefaultAsync(m => m.TaskId == id);
            var userIdClaim = userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;            
            if (tasksProperties == null)
            {
                return NotFound();
            }
            var ubydata = await _context.TaskUpdatedBys.Where(x => x.TaskId == id.ToString()).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();
            if (ubydata == null)
            {
                ViewBag.UpdateBy = "Null";
            }
            else
            {
                ViewBag.UpdateBy = ubydata.UpdatedBy ?? "";
            }
            return View(tasksProperties);
        }

        // GET: TasksProperties/Create
        public async Task<IActionResult> Create()
        {
            var userIdClaim = userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
            var DbData = await _context.Userproperties.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userIdClaim));            
            var userName = new TasksProperties
            {
                CreatedBy = DbData.Name,
                UserId = DbData.UserId,
            };
            return View(userName);
        }

        // POST: TasksProperties/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TasksProperties tasksProperties)
        {
            // Get current user details
            var userIdClaim = userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
            var emailClaim = userAuthenticationService.GetCurrentUser().FindFirst(ClaimTypes.Name)?.Value;

            // Validate user is logged in
            if (string.IsNullOrEmpty(userIdClaim))
            {
                ModelState.AddModelError("", "User is not authenticated.");
                return View(tasksProperties);
            }

            // Set additional fields before validation
            tasksProperties.UserId = Guid.Parse(userIdClaim);
            tasksProperties.TaskId = Guid.NewGuid();
            tasksProperties.CreatedOn = DateTime.Now;
            tasksProperties.LastUpdatedOn = DateTime.Now;

            // Optional: clear validation errors for fields set server-side
            ModelState.Remove("User");
           
            if (ModelState.IsValid)
            {
                _context.Add(tasksProperties);
                await _context.SaveChangesAsync();
                TempData["success"] = "Create Task Successful";

                return RedirectToAction("Index","Home");
            }

            // If model is invalid, return view with validation messages
            return View(tasksProperties);
        }


        // GET: TasksProperties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksProperties = await _context.TasksProperties.FindAsync(id);
            if (tasksProperties == null)
            {
                return NotFound("Not Found !");
            }
            return View(tasksProperties);
        }

        // POST: TasksProperties/Edit/5
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TaskId,TaskTitle,TaskDescription,TaskDueDate,TaskStatus,TaskRemarks,CreatedOn,LastUpdatedOn,CreatedBy,UserId")] TasksProperties tasksProperties)
        {
            if (id != tasksProperties.TaskId)
            {
                return NotFound();
            }
            ModelState.Remove("User");
           
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
                    var currentusername = await _context.Userproperties.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userIdClaim));
                    TasksUpdatedBy tasksUpdatedBy = new TasksUpdatedBy
                    {
                        TaskUpdateId = Guid.NewGuid(),
                        UpdatedBy = currentusername.Name,
                        UserId = userIdClaim,
                        TaskId = tasksProperties.TaskId.ToString(),
                        UpdatedDate = DateTime.Now
                    };
                    _context.Add(tasksUpdatedBy);
                    tasksProperties.LastUpdatedOn = DateTime.Now;                    
                    _context.Update(tasksProperties);
                    
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Update Task Successful";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksPropertiesExists(tasksProperties.TaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(tasksProperties);
        }

        // GET: TasksProperties1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasksProperties = await _context.TasksProperties
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (tasksProperties == null)
            {
                return NotFound();
            }
            TempData["warning"] = "warning !";

            return View(tasksProperties);
        }

        // POST: TasksProperties1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tasksProperties = await _context.TasksProperties.FindAsync(id);
            if (tasksProperties != null)
            {
                _context.TasksProperties.Remove(tasksProperties);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Deleted Task Successful";
            return RedirectToAction("Index", "Home");
        }

        private bool TasksPropertiesExists(Guid id)
        {
            return _context.TasksProperties.Any(e => e.TaskId == id);
        }
    }
}
