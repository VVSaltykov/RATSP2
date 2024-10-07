using RATSP.GrossService.Services;

namespace RATSP.GrossService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly KafkaConsumer kafkaConsumer;

    public Worker(ILogger<Worker> logger, KafkaConsumer kafkaConsumer)
    {
        _logger = logger;
        this.kafkaConsumer = kafkaConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        kafkaConsumer.StartConsuming();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}