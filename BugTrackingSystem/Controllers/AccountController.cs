using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BugTrackingSystem.Models;
using BugTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly BugTrackingSystemContext db;

        public AccountController(BugTrackingSystemContext context)
        {
            db = context;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var username = model.UserName;
                var password = model.Password;

                var result = Authenticate(username, password);
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
                User user = await db.Users.FirstOrDefaultAsync(u => ((u.Email == model.Email) || (u.UserName == model.UserName)));
                if (user == null)
                {
                    db.Users.Add(new User { Email = model.Email, Password = model.Password, UserName = model.UserName, Role = "user" });
                    await db.SaveChangesAsync();

                    var result = Authenticate(model.UserName, model.Password);

                    return Ok(result);
                }
                    return BadRequest(new { message = "User already exist" });
            }
                return BadRequest(new { message = "Invalid data" });
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => (x.UserName == username || x.Email == username) && x.Password.Equals(password));
            if (user != null)
            {
                var claims = new List<Claim>
            {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        private object Authenticate(string userName, string password)
        {
            var identity = GetIdentity(userName, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            notBefore: now,
                            claims: identity.Claims,
                            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                role = identity.Claims.Last().Value
            };

            return response;
        }
    }
}