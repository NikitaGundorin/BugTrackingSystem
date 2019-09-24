using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class BugUpdateStatusViewModel
    {
        [Required]
        public int BugId { get; set; }

        [Required]
        public int NewStatusId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
