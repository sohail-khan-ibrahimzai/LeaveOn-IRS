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
  public class DriversController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();

    // GET: Drivers
    public async Task<ActionResult> Index()
    {
      return View(await db.Drivers.Where(x => x.IsDeleted == false).ToListAsync());
    }

    // GET: Drivers/Details/5
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Driver driver = await db.Drivers.FindAsync(id);
      if (driver == null)
      {
        return HttpNotFound();
      }
      return View(driver);
    }

    // GET: Drivers/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Drivers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,DateCreated,DateModified")] Driver driver)
    {
      if (ModelState.IsValid)
      {
        driver.DateCreated = DateTime.Now;
        driver.IsDeleted = false;
        db.Drivers.Add(driver);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      return View(driver);
    }

    // GET: Drivers/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Driver driver = await db.Drivers.FindAsync(id);
      if (driver == null)
      {
        return HttpNotFound();
      }
      return View(driver);
    }

    // POST: Drivers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateCreated,DateModified")] Driver driver)
    {
      if (ModelState.IsValid)
      {
        driver.DateModified = DateTime.Now;
        driver.IsDeleted = false;
        db.Entry(driver).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      return View(driver);
    }

    // GET: Drivers/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Driver driver = await db.Drivers.FindAsync(id);
      if (driver == null)
      {
        return HttpNotFound();
      }
      return View(driver);
    }

    // POST: Drivers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      // Fetch the driver and their trips
      Driver driver = await db.Drivers.FindAsync(id);
      var driverTrips = await db.Trips.Where(x => x.DriverId == id).ToListAsync();

      if (driverTrips?.Count > 0)
      {
        // Soft delete the driver by marking as deleted
        driver.IsDeleted = true;
        db.Entry(driver).State = EntityState.Modified; // Mark the driver entity as modified
      }
      else
      {
        // Hard delete the driver
        db.Drivers.Remove(driver);
      }

      // Save changes to the database
      await db.SaveChangesAsync();

      // Redirect to Index action
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
