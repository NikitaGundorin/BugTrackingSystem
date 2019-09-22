using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Invalid UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Invalid Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Incorrect Password")]
        public string ConfirmPassword { get; set; }
    }
}
