$(document).ready(function () {
  $('#remarks').summernote({
    height: 250,
    placeholder: "Comment",
    minHeight: null,
    maxHeight: null,
    dialogsInBody: true,
    focus: true
  });
  $('#placeName').focus();
  $('#placeAddress').on('input', function () {
    var address = encodeURIComponent($(this).val());
    if (address) {
      var googleMapsUrl = "https://www.google.com/maps/search/?api=1&query=" + address;
      $('#openMapLink').attr('href', googleMapsUrl);
    } else {
      $('#openMapLink').attr('href', '#'); // Default or empty if no address is provided
    }
  });
  //////////////////Edit case///////////////////////////////////

  debugger
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
      debugger;
      const isBlackListed = checkbox.checked;
      debugger; // Keeps your debugger line

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
});
//function manualTranslateInput(elementId) {
//  var lang = $('html').attr('lang') || 'en'; // Get the current language
//  var inputElement = document.getElementById(elementId);
//  var originalText = inputElement.value;
//  alert('Language:' + lang + 'Text: ' + originalText);
//}
function manualTranslateInput(elementId) {
  var lang = $('html').attr('lang') || 'en'; // Get current language (default to 'en')
  var inputElement = document.getElementById(elementId);
  var originalText = inputElement.value;

  alert('Language: ' + lang + ', Text: ' + originalText);
}
