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
  public class LocationsController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();

    // GET: Locations
    public async Task<ActionResult> Index()
    {
      return View(await db.Locations.Where(x=>x.IsDeleted==false).ToListAsync());
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
    public async Task<ActionResult> Create([Bind(Include = "Id,LocationId,LocationName,Remarks,DateCreated,DateModified")] Location location)
    {
      location.Id = Guid.NewGuid().ToString();
      location.DateCreated = DateTime.Now;
      if (ModelState.IsValid)
      {
        location.IsDeleted = false;
        db.Locations.Add(location);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(location);
    }

    // GET: Locations/Edit/5
    public async Task<ActionResult> Edit(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Location location = await db.Locations.FindAsync(id);
      if (location == null)
      {
        return HttpNotFound();
      }
      return View(location);
    }

    // POST: Locations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Edit([Bind(Include = "Id,LocationId,LocationName,Remarks,DateCreated,DateModified")] Location location)
    {
      location.DateModified = DateTime.Now;
      if (ModelState.IsValid)
      {
        location.IsDeleted = false;
        db.Entry(location).State = EntityState.Modified;
        db.Entry(location).Property(x => x.DateCreated).IsModified = false;

        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(location);
    }

    // GET: Locations/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Location location = await db.Locations.FindAsync(id);
      if (location == null)
      {
        return HttpNotFound();
      }
      return View(location);
    }

    // POST: Locations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteConfirmed(string id)
    {
      Location location = await db.Locations.FindAsync(id);
      db.Locations.Remove(location);
      location.IsDeleted = true;
      db.Entry(location).State = EntityState.Modified;
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
