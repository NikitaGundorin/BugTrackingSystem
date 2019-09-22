using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.Models
{
    public class Bug
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public int ImportanceId { get; set; }

        [Required]
        public Importance Importance { get; set; }
    }
}
