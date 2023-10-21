using Azure.Messaging.ServiceBus;

namespace GrubHubClone.Common.AzureServiceBus;

public interface IBusClient
{
    ServiceBusReceiver GetConsumer<T>();
    Task PublishAsync<T>(T message);
}