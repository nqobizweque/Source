using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;
using Microsoft.AspNet.Identity;

using MySchedule.Models;
namespace MySchedule.Controllers
{
    public class CalendarController : Controller
    {

        public class DashboardViewModel
        {
            public DHXScheduler schedular { get; set; }

        }

        public ActionResult Index()
        {
            //Being initialized in that way, scheduler will use CalendarController.Data as a the datasource and CalendarController.Save to process changes
            var scheduler = new DHXScheduler(this);

            /*
             * It's possible to use different actions of the current controller
             *      var scheduler = new DHXScheduler(this);     
             *      scheduler.DataAction = "ActionName1";
             *      scheduler.SaveAction = "ActionName2";
             * 
             * Or to specify full paths
             *      var scheduler = new DHXScheduler();
             *      scheduler.DataAction = Url.Action("Data", "Calendar");
             *      scheduler.SaveAction = Url.Action("Save", "Calendar");
             */

            /*
             * The default codebase folder is ~/Scripts/dhtmlxScheduler. It can be overriden:
             *      scheduler.Codebase = Url.Content("~/customCodebaseFolder");
             */
            
 
            //scheduler.InitialDate = new DateTime(2012, 09, 03);

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var data = new SchedulerAjaxData();
            data.ServerList.Add("dayoff", db.UserEvents.Where(o => o.Category.Equals(User.Identity.GetUserName())));
            return (ContentResult)data;
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            
            try
            {
                var changedEvent = (UserEvent)DHXEventsHelper.Bind(typeof(UserEvent), actionValues);
                var data = new ApplicationDbContext();
     

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        //do insert
                        //action.TargetId = changedEvent.id;//assign postoperational id
                        changedEvent.ApplicationUserID = User.Identity.GetUserName();
                        data.UserEvents.Add(changedEvent);
                        data.SaveChanges();
                        break;
                    case DataActionTypes.Delete:
                        //do delete
                        UserEvent eVent = data.UserEvents.Find(changedEvent);
                        if(eVent != null)
                        {
                            UserEventsController con = new UserEventsController();
                            con.Delete(eVent.UserEventID);
                        }
                        break;
                    default:// "update"                          
                        //do update
                        data.Entry(changedEvent).State = EntityState.Modified;
                        data.SaveChanges();                      
                        break;
                }
                action.TargetId = changedEvent.UserEventID;
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }
    }
}

