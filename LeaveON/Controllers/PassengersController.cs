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
  public class PassengersController : Controller
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
    // GET: Passengers
    public async Task<ActionResult> Index()
    {
      var currentUser = GetCurrentUserInfo();
      if (User.IsInRole("Admin"))
        //return View(await db.Passengers.Where(x => x.IsDeleted == false).ToListAsync());
        return View(await db.Passengers.Where(x => x.IsDeleted == false).ToListAsync());
      else
        //return View(await db.Passengers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false).ToListAsync());
        return View(await db.Passengers.Where(x => x.CreatedBy == currentUser.UserId && x.IsDeleted == false).ToListAsync());
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
      CreatePassengerDto createPassengerDto = new CreatePassengerDto();
      return View(createPassengerDto);
    }

    // POST: Passengers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "Id,Name,DateCreated,DateModified,Remarks,NickName,PhoneNumber,PickupAddress,ManagerDeal,ManagerComission")] CreatePassengerDto createPassengerDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var passengerExists = await db.Passengers
          .FirstOrDefaultAsync(x => x.Name == createPassengerDto.Name);

        if (passengerExists != null)
        {
          // Add a model state error
          ModelState.AddModelError("Name", "Passenger already exists.");
        }
        else
        {
          var passenger = new Passenger
          {
            DateCreated = DateTime.UtcNow,
            Name = createPassengerDto.Name,
            Remarks = createPassengerDto.Remarks,
            IsDeleted = false,
            NickName = createPassengerDto.NickName,
            PhoneNumber = createPassengerDto.PhoneNumber,
            PickupAddress = createPassengerDto.PickupAddress,
            ManagerDeal = createPassengerDto.ManagerDeal,
            ManagerComission = createPassengerDto.ManagerComission,
            CreatedBy = currentUser.UserId
          };
          db.Passengers.Add(passenger);
          await db.SaveChangesAsync();
          return RedirectToAction("Index");
        }
      }
      return View(createPassengerDto);
    }

    // GET: Passengers/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      //if (id == null)
      //{
      //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      //}
      Passenger passenger = await db.Passengers.FindAsync(id);
      //if (passenger == null)
      //  return HttpNotFound();
      if (passenger == null)
      {
        ModelState.AddModelError("", "Passenger not found."); // Add error message without key
        return View(); // Return the view with the error
      }
      var editPassengerDto = new CreatePassengerDto
      {
        Id = passenger.Id,
        Name = passenger.Name,
        Remarks = passenger.Remarks,
        NickName = passenger.NickName,
        PhoneNumber = passenger.PhoneNumber,
        PickupAddress = passenger.PickupAddress,
        IsActive = passenger.IsActive,
        ManagerDeal = passenger.ManagerDeal.HasValue ? passenger.ManagerDeal.Value : 0,
        ManagerComission = passenger.ManagerComission.HasValue ? passenger.ManagerComission.Value : 0  // Set default to 0.0M if null
      };
      return View(editPassengerDto);
    }

    // POST: Passengers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateCreated,DateModified,Remarks,NickName,PhoneNumber,PickupAddress,ManagerDeal,ManagerComission,IsActive")] CreatePassengerDto updatePassengerDto)
    {
      var currentUser = GetCurrentUserInfo();
      if (ModelState.IsValid)
      {
        var passenger = await db.Passengers.FirstOrDefaultAsync(x => x.Id == updatePassengerDto.Id);
        if (passenger == null)
          return HttpNotFound();
        passenger.DateModified = DateTime.UtcNow;
        //driver.IsDeleted = false;
        passenger.Name = updatePassengerDto.Name;
        passenger.Remarks = updatePassengerDto.Remarks;
        passenger.NickName = updatePassengerDto.NickName;
        passenger.PhoneNumber = updatePassengerDto.PhoneNumber;
        passenger.PickupAddress = updatePassengerDto.PickupAddress;
        passenger.ManagerDeal = updatePassengerDto.ManagerDeal;
        passenger.ManagerComission = updatePassengerDto.ManagerComission;
        passenger.IsActive = updatePassengerDto.IsActive;
        passenger.UpdateBy = currentUser.UserId;
        db.Entry(passenger).State = EntityState.Modified;
        await db.SaveChangesAsync();
      }
      return RedirectToAction("Index");
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
    // POST: UpdatePassengerStatus/5
    public async Task<ActionResult> UpdatePassengerStatus(int? passengerId, bool? isActive)
    {
      if (passengerId == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Passenger passenger = await db.Passengers.FindAsync(passengerId);
      if (passenger == null)
      {
        return HttpNotFound();
      }
      passenger.IsActive = isActive;
      db.Entry(passenger).State = EntityState.Modified;
      await db.SaveChangesAsync();
      return Json(new { success = true });
    }
    public async Task<ActionResult> UpdatePassengersManagers()
    {
    var passenger = await db.Passengers.ToListAsync();
    var manager = await db.AspNetUsers
    .Where(user => user.AspNetRoles.Any(role => role.Name == "Manager")) // Filter to include only managers
    .ToListAsync();
      var editPassengerMgrDto = new PassengerMgrUpdateDto
      {
        Passengers = passenger.Select(ps => new CreatePassengerDto
        {
          Id = ps.Id,
          Name = ps.Name,
          SelectedManagerId = ps.CreatedBy
        }).ToList(),
        Managers = manager.Select(m => new ManagerDto // Create a DTO for each manager
        {
          UserId = m.Id,
          Name = m.Name
        }).ToList() // Convert to list
      };
      return View(editPassengerMgrDto);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> UpdatePassengersManagers(PassengerMgrUpdateDto model)
    {
      if (ModelState.IsValid)
      {
        // Iterate over each passenger in the model
        foreach (var passenger in model.Passengers)
        {
          var existingPassenger = await db.Passengers.FindAsync(passenger.Id);
          if (existingPassenger != null)
          {
            existingPassenger.Name = passenger.Name; // Update passenger name
            existingPassenger.CreatedBy = passenger.SelectedManagerId; // Update the manager
          }
        }

        // Save changes to the database
        await db.SaveChangesAsync();

        // Optionally redirect to the Index or another action
        return RedirectToAction("UpdatePassengersManagers");
      }

      // If model state is not valid, reload managers and passengers
      var managers = await db.AspNetUsers
          .Where(user => user.AspNetRoles.Any(role => role.Name == "Manager"))
          .ToListAsync();

      model.Managers = managers.Select(m => new ManagerDto
      {
        UserId = m.Id,
        Name = m.Name
      }).ToList();

      return View(model);
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
