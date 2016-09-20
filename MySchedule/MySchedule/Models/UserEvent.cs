using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DHTMLX.Scheduler;

namespace MySchedule.Models
{
    public class UserEvent
    {
        [Key]
        [DHXJson(Alias = "id")]
        public int UserEventID { get; set; }

        [Required]
        [DHXJson(Ignore = true)]
        public string ApplicationUserID { get; set; }


        [Required]
        [DHXJson(Alias = "text")]
        public string Description { get; set; }


        [Display(Name = "Category")]
        [DHXJson(Ignore = true)]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [DHXJson(Ignore = true)]
        public int? ModuleID { get; set; }
        public virtual Module Module { get; set; }

        [DHXJson(Ignore = true)]
        public int? LocationID { get; set; }
        public virtual Location Location { get; set; }

        [DHXJson(Alias = "start_date")]
        public DateTime StartTime { get; set; }

        [DHXJson(Alias = "end_date")]
        public DateTime EndTime { get; set; }

        [DHXJson(Ignore = true)]
        public DateTime? Reminder { get; set; }

        [DHXJson(Ignore = true)]
        public bool Recurring { get; set; }

        [DHXJson(Ignore = true)]
        public enum RecurrBy
        {
            Day, Week, Month
        }

        [DHXJson(Ignore = true)]
        public virtual RecurrBy RecurBy { get; set; }

        [DHXJson(Ignore = true)]
        public int? RecurIntervals { get; set; }

        [DHXJson(Ignore = true)]
        public string Notes { get; set; }

        [DHXJson(Ignore = true)]
        public virtual ICollection<EventInvitee> EventInvitees { get; set; }

    }
}