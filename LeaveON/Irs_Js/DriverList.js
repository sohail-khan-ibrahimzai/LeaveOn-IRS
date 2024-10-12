function driverStatusChange(driverId, isChecked) {
  $.post('/Drivers/UpdateDriverStatus', { driverId: driverId, isActive: isChecked }, function (response) {
    console.log("Status updated successfully!");
    // Redirect to the Index page after the update
    window.location.href = '/Drivers/Index';
   });
}
