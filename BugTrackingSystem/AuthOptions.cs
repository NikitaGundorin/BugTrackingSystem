using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BugTrackingSystem
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "http://localhost:5001/";
        const string KEY = "Bug!Tracking!System!Secret!Key!12";
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}