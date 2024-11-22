using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using GoogleMapsComponents;
using Microsoft.FluentUI.AspNetCore.Components;
using Radzen;
using Un2TrekApp.Activities;
using Un2TrekApp.Authentication;
using Un2TrekApp.Storage;
using Un2TrekApp.Trekis;

namespace Un2TrekApp;

internal static class DependencyInjection
{
    public static IServiceCollection AddUn2TrekApp(this IServiceCollection services)
    {
        services.AddMauiBlazorWebView();
        services.AddExternalComponents();
        services.AddStorage();
        services.AddHttpClientServices();
        return services;
    }
    private static void AddExternalComponents(this IServiceCollection services)
    {
        services.AddFluentUIComponents();
        services.AddRadzenComponents();
        services.AddBlazorGoogleMaps("AIzaSyCmn5Breovcda8Ib8ah4TRR9kGdLj8aOzs");

        services.AddScoped<NotificationService>();

        services.AddBlazorise(options =>
           {
               options.Immediate = true;
           }).AddBootstrap5Providers()
           .AddFontAwesomeIcons();
    }

    private static void AddStorage(this IServiceCollection services)
    {
        services.AddTransient<ILocalStorage, LocalStorage>();
    }

    private static void AddHttpClientServices(this IServiceCollection services)
    {
        //string apiBaseUrl = "https://localhost:7211/api/v1/";
        string apiBaseUrl = "http://un2trek-api.sycapps.net/api/v1/";
        services.AddHttpClient<IAuthService, AuthService>(client =>
        {
            client.BaseAddress = new Uri($"{apiBaseUrl}authentication/");
        });

        services.AddHttpClient<IActivitiesService, ActivitiesService>(client =>
        {
            client.BaseAddress = new Uri($"{apiBaseUrl}activities/");
        });

        services.AddHttpClient<ITrekisService, TrekisService>(client =>
        {
            client.BaseAddress = new Uri($"{apiBaseUrl}trekis/");
        });
    }

    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection services, string key)
    {
        services.AddScoped<IBlazorGoogleMapsKeyService>(_ => new BlazorGoogleMapsKeyService(key));
        return services;
    }
}
