using DataAccessLibrary.Data;
using DataAccessLibrary.ValveJsonModel.Current.GetAbilities;
using DataAccessLibrary.ValveJsonModel.Current.GetHeroes;
using DataAccessLibrary.ValveJsonModel.Current.GetItems;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.ValveFileImportation
{
    class ValveFilesImporterCreatorV7_29 : AbstractValveFilesImporterCreator
    {
        public override abstractValveFilesImporter FactoryMethod(string pathJson, Dota2AppDbContext context, PatchVersion cpv)
        {
            return new ValveFilesImporterV7_29(pathJson, context, cpv);
        }
    }

    public class ValveFilesImporterV7_29 : abstractValveFilesImporter
    {
        public ValveFilesImporterV7_29(string pathJson, Dota2AppDbContext context, PatchVersion cpv) : base(pathJson, context, cpv) { }

        protected override string pathJson { get; set; }
        protected override Dota2AppDbContext context { get; set; }
        protected override PatchVersion cpv { get; set; }

        public override void InitializeAbilities()
        {
            try
            {
                PatchVersion contextCpv = context.PatchVersions.Where(pv => pv.PatchVersionId == cpv.PatchVersionId).ToList()[0];
                AbilitylistResponseModel alrm = null;
                string pathJsonHeroAbilities = String.Concat(pathJson, "\\abilitylist.json");
                if (File.Exists(pathJsonHeroAbilities))
                {
                    string jsonString = File.ReadAllText(pathJsonHeroAbilities);
                    var serializeOptions = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    serializeOptions.Converters.Add(new AbilitylistJsonConverter());
                    alrm = JsonSerializer.Deserialize<AbilitylistResponseModel>(jsonString, serializeOptions);

                    foreach (Ability a in alrm.result.data.itemabilities)
                    {
                        pathJsonHeroAbilities = string.Concat(pathJson, "abilities\\", a.AbilityId, " - ", a.Name, ".json");
                        jsonString = File.ReadAllText(pathJsonHeroAbilities);
                        serializeOptions = new JsonSerializerOptions
                        {
                            ReadCommentHandling = JsonCommentHandling.Skip
                        };
                        serializeOptions.Converters.Add(new AbilitydetailJsonConverter());
                        AbilitydetailResponseModel adrm = JsonSerializer.Deserialize<AbilitydetailResponseModel>(jsonString, serializeOptions);
                        if (adrm != null && adrm.result != null && adrm.result.data != null && adrm.result.data.abilities.Count == 1)
                        { 
                            adrm.result.data.abilities[0].PatchVersion = contextCpv;
                            context.Abilities.Add(adrm.result.data.abilities[0]);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override void InitializeHeroes()
        {
            try
            {
                PatchVersion contextCpv = context.PatchVersions.Where(pv => pv.PatchVersionId == cpv.PatchVersionId).ToList()[0];
                HerolistResponseModel alrm = null;
                string pathJsonHeroAbilities = String.Concat(pathJson, "\\herolist.json");
                if (File.Exists(pathJsonHeroAbilities))
                {
                    string jsonString = File.ReadAllText(pathJsonHeroAbilities);
                    var serializeOptions = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    serializeOptions.Converters.Add(new HerolistJsonConverter());
                    alrm = JsonSerializer.Deserialize<HerolistResponseModel>(jsonString, serializeOptions);

                    foreach (Hero a in alrm.result.data.heroes)
                    {
                        pathJsonHeroAbilities = string.Concat(pathJson, "heroes\\", a.HeroId, " - ", a.ShortName, ".json");
                        jsonString = File.ReadAllText(pathJsonHeroAbilities);
                        serializeOptions = new JsonSerializerOptions
                        {
                            ReadCommentHandling = JsonCommentHandling.Skip
                        };
                        serializeOptions.Converters.Add(new HeroDetailJsonConverter());
                        HeroDetailResponseModel adrm = JsonSerializer.Deserialize<HeroDetailResponseModel>(jsonString, serializeOptions);
                        if (adrm != null && adrm.result != null && adrm.result.data != null && adrm.result.data.heroes.Count == 1)
                        {
                            adrm.result.data.heroes[0].PatchVersion = contextCpv;
                            context.Heroes.Add(adrm.result.data.heroes[0]);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override void InitializeItems()
        {
            try
            {
                PatchVersion contextCpv = context.PatchVersions.Where(pv => pv.PatchVersionId == cpv.PatchVersionId).ToList()[0];
                ItemlistResponseModel alrm = null;
                string pathJsonHeroAbilities = String.Concat(pathJson, "\\itemlist.json");
                if (File.Exists(pathJsonHeroAbilities))
                {
                    string jsonString = File.ReadAllText(pathJsonHeroAbilities);
                    var serializeOptions = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    serializeOptions.Converters.Add(new ItemlistJsonConverter());
                    alrm = JsonSerializer.Deserialize<ItemlistResponseModel>(jsonString, serializeOptions);

                    foreach (HeroItem hi in alrm.result.data.itemabilities)
                    {
                        pathJsonHeroAbilities = string.Concat(pathJson, "items\\", hi.HeroItemId, " - ", hi.ShortName, ".json");
                        jsonString = File.ReadAllText(pathJsonHeroAbilities);
                        serializeOptions = new JsonSerializerOptions
                        {
                            ReadCommentHandling = JsonCommentHandling.Skip
                        };
                        serializeOptions.Converters.Add(new ItemDetailJsonConverter());
                        ItemDetailResponseModel adrm = JsonSerializer.Deserialize<ItemDetailResponseModel>(jsonString, serializeOptions);
                        if (adrm != null && adrm.result != null && adrm.result.data != null && adrm.result.data.items.Count == 1)
                        {
                            adrm.result.data.items[0].PatchVersion = contextCpv;
                            context.HeroItems.Add(adrm.result.data.items[0]);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
