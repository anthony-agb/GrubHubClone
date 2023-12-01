using GrubHubClone.Common.AzureServiceBus;

namespace GrubHubClone.Order.Consumers;

public class TestConsumer : ConsumerBase<TestCon>
{
    public TestConsumer(IBusClient busClient) : base(busClient) { }

    protected override Task ProcessMessage(TestCon message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}
