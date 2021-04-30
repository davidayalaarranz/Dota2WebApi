//using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Globalization;
using DataModel.Model;
using System.Text.RegularExpressions;
using DataModel;
using Microsoft.Extensions.Configuration;
using DataModel.Common;
using static DataAccessLibrary.Data.AppConfiguration;
using System.Threading.Tasks;
using Serilog;
using System.Net.Http;
using DataAccessLibrary.ValveFileImportation;
using DataAccessLibrary.ValveJsonModel.Current.GetItems;
using DataAccessLibrary.ValveJsonModel.Current.GetHeroes;
using DataAccessLibrary.ValveJsonModel.Current.GetAbilities;
using DataAccessLibrary.ValveJsonModel.v7_28.GetMatchHistory;

namespace DataAccessLibrary.Data
{
    public class DbInitialize
    {
        private static void InitializeAppConfiguration(Dota2AppDbContext context)
        {
            var AppConfigurationItems = new AppConfigurationItem[]
            {
                new AppConfigurationItem { Key = "CurrentDotaPatchVersion", Value = "7.29b" }
            };
            foreach (AppConfigurationItem aci in AppConfigurationItems)
            {
                context.AppConfiguration.Add(aci);
            }
            context.SaveChanges();
        }
        private static void InitializeDotaPatchVersion(Dota2AppDbContext context)
        {
            var PatchVersions = new PatchVersion[]
            {
                new PatchVersion { Name = "7.27d", Description = "Una versión de dota", Changes = "", ReleaseDate = new DateTime(2020,8,13) },
                new PatchVersion { Name = "7.28a", Description = "Otra versión de dota", Changes = "", ReleaseDate = new DateTime(2020,12,22) },
                new PatchVersion { Name = "7.29a", Description = "Una versión de dota", Changes = "", ReleaseDate = new DateTime(2021,3,28) },
                new PatchVersion { Name = "7.29b", Description = "Una versión de dota", Changes = "", ReleaseDate = new DateTime(2021,3,28) }
            };
            foreach (PatchVersion pv in PatchVersions)
            {
                context.PatchVersions.Add(pv);
            }
            context.SaveChanges();
        }

        public void InitializeMatches(string pathJson, Dota2AppDbContext context, PatchVersion patchVersion)
        {
            JsonDocumentOptions jsonOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            string pathJsonMatchHistory = String.Concat(pathJson, "\\GetMatchHistory.json");
            GetMatchHistoryResponseModel o;
            GetMatchDetailsResponseModel md = null;
            if (File.Exists(pathJsonMatchHistory))
            {
                string jsonString = File.ReadAllText(pathJsonMatchHistory);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new GetMatchHistoryJsonConverter());
                o = JsonSerializer.Deserialize<GetMatchHistoryResponseModel>(jsonString, serializeOptions);

                string pathJsonMatchDetails = String.Concat(pathJson, "\\GetMatchDetails.json");
                if (File.Exists(pathJsonMatchDetails))
                {
                    jsonString = File.ReadAllText(pathJsonMatchDetails);

                    serializeOptions = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    serializeOptions.Converters.Add(new GetMatchDetailsJsonConverter());
                    md = JsonSerializer.Deserialize<GetMatchDetailsResponseModel>(jsonString, serializeOptions);
                }

                foreach (MatchPlayer mp in md.result.MatchPlayers)
                {
                    mp.Hero = context.Heroes.First(h => h.HeroId == mp.Hero.HeroId);
                    mp.Player = md.result.MatchPlayers.First(p => p.PlayerId == mp.Player.PlayerId).Player;

                    foreach (MatchPlayerAbilityUpgrade mpau in mp.HeroUpgrades)
                    {
                        mpau.PatchVersionId = patchVersion.PatchVersionId;
                    }
                    //foreach (MatchPlayerHeroItemUpgrade mphiu in mp.HeroItemUpgrades)
                    //{
                    //    mphiu.PatchVersionId = patchVersion.PatchVersionId;
                    //}
                }
                context.Matches.Add(md.result);
                context.SaveChanges();
            }
        }

