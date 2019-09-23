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
        public async Task<IActionResult> Index(string sortOrder = "IdAsc")
        {
            var bugs = await bugRepository.GetBugsListAsync(sortOrder);

            return Ok(bugs);
        }

        [HttpPost("updatebugstatus")]
        public async Task<IActionResult> UpdateBugStatus([FromBody] BugUpdateStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => (u.Email == User.Identity.Name) || (u.UserName == User.Identity.Name));

                var result = await bugRepository.UpdateBugStatusAsync(model.BugId, model.NewStatusId, model.Comment, user.Id);
                if (result != null)
                {
                    return Ok();
                }
            }

            return BadRequest(new { message = "Parameters are incorrect" });
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

        [HttpGet("bug/{bugId}")]
        public async Task<IActionResult> GetBugChangelog(int bugId)
        {
            var bug = await bugRepository.GetFullBugAsync(bugId);

            if (bug is null)
            {
                return BadRequest( new { message="Bug not found" } );
            }

            return Ok(bug);
        }
    }
}
