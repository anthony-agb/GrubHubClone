using Microsoft.Extensions.DependencyInjection;

namespace GrubHubClone.Common.AzureServiceBus;

public static class DependecyInjectionAzureServiceBus
{
    public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, Action<BusClientConfiguration> configure)
    {
        var config = new BusClientConfiguration(services);
        configure(config);

        services.AddSingleton<IBusClient>(provider => new BusClient(config.ConnectionString));

        return services;
    }
}
