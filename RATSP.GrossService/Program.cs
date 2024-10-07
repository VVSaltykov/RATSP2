using RATSP.Common.Interfaces;
using RATSP.Common.Services;
using RATSP.GrossService.Services;

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
                services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });

                services.AddTransient<ICompaniesService, CompaniesService>();
                services.AddTransient<IFractionsService, FractionsService>();
                services.AddTransient<ExcelService>();
                services.AddTransient<ExcelValuesService>();

                // Настройка Kafka Consumer с передачей всех необходимых зависимостей
                services.AddSingleton(sp => new KafkaConsumer(
                    "localhost:9093",                        // BootstrapServers
                    "consumer-group-id",                     // GroupId
                    "excel-topic",                           // Topic
                    sp.GetRequiredService<ExcelService>(),   // ExcelService
                    sp.GetRequiredService<ExcelValuesService>(), // ExcelValuesService
                    sp.GetRequiredService<ICompaniesService>(),  // ICompaniesService
                    sp.GetRequiredService<IFractionsService>()   // IFractionsService
                ));
            })
            .Build();

        host.Run();
    }
}