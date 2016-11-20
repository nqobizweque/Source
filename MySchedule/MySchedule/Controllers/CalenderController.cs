using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Month;
using MySchedule.Models;
using DayPilot.Web.Mvc.Enums;

namespace MySchedule.Controllers
{
    public class CalenderController : Controller
    {
        // GET: Calender
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend()
        {
            return new Dpm().CallBack(this);
        }

        class Dpm : DayPilotMonth
        {
            ApplicationDbContext db = new ApplicationDbContext();

            protected override void OnInit(InitArgs e)
            {

                
                Events = from ev in db.UserEvents where ev.ApplicationUserID == Controller.User.Identity.Name select ev;

                DataIdField = "UserEventID";
                DataTextField = "Description";
                DataStartField = "StartTime";
                DataEndField = "EndTime";

                Update();
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

                if (!String.IsNullOrWhiteSpace(toBeCreated.Description))
                {
                    var db = new ApplicationDbContext();
                    db.UserEvents.Add(toBeCreated);
                    db.SaveChanges();
                    Update();
                }
            }
            protected override void OnFinish()
            {
               
                if (UpdateType == CallBackUpdateType.None)
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
    }

   
}