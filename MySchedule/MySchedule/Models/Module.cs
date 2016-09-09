using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySchedule.Models
{
    public class Module
    {
        [Key]
        public int ModuleID { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }

        [Required]
        [StringLength(7)]
        public string Code { get; set; }

        [StringLength(30)]
        public string Description { get; set; }
    }
}