using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySchedule.ViewModels;
using MySchedule.Models;

namespace MySchedule.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            ReportViewModel rvm = new ReportViewModel();

            List<UserEvent> totalEvents = db.UserEvents.Where(o => o.ApplicationUserID.Equals(User.Identity.Name)).ToList();
            rvm.totalEvents = totalEvents.Count();
            List<UserEvent> totalUpcomingEvents = new List<UserEvent>();
            foreach (UserEvent o in totalEvents)
            {
                if (o.EndTime >= DateTime.Today)
                    totalUpcomingEvents.Add(o);
            }
            rvm.totalUpcomingEvents = totalUpcomingEvents.Count();

            List<Location> totalLocations = db.Locations.Where(o => o.ApplicationUserID.Equals(User.Identity.Name)).ToList();
            rvm.totalLocations = totalLocations.Count();        
            rvm.totalLocationlessEvents = 0;
            rvm.freqLocationCount = 0;
            List<Location> totalLocationsNoNull = new List<Location>();
            foreach (var loc in totalEvents) {
                if (loc.Location != null)
                    totalLocationsNoNull.Add(loc.Location);
                else
                    rvm.totalLocationlessEvents += 1;
            }
            Location maxLoc = totalLocationsNoNull.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            foreach(var curEv in totalEvents)
            {
                if (curEv != null && curEv.Location == maxLoc)
                    rvm.freqLocationCount += 1;
            }        
            rvm.mostFreqLocation = maxLoc.Venue;


            List<Category> totalCategories = db.Categories.Where(o => o.ApplicationUserID.Equals(User.Identity.Name)).ToList();
            rvm.totalCategories = totalCategories.Count();
            rvm.totalCategorylessEvents = 0;
            rvm.freqCategoryCount = 0;
            List<Category> totalCategoriesNoNull = new List<Category>();
            foreach (var loc in totalEvents)
            {
                if (loc.Category != null)
                    totalCategoriesNoNull.Add(loc.Category);
                else
                    rvm.totalCategorylessEvents += 1;
            }
            Category maxCat = totalCategoriesNoNull.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            foreach (var curEv in totalEvents)
            {
                if (curEv != null && curEv.Category == maxCat)
                    rvm.freqCategoryCount += 1;
            }
            rvm.mostFreqCategory = maxCat.Description;



            List<Module> totalModules = db.Modules.Where(o => o.ApplicationUserID.Equals(User.Identity.Name)).ToList();
            rvm.totalModules = totalModules.Count();
            rvm.totalModulessEvents = 0;
            rvm.freqModuleCount = 0;
            List<Module> totalModulesNoNull = new List<Module>();
            foreach (var loc in totalEvents)
            {
                if (loc.Module != null)
                    totalModulesNoNull.Add(loc.Module);
                else
                    rvm.totalModulessEvents += 1;
            }
            Module maxMod = totalModulesNoNull.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            foreach (var curEv in totalEvents)
            {
                if (curEv != null && curEv.Module == maxMod)
                    rvm.freqModuleCount += 1;
            }
            rvm.mostFreqModule = maxMod.Description;


            List<UserTask> totalTasks = db.UserTasks.Where(o => o.ApplicationUserID.Equals(User.Identity.Name)).ToList();
            rvm.totalTasks = totalTasks.Count();
            rvm.totalUpcomingTasks = totalTasks.Where(t => !t.Status).Count();            

            return View(rvm);
        }
    }
}