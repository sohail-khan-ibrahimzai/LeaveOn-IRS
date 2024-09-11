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
});
