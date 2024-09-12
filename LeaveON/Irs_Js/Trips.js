$(document).ready(function () {
  handleDateTimeChange();
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
  /////////////////////////////////Passengers///////////////////////////////////////
  console.log('Document ready');
  console.log('Available Passengers:', availablePassengers);
  var transformedPassengers = availablePassengers.map(function (passenger) {
    return {
      label: passenger.Text,  // Display this in the input field
      value: passenger.Value  // Store this in the hidden input
    };
  });
  $("#autocompletePassengerId").autocomplete({
    source: function (request, response) {
      console.log('Autocomplete triggered with request:', request);
      response(transformedPassengers); // Use the transformed data
    },
    select: function (event, ui) {
      console.log('Selected item:', ui.item);
      $("#PassengerId").val(ui.item.value); // Store selected ID in hidden input
      $("#autocompletePassengerId").val(ui.item.label); // Display the label in the input field
      return false; // Prevent the default behavior of setting the value to the input
    }
  });
  /////////////////////////////////Passengers///////////////////////////////////////

  /////////////////////////////////Place///////////////////////////////////////
  console.log('Document ready');
  console.log('Available Places:', availablePlaces);
  var transformedPlaces = availablePlaces.map(function (place) {
    return {
      label: place.Text,  // Display this in the input field
      value: place.Value  // Store this in the hidden input
    };
  });
  $("#autocompletePlaceId").autocomplete({
    source: function (request, response) {
      console.log('Autocomplete triggered with request:', request);
      response(transformedPlaces); // Use the transformed data
    },
    select: function (event, ui) {
      console.log('Selected item:', ui.item);
      $("#PlaceId").val(ui.item.value); // Store selected ID in hidden input
      $("#autocompletePlaceId").val(ui.item.label); // Display the label in the input field
      return false; // Prevent the default behavior of setting the value to the input
    }
  });
  /////////////////////////////////Place///////////////////////////////////////
 
  /////////////////////////////////Driver///////////////////////////////////////
  var transformedDrivers = []; 
  $("#autocompleteDriverId").autocomplete({
    source: function (request, response) {
      console.log('Autocomplete triggered with request:', request);
      response(transformedDrivers); // Use the transformed data
    },
    select: function (event, ui) {
      console.log('Selected item:', ui.item);
      $("#DriverId").val(ui.item.value); // Store selected ID in hidden input
      $("#autocompleteDriverId").val(ui.item.label); // Display the label in the input field
      return false; // Prevent the default behavior of setting the value to the input
    }
  });
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
function handleDateTimeChange() {
//function handleDateTimeChange(element) {
  debugger;
  //var aa = element.value;
  var startTripDateTime = $('#fromDateTime').val();
  var endTripDateTime = $('#toDateTime').val();
  $.ajax({
    url: '/Trips/GetAvailableDriver/', // Replace with your backend URL
    method: 'GET',
    data: { startTripDateTime: startTripDateTime, endTripDateTime: endTripDateTime },
    success: function (data) {
      debugger;
      transformedDrivers = data.map(function (driver) {
        return {
          label: driver.Name,  // Display the driver's name
          value: driver.Id  // Store the driver's ID
        };
      });
      // Trigger autocomplete with the new drivers
      $('#autocompleteDriverId').autocomplete("option", "source", transformedDrivers);
    },
    error: function () {
      console.log('Ajax Error')
    }
  });
} 
//////////////////////////////////////////Driver///////////////////////////////////////
function onPlaceChange(dropdown) {
  debugger
  var selectedValue = dropdown.target.value;
  if (selectedValue === "" || isNaN(selectedValue)) {
    $('#placeName').val(selectedValue);
  } else {
    $('#placeName').val('');  // Clear the value of the PlaceName input
  }
}
function updateSelectBackground(selectElement) {
  const value = selectElement.value;
  switch (value) {
    case "Success(Not Paid)":
      selectElement.style.backgroundColor = "#f8d7da"; // Light red
      selectElement.style.color = "#721c24"; // Dark red
      break;
    case "Success":
      selectElement.style.backgroundColor = "#d4edda"; // Light green
      selectElement.style.color = "#155724"; // Dark green
      break;
    case "In Progress":
      selectElement.style.backgroundColor = "#fff3cd"; // Light yellow
      selectElement.style.color = "#856404"; // Dark yellow
      break;
    case "Cancel":
      selectElement.style.backgroundColor = "#7ba5f7"; // Light gray
      selectElement.style.color = "#eaeef5"; // Dark gray
      break;
    default:
      selectElement.style.backgroundColor = ""; // Default background
      selectElement.style.color = ""; // Default text color
      break;
  }
}

// Initialize background color on page load
document.addEventListener("DOMContentLoaded", function () {
  const selectElements = document.querySelectorAll(".status-select");
  selectElements.forEach(function (selectElement) {
    updateSelectBackground(selectElement);

    selectElement.addEventListener("change", function () {
      updateSelectBackground(selectElement);
    });
  });
});
