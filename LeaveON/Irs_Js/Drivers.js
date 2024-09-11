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
});
