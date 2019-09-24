using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class BugChangelogViewModel
    {
        [Required]
        public int BugId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string NewStatus { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
