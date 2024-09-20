//function loadGoogleTranslate() {
//  new google.translate.TranslateElement("google-element")
//}
//function loadGoogleTranslate() {
//  new google.translate.TranslateElement({
//    pageLanguage: 'en', // Set the default language of your page (if it's English)
//    includedLanguages: 'en,el', // Only include English (en) and Greek (el)
//    layout: google.translate.TranslateElement.InlineLayout.SIMPLE
//  }, 'google-element');
//}
function loadGoogleTranslate() {
  // Initialize Google Translate Element
  new google.translate.TranslateElement({
    pageLanguage: 'en', // Default page language (English)
    includedLanguages: 'en,el', // Only allow English (en) and Greek (el)
    layout: google.translate.TranslateElement.InlineLayout.SIMPLE
  }, 'google-element');

  // Monitor language changes after a short delay
  setInterval(checkLanguageChange, 1000); // Check every second
}

// Function to check Google Translate language change and update <html lang="">
function checkLanguageChange() {
  // Get the current translation language from the cookie
  var currentLang = getCookie('googtrans') ? getCookie('googtrans').split('/')[2] : 'en';
  // Update the <html> lang attribute dynamically
  if (currentLang) {
    document.documentElement.setAttribute('lang', currentLang);
  }
}

// Helper function to get cookie value by name
function getCookie(name) {
  var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  if (match) return match[2];
  return null;
}
