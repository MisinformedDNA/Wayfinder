using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Wayfinder.App;

public static class ServiceConfiguration
{
    public static IServiceCollection AddWayfinder(this IServiceCollection services) =>
        services
            .AddMudServices()
            .AddLocalization();
}
