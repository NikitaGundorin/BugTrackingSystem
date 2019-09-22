using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.Models
{
    public class BugChangelog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BugId { get; set; }

        [Required]
        public Bug Bug { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
