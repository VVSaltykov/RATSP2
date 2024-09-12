using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RATSP.Common.Interfaces;
using RATSP.Common.Services;
using RATSP.GrossService.Services;

namespace RATSP.GrossService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавляем сервисы в контейнер
        builder.Services.AddHostedService<Worker>();

        // Добавляем HttpClient для работы с API
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });

        // Добавляем кастомные сервисы
        builder.Services.AddTransient<ICompaniesService, CompaniesService>();
        builder.Services.AddTransient<IFractionsService, FractionsService>();

        builder.Services.AddTransient<ExcelService>();
        builder.Services.AddTransient<ExcelValuesService>();

        // Добавляем поддержку gRPC
        builder.Services.AddGrpc();

        // Настраиваем CORS для поддержки вызовов с клиента Blazor
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                policy =>
                {
                    policy.WithOrigins("https://localhost:7197") // Адрес вашего клиента
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("grpc-status", "grpc-message", "grpc-encoding", "grpc-accept-encoding"); // Важные заголовки
                });
        });

        var app = builder.Build();

        // Включаем использование CORS
        app.UseCors("AllowSpecificOrigins");

        app.UseGrpcWeb();

        // Включаем маршрутизацию для gRPC сервисов
        app.MapGrpcService<ExcelServiceImpl>().EnableGrpcWeb();           // Регистрация gRPC сервисов
        app.MapGrpcService<ExcelValuesServiceImpl>().EnableGrpcWeb();     // Регистрация gRPC сервисов

        // Маршрут по умолчанию
        app.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("gRPC service is running.");
        });

        // Запуск приложения
        app.Run();
    }
}