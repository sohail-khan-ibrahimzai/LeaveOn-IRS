using InventoryRepo.Enums;
using InventoryRepo.Models;
using InventoryRepo.ViewModels;
using LeaveON.Dtos;
using LeaveON.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LeaveON.Controllers
{
  [Authorize(Roles = "Admin,Manager,User")]
  public class TripsController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();
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
    // GET: Trips
    public async Task<ActionResult> Index()
    {

      var currentUser = GetCurrentUserInfo();

      // Convert the current UTC time to Greece time (GTB Standard Time)
      DateTime GRDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"));

      // Start date: 08:00 AM of the current day in Greece time
      var dtStartDate = new DateTime(GRDate.Year, GRDate.Month, GRDate.Day, 8, 0, 0);

      // End date: 07:59 AM of the next day in Greece time
      var dtEndDate = dtStartDate.AddDays(1).AddMinutes(-1);

      // Assigning to ViewBag with time in Greece
      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy hh:mm tt");
      ViewBag.EndDate = dtEndDate.ToString("dd-MMM-yyyy hh:mm tt");

      //var currentUser = GetCurrentUserInfo();
      /////Date Format

      ////Old Working
      ////DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      ////var dtStartDate = DateTime.UtcNow;
      ////var dtEndtDate = DateTime.UtcNow;
      ////ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy");
      ////ViewBag.EndDate = dtEndtDate.ToString("dd-MMM-yyyy");

      //DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      //// Start date: 08:00 AM of the current day in PKT
      //var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, PKDate.Day, 8, 0, 0);
      //// End date: 07:59 AM of the next day in PKT
      //var dtEndDate = dtStartDate.AddDays(1).AddMinutes(-1);

      //// Assigning to ViewBag
      //ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy hh:mm tt");
      //ViewBag.EndDate = dtEndDate.ToString("dd-MMM-yyyy hh:mm tt");



      //DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      //var dtStartDate = DateTime.UtcNow;
      //var dtEndtDate = dtStartDate.AddHours(15).AddMinutes(3).AddSeconds(0);
      //Old working
      //var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, 1);
      //var dtEndtDate = dtStartDate.AddMonths(1).AddSeconds(-1);


      ///////Trips/////////
      if (User.IsInRole("Admin"))
      {
        //var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.IsDeleted == false);
        //return View(await trips.ToListAsync());

        //var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.IsDeleted == false);
        var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false);
        return View(await trips.ToListAsync());
      }
      else
      {
        //var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false);
        //return View(await trips.ToListAsync());

        //var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false);
        var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.CreatedBy == currentUser.UserId && x.IsDeleted == false);
        return View(await trips.ToListAsync());
      }

    }
    public async Task<ActionResult> GetBlackListedLocations()
    {
      var currentUser = GetCurrentUserInfo();

      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      // Start date: 08:00 AM of the current day in PKT
      var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, PKDate.Day, 8, 0, 0);
      // End date: 07:59 AM of the next day in PKT
      var dtEndDate = dtStartDate.AddDays(1).AddMinutes(-1);

      // Assigning to ViewBag
      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy hh:mm tt");
      ViewBag.EndDate = dtEndDate.ToString("dd-MMM-yyyy hh:mm tt");
      if (User.IsInRole("Admin"))
      {
        //var aa = await db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.IsBlackListed == true).ToListAsync();
        //var blacListedLocation = db.Trips.Where(x => x.Status == TripStatus.SuccessNotPaid);
        //var blacListedLocation = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.IsBlackListed == true);
        //var blacListedLocation = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.IsBlackListed == true && x.Place.IsBlackListed == true);
        var blacListedLocation = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.Place.IsBlackListed == true);
        return View(await blacListedLocation.ToListAsync());
      }
      else
      {
        var blacListedLocation = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.Place.IsBlackListed == true && x.CreatedBy == currentUser.UserId);
        //var blacListedLocation = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.IsBlackListed == true && x.Place.IsBlackListed == true && x.CreatedBy == currentUser.UserId);
        return View(await blacListedLocation.ToListAsync());
      }
    }
    public async Task<ActionResult> WeeklyReports()
    {
      var currentUser = GetCurrentUserInfo();
      // Convert the current UTC time to Greece time (GTB Standard Time)
      DateTime GRDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"));

      // Calculate the most recent Sunday (start of the week)
      int daysToLastSunday = ((int)GRDate.DayOfWeek == 0) ? 0 : 7 - (int)GRDate.DayOfWeek;  // Calculate number of days to the last Sunday (0 = Sunday)
      DateTime dtStartDate = new DateTime(GRDate.Year, GRDate.Month, GRDate.Day, 8, 0, 0).AddDays(-daysToLastSunday);  // Sunday 08:00 AM

      // Calculate next Sunday 07:59 AM as the end of the week
      DateTime dtEndDate = dtStartDate.AddDays(7).AddMinutes(-1);  // Saturday 07:59 AM of the next week

      // Assigning to ViewBag
      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy hh:mm tt");
      ViewBag.EndDate = dtEndDate.ToString("dd-MMM-yyyy hh:mm tt");
      //ViewBag.TotalCostFromViewBag = 5000.75;
      // Filter the data based on the role
      if (User.IsInRole("Admin"))
      {
        //var weeklyTripsQuery = db.Trips
        //.Where(x => x.DateCreated >= dtStartDate
        //             && x.DateCreated <= dtEndDate
        //             && x.IsDeleted == false);

        //var weeklyTrips = await weeklyTripsQuery
        //    .Select(trip => new
        //    {
        //      trip,
        //      Driver = db.Drivers.FirstOrDefault(d => d.Id == trip.DriverId),
        //      Manager = db.AspNetUsers.Include(x=>x.Passengers).FirstOrDefault(d => d.Id == trip.CreatedBy),
        //    })
        //    .ToListAsync();

        //// Create a list of TripViewModel
        //var result = weeklyTrips.Select(t => new TripListDto
        //{
        //  Trip = t.trip,
        //  Driver = t.Driver,
        //  //Manager = t.Manager,
        //  TripCost = CalculateTripCost(t.trip),
        //  ManagerComission = CalculateManagerTripComission(t.trip)
        //}).ToList();

        //ViewBag.TotalCostFromViewBag = result.Sum(x => x.TripCost);

        //return View(result);
        var weeklyTripsQuery = db.Trips
    .Where(x => x.DateCreated >= dtStartDate
                 && x.DateCreated <= dtEndDate
                 && x.IsDeleted == false);

        var weeklyTrips = await weeklyTripsQuery
            .Select(trip => new
            {
              trip,
              Driver = db.Drivers.FirstOrDefault(d => d.Id == trip.DriverId),
              Manager = db.AspNetUsers.FirstOrDefault(u => u.Id == trip.CreatedBy), // Manager from AspNetUsers
              Passenger = db.Passengers.FirstOrDefault(p => p.Id == trip.PassengerId), // Passenger details
              Place = db.Places.FirstOrDefault(pl => pl.Id == trip.PlaceId) // Place details (adjust the FK name if necessary)
            })
            .ToListAsync();

        // Create a list of TripListDto
        var result = weeklyTrips.Select(t => new TripListDto
        {
          Trip = t.trip,
          Driver = t.Driver,
          Manager = t.Manager, // This is the Manager's name from AspNetUser
          Passenger = t.Passenger, // Passenger details
          Place = t.Place, // Place details
          TripCost = CalculateTripCost(t.trip), // Call your existing method to calculate cost
          //ManagerComission = CalculateManagerTripComission(t.trip) // Call your method for manager commission
          ManagerComission = t.Passenger.ManagerComission ?? 0 // Call your method for manager commission
        }).ToList();

        ViewBag.TotalCostFromViewBag = result.Sum(x => x.TripCost);

        return View(result);  // Pass the list of TripListDto to the view

        //var weeklyTrips = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false);
        //ViewBag.TotalCostFromViewBag = weeklyTrips.Sum(x => x.Cost);
        //return View(await weeklyTrips.ToListAsync());
      }
      else
      {
        var weeklyTripsQuery = db.Trips
   .Where(x => x.DateCreated >= dtStartDate
                && x.DateCreated <= dtEndDate
                && x.IsDeleted == false && x.CreatedBy == currentUser.UserId);

        var weeklyTrips = await weeklyTripsQuery
            .Select(trip => new
            {
              trip,
              Driver = db.Drivers.FirstOrDefault(d => d.Id == trip.DriverId),
              Manager = db.AspNetUsers.FirstOrDefault(u => u.Id == trip.CreatedBy), // Manager from AspNetUsers
              Passenger = db.Passengers.FirstOrDefault(p => p.Id == trip.PassengerId), // Passenger details
              Place = db.Places.FirstOrDefault(pl => pl.Id == trip.PlaceId) // Place details (adjust the FK name if necessary)
            })
            .ToListAsync();

        // Create a list of TripListDto
        var result = weeklyTrips.Select(t => new TripListDto
        {
          Trip = t.trip,
          Driver = t.Driver,
          Manager = t.Manager, // This is the Manager's name from AspNetUser
          Passenger = t.Passenger, // Passenger details
          Place = t.Place, // Place details
          TripCost = CalculateTripCost(t.trip), // Call your existing method to calculate cost
          ManagerComission = t.Passenger.ManagerComission ?? 0 // Call your method for manager commission
          //ManagerComission = CalculateManagerTripComission(t.trip) // Call your method for manager commission
        }).ToList();

        ViewBag.TotalCostFromViewBag = result.Sum(x => x.TripCost);

        return View(result);  // Pass the list of TripListDto to the view
        //var weeklyTrips = db.Trips.Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndDate && x.IsDeleted == false && x.CreatedBy == currentUser.UserId);
        //ViewBag.TotalCostFromViewBag = weeklyTrips.Sum(x => x.Cost);
        //return View(await weeklyTrips.ToListAsync());
      }
    }
    private decimal CalculateTripCost(Trip trip)
    {
      var driver = db.Drivers.FirstOrDefault(d => d.Id == trip.DriverId);
      decimal totalHours = trip.TotalHours ?? 0;
      if (driver == null) return 0; // Handle case where driver is not found

      if (driver.IsFiveHoursPlusEnabled == true)
      {
        // Default to 0 if TotalHours is null

        if (totalHours >= 5.30m && totalHours < 8.00m) // Compare as decimal
        {
          return 100;
        }
        else if (totalHours >= 9.00m && totalHours < 12.00m)
        {
          return 150;
        }
        else if (totalHours >= 13.00m && totalHours < 24.00m)
        {
          return 200;
        }
      }

      // Use null-coalescing operator to provide a default value if Cost is null
      decimal cost = trip.Cost ?? 0; // Default to 0 if Cost is null
      return cost * totalHours; // Multiply the cost with total hours
    }
    private decimal CalculateManagerTripComission(Trip trip)
    {
      var passenger = db.Passengers.FirstOrDefault(d => d.Id == trip.PassengerId);
      var driver = db.Drivers.FirstOrDefault(d => d.Id == trip.DriverId);
      decimal managerComission = trip.Passenger.ManagerComission ?? 0;
      if (passenger == null) return 0; // Handle case where driver is not found
      decimal cost = trip.Cost ?? 0; // Default to 0 if Cost is null
      return cost * managerComission / 100.00m; // Multiply the cost with total hours
    }
    public ActionResult SearchData(string startDate, string endDate)
    {
      var currentUser = GetCurrentUserInfo();
      DateTime dtStartDate;
      DateTime dtEndtDate;
      IQueryable<Trip> selectedTrips = null;
      if (endDate != string.Empty)
      {
        dtEndtDate = DateTime.Parse(endDate);
        //dtEndtDate = dtEndtDate.AddDays(1);
        endDate = dtEndtDate.ToString();

      }

      if (startDate != string.Empty && endDate != string.Empty)
      {
        dtStartDate = DateTime.Parse(startDate);
        dtEndtDate = DateTime.Parse(endDate);
        if (User.IsInRole("Admin"))
        {
          selectedTrips = db.Trips.Where(trips => trips.DateCreated >= dtStartDate && trips.DateCreated <= dtEndtDate && trips.IsDeleted == false);
        }
        else
        {
          selectedTrips = db.Trips.Where(trips => trips.DateCreated >= dtStartDate && trips.DateCreated <= dtEndtDate && trips.IsDeleted == false && trips.CreatedBy == currentUser.UserId);
        }

      }

      if (startDate == string.Empty && endDate == string.Empty)
      {

        dtStartDate = DateTime.Parse("1-1-1800");
        //dtEndtDate = DateTime.Today.AddDays(1);
        selectedTrips = db.Trips;//.Where(so => so.CustomerId == intCustId && so.Date >= dtStartDate && so.Date <= dtEndtDate && so.SaleReturn==false);

      }
      return PartialView("_SelectedTrips", selectedTrips.OrderByDescending(i => i.DateCreated).ToList());
    }
    // GET: Trips/Details/5
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Trip trip = await db.Trips.FindAsync(id);
      if (trip == null)
      {
        return HttpNotFound();
      }
      return View(trip);
    }

    // GET: Trips/Create
    public ActionResult Create()
    {
      //var model = new Trip();
      var currentUser = GetCurrentUserInfo();
      var model = new CreateTripDto();

      if (User.IsInRole("Admin"))
      {
        var passengers = db.Passengers
                  .Where(x => x.IsDeleted == false)
                  .Select(x => new
                  {
                    Id = x.Id,
                    Name = x.Name,
                    ManagerDeal = x.ManagerDeal,
                    ManagerComission = x.ManagerComission
                  }).ToList();
        var places = db.Places
                 .Where(x => x.IsDeleted == false)
                 .Select(x => new
                 {
                   Id = x.Id,
                   Name = x.Name,
                   IsBlacklistedPlace = x.IsBlackListed,
                   Comment = x.Remarks,
                 }).ToList();

        var drivers = db.Drivers
                 .Where(x => x.IsDeleted == false)
                 .Select(x => new
                 {
                   Id = x.Id,
                   Name = x.Name,
                   IsFiveHoursPlusEnabled = x.IsFiveHoursPlusEnabled,
                   Comment = x.Remarks,
                 }).ToList();

        ViewBag.DriverId = drivers;
        ViewBag.PassengerId = passengers;
        ViewBag.PlaceId = places;
        //ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name");
        //ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name");
        //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name");
      }
      else
      {
        var passengers = db.Passengers
                  .Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false)
                  .Select(x => new
                  {
                    Id = x.Id,
                    Name = x.Name,
                    ManagerDeal = x.ManagerDeal,
                    ManagerComission = x.ManagerComission
                  }).ToList();
        var places = db.Places
                  .Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false)
                  .Select(x => new
                  {
                    Id = x.Id,
                    Name = x.Name,
                    IsBlacklistedPlace = x.IsBlackListed,
                    Comment = x.Remarks,
                  }).ToList();

        var drivers = db.Drivers
               .Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false)
               .Select(x => new
               {
                 Id = x.Id,
                 Name = x.Name,
                 IsFiveHoursPlusEnabled = x.IsFiveHoursPlusEnabled,
                 Comment = x.Remarks,
               }).ToList();

        ViewBag.DriverId = drivers;
        ViewBag.PassengerId = passengers;
        ViewBag.PlaceId = places;
        //ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
        //ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
        //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
      }

      //DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      //var fromDateTime = DateTime.UtcNow;
      //var toDateTime = fromDateTime.AddHours(1).AddMinutes(0).AddSeconds(0);
      //ViewBag.FromDateTime = fromDateTime.ToString("dd-MMM-yyyy HH-mm-ss tt");
      //ViewBag.ToDateTime = toDateTime.ToString("dd-MMM-yyyy HH-mm-ss tt");
      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      var fromDateTime = DateTime.UtcNow;
      var toDateTime = fromDateTime.AddHours(1).AddMinutes(0).AddSeconds(0);

      model.StartDateTime = fromDateTime;
      model.EndDateTime = toDateTime;


      ViewBag.FromDateTime = fromDateTime.ToString("dd-MMM-yyyy hh:mm:ss tt");
      ViewBag.ToDateTime = toDateTime.ToString("dd-MMM-yyyy hh:mm:ss tt");

      return View(model);
    }

    // POST: Trips/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    //public async Task<ActionResult> Create([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,Status")] Trip trip)
    public async Task<ActionResult> Create([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,TotalHours,Status,PlaceName,Remarks")] CreateTripDto addTripDto)
    {
      var currentUser = GetCurrentUserInfo();

      // Get the current UTC time
      DateTime utcNow = DateTime.UtcNow;
      // Retrieve the default time zone (Greece)
      TimeZoneInfo greeceTimeZone = GreeceTimeZoneHelper.GetDefaultGreeceTimeZone();
      // Convert UTC to Greece local time
      DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, greeceTimeZone);

      try
      {
        if (addTripDto.Id == null || addTripDto.Id == 0)
        {
          if (addTripDto.PlaceName != null && addTripDto.PlaceId == null)
          {
            Place addPlace = new Place();
            var exisitingPlace = await db.Places.FirstOrDefaultAsync(x => x.Name == addTripDto.PlaceName);
            if (exisitingPlace != null)
            {
              ModelState.AddModelError("PlaceName", "Place already exists.");
              ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.DriverId);
              ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.PassengerId);
              ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.PlaceId);
              return View(addTripDto);

            }
            else
            {
              addPlace = await AddNewPlace(addTripDto);
              addTripDto.PlaceId = addPlace.Id;

              var addTrip = new Trip
              {

                //DateCreated = DateTime.UtcNow,
                DateCreated = localTime,
                DriverId = addTripDto.DriverId,
                PassengerId = addTripDto.PassengerId,
                PlaceId = addTripDto.PlaceId,
                StartDateTime = addTripDto.StartDateTime,
                EndDateTime = addTripDto.EndDateTime,
                TotalHours = addTripDto.TotalHours,
                Cost = addTripDto.Cost,
                Remarks = addTripDto.Remarks,
                IsDeleted = false,
                Status = addTripDto.Status,
                CreatedBy = currentUser.UserId
              };
              db.Trips.Add(addTrip);
            }
          }
          else
          {
            var addTrip = new Trip
            {
              //DateCreated = DateTime.UtcNow,
              DateCreated = localTime,
              DriverId = addTripDto.DriverId,
              PassengerId = addTripDto.PassengerId,
              PlaceId = addTripDto.PlaceId,
              StartDateTime = addTripDto.StartDateTime,
              EndDateTime = addTripDto.EndDateTime,
              TotalHours = addTripDto.TotalHours,
              Cost = addTripDto.Cost,
              Remarks = addTripDto.Remarks,
              IsDeleted = false,
              Status = addTripDto.Status,
              CreatedBy = currentUser.UserId
            };
            db.Trips.Add(addTrip);
          }
          await db.SaveChangesAsync();
          return RedirectToAction("Index");
        }

        ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.DriverId);
        ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.PassengerId);
        ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", addTripDto.PlaceId);
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("Error", ex);
      }
      return View(addTripDto);
    }

    private async Task<Place> AddNewPlace(CreateTripDto tripDto)
    {
      var currentUser = GetCurrentUserInfo();
      var addPlace = new Place
      {
        DateCreated = DateTime.UtcNow,
        CreatedBy = currentUser.UserId,
        Name = tripDto.PlaceName,
        Remarks = "",
        IsDeleted = false
      };
      db.Places.Add(addPlace);
      await db.SaveChangesAsync();
      return addPlace;
    }

    //public async Task<ActionResult> Edit(int? id)
    //{
    //  if (id == null)
    //  {
    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //  }

    //  // Fetch the trip using the provided id
    //  Trip trip = await db.Trips.FindAsync(id);

    //  if (trip == null)
    //  {
    //    return HttpNotFound();
    //  }

    //  // Create SelectList for dropdowns with the current selected value
    //  ViewBag.DriverId = new SelectList(db.Drivers.ToList(), "Id", "Name", trip.DriverId);
    //  ViewBag.PassengerId = new SelectList(db.Passengers.ToList(), "Id", "Name", trip.PassengerId);

    //  return View(trip);
    //}
    public async Task<ActionResult> Edit(int? id)
    {
      var currentUser = GetCurrentUserInfo();
      //if (id == null)
      //{
      //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      //}

      // Fetch the trip details from the database
      Trip trip = await db.Trips.FindAsync(id);
      //if (trip == null)
      //{
      //  ModelState.AddModelError("Name", "Trips not found.");
      //  //return HttpNotFound();
      //}
      if (trip == null)
      {
        ModelState.AddModelError("", "Trip not found."); // Add error message without key
        return View(); // Return the view with the error
      }
      CreateTripDto editTripDto = new CreateTripDto
      {
        DriverId = trip.DriverId,
        PassengerId = trip.PassengerId,
        PlaceId = trip.PlaceId,
        StartDateTime = trip.StartDateTime,
        EndDateTime = trip.EndDateTime,
        TotalHours = trip.TotalHours,
        Cost = trip.Cost,
        //IsBlackListed = trip.IsBlackListed,
        //Floor = trip.Floor,
        //Bell = trip.Bell,
        IsBlackListed = trip.Place.IsBlackListed,
        Floor = trip.Place.Floor,
        Bell = trip.Place.Bell,
        Remarks = trip.Remarks,
        Status = trip.Status
      };

      // Populate the ViewBag with drivers and passengers data, with selected values pre-set
      var driver = await db.Drivers.FirstOrDefaultAsync(d => d.Id == trip.DriverId);
      var passenger = await db.Passengers.FirstOrDefaultAsync(p => p.Id == trip.PassengerId);
      var place = await db.Places.FirstOrDefaultAsync(pl => pl.Id == trip.PlaceId);

      ViewBag.DriverName = driver?.Name;
      ViewBag.PassengerName = passenger?.Name;
      ViewBag.PlaceName = place?.Name;
      ViewBag.DriversId = driver?.Id;  // Add PassengerId to ViewBag
      ViewBag.PassengersId = passenger?.Id;  // Add PassengerId to ViewBag
      ViewBag.PlacesId = place?.Id;

      if (User.IsInRole("Admin"))
      {
        var passengers = db.Passengers
                  .Where(x => x.IsDeleted == false)
                  .Select(x => new
                  {
                    Id = x.Id,
                    Name = x.Name,
                    ManagerDeal = x.ManagerDeal,
                    ManagerComission = x.ManagerComission
                  }).ToList();
        var places = db.Places
                 .Where(x => x.IsDeleted == false)
                 .Select(x => new
                 {
                   Id = x.Id,
                   Name = x.Name,
                   IsBlacklistedPlace = x.IsBlackListed,
                   Comment = x.Remarks,
                 }).ToList();
        ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name");
        ViewBag.PassengerId = passengers;
        //ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name");
        //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name");
        ViewBag.PlaceId = places;
      }
      else
      {
        var passengers = db.Passengers
                  .Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false)
                  .Select(x => new
                  {
                    Id = x.Id,
                    Name = x.Name,
                    ManagerDeal = x.ManagerDeal,
                    ManagerComission = x.ManagerComission
                  }).ToList();
        var places = db.Places
                 .Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false)
                 .Select(x => new
                 {
                   Id = x.Id,
                   Name = x.Name,
                   IsBlacklistedPlace = x.IsBlackListed,
                   Comment = x.Remarks,
                 }).ToList();
        ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name", editTripDto.DriverId);
        ViewBag.PassengerId = passengers;
        //ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
        //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name", editTripDto.PlaceId);
        ViewBag.PlaceId = places;
      }
      //ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.DriverId);
      //ViewBag.PassengerId = passengers;
      ////ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.PassengerId);
      //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.PlaceId);

      // Return the trip object to the view for editing
      return View(editTripDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,TotalHours,Status,PlaceName,Remarks,IsBlackListed,Floor,Bell")] CreateTripDto updateTripDto)
    {
      var currentUser = GetCurrentUserInfo();
      //if (ModelState.IsValid)
      //{
      var trip = await db.Trips.FirstOrDefaultAsync(x => x.Id == updateTripDto.Id);
      var exisitingPlace = await db.Places.FirstOrDefaultAsync(x => x.Name == updateTripDto.PlaceName);
      var driver = await db.Drivers.FirstOrDefaultAsync(d => d.Id == trip.DriverId);
      var passenger = await db.Passengers.FirstOrDefaultAsync(p => p.Id == trip.PassengerId);
      var place = await db.Places.FirstOrDefaultAsync(pl => pl.Id == trip.PlaceId);
      if (updateTripDto.IsBlackListed == true && trip.DateBlackList == null && place.IsBlackListed == false && place.BlacklistedDate == null)
      {
        trip.IsBlackListed = updateTripDto.IsBlackListed ?? false;
        trip.DateBlackList = DateTime.UtcNow;
        place.IsBlackListed = updateTripDto.IsBlackListed ?? false;
        place.BlacklistedDate = DateTime.UtcNow;
        place.Floor = updateTripDto.Floor;
        place.Bell = updateTripDto.Bell;
      }
      if (updateTripDto.PlaceName != null && updateTripDto.PlaceId == null)
      {

        if (exisitingPlace != null)
        {
          ModelState.AddModelError("PlaceName", "Place already exists.");
          ViewBag.DriverName = driver?.Name;
          ViewBag.PassengerName = passenger?.Name;
          ViewBag.PlaceName = place?.Name;
          ViewBag.DriversId = driver?.Id;  // Add PassengerId to ViewBag
          ViewBag.PassengersId = passenger?.Id;  // Add PassengerId to ViewBag
          ViewBag.PlacesId = place?.Id;
          ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", updateTripDto.DriverId);
          ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", updateTripDto.PassengerId);
          ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", updateTripDto.PlaceId);
          return View(updateTripDto);
        }
        else
        {
          Place addPlace = await AddNewPlace(updateTripDto);
          trip.PlaceId = addPlace.Id;
        }
      }
      else
      {

        if (trip == null)
          return HttpNotFound();
        trip.DriverId = updateTripDto.DriverId;
        trip.PassengerId = updateTripDto.PassengerId;
        trip.PassengerId = updateTripDto.PassengerId;
        trip.PlaceId = updateTripDto.PlaceId;
        trip.Cost = updateTripDto.Cost;
        trip.DateModified = DateTime.Now;
        trip.StartDateTime = updateTripDto.StartDateTime;
        trip.EndDateTime = updateTripDto.EndDateTime;
        trip.TotalHours = updateTripDto.TotalHours;
        trip.Status = updateTripDto.Status;
        trip.Remarks = updateTripDto.Remarks;
        //trip.IsBlackListed = updateTripDto.IsBlackListed ?? false;
        //trip.Floor = updateTripDto.Floor;
        //trip.Bell = updateTripDto.Bell;
        trip.UpdatedBy = currentUser.UserId;
        db.Entry(trip).State = EntityState.Modified;
        db.Entry(place).State = EntityState.Modified;
      }
      await db.SaveChangesAsync();
      //}
      // Repopulate dropdowns if the model state is invalid
      //ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", trip.DriverId);
      //ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", trip.PassengerId);
      //ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", trip.PlaceId);

      return RedirectToAction("Index");
    }

    // GET: Trips/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Trip trip = await db.Trips.FindAsync(id);
      if (trip == null)
      {
        return HttpNotFound();
      }
      return View(trip);
    }

    // POST: Trips/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      // Fetch the trip details from the database
      Trip trip = await db.Trips.FindAsync(id);

      if (trip == null)
      {
        return HttpNotFound();
      }
      trip.IsDeleted = true;
      db.Entry(trip).State = EntityState.Modified;
      // Fetch the associated Driver and Passenger
      //Driver driver = await db.Drivers.FindAsync(trip.DriverId);
      //Passenger passenger = await db.Passengers.FindAsync(trip.PassengerId);

      //// Remove the Trip record
      //db.Trips.Remove(trip);

      //// Check if driver exists and remove it
      //if (driver != null)
      //{
      //  db.Drivers.Remove(driver);
      //}

      //// Check if passenger exists and remove it
      //if (passenger != null)
      //{
      //  db.Passengers.Remove(passenger);
      //}

      // Save all changes to the database
      await db.SaveChangesAsync();

      // Redirect back to the index action
      return RedirectToAction("Index");
    }
    [HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> Create([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,Status")] Trip trip)
    public async Task<ActionResult> UpdateStatus(int id, string status)
    {
      var trip = await db.Trips.FirstOrDefaultAsync(x => x.Id == id);
      if (trip == null)
        return HttpNotFound();
      trip.Status = status;
      db.Entry(trip).State = EntityState.Modified;
      await db.SaveChangesAsync();
      return RedirectToAction("Index");
    }
    public JsonResult SearchDrivers(string searchTerm)
    {
      var drivers = db.Drivers
                      .Where(x => x.IsDeleted == false && x.Name.Contains(searchTerm))
                      .Select(x => new { id = x.Id, text = x.Name })
                      .ToList();
      return Json(drivers, JsonRequestBehavior.AllowGet);
    }
    public JsonResult SearchPassengers(string searchTerm)
    {
      var drivers = db.Passengers
                      .Where(x => x.IsDeleted == false && x.Name.Contains(searchTerm))
                      .Select(x => new { id = x.Id, text = x.Name })
                      .ToList();
      return Json(drivers, JsonRequestBehavior.AllowGet);
    }
    public JsonResult SearchPlaces(string searchTerm)
    {
      var drivers = db.Places
                      .Where(x => x.IsDeleted == false && x.Name.Contains(searchTerm))
                      .Select(x => new { id = x.Id, text = x.Name })
                      .ToList();
      return Json(drivers, JsonRequestBehavior.AllowGet);
    }
    public JsonResult GetAvailableDriver(string startTripDateTime, string endTripDateTime)
    {
      //var currentUser = GetCurrentUserInfo();
      //DateTime dd = Convert.ToDateTime(startTripDateTime);
      //DateTime ddd = Convert.ToDateTime(endTripDateTime);
      //var availableDrivers = db.Trips
      //                .Where(x => x.IsDeleted == false && x.StartDateTime != dd && x.EndDateTime != ddd && x.CreatedBy == currentUser.UserId)
      //                   .Select(y => new { Id = y.Driver.Id, Name = y.Driver.Name })
      //                      .ToList()
      //                .ToList();

      try
      {
        var currentUser = GetCurrentUserInfo();

        // Check if date parameters are provided and valid
        if (!string.IsNullOrEmpty(startTripDateTime) && !string.IsNullOrEmpty(endTripDateTime) &&
            DateTime.TryParse(startTripDateTime, out DateTime startDate) &&
            DateTime.TryParse(endTripDateTime, out DateTime endDate))
        {
          // Find drivers who are available (i.e., have no overlapping trips)
          var availableDrivers = db.Drivers
              .Where(driver => !db.Trips
                  .Any(trip => trip.DriverId == driver.Id
                      && trip.IsDeleted == false
                      && trip.CreatedBy == currentUser.UserId
                      // Overlap condition: the trip cannot overlap the provided time window
                      //&& ((trip.StartDateTime <= endDate && trip.EndDateTime >= startDate))))
                      && ((startDate >= trip.StartDateTime))))
              .Select(driver => new
              {
                Id = driver.Id,
                Name = driver.Name,
                IsFiveHoursPlusEnabled = driver.IsFiveHoursPlusEnabled,
                Comment = driver.Remarks,
              })
              .ToList();

          // Return the available drivers as JSON
          return Json(new { success = true, data = availableDrivers }, JsonRequestBehavior.AllowGet);
        }
        else
        {
          // If dates are not provided or invalid, return all drivers
          var allDrivers = db.Drivers
              .Select(driver => new
              {
                Id = driver.Id,
                Name = driver.Name
              })
              .ToList();

          // Return all drivers as JSON
          return Json(new { success = true, data = allDrivers }, JsonRequestBehavior.AllowGet);
        }
      }
      catch (Exception ex)
      {
        // Log the exception (you can use any logging framework here)
        // Example: Logger.Error(ex, "Error in GetAvailableDriver method");
        return Json(new { success = false, message = "An error occurred while retrieving available drivers. Please try again later." }, JsonRequestBehavior.AllowGet);
      }

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
