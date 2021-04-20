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
using DataModel.ValveJsonModel.Current.GetHeroes;
using System.Text.Json;
using DataModel.ValveJsonModel.Current.GetItems;
using Serilog.Core;
using DataModel.ValveJsonModel.Current.GetAbilities;

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

        private static async void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Dota2AppDbContext>();

                    
                    DbInitialize dbi = new DbInitialize();
                    dbi.Initialize(context);
                    //await DbInitialize.GetValveJsonFiles();

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

        //private async static Task<int> GetValveJsonFiles()
        //{
        //    int ret = 0;
        //    string valveFilesFolderPath = string.Concat("ValveFiles/", AppConfiguration.CurrentDotaPatchVersion.Name, "/");
        //    if (!Directory.Exists(valveFilesFolderPath))
        //    {
        //        Directory.CreateDirectory(valveFilesFolderPath);
        //    }

        //     string valveFilePath = string.Concat(valveFilesFolderPath, "herolist.json");
        //    //await GetRemoteFile("https://www.dota2.com/datafeed/herolist?language=english", valveFilePath);
        //    if (File.Exists(valveFilePath))
        //    {
        //        await GetHeroesFiles(valveFilePath, string.Concat(valveFilesFolderPath, "heroes/"));
        //        ret++;
        //    }

        //    valveFilePath = string.Concat(valveFilesFolderPath, "itemlist.json");
        //    //await GetRemoteFile("https://www.dota2.com/datafeed/itemlist?language=english", valveFilePath);
        //    if (File.Exists(valveFilePath))
        //    {
        //        await GetItemsFiles(valveFilePath, string.Concat(valveFilesFolderPath, "items/"));
        //        ret++;
        //    }

        //    valveFilePath = string.Concat(valveFilesFolderPath, "abilitylist.json");
        //    //await GetRemoteFile("https://www.dota2.com/datafeed/abilitylist?language=english", valveFilePath);
        //    if (File.Exists(valveFilePath))
        //    {
        //        await GetIAbilitiesFiles(valveFilePath, string.Concat(valveFilesFolderPath, "abilities/"));
        //        ret++;
        //    }
        //    return ret;
        //}

        //private async static Task<int> GetIAbilitiesFiles(string valveFilePath, string targetFolder)
        //{
        //    int ret = 0;
        //    AbilitylistResponseModel o;
        //    if (File.Exists(valveFilePath))
        //    {
        //        if (!Directory.Exists(targetFolder))
        //        {
        //            Directory.CreateDirectory(targetFolder);
        //        }

        //        string jsonString = File.ReadAllText(valveFilePath);
        //        var serializeOptions = new JsonSerializerOptions
        //        {
        //            ReadCommentHandling = JsonCommentHandling.Skip
        //        };
        //        serializeOptions.Converters.Add(new AbilitylistJsonConverter());
        //        o = JsonSerializer.Deserialize<AbilitylistResponseModel>(jsonString, serializeOptions);
        //        using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
        //        {
        //            foreach (Ability hi in o.result.data.itemabilities)
        //            {
        //                try
        //                {
        //                    //if (!File.Exists(string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json")))
        //                    //{

        //                    //    using (Stream s = await client.GetStreamAsync(string.Concat("https://www.dota2.com/datafeed/abilitydata?language=english&ability_id=", hi.AbilityId.ToString())))
        //                    //    {

        //                    //        FileStream fs = new FileStream(string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json"), FileMode.OpenOrCreate);
        //                    //        s.CopyTo(fs);
        //                    //        await fs.FlushAsync();
        //                    //        fs.Close();
        //                    //    }
        //                    //}
        //                    if (!File.Exists(string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json")))
        //                    {
        //                        await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/abilitydata?language=english&ability_id=", hi.AbilityId.ToString()), string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json"));
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Log.Error(ex, string.Concat("Error obteniendo fichero json de ability: ", hi.AbilityId.ToString()));
        //                }
        //            }
        //            ret = 1;
        //        }
        //    }
        //    return ret;
        //}

        //private async static Task<int> GetItemsFiles(string valveFilePath, string targetFolder)
        //{
        //    int ret = 0;
        //    ItemlistResponseModel o;
        //    if (File.Exists(valveFilePath))
        //    {
        //        if (!Directory.Exists(targetFolder))
        //        {
        //            Directory.CreateDirectory(targetFolder);
        //        }

        //        string jsonString = File.ReadAllText(valveFilePath);
        //        var serializeOptions = new JsonSerializerOptions
        //        {
        //            ReadCommentHandling = JsonCommentHandling.Skip
        //        };
        //        serializeOptions.Converters.Add(new ItemlistJsonConverter());
        //        o = JsonSerializer.Deserialize<ItemlistResponseModel>(jsonString, serializeOptions);
        //        using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
        //        {
        //            foreach (HeroItem hi in o.result.data.itemabilities)
        //            {
        //                try
        //                {

        //                    if (!File.Exists(string.Concat(targetFolder, hi.HeroItemId.ToString(), " - ", hi.ShortName, ".json")))
        //                    { 
        //                        await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/itemdata?language=english&item_id=", hi.HeroItemId.ToString()), string.Concat(targetFolder, hi.HeroItemId.ToString(), " - ", hi.ShortName, ".json"));
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Log.Error(ex, string.Concat("Error obteniendo fichero json de item: ", hi.HeroItemId.ToString()));
        //                }
        //            }
        //            ret = 1;
        //        }
        //    }
        //    return ret;
        //}

        //private async static Task<int> GetHeroesFiles(string valveFilePath, string targetFolder)
        //{
        //    int ret = 0;
        //    HerolistResponseModel o;
        //    if (File.Exists(valveFilePath))
        //    {
        //        if (!Directory.Exists(targetFolder))
        //        {
        //            Directory.CreateDirectory(targetFolder);
        //        }

        //        string jsonString = File.ReadAllText(valveFilePath);
        //        var serializeOptions = new JsonSerializerOptions
        //        {
        //            ReadCommentHandling = JsonCommentHandling.Skip
        //        };
        //        serializeOptions.Converters.Add(new HerolistJsonConverter());
        //        o = JsonSerializer.Deserialize<HerolistResponseModel>(jsonString, serializeOptions);
        //        foreach (Hero h in o.result.data.heroes)
        //        {
        //            try
        //            {
        //                if (!File.Exists(string.Concat(targetFolder, h.HeroId.ToString(), " - ", h.ShortName, ".json")))
        //                {
        //                    await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/herodata?language=english&hero_id=", h.HeroId.ToString()), string.Concat(targetFolder, h.HeroId.ToString(), " - ", h.ShortName, ".json"));
        //                }
        //            }
        //            catch(Exception ex)
        //            {
        //                Log.Error(ex, string.Concat("Error obteniendo fichero json de héroe: ", h.HeroId.ToString()));
        //            }
        //        }
        //        ret = 1;
        //    }
        //    return ret;
        //}

        //private async static Task<int> GetRemoteFile(string url, string path)
        //{
        //    int ret = 0;
        //    using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
        //    {
        //        using (Stream s = await client.GetStreamAsync(url))
        //        {

        //            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        //            s.CopyTo(fs);
        //            await fs.FlushAsync();
        //            fs.Close();
        //            ret = 1;
        //        }
        //    }
        //    return ret;
        //}

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
