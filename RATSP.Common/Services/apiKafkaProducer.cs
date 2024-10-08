using Confluent.Kafka;

namespace RATSP.Common.Services;

public class apiKafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public apiKafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendMessageAsync(string topic, string message)
    {
        try
        {
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}