        public async static Task<int> GetValveJsonFiles()
        {
            int ret = 0;
            string valveFilesFolderPath = string.Concat("ValveFiles/", AppConfiguration.CurrentDotaPatchVersion.Name, "/");
            if (!Directory.Exists(valveFilesFolderPath))
            {
                Directory.CreateDirectory(valveFilesFolderPath);
            }

            string valveFilePath = string.Concat(valveFilesFolderPath, "herolist.json");
            await GetRemoteFile("https://www.dota2.com/datafeed/herolist?language=english", valveFilePath);
            if (File.Exists(valveFilePath))
            {
                await GetHeroesFiles(valveFilePath, string.Concat(valveFilesFolderPath, "heroes/"));
                ret++;
            }

            valveFilePath = string.Concat(valveFilesFolderPath, "itemlist.json");
            await GetRemoteFile("https://www.dota2.com/datafeed/itemlist?language=english", valveFilePath);
            if (File.Exists(valveFilePath))
            {
                await GetItemsFiles(valveFilePath, string.Concat(valveFilesFolderPath, "items/"));
                ret++;
            }

            valveFilePath = string.Concat(valveFilesFolderPath, "abilitylist.json");
            await GetRemoteFile("https://www.dota2.com/datafeed/abilitylist?language=english", valveFilePath);
            if (File.Exists(valveFilePath))
            {
                await GetIAbilitiesFiles(valveFilePath, string.Concat(valveFilesFolderPath, "abilities/"));
                ret++;
            }
            return ret;
        }

        public async static Task<int> GetIAbilitiesFiles(string valveFilePath, string targetFolder)
        {
            int ret = 0;
            AbilitylistResponseModel o;
            if (File.Exists(valveFilePath))
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string jsonString = File.ReadAllText(valveFilePath);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new AbilitylistJsonConverter());
                o = JsonSerializer.Deserialize<AbilitylistResponseModel>(jsonString, serializeOptions);
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    foreach (Ability hi in o.result.data.itemabilities)
                    {
                        try
                        {
                            if (!File.Exists(string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json")))
                            {
                                await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/abilitydata?language=english&ability_id=", hi.AbilityId.ToString()), string.Concat(targetFolder, hi.AbilityId.ToString(), " - ", hi.Name, ".json"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, string.Concat("Error obteniendo fichero json de ability: ", hi.AbilityId.ToString()));
                        }
                    }
                    ret = 1;
                }
            }
            return ret;
        }

