//function loadGoogleTranslate() {
//  new google.translate.TranslateElement("google-element")
//}
function loadGoogleTranslate() {
  new google.translate.TranslateElement({
    pageLanguage: 'en', // Set the default language of your page (if it's English)
    includedLanguages: 'en,el', // Only include English (en) and Greek (el)
    layout: google.translate.TranslateElement.InlineLayout.SIMPLE
  }, 'google-element');
}
