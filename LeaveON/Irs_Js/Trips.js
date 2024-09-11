$(document).ready(function () {
  $('#remarks').summernote({
    height: 250,
    placeholder: "Comment",
    minHeight: null,
    maxHeight: null,
    dialogsInBody: true,
    focus: true
  });
  // Initialize Select2 on dropdowns
  $('.driver-dropdown').select2({
    placeholder: "Select a Driver",
    allowClear: true
  });
  $('.passenger-dropdown').select2({
    placeholder: "Select a Passenger",
    allowClear: true
  });
  $('.place-dropdown').select2({
    placeholder: "Select a Place",
    allowClear: true,
    tags: true
  });
  $('.status-dropdown').select2({
    placeholder: "Select Status",
    allowClear: true
  });
  $('.passenger-dropdown').focus();
  $('.from-date-picker').datetimepicker({
    dateFormat: "dd-M-yy",
    timeFormat: "hh:mm tt",
    showOn: "both",
    timeInput: true,
    stepHour: 1,
    stepMinute: 5,
    oneLine: true,
    //changeMonth: true,
    //changeYear: true,
    controlType: 'select',
    buttonImageOnly: true,
    buttonImage: "http://jqueryui.com/resources/demos/datepicker/images/calendar.gif",
    buttonText: "Select date"
  });
  debugger;

  ////////////////////////////////////////////Working for Driver
  //console.log('Document ready');
  //console.log('Available Drivers:', availableDrivers);
  //var transformedDrivers = availableDrivers.map(function (driver) {
  //  return {
  //    label: driver.Text,  // Display this in the input field
  //    value: driver.Value  // Store this in the hidden input
  //  };
  //});
  //$("#autocompleteDriverId").autocomplete({
  //  source: function (request, response) {
  //    console.log('Autocomplete triggered with request:', request);
  //    response(transformedDrivers); // Use the transformed data
  //  },
  //  select: function (event, ui) {
  //    console.log('Selected item:', ui.item);
  //    $("#DriverId").val(ui.item.value); // Store selected ID in hidden input
  //    $("#autocompleteDriverId").val(ui.item.label); // Display the label in the input field
  //    return false; // Prevent the default behavior of setting the value to the input
  //  }
  //});
  ////////////////////////////////////

  //$('#autocompleteDriverId').on('input', function () {
  //  let query = $(this).val();

  //  if (query.length > 0) {
  //    $.ajax({
  //      url: '/Trips/SearchDrivers/', // Replace with your backend URL
  //      method: 'GET',
  //      data: { searchTerm: query },
  //      success: function (data) {
  //        $('#driversList').empty();
  //        data.forEach(function (item) {
  //          $('#driversList').append('<li data-id="' + item.id + '">' + item.text + '</li>');
  //        });
  //      },
  //      error: function () {
  //        $('#driversList').empty();
  //        $('#driversList').append('<li>Error fetching data</li>');
  //      }
  //    });
  //  } else {
  //    $('#driversList').empty();
  //  }
  //});
  //// Handle suggestion click
  //$('#driversList').on('click', 'li', function () {
  //  let selectedText = $(this).text();
  //  let driverId = $(this).data('id');

  //  $('#autocompleteDriverId').val(selectedText); // Set the text in the input field
  //  $('#hiddenDriverId').val(driverId); // Set the DriverId in the hidden input field
  //  $('#driversList').empty();
  //});

  //$('#autocompletePassengerId').on('input', function () {
  //  let query = $(this).val();

  //  if (query.length > 0) {
  //    $.ajax({
  //      url: '/Trips/SearchPassengers/', // Replace with your backend URL
  //      method: 'GET',
  //      data: { searchTerm: query },
  //      success: function (data) {
  //        $('#passengersList').empty();
  //        data.forEach(function (item) {
  //          $('#passengersList').append('<li data-id="' + item.id + '">' + item.text + '</li>');
  //        });
  //      },
  //      error: function () {
  //        $('#passengersList').empty();
  //        $('#passengersList').append('<li>Error fetching data</li>');
  //      }
  //    });
  //  } else {
  //    $('#passengersList').empty();
  //  }
  //});
  //// Handle suggestion click
  //$('#passengersList').on('click', 'li', function () {
  //  let selectedText = $(this).text();
  //  let passengerId = $(this).data('id');

  //  $('#autocompletePassengerId').val(selectedText); // Set the text in the input field
  //  $('#hiddenPassengerId').val(passengerId); // Set the DriverId in the hidden input field
  //  $('#passengersList').empty();
  //});
  //$('#autocompletePlaceId').on('input', function () {
  //  let query = $(this).val();

  //  if (query.length > 0) {
  //    $.ajax({
  //      url: '/Trips/SearchPlaces/', // Replace with your backend URL
  //      method: 'GET',
  //      data: { searchTerm: query },
  //      success: function (data) {
  //        $('#placesList').empty();
  //        data.forEach(function (item) {
  //          $('#placesList').append('<li data-id="' + item.id + '">' + item.text + '</li>');
  //        });
  //      },
  //      error: function () {
  //        $('#placesList').empty();
  //        $('#placesList').append('<li>Error fetching data</li>');
  //      }
  //    });
  //  } else {
  //    $('#placesList').empty();
  //  }
  //});
  //// Handle suggestion click
  //$('#placesList').on('click', 'li', function () {
  //  let selectedText = $(this).text();
  //  let driverId = $(this).data('id');

  //  $('#autocompletePlaceId').val(selectedText); // Set the text in the input field
  //  $('#hiddenPlaceId').val(driverId); // Set the DriverId in the hidden input field
  //  $('#placesList').empty();
  //});
});
function onPlaceChange(dropdown) {
  debugger
  var selectedValue = dropdown.target.value;
  if (selectedValue === "" || isNaN(selectedValue)) {
    $('#placeName').val(selectedValue);
  } else {
    $('#placeName').val('');  // Clear the value of the PlaceName input
  }
}
