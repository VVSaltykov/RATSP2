using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RATSP.Common.Interfaces;
using RATSP.Common.Services;

namespace RATSP.WebClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7173") });
        
        builder.Services.AddTransient<ICompaniesService, CompaniesService>();
        
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        
        builder.Services.AddRadzenComponents();
        
        await builder.Build().RunAsync();
    }
}