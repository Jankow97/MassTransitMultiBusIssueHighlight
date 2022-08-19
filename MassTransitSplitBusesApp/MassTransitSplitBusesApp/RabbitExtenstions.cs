
using MassTransit;

namespace MassTransitSplitBusesApp;

internal static class RabbitExtensions
{
    public static IServiceCollection AddQueueManagement(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TestConsumer>()
               .Endpoint(e =>
               {
                   e.Name = "test-consumer";
                   e.Temporary = false;
                   e.ConcurrentMessageLimit = 1;
                   e.InstanceId = "someid";
               });

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host($"localhost", 5672, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransit<ISecondBus>(x =>
        {
            x.AddConsumer<TestConsumer2>()
               .Endpoint(e =>
               {
                   e.Name = "test-consumer-2";
                   e.Temporary = false;
                   e.ConcurrentMessageLimit = 1;
                   e.InstanceId = "someid";
               });

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host($"localhost", 5673, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public interface ISecondBus :
        IBus
    {
    }

    public class TestMessage2
    {
        public string Message2 { get; set; }
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
        public string Message { get; set; }
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
}
