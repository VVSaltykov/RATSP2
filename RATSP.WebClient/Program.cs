using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RATSP.Common.Interfaces;
using RATSP.Common.Services;
using RATSP.GrossService.Services;

namespace RATSP.WebClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });
        
        builder.Services.AddScoped(services =>
        {
            // Создание канала для gRPC с помощью GrpcWebHandler
            //var baseUri = new Uri(builder.HostEnvironment.BaseAddress);
            var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

            return GrpcChannel.ForAddress("https://localhost:7123", new GrpcChannelOptions
            {
                HttpHandler = httpHandler,
                MaxReceiveMessageSize = 10 * 1024 * 1024, // Устанавливаем лимит на получение сообщения
                MaxSendMessageSize = 10 * 1024 * 1024 
            });
        });
        
        builder.Services.AddScoped<ExcelService.ExcelServiceClient>(services =>
        {
            var channel = services.GetRequiredService<GrpcChannel>();
            return new ExcelService.ExcelServiceClient(channel);
        });

        builder.Services.AddScoped<ExcelValuesService.ExcelValuesServiceClient>(services =>
        {
            var channel = services.GetRequiredService<GrpcChannel>();
            return new ExcelValuesService.ExcelValuesServiceClient(channel);
        });
        
        builder.Services.AddTransient<ICompaniesService, CompaniesService>();
        builder.Services.AddTransient<IFractionsService, FractionsService>();
        
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        
        builder.Services.AddRadzenComponents();
        
        await builder.Build().RunAsync();
    }
}