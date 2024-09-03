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
  public class PlacesController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();

    // GET: Locations
    public async Task<ActionResult> Index()
    {
      return View(await db.Places.Where(x=>x.IsDeleted==false).ToListAsync());
    }

    // GET: Locations/Create
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Create()
    {
      return View();
    }

    // POST: Locations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,Remarks,DateCreated,DateUpdated")] Place place)
    {
      place.DateCreated = DateTime.Now;
      if (ModelState.IsValid)
      {
        place.IsDeleted = false;
        db.Places.Add(place);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(place);
    }

    // GET: Locations/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Place place = await db.Places.FindAsync(id);
      if (place == null)
      {
        return HttpNotFound();
      }
      return View(place);
    }

    // POST: Locations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Remarks,DateCreated,DateUpdated")] Place place)
    {
      place.DateUpdated = DateTime.Now;
      if (ModelState.IsValid)
      {
        place.IsDeleted = false;
        db.Entry(place).State = EntityState.Modified;
        db.Entry(place).Property(x => x.DateCreated).IsModified = false;

        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(place);
    }

    // GET: Locations/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Place place = await db.Places.FindAsync(id);
      if (place == null)
      {
        return HttpNotFound();
      }
      return View(place);
    }

    // POST: Locations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      Place place = await db.Places.FindAsync(id);
      var placeTrips = await db.Trips.Where(x => x.PlaceId == id).ToListAsync();
      if (placeTrips?.Count > 0)
      {
        place.IsDeleted = true;
        db.Entry(place).State = EntityState.Modified;
      }
      else
      {
        db.Places.Remove(place);
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
