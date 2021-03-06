using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DataAccessLibrary.Data;
using Serilog;
using System.IO;
using DataModel.Model;
using System.Net.Http;
using System.Diagnostics;
using System.Text.Json;
using Serilog.Core;

namespace dota2WebApi
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

            //Serilog.Debugging.SelfLog.Enable(msg => { Debug.Print(msg); Debugger.Break(); });

            try
            {
                

                //CreateHostBuilder(args).Build().Run();
                var host = CreateHostBuilder(args).Build();
                // El siguiente servicio solo lo a�adimos cuando la base de datos no existe.
                CreateDbIfNotExists(host);
                Log.Warning("Arrancan los motores...");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Dota2AppDbContext>();
                    
                    //DbInitialize dbi = new DbInitialize();
                    //DbInitialize.Initialize(context);
                    //await DbInitialize.GetDotaImages();
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseConfiguration(Configuration)
                    .UseSerilog(); ;
                });
    }
}
