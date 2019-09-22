using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class BugChangelogViewModel
    {
        [Required(ErrorMessage = "Invalid BugId")]
        public int BugId { get; set; }

        [Required(ErrorMessage = "Invalid Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Invalid New Status")]
        public string NewStatus { get; set; }

        [Required(ErrorMessage = "Invalid UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Invalid Comment")]
        public string Comment { get; set; }
    }
}
