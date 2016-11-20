using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySchedule.Models;

namespace MySchedule.ViewModels
{
    public class TaskViewModel
    {
        public int UserTaskID { get; set; }

        public string ApplicationUserID { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public bool Selected { get; set; }
        public List<ApplicationUser> users { get; set; }
    }
}