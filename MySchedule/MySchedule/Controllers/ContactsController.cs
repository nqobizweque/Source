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

    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index()
        {

            IEnumerable<ApplicationUser> users = db.Users.ToList();

            IEnumerable<Contact> contacts = db.Contacts.ToList().Where(c => c.ApplicationUserID.Equals(User.Identity.Name));

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

            ContactsViewModel cvm = new ContactsViewModel();
            cvm.users = curUsers;
            cvm.contacts = contacts;

            return View(cvm);
        }

        // GET: Contacts/Details/5
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        public JsonResult CheckName(FormCollection form)
        {
            string name = form["username"];
            var count = db.Users.Count(u => u.UserName == name);
            if (count == 0)
                return Json(false);
            else
            {
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

        // GET: Contacts/Edit/5
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

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationUserID,ContactUserID,Date")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(string applicationUserID, string contactUserID)
        {
            Contact contact = db.Contacts.Find(applicationUserID, contactUserID);
            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
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
