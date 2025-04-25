using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Services;
using SimpleTaskTrackr.Data;
using SimpleTaskTrackr.DTO;
using SimpleTaskTrackr.Models.SimpleTaskTrackModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleTaskTrackr.Controllers
{
    public class UserpropertiesController : Controller
    {
        private readonly SimpleTaskTrackrDbContext _context;
        private readonly UserAuthenticationService _userAuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserpropertiesController(SimpleTaskTrackrDbContext context, UserAuthenticationService userAuthenticationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userAuthenticationService = userAuthenticationService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet(Name = "GetUserProperties")]
        public async Task<IActionResult> Details()
        {
            var userid = _userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
            var UserData = _context.Userproperties.FirstOrDefault(x => x.UserId == Guid.Parse(userid));
            if (UserData == null)
            {
                return NotFound("User profile not found.");
            }
            ViewBag.UserTasks = await _context.TasksProperties.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userid));
            return View(UserData);
           
        }

        // update details
        [HttpPost]
        public async Task<IActionResult> Details(Guid id, Userproperty userproperty)
        {
            var UserData = await _context.Userproperties.FirstOrDefaultAsync(x => x.UserId == userproperty.UserId);
            if (UserData == null)
            {
                return NotFound("User profile not found.");
            }
            if (ModelState.IsValid)
            {
                UserData.Name = userproperty.Name;
                UserData.Email = userproperty.Email;
                UserData.Password = userproperty.Password;

                _context.Update(UserData);
                await _context.SaveChangesAsync();
                TempData["success"] = "Update Successful";
                return RedirectToAction("Details", "Userproperties");
            }
            return View(UserData);
        }


        // get all users
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Userproperties.ToListAsync();         

            List<UserTaskDTO> userTaskDTOs = new List<UserTaskDTO>();
            foreach (var item in users)
            {
        
                var taskCount = _context.TasksProperties.Count(x => x.UserId == item.UserId);
                var pendingCount = _context.TasksProperties.Count(x => x.UserId == item.UserId && x.TaskStatus == "Pending");
                var inProgressCount = _context.TasksProperties.Count(x => x.UserId == item.UserId && x.TaskStatus == "In-Progress");
                var completedCount = _context.TasksProperties.Count(x => x.UserId == item.UserId && x.TaskStatus == "Completed");
                var cancelledCount = _context.TasksProperties.Count(x => x.UserId == item.UserId && x.TaskStatus == "Cancelled");
                var overdueCount = _context.TasksProperties.Count(x => x.UserId == item.UserId && x.TaskDueDate<DateTime.Today);

                UserTaskDTO userTaskDTO = new UserTaskDTO();
                userTaskDTO.UserId = item.UserId;
                userTaskDTO.Name = item.Name;
                userTaskDTO.Email = item.Email;
                userTaskDTO.TotalTasks = taskCount;
                userTaskDTO.PendingTasks = pendingCount;
                userTaskDTO.InProgressTasks = inProgressCount;
                userTaskDTO.CompletedTasks = completedCount;
                userTaskDTO.CancelledTasks = cancelledCount;
                userTaskDTO.OverdueTasks= overdueCount;
                userTaskDTOs.Add(userTaskDTO);




            }

            return View(userTaskDTOs);
        }
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userid = _userAuthenticationService.GetCurrentUser().FindFirst("UserId")?.Value;
            var userdata = await _context.Userproperties.FirstOrDefaultAsync(x=>x.UserId==Guid.Parse(userid));
            return View(userdata);
        }
            // GET: Userproperties/Create
            [AllowAnonymous]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Userproperties/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Password")] Userproperty userproperty)
        {

            if (ModelState.IsValid)
            {
                var existingUser = await _context.Userproperties.FirstOrDefaultAsync(u => u.Email == userproperty.Email);
                if (existingUser != null)
                {
                    //ModelState.AddModelError("", "Email already exists.");
                    TempData["success"] = "Email already exists.";
                    return View(userproperty);
                }

                var newUser = new Userproperty
                {
                    UserId = Guid.NewGuid(),
                    Name = userproperty.Name,
                    Email = userproperty.Email,
                    Password = userproperty.Password // ⚠️ You should hash this in real apps!
                };

                _context.Userproperties.Add(newUser);
                await _context.SaveChangesAsync();
                TempData["success"] = "Account Created Successful !";
                // Auto-login after registration
                await _userAuthenticationService.AuthenticateUserAsync(userproperty.Email, userproperty.Password);
                return RedirectToAction("Index", "Home");
            }

            return View(userproperty);
        }



        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string UserName, String Password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Userproperties.FirstOrDefault(u => u.Email == UserName && u.Password == Password);
                bool auth = await _userAuthenticationService.AuthenticateUserAsync(UserName, Password);

                if (auth)
                {
                    string fullName = user.Name;
                    string[] nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    string shortName = nameParts[0][0].ToString() + nameParts[^1][0].ToString();

                    //ViewBag.name = fullName;
                    //ViewBag.Shortname = shortName;

                    _httpContextAccessor.HttpContext.Session.SetString("UserName", fullName);
                    _httpContextAccessor.HttpContext.Session.SetString("ShortName", shortName);

                    TempData["success"] = "Login Successful";
                    return RedirectToAction("Index", "Home");
                }
                TempData["error"] = "Invalid username or password !";
                //ModelState.AddModelError("", "Invalid username or password");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _userAuthenticationService.LogoutAsync();
            _httpContextAccessor.HttpContext.Session.Clear();
            TempData["success"] = "Logout Successful";
            return RedirectToAction("Login", "Userproperties");
        }

    }
}
