using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySchedule.Models
{
    public class UserEvent
    {
        [Key]
        public int UserEventID { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public string ApplicationUserID { get; set; }

        [Required(ErrorMessage = "Description required")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Module")]
        public int? ModuleID { get; set; }
        public virtual Module Module { get; set; }

        [Display(Name = "Location/Venue")]
        public int? LocationID { get; set; }
        public virtual Location Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? Reminder { get; set; }

        public bool Recurring { get; set; }

        public enum RecurrBy
        {
            Day, Week, Month
        }

        public virtual RecurrBy RecurBy { get; set; }

        public int? RecurIntervals { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<EventInvitee> EventInvitees { get; set; }

    }
}