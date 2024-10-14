$(document).ready(function () {
  // Cache the input fields using jQuery selectors
  getAvailableDriversOnDateTimeChange();
  populateDropdown(totalTripHours);
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
  $('.totalHours-dropdown').select2({
    placeholder: "Total Hours",
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
  //$('.passenger-dropdown').focus();
  $('.passenger-autocomplete').focus();
  $('.from-date-picker').datetimepicker({
    timeFormat: "hh:mm tt",
    showOn: "both",
    showHour: true,
    showMinute: true,
    timeOnly: true, // Only time picker, no date
    controlType: 'select',
    oneLine: true,
    buttonImageOnly: true,
    buttonImage: "http://jqueryui.com/resources/demos/datepicker/images/calendar.gif",
    buttonText: "Select time",
    onClose: function (timeText, inst) {
      // Get the selected time
      const selectedTime = timeText;

      // Get the current date (or any default date if required)
      const currentDate = new Date().toLocaleDateString('en-US'); // This will give MM/DD/YYYY format

      // Combine date and time
      const fullDateTime = currentDate + ' ' + selectedTime;

      // Set the combined value in the hidden field
      $('#fullDateTime').val(fullDateTime);
    }
  });
  var tripTotalTime = document.getElementById('totalHoursDropdown');
  if (tripTotalTime) {
    // Get the selected value and parse it to a float
    var selectedHours = parseFloat(tripTotalTime.value);
    // Get the cost per hour
    var tripCost = parseFloat(document.getElementById('tripCost').value);
    // Calculate the total cost
    var totalCost = selectedHours * tripCost;
    // Alert the total cost
    //alert(totalCost.toFixed(2)); // Show two decimal places
    if (isNaN(totalCost)) {
      totalCost = 0;
    }
    
    $('#tripTotalCost').val(totalCost);
    // Alert the total cost
    //alert(totalCost);
  }
  //$('.from-date-picker').datetimepicker({
  //  timeFormat: "hh:mm tt",
  //  showOn: "both",
  //  showHour: true,
  //  showMinute: true,
  //  timeOnly: true, // Only time picker, no date
  //  controlType: 'select',
  //  //stepHour: 1,
  //  //stepMinute: 5,
  //  oneLine: true,
  //  buttonImageOnly: true,
  //  buttonImage: "http://jqueryui.com/resources/demos/datepicker/images/calendar.gif",
  //  buttonText: "Select time",
  //  //dateFormat: "dd-M-yy",
  //  //timeFormat: "hh:mm tt",
  //  //showOn: "both",
  //  //timeInput: true,
  //  //stepHour: 1,
  //  //stepMinute: 5,
  //  //oneLine: true,
  //  ////changeMonth: true,
  //  ////changeYear: true,
  //  //controlType: 'select',
  //  //buttonImageOnly: true,
  //  //buttonImage: "http://jqueryui.com/resources/demos/datepicker/images/calendar.gif",
  //  //buttonText: "Select date"
  //});
  $('#autocompletePassengerId, #autocompletePlaceId,#autocompleteDriverId').on('keydown', function (event) {
    if (event.key === 'Enter') {
      event.preventDefault(); // Prevent default form submission behavior
    }
  });
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
      label: passenger.Name,  // Display this in the input field
      value: passenger.Id,  // Store this in the hidden input
      managerDeal: passenger.ManagerDeal,  // Manager Deal
      managerComission: passenger.ManagerComission,  // Manager Comission
      //label: passenger.Text,  // Display this in the input field
      //value: passenger.Value  // Store this in the hidden input
    };
  });
  $("#autocompletePassengerId").autocomplete({
    source: function (request, response) {
      console.log('Autocomplete triggered with request:', request);
      response(transformedPassengers); // Use the transformed data
    },
    focus: function (event, ui) {
      // When navigating through the list with the arrow keys, show the label (name) in the input field
      $("#autocompletePassengerId").val(ui.item.label);
      return false; // Prevent default behavior of setting the value (ID) in the input
    },
    select: function (event, ui) {
      if (ui.item.managerDeal != null && ui.item.managerComission != null) {
        $('#tripCost').val(ui.item.managerDeal);
      }
      else {
        $('#tripCost').val('0');
      }
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
  //var transformedPlaces = availablePlaces.map(function (place) {
  //  return {
  //    label: place.Text,  // Display this in the input field
  //    value: place.Value  // Store this in the hidden input
  //  };
  //});
  //$("#autocompletePlaceId").autocomplete({
  //  source: function (request, response) {
  //    console.log('Autocomplete triggered with request:', request);
  //    response(transformedPlaces); // Use the transformed data
  //  },
  //  select: function (event, ui) {
  //    console.log('Selected item:', ui.item);
  //    $("#PlaceId").val(ui.item.value); // Store selected ID in hidden input
  //    $("#autocompletePlaceId").val(ui.item.label); // Display the label in the input field
  //    $("#placeNameContainer").hide(); // Hide the custom place name input
  //    return false; // Prevent the default behavior of setting the value to the input
  //  },
  //  change: function (event, ui) {
  //    if (!ui.item) {
  //      // If no place is selected from the autocomplete
  //      $("#PlaceId").val(''); // Set PlaceId to null
  //      $("#placeNameContainer").show(); // Show the custom place name input
  //      $("#placeName").val($("#autocompletePlaceId").val()); // Set the custom place name
  //    }
  //  }
  //});
  console.log('Document ready');
  console.log('Available Places:', availablePlaces);

  // Transform the available places data
  var transformedPlaces = availablePlaces.map(function (place) {
    return {
      label: place.Name,
      value: place.Id,
      isBlacklistedPlace: place.IsBlacklistedPlace,  // include this custom property
      commentReason: place.Comment  // include this custom property
    };
  });
  // Initialize the autocomplete
  $("#autocompletePlaceId").autocomplete({
    source: function (request, response) {
      var results = $.ui.autocomplete.filter(transformedPlaces, request.term);
      response(results);
    },
    focus: function (event, ui) {
      $("#autocompletePlaceId").val(ui.item.label);
      return false;
    },
    select: function (event, ui) {
      // Check if the selected item is blacklisted
      if (ui.item.isBlacklistedPlace === true) {
        var message = ui.item.commentReason;
        //var message = 'This place is blacklisted. Reason: ' + ui.item.commentReason;

        // Initialize or update the Summernote editor with the blacklist message
        $('#blacklistCommentEditor').summernote({
          height: 250,
          placeholder: "Blacklist Comment",
          dialogsInBody: true,
          focus: true
        }).summernote('code', message); // Set the content in Summernote editor

        // Show the modal
        $('#blacklistModal').modal('show');
      }

      $("#PlaceId").val(ui.item.value);
      $("#autocompletePlaceId").val(ui.item.label);
      return false;
    },
    open: function () {
      var menuItems = $("ul.ui-autocomplete li");

      menuItems.each(function () {
        var itemData = $(this).data("ui-autocomplete-item");

        if (itemData && itemData.isBlacklistedPlace === true) {
          $(this).addClass("blacklisted-place");
        }
      });
    }
    //$("#autocompletePlaceId").autocomplete({
    //  source: function (request, response) {
    //    // Filter places based on user input
    //    var results = $.ui.autocomplete.filter(transformedPlaces, request.term);
    //    response(results);
    //  },
    //  focus: function (event, ui) {
    //    $("#autocompletePlaceId").val(ui.item.label);
    //    return false;
    //  },
    //  select: function (event, ui) {
    //    debugger;
    //    // Check if the selected item is blacklisted
    //    if (ui.item.isBlacklistedPlace === true) {
    //      var message = 'This place is blacklisted. Reason: ' + ui.item.commentReason;
    //      $('#blacklistMessage').text(message);
    //      $('#blacklistModal').modal('show');
    //    }
    //    $("#PlaceId").val(ui.item.value);
    //    $("#autocompletePlaceId").val(ui.item.label);
    //    return false;
    //  },
    //  open: function () {
    //    // Once the menu is opened, find the items in the dropdown and apply custom styles
    //    var menuItems = $("ul.ui-autocomplete li");

    //    menuItems.each(function () {
    //      var itemData = $(this).data("ui-autocomplete-item");

    //      // If the place is blacklisted, apply a custom class
    //      if (itemData && itemData.isBlacklistedPlace === true) {
    //        $(this).addClass("blacklisted-place");  // Add a custom class for styling
    //      }
    //    });
    //  }

    // Initialize the autocomplete
    //$("#autocompletePlaceId").autocomplete({
    //  source: function (request, response) {
    //    console.log('Autocomplete triggered with request:', request);
    //    response(transformedPlaces); // Provide transformed data including isBlacklistedPlace
    //  },
    //  focus: function (event, ui) {
    //    // When navigating through the list with arrow keys, show the label in the input field
    //    $("#autocompletePlaceId").val(ui.item.label);
    //    return false; // Prevent default behavior of setting the value (ID) in the input
    //  },
    //  select: function (event, ui) {
    //    debugger;
    //    // Check if the selected item has the isBlacklistedPlace property
    //    if (ui.item.isBlacklistedPlace === true) {
    //      // Set the message in the modal
    //      var message = 'This place is blacklisted. Reason: ' + ui.item.commentReason;
    //      $('#blacklistMessage').text(message); // Set the blacklist reason text in the modal
    //      $('#blacklistModal').modal('show');   // Show the modal
    //    } else {
    //      console.log('Selected place is not blacklisted.');
    //    }
    //    console.log('Selected item:', ui.item);

    //    // Set values based on the selection
    //    $("#PlaceId").val(ui.item.value); // Set the hidden field with the selected place ID
    //    $("#autocompletePlaceId").val(ui.item.label); // Set the autocomplete input field with the selected place name

    //    return false; // Prevent default behavior
    //  }

    //select: function (event, ui) {
    //  debugger;
    //  // Check if the selected item has the isBlacklistedPlace property
    //  if (ui.item.isBlacklistedPlace === true) {
    //    alert('This place is blacklisted.' + ui.item.commentReason);
    //  } else {
    //    console.log('Selected place is not blacklisted.');
    //  }
    //  console.log('Selected item:', ui.item);

    //  // Set values based on the selection
    //  $("#PlaceId").val(ui.item.value); // Set the hidden field with the selected place ID
    //  $("#autocompletePlaceId").val(ui.item.label); // Set the autocomplete input field with the selected place name

    //  return false; // Prevent default behavior
    //}
  });
  //////
  //var transformedPlaces = availablePlaces.map(function (place) {
  //  return {
  //    label: place.Text,  // Display this in the input field (the name)
  //    value: place.Value  // Store this in the hidden input (the ID)
  //  };
  //});

  //$("#autocompletePlaceId").autocomplete({
  //  source: function (request, response) {
  //    console.log('Autocomplete triggered with request:', request);
  //    response(transformedPlaces); // Use the transformed data
  //  },
  //  focus: function (event, ui) {
  //    // When navigating through the list with the arrow keys, show the label (name) in the input field
  //    $("#autocompletePlaceId").val(ui.item.label);
  //    return false; // Prevent default behavior of setting the value to the input
  //  },
  //  select: function (event, ui) {
  //    console.log('Selected item:', ui.item);
  //    $("#PlaceId").val(ui.item.value); // Store selected ID in hidden input
  //    $("#autocompletePlaceId").val(ui.item.label); // Display the label (name) in the input field
  //    $("#placeNameContainer").hide(); // Hide the custom place name input
  //    return false; // Prevent default behavior of setting the value to the input
  //  },
  //  change: function (event, ui) {
  //    if (!ui.item) {
  //      // If no place is selected from the autocomplete
  //      $("#PlaceId").val(''); // Set PlaceId to null
  //      //$("#placeNameContainer").show(); // Show the custom place name input
  //      $("#placeName").val($("#autocompletePlaceId").val()); // Set the custom place name
  //    }
  //  }
  //});
  ////////

  //$("#autocompletePlaceId").autocomplete({
  //  source: function (request, response) {
  //    console.log('Autocomplete triggered with request:', request);
  //    response(transformedPlaces); // Use the transformed data
  //  },
  //  select: function (event, ui) {
  //    console.log('Selected item:', ui.item);
  //    $("#PlaceId").val(ui.item.value); // Store selected ID in hidden input
  //    $("#autocompletePlaceId").val(ui.item.label); // Display the label in the input field
  //    return false; // Prevent the default behavior of setting the value to the input
  //  }
  //});
  /////////////////////////////////Place///////////////////////////////////////

  /////////////////////////////////Driver///////////////////////////////////////
  var transformedDrivers = [];
  $("#autocompleteDriverId").autocomplete({
    source: function (request, response) {
      console.log('Autocomplete triggered with request:', request);
      response(transformedDrivers); // Use the transformed data
    },
    focus: function (event, ui) {
      // When navigating through the list with the arrow keys, show the label (name) in the input field
      $("#autocompleteDriverId").val(ui.item.label);
      return false; // Prevent default behavior of setting the value (ID) in the input
    },
    select: function (event, ui) {
      var getTotlaHoursTime = $('#totalHoursDropdown').val();
      var totalHours = parseFloat(getTotlaHoursTime);
      if (ui.item.isFiveHoursPlusEnabled == true) {
        if (totalHours > 5 && totalHours <= 8) {
          $('#tripCost').val('100').attr('disabled', true);
          $('#tripTotalCost').val('100').attr('disabled', true);
          $('#tripCostHidden').val('100'); // Update hidden field
        } else if (totalHours > 8 && totalHours <= 12) {
          $('#tripCost').val('150').attr('disabled', true);
          $('#tripTotalCost').val('150').attr('disabled', true);
          $('#tripCostHidden').val('150'); // Update hidden field
        } else if (totalHours > 12 && totalHours <= 24) {
          $('#tripCost').val('200').attr('disabled', true);
          $('#tripTotalCost').val('200').attr('disabled', true);
          $('#tripCostHidden').val('200'); // Update hidden field
        } else {
          $('#tripCost').val('0').attr('disabled', false);
          $('#tripTotalCost').val('0').attr('disabled', false);
          $('#tripCostHidden').val('0'); // Update hidden field
        }
      }
      else {
        $('#tripCost').val('0').attr('disabled', false);
        $('#tripCostHidden').val('0'); // Update hidden field
        $('#tripTotalCost').val('0');
      }
      console.log('Selected item:', ui.item);
      $("#DriverId").val(ui.item.value); // Store selected ID in hidden input
      $("#autocompleteDriverId").val(ui.item.label); // Display the label in the input field
      return false; // Prevent the default behavior of setting the value to the input
    }
  });
  ////////////////////////////////////

  /////

  /////

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



  ////Submit Form  With Loader////////////////////////////////
  //$("#submitButton").on("click", function (event) {
  //  // Prevent the default form submission behavior
  //  event.preventDefault();

  //  // Get the form, button, and spinner elements by their IDs
  //  var $form = $("#tripForm");
  //  var $submitButton = $("#submitButton");
  //  var $spinner = $("#spinner");

  //  // Serialize the form data
  //  var formData = $form.serialize();

  //  // Show the spinner and disable the submit button to prevent double-clicks
  //  $spinner.show();
  //  $submitButton.prop("disabled", true);

  //  // Perform AJAX request to submit the form
  //  $.ajax({
  //    url: $form.attr("action"),    // Get the form's action URL
  //    method: $form.attr("method"), // Get the form's method (POST/GET)
  //    data: formData,               // Form data serialized
  //    success: function (response) {
  //      // Redirect to the Trips Index page on successful submission
  //      window.location.href = '/Trips/Index';
  //    },
  //    error: function (xhr, status, error) {
  //      // Handle error case
  //      alert("An error occurred. Please try again.");
  //    },
  //    complete: function () {
  //      // Hide the spinner and re-enable the submit button after the request completes
  //      $spinner.hide();
  //      $submitButton.prop("disabled", false);
  //    }
  //  });
  //});
  ////Submit Form  With Loader/////////////////////////////////////


  //////////////////Edit case///////////////////////////////////


  const checkbox = document.getElementById('isBlackListed');
  if (checkbox != null) {
    const isBlackListed = checkbox.checked;
    // Set the value of the checkbox to be sent
    checkbox.value = isBlackListed;

    // Initial state - show or hide elements based on the initial checkbox state
    const elements = document.getElementsByClassName('isHiddenBlackListedCntrls');
    for (let i = 0; i < elements.length; i++) {
      elements[i].style.display = isBlackListed ? 'block' : 'none';
    }

    document.getElementById('isBlackListed').addEventListener('change', function () {
      const isBlackListed = checkbox.checked;
      // Keeps your debugger line

      // Toggle visibility of elements based on checkbox state
      for (let i = 0; i < elements.length; i++) {
        elements[i].style.display = isBlackListed ? 'block' : 'none';
      }

      // Set the value of the checkbox to be sent
      checkbox.value = isBlackListed;
    });
  }
  //const checkbox = document.getElementById('isBlackListed');
  //if (checkbox!=null) {
  //  const isBlackListed = checkbox.checked;
  //  // Set the value of the checkbox to be sent
  //  checkbox.value = isBlackListed;
  //  document.getElementById('isBlackListed').addEventListener('change', function () {
  //    const checkbox = document.getElementById('isBlackListed');
  //    const isBlackListed = checkbox.checked;
  //    debugger;
  //    const elements = document.getElementsByClassName('isHiddenBlackListedCntrls');

  //    for (let i = 0; i < elements.length; i++) {
  //      elements[i].style.display = 'block';
  //    }
  //    // Set the value of the checkbox to be sent
  //    checkbox.value = isBlackListed;
  //  });
  //}
  /////////////////////////////////////////////////////////////
  $('#tripCost').on('keyup', function () {
    var tripCost = $('#tripCost').val();
    var _tripCost = parseFloat(tripCost);
    var totalHours = $('#totalHoursDropdown').val();
    var _totalHours = parseFloat(totalHours);
    var totalTripCost = _tripCost * _totalHours;
    if (!isNaN(totalTripCost)) {
      $('#tripTotalCost').val(totalTripCost.toFixed(0));
    }
    else {
      $('#tripTotalCost').val('0');
    }
  });
});

function getAvailableDriversOnDateTimeChange() {
  //function handleDateTimeChange(element) {
  //var aa = element.value;
  var startTripDateTime = $('#fromDateTime').val();
  var endTripDateTime = $('#toDateTime').val();
  $.ajax({
    url: '/Trips/GetAvailableDriver/', // Replace with your backend URL
    method: 'GET',
    data: { startTripDateTime: startTripDateTime, endTripDateTime: endTripDateTime },
    success: function (data) {
      if (data.success) {
        // Access the data array from the response object
        var drivers = data.data;

        // Ensure drivers is an array before calling map
        if (Array.isArray(drivers)) {
          var transformedDrivers = drivers.map(function (driver) {
            //return {
            //  label: driver.Name,  // Display the driver's name
            //  value: driver.Id     // Store the driver's ID
            //};
            return {
              label: driver.Name,  // Display the driver's name
              value: driver.Id,    // Store the driver's ID
              isFiveHoursPlusEnabled: driver.IsFiveHoursPlusEnabled,  // Include IsFiveHoursPlusEnabled
              comment: driver.Comment  // Include Comment
            };
          });

          // Trigger autocomplete with the new drivers
          $('#autocompleteDriverId').autocomplete("option", "source", transformedDrivers);
        } else {
          console.error('Expected an array of drivers, but got:', drivers);
        }
      } else {
        console.error('Error fetching drivers:', data.message);
      }
    },
    error: function (xhr, status, error) {
      console.error('AJAX error:', status, error);
    }
  });
}
//////////////////////////////////////////Driver///////////////////////////////////////
function onPlaceChange(dropdown) {
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

//Format 1,1.30 till 24

// Function to generate time slots with 30-minute intervals in the format "X hour" or "X.30 hour"
//function generateTimeSlots() {
//  var timeSlots = [];
//  var maxHours = 24;

//  for (var i = 1; i < maxHours; i++) {
//    // Add whole hours
//    timeSlots.push(i + " hour" + (i > 1 ? "s" : ""));
//    // Add half-hours (e.g., 1.30 hour)
//    timeSlots.push(i + ".30 hour" + (i > 1 ? "s" : ""));
//  }

//  // Add "24 hours" explicitly
//  timeSlots.push("24 hours");

//  return timeSlots;
//}
//function generateTimeSlots() {
//  var timeSlots = [];
//  var maxHours = 24;

//  for (var i = 1; i <= maxHours; i++) {
//    // Add whole hours
//    timeSlots.push({ value: i, text: i + " hour" + (i > 1 ? "s" : "") });
//    // Add half-hours (e.g., 1.5 hours)
//    if (i < maxHours) {
//      timeSlots.push({ value: i + 0.30, text: i + ".30 hours" });
//    }
//  }

//  return timeSlots;
//}

//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot.value; // Numeric value for binding
//    option.text = timeSlot.text;   // Display text
//    dropdown.appendChild(option);
//  });
//}
//function generateTimeSlots() {
//  var timeSlots = [];
//  var maxHours = 24;
//  debugger;
//  for (var i = 1; i <= maxHours; i++) {
//    // Add whole hours
//    timeSlots.push({ value: i, text: i + " hour" + (i > 1 ? "s" : "") });
//    // Add half-hour slots (e.g., 1.30 hours, 2.30 hours)
//    if (i < maxHours) {
//      timeSlots.push({ value: i + 0.30, text: i + ".30 hours" });
//    }
//  }

//  return timeSlots;
//}

//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot.value; // Numeric value for binding
//    option.text = timeSlot.text;   // Display text (e.g., "1.30 hours")
//    dropdown.appendChild(option);
//  });
//}
//function generateTimeSlots() {
//  var timeSlots = [];
//  var maxHours = 24;
//  debugger;
//  for (var i = 1; i <= maxHours; i++) {
//    // Add whole hours
//    timeSlots.push({
//      value: i.toFixed(2), // Format value to two decimal places
//      text: i + " hour" + (i > 1 ? "s" : "")
//    });
//    // Add half-hour slots (e.g., 1.30 hours, 2.30 hours)
//    if (i < maxHours) {
//      timeSlots.push({
//        value: (i + 0.30).toFixed(2), // Format value to two decimal places
//        text: i + ".30 hours"
//      });
//    }
//  }

//  return timeSlots;
//}
//function populateDropdown(selectedValue) {
//  debugger;
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  dropdown.innerHTML = ''; // Clear existing options

//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot.value; // Numeric value formatted as a string
//    option.text = timeSlot.text;   // Display text
//    dropdown.appendChild(option);
//  });

//  // Set the selected value
//  if (selectedValue) {
//    dropdown.value = selectedValue;
//  }
//}
function generateTimeSlots() {
  var timeSlots = [];
  var maxHours = 24;

  for (var i = 1; i <= maxHours; i++) {
    // Add whole hours
    timeSlots.push({
      value: i.toFixed(2), // Format value to two decimal places
      text: i + " hour" + (i > 1 ? "s" : "")
    });
    // Add half-hour slots (e.g., 1.30 hours, 2.30 hours)
    if (i < maxHours) {
      timeSlots.push({
        value: (i + 0.30).toFixed(2), // Format value to two decimal places
        text: i + ".30 hours"
      });
    }
  }

  return timeSlots;
}

function populateDropdown(selectedValue) {
  var dropdown = document.getElementById("totalHoursDropdown");
  var timeSlots = generateTimeSlots();

  dropdown.innerHTML = ''; // Clear existing options

  timeSlots.forEach(function (timeSlot) {
    var option = document.createElement("option");
    option.value = timeSlot.value; // Numeric value formatted as a string
    option.text = timeSlot.text;   // Display text
    dropdown.appendChild(option);
  });

  // Ensure selectedValue is in the correct format
  if (selectedValue) {
    dropdown.value = selectedValue.toFixed(2); // Format selectedValue to match options
  }
}
//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot.value; // Numeric value formatted as a string
//    option.text = timeSlot.text;   // Display text (e.g., "1.30 hours")
//    dropdown.appendChild(option);
//  });
//}
/////////////////////////
// Function to populate the dropdown
//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot;
//    option.text = timeSlot;
//    dropdown.appendChild(option);
//  });
//}

//



///
// Function to generate time slots with 30-minute intervals in the format "X hours" or "X.Y hours"
//Format should be like 1 , 1.5 till 24
//function generateTimeSlots() {
//  var timeSlots = [];
//  var maxHours = 24;

//  for (var i = 1; i <= maxHours; i++) {
//    // Add whole hours
//    timeSlots.push(i + " hour" + (i > 1 ? "s" : ""));
//    // Add half-hours (e.g., 1.5 hours)
//    if (i < maxHours) {
//      timeSlots.push(i + 0.5 + " hours");
//    }
//  }

//  return timeSlots;
//}

//// Function to populate the dropdown
//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot;
//    option.text = timeSlot;
//    dropdown.appendChild(option);
//  });
//}

// Function to populate the dropdown
//function populateDropdown() {
//  var dropdown = document.getElementById("totalHoursDropdown");
//  var timeSlots = generateTimeSlots();

//  // Clear existing options (if any)
//  dropdown.innerHTML = '';

//  // Add each time slot as an option to the dropdown
//  timeSlots.forEach(function (timeSlot) {
//    var option = document.createElement("option");
//    option.value = timeSlot;
//    option.text = timeSlot;
//    dropdown.appendChild(option);
//  });
//}
////

