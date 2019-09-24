using System;
using System.Collections.Generic;

namespace BugTrackingSystem.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<BugPreviewViewModel> Bugs { get; set; }
        public PageViewModel Pages { get; set; }
    }
}
