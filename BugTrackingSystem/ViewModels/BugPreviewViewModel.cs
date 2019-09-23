using System;

namespace BugTrackingSystem.ViewModels
{
    public class BugPreviewViewModel
    {
        public int Id { get; set; }

        public string CreationDate { get; set; }

        public string ShortDescription { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Importance { get; set; }
    }
}
