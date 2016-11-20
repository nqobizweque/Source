using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MySchedule.Models;
using MySchedule.ViewModels;

namespace MySchedule.Controllers
{
    [Authorize]
    public class UserTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Complete(int id, bool status, DateTime complete, ApplicationUser ApplicationUserID)
        {
            UserTask task = db.UserTasks.Find(id);
            task.Status = status;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }


        // GET: UserTasks
        public ActionResult Index()
        {
            //Assigns all user tasks belonging to current logged in user to sortedTasks
            var sortedTasks = db.UserTasks.ToList().Where(m => m.ApplicationUserID.Equals(User.Identity.Name));

        //Sorts list of user tasks by status. Checked tasks will be at the bottom of the list. Then sorts by date from oldest to latest
            sortedTasks = sortedTasks.OrderByDescending(d => d.Status).ThenBy(i => i.Date);
            return View(sortedTasks);
        }

        // GET: UserTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // GET: UserTasks/Create
        public ActionResult Create()
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

            TaskViewModel tvm = new TaskViewModel();
            tvm.users = curUsers;

            return View(tvm);
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserTaskID,ApplicationUserID,Title,Date,Status")] UserTask userTask, TaskViewModel tvm)
        {
           

            if (!String.IsNullOrWhiteSpace(tvm.Title))
            {
                userTask.UserTaskID = tvm.UserTaskID;
                userTask.ApplicationUserID = User.Identity.Name;
                userTask.Date = DateTime.Today;
                userTask.Status = false;
                userTask.Title = tvm.Title;
                db.UserTasks.Add(userTask);
                db.SaveChanges();

                //foreach(var item in list)
                //  {
                //      if(item.Selected)
                //      {
                //      send task invitee
                //      }
                //  }
                return RedirectToAction("Index");
            }

            return View(userTask);
        }

        // GET: UserTasks/Edit/5
        public ActionResult Edit(int? id)
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

            ViewBag.ContactsList = curUsers;

            ViewBag.taskid = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTaskID,ApplicationUserID,Title,Date,Status")] UserTask userTask)
        {
            

            if (!String.IsNullOrWhiteSpace(userTask.Title))
            {
                if (userTask.Date >= DateTime.Today)
                {
                               
                    db.Entry(userTask).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", "Date must be today or later");

            }
            return View(userTask);
        }

        // GET: UserTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTask userTask = db.UserTasks.Find(id);
            db.UserTasks.Remove(userTask);
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
