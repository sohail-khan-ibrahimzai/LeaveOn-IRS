using InventoryRepo.Enums;
using InventoryRepo.Models;
using InventoryRepo.ViewModels;
using LeaveON.Dtos;
using Microsoft.AspNet.Identity;
using System;
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
      ///Date Format
      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      var dtStartDate = DateTime.UtcNow;
      var dtEndtDate = dtStartDate.AddHours(15).AddMinutes(3).AddSeconds(0);
      //Old working
      //var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, 1);
      //var dtEndtDate = dtStartDate.AddMonths(1).AddSeconds(-1);

      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy");
      ViewBag.EndDate = dtEndtDate.ToString("dd-MMM-yyyy");

      ///////Trips/////////
      if (User.IsInRole("Admin"))
      {
        var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndtDate && x.IsDeleted == false);
        return View(await trips.ToListAsync());
      }
      else
      {
        var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndtDate && x.CreatedBy == currentUser.UserId && x.IsDeleted == false);
        return View(await trips.ToListAsync());
      }

    }
    public async Task<ActionResult> GetBlackListedLocations()
    {
      var currentUser = GetCurrentUserInfo();
      if (User.IsInRole("Admin"))
      {
        var blacListedLocation = db.Trips.Where(x => x.Status == TripStatus.SuccessNotPaid);
        return View(await blacListedLocation.ToListAsync());
      }
      else
      {
        var blacListedLocation = db.Trips.Where(x => x.Status == TripStatus.SuccessNotPaid && x.CreatedBy == currentUser.UserId);
        return View(await blacListedLocation.ToListAsync());
      }
    }
    public ActionResult SearchData(string startDate, string endDate)
    {
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

        selectedTrips = db.Trips.Where(so => so.DateCreated >= dtStartDate && so.DateCreated <= dtEndtDate);

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
      ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
      ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");
      ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false), "Id", "Name");

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
    public async Task<ActionResult> Create([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,Status,PlaceName,Remarks")] CreateTripDto tripDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (tripDto.Id == null || tripDto.Id == 0)
      {
        if (tripDto.PlaceName != null)
        {
          Place addPlace = await AddNewPlace(tripDto);
          var addTrip = new Trip
          {

            DateCreated = DateTime.Now,
            DriverId = tripDto.DriverId,
            PassengerId = tripDto.PassengerId,
            PlaceId = addPlace.Id,
            StartDateTime = tripDto.StartDateTime,
            EndDateTime = tripDto.EndDateTime,
            Cost = tripDto.Cost,
            Remarks = tripDto.Remarks,
            IsDeleted = false,
            Status = tripDto.Status,
            CreatedBy = currentUser.UserId
          };
          db.Trips.Add(addTrip);
        }
        else
        {
          var addTrip = new Trip
          {
            DateCreated = DateTime.Now,
            DriverId = tripDto.DriverId,
            PassengerId = tripDto.PassengerId,
            PlaceId = tripDto.PlaceId,
            StartDateTime = tripDto.StartDateTime,
            EndDateTime = tripDto.EndDateTime,
            Cost = tripDto.Cost,
            Remarks = tripDto.Remarks,
            IsDeleted = false,
            Status = tripDto.Status,
            CreatedBy = currentUser.UserId
          };
          db.Trips.Add(addTrip);
        }
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", tripDto.DriverId);
      ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", tripDto.PassengerId);
      ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", tripDto.PlaceId);
      return View(tripDto);
    }

    private async Task<Place> AddNewPlace(CreateTripDto tripDto)
    {
      var addPlace = new Place
      {
        DateCreated = DateTime.Now,
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
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      // Fetch the trip details from the database
      Trip trip = await db.Trips.FindAsync(id);
      CreateTripDto editTripDto = new CreateTripDto
      {
        DriverId = trip.DriverId,
        PassengerId = trip.PassengerId,
        PlaceId = trip.PlaceId,
        StartDateTime = trip.StartDateTime,
        EndDateTime = trip.EndDateTime,
        Cost = trip.Cost,
        Remarks = trip.Remarks,
        Status = trip.Status
      };
      if (trip == null)
      {
        return HttpNotFound();
      }

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

      ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.DriverId);
      ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.PassengerId);
      ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", editTripDto.PlaceId);

      // Return the trip object to the view for editing
      return View(editTripDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,Status,Remarks")] CreateTripDto updateTripDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var trip = await db.Trips.FirstOrDefaultAsync(x => x.Id == updateTripDto.Id);
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
        trip.Status = updateTripDto.Status;
        trip.Remarks = updateTripDto.Remarks;
        trip.UpdatedBy = currentUser.UserId;
        db.Entry(trip).State = EntityState.Modified;
        await db.SaveChangesAsync();
      }
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
      var currentUser = GetCurrentUserInfo();
      DateTime dd = Convert.ToDateTime(startTripDateTime);
      DateTime ddd = Convert.ToDateTime(endTripDateTime);

      // Find drivers who are available (i.e., have no overlapping trips)
      // Find drivers who are available at the specified time window
      var availableDrivers = db.Drivers
          .Where(driver => !db.Trips
              .Any(trip => trip.DriverId == driver.Id
                  && trip.IsDeleted == false
                  && trip.CreatedBy == currentUser.UserId
                  // Overlap condition: the trip cannot overlap the provided time window
                  && ((trip.StartDateTime <= ddd && trip.EndDateTime >= dd))))
          .Select(driver => new {
            Id = driver.Id,
            Name = driver.Name
          })
          .ToList();
      return Json(availableDrivers, JsonRequestBehavior.AllowGet);
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
