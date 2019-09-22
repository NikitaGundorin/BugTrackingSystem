using System;
using System.ComponentModel.DataAnnotations;

namespace BugTrackingSystem.Models
{
    public class Importance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
