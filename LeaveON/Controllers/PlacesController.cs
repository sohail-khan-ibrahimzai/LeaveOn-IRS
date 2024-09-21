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

namespace LeaveON.Controllers
{
  [Authorize(Roles = "Admin,Manager,User")]
  public class PlacesController : Controller
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
    // GET: Locations
    public async Task<ActionResult> Index()
    {
      var currentUser = GetCurrentUserInfo();
      if (User.IsInRole("Admin"))
        return View(await db.Places.Where(x => x.IsDeleted == false).ToListAsync());
      else
        return View(await db.Places.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false).ToListAsync());
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
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,Remarks,DateCreated,DateUpdated,Remarks,Address,IsBlackListed")] CreatePlaceDto createPlaceDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var placeExists = await db.Places
          .FirstOrDefaultAsync(x => x.Name == createPlaceDto.Name);

        if (placeExists != null)
        {
          // Add a model state error
          ModelState.AddModelError("Name", "Place already exists.");
        }
        else
        {
          var place = new Place
          {
            DateCreated = DateTime.Now,
            Name = createPlaceDto.Name,
            Remarks = createPlaceDto.Remarks,
            Address = createPlaceDto.Address,
            IsBlackListed = createPlaceDto.IsBlackListed,
            IsDeleted = false,
            CreatedBy = currentUser.UserId
          };
          db.Places.Add(place);
          await db.SaveChangesAsync();
          return RedirectToAction("Index");
        }
      }
      return View(createPlaceDto);
    }

    // GET: Locations/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      //if (id == null)
      //{
      //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      //}
      Place place = await db.Places.FindAsync(id);
      //if (place == null)
      //  return HttpNotFound();
      if (place == null)
      {
        ModelState.AddModelError("", "Place not found."); // Add error message without key
        return View(); // Return the view with the error
      }
      var editPlaceDto = new CreatePlaceDto
      {
        Id = place.Id,
        Name = place.Name,
        Remarks = place.Remarks,
        Address = place.Address,
        Floor = place.Floor,
        Bell = place.Bell,
        IsBlackListed = place.IsBlackListed,
      };
      return View(editPlaceDto);
    }

    // POST: Locations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateCreated,DateUpdated,Remarks,Address,IsBlackListed,Floor,Bell")] CreatePlaceDto updatePlaceDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var place = await db.Places.FirstOrDefaultAsync(x => x.Id == updatePlaceDto.Id);
        //if (updatePlaceDto.IsBlackListed == true && place.BlacklistedDate == null)
        //{
        //  place.IsBlackListed = updatePlaceDto.IsBlackListed ?? false;
        //  place.BlacklistedDate = DateTime.UtcNow;
        //}
        if (place == null)
          return HttpNotFound();
        place.DateUpdated = DateTime.Now;
        place.Name = updatePlaceDto.Name;
        place.Remarks = updatePlaceDto.Remarks;
        place.Address = updatePlaceDto.Address;
        place.Floor = updatePlaceDto.Floor;
        place.Bell = updatePlaceDto.Bell;
        place.UpdateBy = currentUser.UserId;
        // Update Blacklist properties if needed
        if (updatePlaceDto.IsBlackListed == true)
        {
          if (place.BlacklistedDate == null)
          {
            place.BlacklistedDate = DateTime.UtcNow;
            place.IsBlackListed = true;
          }

          // Update trips associated with this place
          var trips = await db.Trips.Where(t => t.PlaceId == place.Id).ToListAsync();
          foreach (var trip in trips)
          {
            trip.IsBlackListed = true;
            trip.DateBlackList = DateTime.UtcNow;
          }
        }

        // Save changes to the place
        db.Entry(place).State = EntityState.Modified;

        // Save changes to the trips if there are any updates
        await db.SaveChangesAsync();
        //db.Entry(place).State = EntityState.Modified;
        //await db.SaveChangesAsync();
      }
      return RedirectToAction("Index");
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
        place.IsDeleted = true;
        //db.Places.Remove(place);
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
