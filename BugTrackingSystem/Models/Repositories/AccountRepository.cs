using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BugTrackingSystem.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace BugTrackingSystem.Models.Repositories
{
    public class AccountRepository
    {
        private readonly BugTrackingSystemContext db;

        public AccountRepository(BugTrackingSystemContext context)
        {
            db = context;
        }

        private ClaimsIdentity GetIdentity(string username, string passwordHash)
        {
            User user = db.Users.FirstOrDefault(x => (x.UserName == username || x.Email == username) && x.PasswordHash == passwordHash);

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

        private object Authenticate(string userName, string passwordHash)
        {
            var identity = GetIdentity(userName, passwordHash);
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

        public object Token(LoginModel model)
        {
            string passwordHash = GetHashPassword(model.Password);
            var result = Authenticate(model.UserName, passwordHash);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public object Register(RegisterModel model)
        {
            User user = db.Users.FirstOrDefault(u => (u.Email == model.Email) || (u.UserName == model.UserName));

            if (user == null)
            {
                string passwordHash = GetHashPassword(model.Password);
                db.Users.Add(new User { Email = model.Email, PasswordHash = passwordHash, UserName = model.UserName, Role = "user" });
                db.SaveChangesAsync();

                var result = Authenticate(model.UserName, passwordHash);

                return result;
            }
            return null;
        }

        private string GetHashPassword(string password)
        {
            string hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, password);
            }

            return hash;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
