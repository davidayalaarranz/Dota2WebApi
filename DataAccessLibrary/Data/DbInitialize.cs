//using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using DataModel.ValveJsonModel.GetHeroes;
using System.Globalization;
using DataModel.ValveJsonModel.GetItems;
using DataModel.Model;
using DataModel.ValveJsonModel.GetMatchHistory;

namespace DataAccessLibrary.Data
{
    public static class DbInitialize
    {
        public static void InitializeMatches(string pathJson, Dota2AppDbContext context)
        {
            JsonDocumentOptions jsonOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            string pathJsonMatchHistory = String.Concat(pathJson, "\\GetMatchHistory.json");
            GetMatchHistoryResponseModel o;
            if (File.Exists(pathJsonMatchHistory))
            {
                string jsonString = File.ReadAllText(pathJsonMatchHistory);
                var serializeOptions = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                serializeOptions.Converters.Add(new GetMatchHistoryJsonConverter());
                o = JsonSerializer.Deserialize<GetMatchHistoryResponseModel>(jsonString, serializeOptions);


                GetMatchDetailsResponseModel md = null;
                string pathJsonMatchDetails = String.Concat(pathJson, "\\GetMatchDetails.json");
                if (File.Exists(pathJsonMatchDetails))
                {
                    jsonString = File.ReadAllText(pathJsonMatchDetails);

                    JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;
                    JsonElement.ObjectEnumerator oe = root.EnumerateObject();

                    while (oe.MoveNext())
                    {
                        System.Text.Json.JsonProperty aux = oe.Current;
                        string name = aux.Name;
                        if (o != null)
                        {
                            long currentMatchId = aux.Value.GetProperty("match_id").GetInt64();
                            Match currentMatch = o.result.matches.Find(m => m.MatchId == currentMatchId);
                            if (currentMatch != null)
                            { 
                                currentMatch.RadiantWin = aux.Value.GetProperty("radiant_win").GetBoolean();
                                currentMatch.Duration = aux.Value.GetProperty("duration").GetInt32();
                                currentMatch.PreGameDuration = aux.Value.GetProperty("pre_game_duration").GetInt32();
                                currentMatch.TowerStatusRadiant = aux.Value.GetProperty("tower_status_radiant").GetInt32();
                                currentMatch.TowerStatusDire = aux.Value.GetProperty("tower_status_dire").GetInt32();
                                currentMatch.BarracksStatusRadiant = aux.Value.GetProperty("barracks_status_radiant").GetInt32();
                                currentMatch.BarracksStatusDire = aux.Value.GetProperty("barracks_status_dire").GetInt32();
                                currentMatch.FirstBloodTime = aux.Value.GetProperty("first_blood_time").GetInt32();
                                currentMatch.GameMode = aux.Value.GetProperty("game_mode").GetInt32();
                                currentMatch.RadiantScore = aux.Value.GetProperty("radiant_score").GetInt32();
                                currentMatch.DireScore = aux.Value.GetProperty("dire_score").GetInt32();
                            }
                        }
                    }
                }

                foreach (Match m in o.result.matches)
                {
                    foreach (MatchPlayer mp in m.Players)
                    {
                        mp.Hero = context.Heroes.First(h => h.HeroId == mp.Hero.HeroId);
                        mp.Player = m.Players.First(p => p.PlayerId == mp.Player.PlayerId).Player;
                    }
                    context.Matches.Add(m);
                }
                context.SaveChanges();
            }
        }
        public static void InitializeHeroes(string pathJson, Dota2AppDbContext context)
        {
            JsonDocumentOptions jsonOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
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
                GetHeropickerdataResponseModel hprm = null;
                string pathJsonHeroPickerData = String.Concat(pathJson, "\\heropickerdata.json");
                if (File.Exists(pathJsonHeroPickerData))
                {
                    jsonString = File.ReadAllText(pathJsonHeroPickerData);
                    JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;
                    JsonElement.ObjectEnumerator oe = root.EnumerateObject();

                    while (oe.MoveNext())
                    {
                        System.Text.Json.JsonProperty aux = oe.Current;
                        string name = aux.Name;
                        if (o != null)
                        {
                            Hero currentHero = o.result.heroes.Find(h => h.ShortName == name);
                            if (currentHero != null)
                            {
                                currentHero.Biography = aux.Value.GetProperty("bio").GetString();
                            }
                        }
                    }
                }

                string pathJsonMyHeroesData = String.Concat(pathJson, "\\MyHeroes.json");
                if (File.Exists(pathJsonMyHeroesData))
                {
                    jsonString = File.ReadAllText(pathJsonMyHeroesData);
                    JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;
                    JsonElement.ObjectEnumerator oe = root.EnumerateObject();

                    while (oe.MoveNext())
                    {
                        System.Text.Json.JsonProperty aux = oe.Current;
                        string name = aux.Name;
                        if (o != null)
                        {
                            Hero currentHero = o.result.heroes.Find(h => h.ShortName == name);
                            if (currentHero != null)
                            {
                                currentHero.BaseArmor = aux.Value.GetProperty("baseArmor").GetDecimal();
                            }
                        }
                    }
                }

                GetHeroAbilitiesResponseModel harm = null;
                string pathJsonHeroAbilities = String.Concat(pathJson, "\\abilities.json");
                if (File.Exists(pathJsonHeroAbilities))
                {
                    jsonString = File.ReadAllText(pathJsonHeroAbilities);
                    var serializeOptions2 = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };
                    serializeOptions2.Converters.Add(new GetHeroAbilitiesJsonConverter());
                    harm = JsonSerializer.Deserialize<GetHeroAbilitiesResponseModel>(jsonString, serializeOptions2);
                }
                

