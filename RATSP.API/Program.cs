
using Microsoft.EntityFrameworkCore;
using RATSP.API.Repositories;

namespace RATSP.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
        });
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7197")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
        
        builder.Services.AddTransient<CompaniesRepository>();
        builder.Services.AddTransient<FractionRepository>();
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        app.UseCors("AllowSpecificOrigin");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
