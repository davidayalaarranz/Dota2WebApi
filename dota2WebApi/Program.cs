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
                // El siguiente servicio solo lo añadimos cuando la base de datos no existe.
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
                    DbInitialize dbi = new DbInitialize();
                    dbi.Initialize(context);
                    
                    //DbInitialize.Initialize(context);
                    //GetDotaImages();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
        private async static void GetDotaImages()
        {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                
                string itemImageFolderPath = "images/7.28a/items/";
                if (!Directory.Exists(itemImageFolderPath))
                {
                    Directory.CreateDirectory(itemImageFolderPath);
                }
                string heroImageFolderPath = "images/7.28a/heroes/";
                if (!Directory.Exists(heroImageFolderPath))
                {
                    Directory.CreateDirectory(heroImageFolderPath);
                }
                string abilityImageFolderPath = "images/7.28a/abilities/";
                if (!Directory.Exists(abilityImageFolderPath))
                {
                    Directory.CreateDirectory(abilityImageFolderPath);
                }

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    List<HeroItem> lhi = (from a in db.HeroItems
                                          select a).ToList();
                    foreach (HeroItem hi in lhi)
                    {
                        try
                        {
                            using (Stream s = await client.GetStreamAsync(string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/", hi.ImagePath)))
                            {

                                FileStream fs = new FileStream(string.Concat(itemImageFolderPath, hi.ImagePath), FileMode.OpenOrCreate);
                                s.CopyTo(fs);
                                await fs.FlushAsync();
                                fs.Close();
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Log.Warning(hre, string.Concat("Imagen de item no encontrada. HeroItemID = ", hi.HeroItemId.ToString()));
                        }
                    }

                    List<Hero> lh = (from a in db.Heroes
                                     select a).ToList();
                    foreach (Hero h in lh)
                    {
                        try
                        {
                            using (Stream s = await client.GetStreamAsync(string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/", h.ImageUrl)))
                            {

                                FileStream fs = new FileStream(string.Concat(heroImageFolderPath, h.ImageUrl), FileMode.OpenOrCreate);
                                s.CopyTo(fs);
                                await fs.FlushAsync();
                                fs.Close();
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Log.Warning(hre, string.Concat("Imagen de héroe no encontrada. HeroID = ", h.HeroId.ToString()));
                        }
                        try
                        {
                            using (Stream s = await client.GetStreamAsync(string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/", h.VerticalImageUrl)))
                            {

                                FileStream fs = new FileStream(string.Concat(heroImageFolderPath, h.VerticalImageUrl), FileMode.OpenOrCreate);
                                s.CopyTo(fs);
                                await fs.FlushAsync();
                                fs.Close();
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Log.Warning(hre, string.Concat("Imagen de héroe no encontrada. HeroID = ", h.HeroId.ToString()));
                        }
                    }

                    List<Ability> la = (from a in db.Abilities
                                          select a).ToList();
                    foreach (Ability a in la)
                    {
                        try
                        {
                            using (Stream s = await client.GetStreamAsync(string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/abilities/", a.ImageUrl)))
                            {

                                FileStream fs = new FileStream(string.Concat(abilityImageFolderPath, a.ImageUrl), FileMode.OpenOrCreate);
                                s.CopyTo(fs);
                                await fs.FlushAsync();
                                fs.Close();
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Log.Warning(hre, string.Concat("Imagen de habilidad no encontrada. AbilityID = ", a.AbilityId.ToString()));
                        }
                    }
                }
                return;
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
