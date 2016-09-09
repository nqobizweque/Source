using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MySchedule.Models
{
    public class UserTask
    {
        [Key]
        public int UserTaskID { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Task Name")]
        public string Title { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Display(Name = "Done")]
        public bool Status { get; set; }

        public virtual ICollection<TaskInvitee> TaskInvitees { get; set; }
    }
}