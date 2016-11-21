using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySchedule.Models;

namespace MySchedule.ViewModels
{
    public class UpcomingViewModel
    {
        public IEnumerable<UserEvent> UpcomingEvents { get; set; }
        public IEnumerable<UserTask> ToDoTasks { get; set; }
    }
}