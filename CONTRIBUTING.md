@using SafeCasino
@using SafeCasino.Models
@using SafeCasino.Services
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

@* 
    BELANGRIJK: In ASP.NET Core kunnen we GEEN @helper gebruiken in _ViewImports!
    We moeten de functies in elke view definiëren OF inline code gebruiken.
    
    Deze variabelen worden beschikbaar gemaakt voor alle views:
*@

@inject ILocalizationService localizationService

@{
    var currentLanguage = ViewContext.HttpContext.Request.Cookies["language"] ?? "nl";
}

@functions {
    // Expose a simple Localize helper to all views so they can call @Localize("Key")
    // Uses the injected localizationService and currentLanguage computed above.
    public string Localize(string key) => localizationService?.GetString(key, currentLanguage) ?? key;

    // Allow views to read the current language
    public string GetLanguage() => currentLanguage;
}