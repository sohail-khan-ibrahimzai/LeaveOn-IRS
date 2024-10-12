function passengerStatusChange(passengerId, isChecked) {
  $.post('/Passengers/UpdatePassengerStatus', { passengerId: passengerId, isActive: isChecked }, function (response) {
    console.log("Status updated successfully!");
    window.location.href = '/Passengers/Index';
  });
}
