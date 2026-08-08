
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace MoviePlatform1L.Extentions
{
    public static class LocalizationExtentions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection Services)
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            const string defaultCulture = "ar";
            var supportedCultures = new[]
            {
                new CultureInfo("ar"),
                new CultureInfo("en")
            };

            Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });
            return Services;
        }
    }
}

