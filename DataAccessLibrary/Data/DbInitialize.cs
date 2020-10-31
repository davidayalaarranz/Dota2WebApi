//using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using DataModel.ValveJsonModel.GetHeroes;
using DataModel;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using DataModel.ValveJsonModel.GetItems;

namespace DataAccessLibrary.Data
{
    public static class DbInitialize
    {
        public static void InitializeHeroes(string pathJson, Dota2AppDbContext context)
        {
            string pathJsonHeroes = String.Concat(pathJson, "\\GetHeroes en_US.json");
            GetHeroesResponseModel o;
            if (File.Exists(pathJsonHeroes))
            {
                string jsonString = File.ReadAllText(pathJsonHeroes);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new GetHeroesJsonConverter());
                o = JsonSerializer.Deserialize<GetHeroesResponseModel>(jsonString, serializeOptions);

                string pathJsonHeroes2 = String.Concat(pathJson, "\\heroes.json");
                if (File.Exists(pathJsonHeroes2))
                {
                    jsonString = File.ReadAllText(pathJsonHeroes2);
                    JsonDocumentOptions jsonOptions = new JsonDocumentOptions
                    {
                        CommentHandling = JsonCommentHandling.Skip
                    };
                    JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString, jsonOptions).RootElement.GetProperty("herodata");
                    JsonElement.ObjectEnumerator ae = root.EnumerateObject();//.EnumerateArray();
                    NumberFormatInfo fp = new NumberFormatInfo();
                    fp.NumberDecimalSeparator = ".";

                    while (ae.MoveNext())
                    {
                        System.Text.Json.JsonProperty aux = ae.Current;
                        string name = aux.Name;
                        if (o != null)
                        {
                            Hero currentHero = o.result.heroes.Find(h => h.ShortName == name);
                            if (currentHero != null)
                            {
                                currentHero.Strength.Initial = aux.Value.GetProperty("attribs").GetProperty("str").GetProperty("b").GetDecimal();
                                currentHero.Strength.Gain = decimal.Parse(aux.Value.GetProperty("attribs").GetProperty("str").GetProperty("g").GetString(), fp);
                                currentHero.Inteligence.Initial = aux.Value.GetProperty("attribs").GetProperty("int").GetProperty("b").GetDecimal();
                                currentHero.Inteligence.Gain = decimal.Parse(aux.Value.GetProperty("attribs").GetProperty("int").GetProperty("g").GetString(), fp);
                                currentHero.Agility.Initial = aux.Value.GetProperty("attribs").GetProperty("agi").GetProperty("b").GetDecimal();
                                currentHero.Agility.Gain = decimal.Parse(aux.Value.GetProperty("attribs").GetProperty("agi").GetProperty("g").GetString(), fp);
                                currentHero.MovementSpeed = aux.Value.GetProperty("attribs").GetProperty("ms").GetInt32();
                                currentHero.MinDamage = aux.Value.GetProperty("attribs").GetProperty("dmg").GetProperty("min").GetInt32();
                                currentHero.MaxDamage = aux.Value.GetProperty("attribs").GetProperty("dmg").GetProperty("max").GetInt32();
                                currentHero.Armor = aux.Value.GetProperty("attribs").GetProperty("armor").GetDecimal();

                                switch (aux.Value.GetProperty("pa").GetString())
                                {
                                    case "str":
                                        currentHero.PrincipalAttribute = HeroPrincipalAttribute.Strength;
                                        break;
                                    case "agi":
                                        currentHero.PrincipalAttribute = HeroPrincipalAttribute.Agility;
                                        break;
                                    default:
                                        currentHero.PrincipalAttribute = HeroPrincipalAttribute.Inteligence;
                                        break;
                                }
                                currentHero.Roles = aux.Value.GetProperty("droles").GetString();
                                currentHero.RightClickAttack = aux.Value.GetProperty("dac").GetString();
                            }
                        }
                    }
                }
                foreach (Hero h in o.result.heroes)
                {
                    context.Heroes.Add(h);
                }
            }
        }

        public static void InitializeItems(string pathJson, Dota2AppDbContext context)
        {
            string pathJsonItems = String.Concat(pathJson, "\\GetItems en_US.json");
            GetItemsResponseModel o;
            if (File.Exists(pathJsonItems))
            {
                string jsonString = File.ReadAllText(pathJsonItems);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new GetItemsJsonConverter());
                o = JsonSerializer.Deserialize<GetItemsResponseModel>(jsonString, serializeOptions);
                string pathJsonItems2 = String.Concat(pathJson, "\\items.json");
                if (File.Exists(pathJsonItems2))
                {
                    jsonString = File.ReadAllText(pathJsonItems2);
                    JsonDocumentOptions jsonOptions = new JsonDocumentOptions
                    {
                        CommentHandling = JsonCommentHandling.Skip
                    };
                    JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString, jsonOptions).RootElement.GetProperty("itemdata");
                    JsonElement.ObjectEnumerator ae = root.EnumerateObject();
                    NumberFormatInfo fp = new NumberFormatInfo();
                    fp.NumberDecimalSeparator = ".";

                    while (ae.MoveNext())
                    {
                        System.Text.Json.JsonProperty aux = ae.Current;
                        string name = aux.Name;
                        if (o != null)
                        {
                            HeroItem currentHeroItem = o.result.items.Find(h => h.ShortName == name);
                            if (currentHeroItem != null)
                            {
                                currentHeroItem.HeroItemId = aux.Value.GetProperty("id").GetInt32();
                                currentHeroItem.Descripction = aux.Value.GetProperty("desc").GetString();
                                currentHeroItem.Notes = aux.Value.GetProperty("notes").GetString();
                                currentHeroItem.Lore = aux.Value.GetProperty("lore").GetString();
                                currentHeroItem.Attrib = aux.Value.GetProperty("attrib").GetString();
                                currentHeroItem.Created = aux.Value.GetProperty("created").GetBoolean();

                                if (aux.Value.GetProperty("components").ValueKind != JsonValueKind.Null && aux.Value.GetProperty("components").GetArrayLength() > 0)
                                {
                                    JsonElement.ArrayEnumerator aeComponents = aux.Value.GetProperty("components").EnumerateArray();
                                    while (aeComponents.MoveNext())
                                    {
                                        if (aeComponents.Current.ValueKind == JsonValueKind.String)
                                        {
                                            string componentName = aeComponents.Current.GetString();
                                            // Aquí tenemos que buscar el HeroItem que tenga el nombre del componente
                                            HeroItem componentHeroItem = o.result.items.Find(h => h.ShortName.Replace("_","") == componentName.Replace("_",""));
                                            if (componentHeroItem != null)
                                            {
                                                HeroItemComponent currentHic = currentHeroItem.Components.Find(hi => hi.ComponentId == componentHeroItem.HeroItemId);
                                                if (currentHic != null) 
                                                {
                                                    currentHic.Quantity++;
                                                }
                                                else 
                                                {
                                                    HeroItemComponent hic = new HeroItemComponent();
                                                    hic.HeroItem = currentHeroItem;
                                                    hic.HeroItemId = currentHeroItem.HeroItemId;
                                                    hic.Component = componentHeroItem;
                                                    hic.ComponentId = componentHeroItem.HeroItemId;
                                                    hic.Quantity = 1;

                                                    currentHeroItem.Components.Add(hic);
                                                    componentHeroItem.IsComponentOf.Add(hic);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (HeroItem hi in o.result.items)
                {
                    context.HeroItems.Add(hi);
                }
            }
        }

        public static void Initialize(Dota2AppDbContext context)
        {
            try
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                if (context.Heroes.Any())
                    return;
                string pathJson = Path.GetFullPath("..\\DataModel\\json");
                InitializeHeroes(pathJson, context);
                InitializeItems(pathJson, context);

                context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}
