using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySchedule.Models;

namespace MySchedule.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<Module> Modules { get; set; }
        public IEnumerable<UserEvent> UserEvents { get; set; }
    }
}