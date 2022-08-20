using MassTransit;

namespace MassTransitSplitBusesApp.MassTransit;

public interface ISecondBus :
    IBus
{
}

public class TestMessage2
{
    public string? Message2 { get; set; }
}

public class TestConsumer2 : IConsumer<TestMessage2>
{
    public static int RequestCount;

    public Task Consume(ConsumeContext<TestMessage2> context)
    {
        RequestCount++;
        return Task.CompletedTask;
    }
}

public class TestMessage
{
    public string? Message { get; set; }
}

public class TestConsumer : IConsumer<TestMessage>
{
    public static int RequestCount;

    public Task Consume(ConsumeContext<TestMessage> context)
    {
        RequestCount++;
        return Task.CompletedTask;
    }
}
