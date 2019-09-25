using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BugTrackingSystem.ViewModels;
using BugTrackingSystem.Models.Repositories;

namespace BugTrackingSystem.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly AccountRepository accountRepository;


        public AccountController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.TokenAsync(model);

                if (result == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                return Ok(result);
            }

            return BadRequest(new { message = "Username or password is incorrect" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.RegisterAsync(model);

                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest(new { message = "User already exist" });
            }

            return BadRequest(new { message = "Invalid data" });
        }
    }
}