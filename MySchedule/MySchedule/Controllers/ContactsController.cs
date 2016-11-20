using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MySchedule.Models;
using Microsoft.AspNet.Identity;
using MySchedule.ViewModels;

namespace MySchedule.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index()
        {
            //List of all existing users
            IEnumerable<ApplicationUser> users = db.Users.ToList();

            //List of contacts(users) belonging to the current logged in user
            IEnumerable<Contact> contacts = db.Contacts.ToList().Where(c => c.ApplicationUserID.Equals(User.Identity.Name));

            //Compares  contacts with list of existing users to extract details like Name, Surname and Email
            List<ApplicationUser> curUsers = new List<ApplicationUser>();

            foreach (var contact in contacts)
            {
                foreach (var user in users)
                {
                    if (contact.ContactUserID.Equals(user.UserName))
                    {
                        curUsers.Add(user);
                    }
                }
            }

            //New ViewModel to display contact details on index view
            ContactsViewModel cvm = new ContactsViewModel();
            cvm.users = curUsers;
            cvm.contacts = contacts;

            return View(cvm);
        }

        // GET: Contacts/Details
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationUserID,ContactUserID,Date")] Contact contact)
        {
            if (!String.IsNullOrWhiteSpace(contact.ContactUserID))
            {

                var count = db.Users.Count(u => u.UserName == contact.ContactUserID);

                if (count != 0)
                {
                    contact.ApplicationUserID = User.Identity.Name;
                    contact.Date = DateTime.Today;
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", "User does not exist");
            }

            return View(contact);
        }

        //Checks the entered email against the user database to verify if the user exists or not
        public JsonResult CheckName(FormCollection form)
        {
            
            System.Threading.Thread.Sleep(3000);
            string name = form["username"];
            //Checks for a user email with submited email by counting the number of times a user with said email is found in the database
            var count = db.Users.Count(u => u.UserName.Equals(name));

          //If count is 0, user does not exist. Return false
            if (count == 0)
                return Json(false);
            //If the count is greater than 0 then user exist. Return true
            else
            {
                //Gets user's details, name and surname, using submitted email from User database.
                var Conname = (from a in db.Users
                               where a.Email == name
                               select a.FirstName).FirstOrDefault();
                ViewBag.Conname = Conname;
                var Consur = (from b in db.Users
                              where b.Email == name
                              select b.LastName).FirstOrDefault();
                ViewBag.Consur = Consur;
                return Json(true);
            }
        }

        // GET: Contacts/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationUserID,ContactUserID,Date")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified; //Applies changes to database entry
                db.SaveChanges(); //Saves changes
                return RedirectToAction("Index"); //Return to Index page
            }
            return View(contact);
        }

        // GET: Contacts/Delete
        public ActionResult Delete(string applicationUserID, string contactUserID)
        {
           // Finds contact entry using composite key
            Contact contact = db.Contacts.Find(applicationUserID, contactUserID);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string applicationUserID, string contactUserID)
        {
            Contact contact = db.Contacts.Find(applicationUserID, contactUserID);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
