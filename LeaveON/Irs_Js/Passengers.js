$(document).ready(function () {
  $('#remarks').summernote({
    height: 250,
    placeholder: "Comment",
    minHeight: null,
    maxHeight: null,
    dialogsInBody: true,
    focus: true
  });
  $('#passengerName').focus();

  const checkboxPassengerStatus = document.getElementById('isActivePassenger');
  if (checkboxPassengerStatus != null) {
    // Set the initial value
    checkboxPassengerStatus.value = checkboxPassengerStatus.checked;
    // Add event listener
    checkboxPassengerStatus.addEventListener('change', function () {
      // Update the value based on the checkbox state
      checkboxPassengerStatus.value = this.checked;
      console.log('Checkbox state:', this.checked); // Debugging output
    });
  }

});
