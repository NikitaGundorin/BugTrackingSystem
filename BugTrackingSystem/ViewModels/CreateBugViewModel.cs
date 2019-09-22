using System;
using System.ComponentModel.DataAnnotations;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class CreateBugViewModel
    {
        [Required(ErrorMessage = "Invalid parameters")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Invalid parameters")]
        public string FullDescription { get; set; }

        [Required(ErrorMessage = "Invalid parameters")]
        public int PriorityId { get; set; }

        [Required(ErrorMessage = "Invalid parameters")]
        public int ImportanceId { get; set; }
    }
}
