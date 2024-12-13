using RATSP.Common.Interfaces;
using RATSP.Common.Services;
using RATSP.GrossService.Services;
using StackExchange.Redis;

namespace RATSP.GrossService;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Добавляем сервисы в контейнер
                services.AddHostedService<Worker>();

                // Добавляем HttpClient для работы с API
                services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });
                
                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var redisConnection = configuration.GetSection("Redis")["Connection"];
                    
                    // Create a ConfigurationOptions object
                    var options = ConfigurationOptions.Parse(redisConnection);
                    options.AbortOnConnectFail = false; // Set AbortOnConnectFail to false
                    
                    return ConnectionMultiplexer.Connect(redisConnection);
                });


                services.AddSingleton<IRedisService, RedisService>();
                
                services.AddTransient<ICompaniesService, CompaniesService>();
                services.AddTransient<IFractionsService, FractionsService>();
                services.AddTransient<ExcelService>();
                services.AddTransient<ExcelValuesService>();

                // Настройка Kafka Consumer с передачей всех необходимых зависимостей
                services.AddSingleton(sp => new serviceKafkaConsumer(
                    sp.GetRequiredService<IConfiguration>(),
                    "consumer-group-id",                     // GroupId
                    "excel-topic",                           // Topic
                    sp.GetRequiredService<ExcelService>(),   
                    sp.GetRequiredService<ExcelValuesService>(), 
                    sp.GetRequiredService<ICompaniesService>(),  
                    sp.GetRequiredService<IFractionsService>(),
                    sp.GetRequiredService<IRedisService>()
                ));
            })
            .Build();

        host.Run();
    }
}