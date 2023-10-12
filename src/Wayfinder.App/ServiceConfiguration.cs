using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Wayfinder.App.Features.RequiredUnits;

namespace Wayfinder.App;

public static class ServiceConfiguration
{
    public static IServiceCollection AddWayfinder(this IServiceCollection services) =>
        services
            .AddMudServices()
            .AddLocalization()
            .AddTransient<RequiredRelicUnitsViewModel>();
}
