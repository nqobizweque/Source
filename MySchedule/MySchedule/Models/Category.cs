using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace MySchedule.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }

        [Required]
        [StringLength(30)]
        public string Description { get; set; }

        [StringLength(4)]
        public string Abbreviation { get; set; }

        [StringLength(7)]
        public string Colour { get; set; }

    }
}