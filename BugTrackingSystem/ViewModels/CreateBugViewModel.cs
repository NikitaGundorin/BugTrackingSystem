using System;
using System.ComponentModel.DataAnnotations;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class CreateBugViewModel
    {
        [Required]
        [StringLength(35)]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [Required]
        public int ImportanceId { get; set; }
    }
}
