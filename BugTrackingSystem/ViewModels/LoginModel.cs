using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Invalid UserName or Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Invalid Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
