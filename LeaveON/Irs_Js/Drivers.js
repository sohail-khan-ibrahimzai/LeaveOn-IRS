//$(document).ready(function () {
//  $('#remarks').summernote({
//    height: 250,
//    placeholder: "Comment",
//    minHeight: null,
//    maxHeight: null,
//    dialogsInBody: true,
//    focus: true
//  });
//  $('#driverName').focus();
//  debugger
//  const checkbox = document.getElementById('isFiveHrsPlusEnabled');
//  if (checkbox != null) {
//    debugger;
//    const isBlackListed = checkbox.checked;
//    checkbox.value = isBlackListed;
//    document.getElementById('isFiveHrsPlusEnabled').addEventListener('change', function () {
//      const isBlackListed = checkbox.checked;
//      checkbox.value = isBlackListed;
//    });
//  }
//});
$(document).ready(function () {
  $('#remarks').summernote({
    height: 250,
    placeholder: "Comment",
    minHeight: null,
    maxHeight: null,
    dialogsInBody: true,
    focus: true
  });

  $('#driverName').focus();

  const checkbox = document.getElementById('isFiveHrsPlusEnabled');
  if (checkbox != null) {
    // Set the initial value
    checkbox.value = checkbox.checked;

    // Add event listener
    checkbox.addEventListener('change', function () {
      // Update the value based on the checkbox state
      checkbox.value = this.checked;
      console.log('Checkbox state:', this.checked); // Debugging output
    });
  }
  const checkboxDriverStatus = document.getElementById('isActiveDriver');
  if (checkboxDriverStatus != null) {
    debugger;
    // Set the initial value
    checkboxDriverStatus.value = checkboxDriverStatus.checked;

    // Add event listener
    checkboxDriverStatus.addEventListener('change', function () {
      // Update the value based on the checkbox state
      checkboxDriverStatus.value = this.checked;
      console.log('Checkbox state Status:', this.checked); // Debugging output
    });
  }
  //function handleCheckboxChange(id, isChecked) {
  //  console.log("Checkbox ID: " + id + ", Checked: " + isChecked);
  //}
});
