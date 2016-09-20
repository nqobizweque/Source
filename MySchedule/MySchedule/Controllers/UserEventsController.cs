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

namespace MySchedule.Controllers
{
    public class UserEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserEvents
        public ActionResult Index()
        {
            var userEvents = db.UserEvents.Include(u => u.Category).
                Include(u => u.Location)
                .Include(u => u.Module);

            return View(userEvents.ToList());
        }

        // GET: UserEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvent userEvent = db.UserEvents.Find(id);
            if (userEvent == null)
            {
                return HttpNotFound();
            }
            return View(userEvent);
        }

        // GET: UserEvents/Create
        public ActionResult Create()
        {
            string appUser = User.Identity.GetUserName();
            ViewBag.CategoryID = new SelectList(db.Categories.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "CategoryID", "Description");
            ViewBag.LocationID = new SelectList(db.Locations.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "LocationID", "Venue");
            ViewBag.ModuleID = new SelectList(db.Modules.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "ModuleID", "Code");
            return View();
        }

        // POST: UserEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserEventID,ApplicationUserID,Description,CategoryID,ModuleID,LocationID,StartTime,EndTime,Reminder,Recurring,RecurBy,RecurIntervals,Notes")] UserEvent userEvent)
        {
            if (!String.IsNullOrEmpty(userEvent.Description) && !userEvent.StartTime.Equals(null))
            {
                userEvent.ApplicationUserID = User.Identity.GetUserName();
                db.UserEvents.Add(userEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string appUser = User.Identity.GetUserName();
            ViewBag.CategoryID = new SelectList(db.Categories.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "CategoryID", "Description", userEvent.CategoryID);
            ViewBag.LocationID = new SelectList(db.Locations.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "LocationID", "Venue", userEvent.LocationID);
            ViewBag.ModuleID = new SelectList(db.Modules.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "ModuleID", "Code", userEvent.ModuleID);
            return View(userEvent);
        }

        // GET: UserEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvent userEvent = db.UserEvents.Find(id);
            if (userEvent == null)
            {
                return HttpNotFound();
            }
            string appUser = User.Identity.GetUserName();
            ViewBag.CategoryID = new SelectList(db.Categories.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "CategoryID", "Description", userEvent.CategoryID);
            ViewBag.LocationID = new SelectList(db.Locations.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "LocationID", "Venue", userEvent.LocationID);
            ViewBag.ModuleID = new SelectList(db.Modules.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "ModuleID", "Code", userEvent.ModuleID);
            return View(userEvent);
        }

        // POST: UserEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserEventID,ApplicationUserID,Description,CategoryID,ModuleID,LocationID,StartTime,EndTime,Reminder,Recurring,RecurBy,RecurIntervals,Notes")] UserEvent userEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string appUser = User.Identity.GetUserName();
            ViewBag.CategoryID = new SelectList(db.Categories.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "CategoryID", "Description", userEvent.CategoryID);
            ViewBag.LocationID = new SelectList(db.Locations.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "LocationID", "Venue", userEvent.LocationID);
            ViewBag.ModuleID = new SelectList(db.Modules.Where(c => c.ApplicationUserID.Equals(appUser)).ToList(), "ModuleID", "Code", userEvent.ModuleID);
            return View(userEvent);
        }

        // GET: UserEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvent userEvent = db.UserEvents.Find(id);
            if (userEvent == null)
            {
                return HttpNotFound();
            }
            return View(userEvent);
        }

        // POST: UserEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserEvent userEvent = db.UserEvents.Find(id);
            db.UserEvents.Remove(userEvent);
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
