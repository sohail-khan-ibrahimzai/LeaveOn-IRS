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
using InventoryRepo.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;

namespace LeaveON.Controllers
{
  public class DriversController : Controller
  {
    private InventoryPortalEntities db = new InventoryPortalEntities();
    //private readonly ApplicationUserManager _userManager;
    //public DriversController(ApplicationUserManager userManager)
    //{
    //  _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    //}
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
    // GET: Drivers
    public async Task<ActionResult> Index()
    {
      var currentUser = GetCurrentUserInfo();
      //bool isAdmin = User.IsInRole("Admin");
      if (User.IsInRole("Admin"))
        return View(await db.Drivers.Where(x => x.IsDeleted == false).ToListAsync());
      else
        return View(await db.Drivers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false).ToListAsync());
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
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,DateCreated,DateModified,Remarks")] CreateDriverDto createDriverDto)
    {
      //var userInfo = GetCurrentUserInfo();
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        // Check if a driver with the same name already exists
        var driverExists = await db.Drivers
           .FirstOrDefaultAsync(x => x.Name == createDriverDto.Name);

        if (driverExists !=null)
        {
          ModelState.AddModelError("Name", "Driver already exists.");
        }
        else
        {
          var driver = new Driver
          {
            DateCreated = DateTime.Now,
            Name = createDriverDto.Name,
            Remarks = createDriverDto.Remarks,
            IsDeleted = false,
            CreatedBy = currentUser.UserId
          };
          db.Drivers.Add(driver);
          await db.SaveChangesAsync();
          return RedirectToAction("Index");
        }
      }

      // Return the same view with the model state errors
      return View(createDriverDto);
    }

    // GET: Drivers/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Driver driver = await db.Drivers.FindAsync(id);
      var editDriverDto = new CreateDriverDto
      {
        Id = driver.Id,
        Name = driver.Name,
        Remarks = driver.Remarks,
      };
      if (driver == null)
      {
        return HttpNotFound();
      }
      return View(editDriverDto);
    }

    // POST: Drivers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateCreated,DateModified,Remarks")] CreateDriverDto updateDiverDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var driver = await db.Drivers.FirstOrDefaultAsync(x => x.Id == updateDiverDto.Id);
        if (driver == null)
          return HttpNotFound();
        driver.DateModified = DateTime.Now;
        //driver.IsDeleted = false;
        driver.Name = updateDiverDto.Name;
        driver.Remarks = updateDiverDto.Remarks;
        driver.UpdatedBy = currentUser.UserId;
        db.Entry(driver).State = EntityState.Modified;
        await db.SaveChangesAsync();
      }
      return RedirectToAction("Index");
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
