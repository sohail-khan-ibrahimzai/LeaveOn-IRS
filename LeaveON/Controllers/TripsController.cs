using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryRepo.Models;
using System.Text;
using LeaveON.Dtos;
using InventoryRepo.Enums;

namespace LeaveON.Controllers
{
  [Authorize(Roles = "Admin,Manager,User")]
  public class TripsController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();

    // GET: Trips
    public async Task<ActionResult> Index()
    {
      ///Date Format
      DateTime PKDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));
      var dtStartDate = new DateTime(PKDate.Year, PKDate.Month, 1);
      var dtEndtDate = dtStartDate.AddMonths(1).AddSeconds(-1);

      ViewBag.StartDate = dtStartDate.ToString("dd-MMM-yyyy");
      ViewBag.EndDate = dtEndtDate.ToString("dd-MMM-yyyy");

      ///////Trips/////////
      var trips = db.Trips.Include(t => t.Driver).Include(t => t.Passenger).Where(x => x.DateCreated >= dtStartDate && x.DateCreated <= dtEndtDate && x.IsDeleted == false);
      return View(await trips.ToListAsync());
    }
    public async Task<ActionResult> GetBlackListedLocations()
    {
      var blacListedLocation = db.Trips.Where(x => x.Status == TripStatus.SuccessNotPaid);
      return View(await blacListedLocation.ToListAsync());
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
      var model = new CreateTripDto();
      ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name");
      ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name");
      ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name");
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
            Status = tripDto.Status
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
            Status = tripDto.Status
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
      CreateTripDto createTripDto = new CreateTripDto
      {
        DriverId = trip.DriverId,
        PassengerId = trip.PassengerId,
        PlaceId = trip.PlaceId,
        StartDateTime = trip.StartDateTime,
        EndDateTime = trip.EndDateTime,
        Cost = trip.Cost,
        Status = trip.Status
      };
      if (trip == null)
      {
        return HttpNotFound();
      }

      // Populate the ViewBag with drivers and passengers data, with selected values pre-set
      ViewBag.DriverId = new SelectList(db.Drivers.Where(x => x.IsDeleted == false), "Id", "Name", trip.DriverId);
      ViewBag.PassengerId = new SelectList(db.Passengers.Where(x => x.IsDeleted == false), "Id", "Name", trip.PassengerId);
      ViewBag.PlaceId = new SelectList(db.Places.Where(x => x.IsDeleted == false), "Id", "Name", trip.PlaceId);

      // Return the trip object to the view for editing
      return View(createTripDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,DriverId,PassengerId,PlaceId,Cost,DateCreated,DateModified,StartDateTime,EndDateTime,Status")] Trip trip)
    {
      if (ModelState.IsValid)
      {
        var getTrip = await db.Trips.FirstOrDefaultAsync(x => x.Id == trip.Id);
        if (getTrip == null)
          return HttpNotFound();
        getTrip.DateModified = DateTime.Now;
        getTrip.StartDateTime = trip.StartDateTime;
        getTrip.EndDateTime = trip.EndDateTime;
        getTrip.Cost = trip.Cost;
        getTrip.Status = trip.Status;
        getTrip.DriverId = trip.DriverId;
        getTrip.PassengerId = trip.PassengerId;
        getTrip.PassengerId = trip.PassengerId;
        getTrip.PlaceId = trip.PlaceId;
        db.Entry(getTrip).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
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
