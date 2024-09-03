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

namespace LeaveON.Controllers
{
  [Authorize(Roles = "Admin,Manager,User")]
  public class PassengersController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();

    // GET: Passengers
    public async Task<ActionResult> Index()
    {
      return View(await db.Passengers.Where(x => x.IsDeleted == false).ToListAsync());
    }

    // GET: Passengers/Details/5
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Passenger passenger = await db.Passengers.FindAsync(id);
      if (passenger == null)
      {
        return HttpNotFound();
      }
      return View(passenger);
    }

    // GET: Passengers/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Passengers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,DateCreated,DateModified")] Passenger passenger)
    {
      if (ModelState.IsValid)
      {
        passenger.DateCreated = DateTime.Now;
        passenger.IsDeleted = false;
        db.Passengers.Add(passenger);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(passenger);
    }

    // GET: Passengers/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Passenger passenger = await db.Passengers.FindAsync(id);
      if (passenger == null)
      {
        return HttpNotFound();
      }
      return View(passenger);
    }

    // POST: Passengers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateCreated,DateModified")] Passenger passenger)
    {
      if (ModelState.IsValid)
      {
        passenger.DateModified = DateTime.Now;
        passenger.IsDeleted = false;
        db.Entry(passenger).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(passenger);
    }

    // GET: Passengers/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Passenger passenger = await db.Passengers.FindAsync(id);
      if (passenger == null)
      {
        return HttpNotFound();
      }
      return View(passenger);
    }

    // POST: Passengers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      Passenger passenger = await db.Passengers.FindAsync(id);
      var passengerTrips = await db.Trips.Where(x => x.PassengerId == id).ToListAsync();
      if (passengerTrips?.Count > 0)
      {
        passenger.IsDeleted = true;
        db.Entry(passenger).State = EntityState.Modified;
      }
      else
      {
        db.Passengers.Remove(passenger);
      }
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
