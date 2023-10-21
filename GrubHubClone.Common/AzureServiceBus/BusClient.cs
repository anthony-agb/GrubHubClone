using Azure.Messaging.ServiceBus;
using GrubHubClone.Common.Utilities;
using System.Text;
using System.Text.Json;

namespace GrubHubClone.Common.AzureServiceBus;

public class BusClient : IBusClient
{
    private readonly ServiceBusClient _client;

    private readonly Dictionary<Type, ServiceBusSender> _senderQueues;
    private readonly Dictionary<Type, ServiceBusReceiver> _consumerQueues;

    public BusClient(string? connectionString)
    {
        _client = Connect(connectionString);
        _senderQueues = new();
        _consumerQueues = new();
    }

    private ServiceBusClient Connect(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new NullReferenceException("The Azure Service Bus connection string is null.");
        }

        return new ServiceBusClient(connectionString);
    }

    public async Task PublishAsync<T>(T message)
    {
        bool senderQueueExists = _senderQueues.ContainsKey(typeof(T));

        if (!senderQueueExists)
        {
            var queueName = StringTransformer.FormatClassToQueueName(typeof(T), true);

            var sdr = _client.CreateSender(queueName);

            _senderQueues.Add(typeof(T), sdr);
        }

        var msgBody = JsonSerializer.Serialize(message);

        var msg = new ServiceBusMessage(Encoding.UTF8.GetBytes(msgBody));

        var sender = _senderQueues[typeof(T)];

        await sender.SendMessageAsync(msg);
    }

    public ServiceBusReceiver GetConsumer<T>()
    {
        bool senderQueueExists = _consumerQueues.ContainsKey(typeof(T));

        if (!senderQueueExists)
        {
            var queueName = StringTransformer.FormatClassToQueueName(typeof(T), true);

            var options = new ServiceBusReceiverOptions
            {
                PrefetchCount = 1,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            };

            var rcv = _client.CreateReceiver(queueName, options);

            _consumerQueues.Add(typeof(T), rcv);
        }

        return _consumerQueues[typeof(T)];
    }
}
