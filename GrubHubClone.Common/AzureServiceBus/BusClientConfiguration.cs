using Microsoft.Extensions.DependencyInjection;

namespace GrubHubClone.Common.AzureServiceBus;

public class BusClientConfiguration
{
    private readonly IServiceCollection _collection;

    public BusClientConfiguration(IServiceCollection collection)
    {
        _collection = collection;
    }


    public string? ConnectionString { get; set; } = string.Empty;

    //public void AddConsumer<T>() where T : class, IHostedService
    //{
    //    _collection.AddHostedService<T>();
    //}
}
