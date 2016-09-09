using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySchedule.Models;

namespace MySchedule.ViewModels
{
    public class ContactsViewModel
    {
        public IEnumerable<ApplicationUser> users { get; set; }
        public IEnumerable<Contact> contacts { get; set; }

    }
}