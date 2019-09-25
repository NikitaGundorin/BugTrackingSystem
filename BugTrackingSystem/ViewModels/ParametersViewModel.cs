using System;
using System.Collections.Generic;
using BugTrackingSystem.Models;

namespace BugTrackingSystem.ViewModels
{
    public class ParametersViewModel
    {
        public List<Importance> Importances { get; set; }

        public List<Priority> Priorities { get; set; }
    }
}
