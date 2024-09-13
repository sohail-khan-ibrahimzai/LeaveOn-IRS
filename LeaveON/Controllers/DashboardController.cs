using InventoryRepo.Models;
using InventoryRepo.ViewModels;
using Microsoft.AspNet.Identity;
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
    public UserInfoDto GetCurrentUserInfo()
    {
      // Get the current user's ID
      var userId = User.Identity.GetUserId();
      if (userId == null)
      {
        return null; // No user is logged in
      }
      var userName = User.Identity.GetUserName();
      return new UserInfoDto
      {
        UserId = userId,
        UserName = userName,
      };
    }
    // GET: SOes
    public ActionResult Index()
    {
      //if (!User.Identity.IsAuthenticated)
      //{
      //  return RedirectToAction("Login", "Account");
      //}
      //EnterProfit();
      dashboard = new Dashboard();
      //Old Working
      //DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      //var dtStartDate = DateTime.UtcNow;
      //var dtEndtDate = DateTime.UtcNow;
      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      // Start date: 08:00 AM of the current day in PKT
      var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, PKDate.Day, 8, 0, 0);
      // End date: 07:59 AM of the next day in PKT
      var dtEndtDate = dtStartDate.AddDays(1).AddMinutes(-1);

      //DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      //var dtStartDate = DateTime.UtcNow;
      //var dtEndtDate = dtStartDate.AddHours(15).AddMinutes(3).AddSeconds(0);

      //Old working
      //var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, 1);
      //var dtEndtDate = dtStartDate.AddMonths(1).AddSeconds(-1);

      var currentUser = GetCurrentUserInfo();
      if (User.IsInRole("Admin"))
      {
        dashboard.Drivers = db.Drivers.Where(x => x.IsDeleted == false).Count();
        dashboard.Passengers = db.Passengers.Where(x => x.IsDeleted == false).Count();
        dashboard.Trips = db.Trips.Where(x => x.IsDeleted == false).Count();
        dashboard.AspNetUsers = db.AspNetUsers.Where(x => x.IsDeleted == false).Count();
        dashboard.PassengerTrips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndtDate && x.IsDeleted == false);
      }
      else
      {
        dashboard.Drivers = db.Drivers.Where(x => x.IsDeleted == false && x.CreatedBy == currentUser.UserId).Count();
        dashboard.Passengers = db.Passengers.Where(x => x.IsDeleted == false && x.CreatedBy == currentUser.UserId).Count();
        dashboard.Trips = db.Trips.Where(x => x.IsDeleted == false && x.CreatedBy == currentUser.UserId).Count();
        dashboard.AspNetUsers = db.AspNetUsers.Where(x => x.IsDeleted == false).Count();
        dashboard.PassengerTrips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndtDate && x.CreatedBy == currentUser.UserId && x.IsDeleted == false);
        //dashboard.PassengerTrips = db.Trips.Where(x => x.IsDeleted == false && x.CreatedBy == currentUser.UserId).ToList();
        //dashboard.AspNetUsers = db.AspNetUsers.Where(x => x.IsDeleted == false && x.CreatedBy == currentUser.UserId).Count();
      }
      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy hh:mm tt");
      ViewBag.EndDate = dtEndtDate.ToString("dd-MMM-yyyy hh:mm tt");

      return View(dashboard);
    }

    public ActionResult SearchData(string startDate, string endDate)
    {

      DateTime dtStartDate;
      DateTime dtEndtDate;
      dashboard = new Dashboard();

      if (startDate != string.Empty && endDate != string.Empty)
      {
        dtStartDate = DateTime.Parse(startDate).Date; // .Date removes the time part
        dtEndtDate = DateTime.Parse(endDate).Date;

        // Query the database with date-only comparison
        dashboard.Drivers = db.Drivers
            .Where(so => DbFunctions.TruncateTime(so.DateCreated) >= dtStartDate && DbFunctions.TruncateTime(so.DateCreated) <= dtEndtDate)
            .Count();

        dashboard.Passengers = db.Passengers
            .Where(so => DbFunctions.TruncateTime(so.DateCreated) >= dtStartDate && DbFunctions.TruncateTime(so.DateCreated) <= dtEndtDate)
            .Count();

        dashboard.Trips = db.Trips
            .Where(so => DbFunctions.TruncateTime(so.DateCreated) >= dtStartDate && DbFunctions.TruncateTime(so.DateCreated) <= dtEndtDate)
            .Count();

        dashboard.AspNetUsers = db.AspNetUsers
            .Where(so => DbFunctions.TruncateTime(so.DateCreated) >= dtStartDate && DbFunctions.TruncateTime(so.DateCreated) <= dtEndtDate)
            .Count();

        dashboard.PassengerTrips = db.Trips
            .Where(trip => DbFunctions.TruncateTime(trip.DateCreated) >= dtStartDate && DbFunctions.TruncateTime(trip.DateCreated) <= dtEndtDate)
            .ToList();
        //dtStartDate = DateTime.Parse(startDate);
        //dtEndtDate = DateTime.Parse(endDate);

        //dashboard.Drivers = db.Drivers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        //dashboard.Passengers = db.Passengers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        //dashboard.Trips = db.Trips.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        //dashboard.AspNetUsers = db.AspNetUsers.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate).Count();
        //dashboard.PassengerTrips = db.Trips.Where(trip => trip.DateCreated >= dtStartDate && trip.DateCreated <= dtEndtDate).ToList();
      }
      return PartialView("_SelectedSOSR", dashboard);
    }
  }

}
