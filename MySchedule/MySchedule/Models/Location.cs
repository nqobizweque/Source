using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySchedule.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }

        [Required]
        [StringLength(15)]
        public string Venue { get; set; }

        [StringLength(30)]
        public string Address { get; set; }
    }
}