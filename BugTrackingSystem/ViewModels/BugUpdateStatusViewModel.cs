using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class BugUpdateStatusViewModel
    {
        [Required(ErrorMessage = "Invalid BugId")]
        public int BugId { get; set; }

        [Required(ErrorMessage = "Invalid New Status")]
        public int NewStatusId { get; set; }

        [Required(ErrorMessage = "Invalid Comment")]
        public string Comment { get; set; }
    }
}
