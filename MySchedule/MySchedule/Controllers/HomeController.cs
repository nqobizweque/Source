using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Calendar;
using MySchedule.Models;
using DayPilot.Web.Mvc.Enums;
using Microsoft.AspNet.Identity;

namespace MySchedule.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return new Dpc().CallBack(this);
        }        
       
        class Dpc : DayPilotCalendar
        {
            ApplicationDbContext db = new ApplicationDbContext();

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);
            }
            protected override void OnEventResize(EventResizeArgs e)
            {
                var toBeResized = (from ev in db.UserEvents where ev.UserEventID == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.StartTime = e.NewStart;
                toBeResized.EndTime = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var toBeResized = (from ev in db.UserEvents where ev.UserEventID == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.StartTime = e.NewStart;
                toBeResized.EndTime = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                var toBeCreated = new UserEvent
                {
                    ApplicationUserID = Controller.User.Identity.Name,
                    StartTime = e.Start,
                    EndTime = e.End,
                    Description = (string)e.Data["name"]
                };              
                db.UserEvents.Add(toBeCreated);
                db.SaveChanges();
                Update();
            }

            protected override void OnFinish()
            {
                if(UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = from ev in db.UserEvents where ev.ApplicationUserID == Controller.User.Identity.Name select ev;

                DataIdField = "UserEventID";
                DataTextField = "Description";
                DataStartField = "StartTime";
                DataEndField = "EndTime";

            }
        }     
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DashScreen()
        {
            return View("Dashboard");
        }
    }
}