                foreach (Hero h in o.result.heroes)
                {
                    List<KeyValuePair<string, HeroAbility>> keys = harm.abilitydata.Where(
                        hi => compara(hi.Key, h.ShortName)
                    ).ToList();
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        KeyValuePair<string, HeroAbility> kvp = keys[i];
                        kvp.Value.Order = (i + 1);
                        kvp.Value.Name = kvp.Key;
                    }

                    h.Abilities.AddRange(keys.Select(k => k.Value));

                    context.Heroes.Add(h);
                }
                context.SaveChanges();
            }
        }

        private static bool compara (string key, string heroName)
        {
            return (key.IndexOf(heroName) == 0);
        }

        public static void InitializeItems(string pathJson, Dota2AppDbContext context)
        {
            try { 
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
                                    currentHeroItem.Description = aux.Value.GetProperty("desc").ValueKind != JsonValueKind.Null ? aux.Value.GetProperty("desc").GetString() : "";
                                    if (aux.Value.GetProperty("cd").ValueKind == JsonValueKind.Number)
                                        currentHeroItem.Cooldown = aux.Value.GetProperty("cd").GetInt32();
                                    if (aux.Value.GetProperty("mc").ValueKind == JsonValueKind.Number)
                                        currentHeroItem.ManaCost = aux.Value.GetProperty("mc").GetInt32();
                                    currentHeroItem.Notes = aux.Value.GetProperty("notes").GetString();
                                    currentHeroItem.Lore = aux.Value.GetProperty("lore").GetString();
                                    currentHeroItem.Attrib = aux.Value.GetProperty("attrib").GetString();
                                    currentHeroItem.Created = aux.Value.GetProperty("created").GetBoolean();

                                    if (aux.Value.GetProperty("components").ValueKind != JsonValueKind.Null && aux.Value.GetProperty("components").GetArrayLength() > 0)
                                    {
                                        HeroItem recipe = o.result.items.Find(h => h.Name == string.Concat("item_recipe_", currentHeroItem.Name.Substring(5)));
                                        if (recipe != null)
                                        {
                                            HeroItemComponent hicRecipe = new HeroItemComponent();
                                            hicRecipe.HeroItem = currentHeroItem;
                                            hicRecipe.HeroItemId = currentHeroItem.HeroItemId;
                                            hicRecipe.Component = recipe;
                                            hicRecipe.ComponentId = recipe.HeroItemId;
                                            hicRecipe.Quantity = 1;
                                            currentHeroItem.Components.Add(hicRecipe);
                                            recipe.IsComponentOf.Add(hicRecipe);
                                        }
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
                    context.SaveChanges();
                }
            }catch(Exception e)
            {

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
                InitializeMatches(pathJson, context);

                //context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}
