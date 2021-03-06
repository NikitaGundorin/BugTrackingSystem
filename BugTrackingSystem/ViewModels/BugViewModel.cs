﻿using System;
using System.Collections.Generic;

namespace BugTrackingSystem.ViewModels
{
    public class BugViewModel
    {
        public int Id { get; set; }

        public string CreationDate { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Importance { get; set; }

        public List<BugChangelogViewModel> BugChangeLog { get; set; }
    }
}
