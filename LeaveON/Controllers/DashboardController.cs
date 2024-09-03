using InventoryRepo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaveON.Controllers
{

  //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
  //[NoCache]
  [Authorize(Roles = "Admin,Manager,User")]
  public class DashboardController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();
    Dashboard dashboard;
    // GET: SOes
    public ActionResult Index()
    {
      //if (!User.Identity.IsAuthenticated)
      //{
      //  return RedirectToAction("Login", "Account");
      //}
      //EnterProfit();
      dashboard = new Dashboard();
      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, 1);
      var dtEndtDate = dtStartDate.AddMonths(1).AddSeconds(-1);

      dashboard.Drivers = db.Drivers.Where(x => x.IsDeleted == false).Count();
      dashboard.Passengers = db.Passengers.Where(x => x.IsDeleted == false).Count();
      dashboard.Trips = db.Trips.Where(x => x.IsDeleted == false).Count();
      dashboard.AspNetUsers = db.AspNetUsers.Where(x => x.IsDeleted == false).Count();

      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy");
      ViewBag.EndDate = dtEndtDate.ToString("dd-MMM-yyyy");

      return View(dashboard);
    }

    public ActionResult SearchData(string startDate, string endDate)
    {

      DateTime dtStartDate;
      DateTime dtEndtDate;
      dashboard = new Dashboard();

      if (startDate != string.Empty && endDate != string.Empty)
      {
        dtStartDate = DateTime.Parse(startDate);
        dtEndtDate = DateTime.Parse(endDate);

        dashboard.Drivers = db.Drivers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        dashboard.Passengers = db.Passengers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        dashboard.Trips = db.Trips.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        dashboard.AspNetUsers = db.AspNetUsers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
      }
      return PartialView("_SelectedSOSR", dashboard);
    }
  }

}
