using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using BugTrackingSystem.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BugTrackingSystemContext db;
        private readonly BugRepository bugRepository;

        public HomeController(BugTrackingSystemContext context)
        {
            db = context;
            bugRepository = new BugRepository(context);
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            List<BugViewModel> bugs = await db.Bugs.Select(b => new BugViewModel
            {
                Id = b.Id,
                CreationDate = b.CreationDate.ToString("dd.MM.yyy hh:mm"),
                ShortDescription = b.ShortDescription,
                FullDescription = b.FullDescription,
                Importance = b.Importance.Name,
                Priority = b.Priority.Name,
                Status = b.Status.Name,
                UserName = b.User.UserName
            }).ToListAsync();

            return Ok(bugs);
        }

        [HttpPost("updatebugstatus")]
        public async Task<IActionResult> UpdateBugStatus(int id, int newBugStatusId, string comment)
        {

            User user = await db.Users.FirstOrDefaultAsync(u => (u.Email == User.Identity.Name) || (u.UserName == User.Identity.Name));

            await bugRepository.UpdateBugStatusAsync(id, newBugStatusId, comment, user.Id);

            return Ok();
        }

        [HttpGet("addbug")]
        public IActionResult AddBug()
        {
            List<List<object>> parameters = new List<List<object>>();
            List<object> importance = new List<object>();
            foreach (Importance i in db.Importances)
            {
                importance.Add(i);
            }
            parameters.Add(importance);

            List<object> priority = new List<object>();
            foreach (Priority p in db.Priorities)
            {
                priority.Add(p);
            }
            parameters.Add(priority);

            return Ok(parameters);
        }

        [HttpPost("addbug")]
        public async Task<IActionResult> AddBug([FromBody] CreateBugViewModel bug)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => (u.Email == User.Identity.Name) || (u.UserName == User.Identity.Name));

                await bugRepository.CreateBug(bug, user);
                return Ok();
            }
            return BadRequest(new { message = "Parameters are incorrect" });
        }

        [HttpGet("getbugchangelog")]
        public async Task<IActionResult> GetBugChangelog(int bugId)
        {
            List<BugChangelogViewModel> bugChangelogs = await bugRepository.GetBugChangelogs(bugId);

            return Ok(bugChangelogs);
        }
    }
}