        public async static Task<int> GetItemsFiles(string valveFilePath, string targetFolder)
        {
            int ret = 0;
            ItemlistResponseModel o;
            if (File.Exists(valveFilePath))
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string jsonString = File.ReadAllText(valveFilePath);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new ItemlistJsonConverter());
                o = JsonSerializer.Deserialize<ItemlistResponseModel>(jsonString, serializeOptions);
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    foreach (HeroItem hi in o.result.data.itemabilities)
                    {
                        try
                        {

                            if (!File.Exists(string.Concat(targetFolder, hi.HeroItemId.ToString(), " - ", hi.ShortName, ".json")))
                            {
                                await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/itemdata?language=english&item_id=", hi.HeroItemId.ToString()), string.Concat(targetFolder, hi.HeroItemId.ToString(), " - ", hi.ShortName, ".json"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, string.Concat("Error obteniendo fichero json de item: ", hi.HeroItemId.ToString()));
                        }
                    }
                    ret = 1;
                }
            }
            return ret;
        }

        public async static Task<int> GetHeroesFiles(string valveFilePath, string targetFolder)
        {
            int ret = 0;
            HerolistResponseModel o;
            if (File.Exists(valveFilePath))
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string jsonString = File.ReadAllText(valveFilePath);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new HerolistJsonConverter());
                o = JsonSerializer.Deserialize<HerolistResponseModel>(jsonString, serializeOptions);
                foreach (Hero h in o.result.data.heroes)
                {
                    try
                    {
                        if (!File.Exists(string.Concat(targetFolder, h.HeroId.ToString(), " - ", h.ShortName, ".json")))
                        {
                            await GetRemoteFile(string.Concat("https://www.dota2.com/datafeed/herodata?language=english&hero_id=", h.HeroId.ToString()), string.Concat(targetFolder, h.HeroId.ToString(), " - ", h.ShortName, ".json"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, string.Concat("Error obteniendo fichero json de héroe: ", h.HeroId.ToString()));
                    }
                }
                ret = 1;
            }
            return ret;
        }

        public async static Task<int> GetRemoteFile(string url, string path)
        {
            int ret = 0;
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                using (Stream s = await client.GetStreamAsync(url))
                {

                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                    s.CopyTo(fs);
                    await fs.FlushAsync();
                    fs.Close();
                    ret = 1;
                }
            }
            return ret;
        }
        
        public static void Initialize(Dota2AppDbContext context)
        {
            try
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                if (context.Heroes.Any())
                    return;

                InitializeDotaPatchVersion(context);
                InitializeAppConfiguration(context);

                //InitializeV7_27d(context);
                //InitializeV7_28a(context);
                //InitializeV7_29a(context);
                InitializeV7_29b(context);
                return;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public static void InitializeV7_29b(Dota2AppDbContext context)
        {
            PatchVersion cpv = (from a in context.PatchVersions select a)
                    .Where(x => x.Name == "7.29b")
                    .First();
            string pathJson = Path.GetFullPath(string.Concat("ValveFiles/", cpv.Name, "/"));

            AbstractValveFilesImporterCreator fic = new ValveFilesImporterCreatorV7_29();
            abstractValveFilesImporter fi = fic.FactoryMethod(pathJson, context, cpv);
            fi.InitializeAbilities();
            fi.InitializeHeroes();
            //fi.InitializeItems();
            //InitializeMatches(pathJson, context, cpv);
        }

        public static void InitializeV7_29a(Dota2AppDbContext context)
        {
            PatchVersion cpv = (from a in context.PatchVersions select a)
                    .Where(x => x.Name == "7.29a")
                    .First();
            string pathJson = Path.GetFullPath(string.Concat("ValveFiles/", cpv.Name, "/"));

            AbstractValveFilesImporterCreator fic = new ValveFilesImporterCreatorV7_29();
            abstractValveFilesImporter fi = fic.FactoryMethod(pathJson, context, cpv);
            fi.InitializeAbilities();
            fi.InitializeHeroes();
            fi.InitializeItems();
            //InitializeMatches(pathJson, context, cpv);
        }

        public static void InitializeV7_28a(Dota2AppDbContext context)
        {
            PatchVersion cpv = (from a in context.PatchVersions select a)
                    .Where(x => x.Name == "7.28a")
                    .First();
            string pathJson = Path.GetFullPath(string.Concat("ValveFiles/", cpv.Name, "/"));

            AbstractValveFilesImporterCreator fic = new ValveFilesImporterCreatorV7_28();
            abstractValveFilesImporter fi = fic.FactoryMethod(pathJson, context, cpv);
            fi.InitializeAbilities();
            fi.InitializeHeroes();
            fi.InitializeItems();
            //InitializeMatches(pathJson, context, cpv);
        }

        public static void InitializeV7_27d(Dota2AppDbContext context)
        {
            PatchVersion cpv = (from a in context.PatchVersions select a)
                .Where(x => x.Name == "7.27d")
                .First();
            
            string pathJson = Path.GetFullPath(string.Concat("ValveFiles/", cpv.Name, "/"));
            AbstractValveFilesImporterCreator fic = new ValveFilesImporterCreatorV7_28();
            abstractValveFilesImporter fi = fic.FactoryMethod(pathJson, context, cpv);
            fi.InitializeAbilities();
            fi.InitializeHeroes();
            fi.InitializeItems();
            //InitializeMatches(pathJson, context, cpv);
        }

        public async static Task<int> GetDotaImages()
        {
            int ret = 0;
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {

                string itemImageFolderPath = "images/7.29b/items/";
                if (!Directory.Exists(itemImageFolderPath))
                {
                    Directory.CreateDirectory(itemImageFolderPath);
                }
                string heroImageFolderPath = "images/7.29b/heroes/";
                if (!Directory.Exists(heroImageFolderPath))
                {
                    Directory.CreateDirectory(heroImageFolderPath);
                }
                string abilityImageFolderPath = "images/7.29b/abilities/";
                if (!Directory.Exists(abilityImageFolderPath))
                {
                    Directory.CreateDirectory(abilityImageFolderPath);
                }

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    List<HeroItem> lhi = (from a in db.HeroItems
                                          select a)
                                          .Where(hi => hi.PatchVersionId == 4).ToList();
                    foreach (HeroItem hi in lhi)
                    {
                        try
                        {
                            using (Stream s = await client.GetStreamAsync(string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/", hi.ShortName, "_lg.png")))
                            {

                                FileStream fs = new FileStream(string.Concat(itemImageFolderPath, hi.ShortName, "_lg.png"), FileMode.OpenOrCreate);
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
                                     select a).Where(hi => hi.PatchVersionId == 4).ToList();
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
                                        select a).Where(hi => hi.PatchVersionId == 4).ToList();
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
                    ret = 1;
                }
                return ret;
            }
        }
    }
}
