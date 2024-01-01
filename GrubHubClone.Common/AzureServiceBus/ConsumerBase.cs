using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;

namespace GrubHubClone.Common.AzureServiceBus;

public abstract class ConsumerBase<T> : BackgroundService
{
    private readonly IBusClient _busClient;

    protected abstract Task ProcessMessage(T message);

    public ConsumerBase(IBusClient busClient)
    {
        _busClient = busClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiver = _busClient.GetConsumer<T>();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ServiceBusReceivedMessage message = await receiver.ReceiveMessageAsync(cancellationToken: stoppingToken);

                if (message == null) continue;

                string decodedMessage = Encoding.UTF8.GetString(message.Body);

                var msg = JsonSerializer.Deserialize<T>(decodedMessage);

                if (msg == null) continue;

                await ProcessMessage(msg);

                await receiver.CompleteMessageAsync(message, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // This exception is thrown when stopping the background service
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while receiving message: {ex.Message}");
            }
        }
    }
}
