using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.ViewModels
{
    public class BugUpdateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Required]
        public int ImportanceId { get; set; }

        [Required]
        public int PriorityId { get; set; }
    }
}
