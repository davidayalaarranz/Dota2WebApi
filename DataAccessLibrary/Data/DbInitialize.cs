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
using System.Text.RegularExpressions;

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
                            DataModel.Model.Match currentMatch = o.result.matches.Find(m => m.MatchId == currentMatchId);
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

                                JsonElement.ArrayEnumerator ae = aux.Value.GetProperty("players").EnumerateArray();
                                while (ae.MoveNext())
                                {
                                    MatchPlayer mp = currentMatch.MatchPlayers.First(p => p.PlayerId == ae.Current.GetProperty("account_id").GetInt64() &&
                                                                                    p.PlayerSlot == ae.Current.GetProperty("player_slot").GetInt32());
                                    mp.Kills = ae.Current.GetProperty("kills").GetInt32();
                                    mp.Deaths = ae.Current.GetProperty("deaths").GetInt32();
                                    mp.Assists = ae.Current.GetProperty("assists").GetInt32();
                                    mp.LastHits = ae.Current.GetProperty("last_hits").GetInt32();
                                    mp.Denies = ae.Current.GetProperty("denies").GetInt32();
                                    mp.GPM = ae.Current.GetProperty("gold_per_min").GetInt32();
                                    mp.XPM = ae.Current.GetProperty("xp_per_min").GetInt32();
                                    mp.HeroDamage = ae.Current.GetProperty("hero_damage").GetInt32();
                                    mp.TowerDamage = ae.Current.GetProperty("tower_damage").GetInt32();
                                    mp.HeroHealing = ae.Current.GetProperty("hero_healing").GetInt32();
                                    mp.Gold = ae.Current.GetProperty("gold").GetInt32();
                                    mp.Level = ae.Current.GetProperty("level").GetInt32();
                                    JsonElement.ArrayEnumerator aeUpgrades = ae.Current.GetProperty("ability_upgrades").EnumerateArray();
                                    while (aeUpgrades.MoveNext())
                                    {
                                        MatchPlayerAbilityUpgrade hau = new MatchPlayerAbilityUpgrade();
                                        hau.AbilityId = aeUpgrades.Current.GetProperty("ability").GetInt32();
                                        hau.Time = new TimeSpan(0, 0, aeUpgrades.Current.GetProperty("time").GetInt32());
                                        hau.Level = aeUpgrades.Current.GetProperty("level").GetInt32();
                                        mp.HeroUpgrades.Add(hau);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DataModel.Model.Match m in o.result.matches)
                {
                    foreach (MatchPlayer mp in m.MatchPlayers)
                    {
                        mp.Hero = context.Heroes.First(h => h.HeroId == mp.Hero.HeroId);
                        mp.Player = m.MatchPlayers.First(p => p.PlayerId == mp.Player.PlayerId).Player;
                    }
                    context.Matches.Add(m);
                }
                context.SaveChanges();
            }
        }
        public static void InitializeHeroes(string pathJson, Dota2AppDbContext context)
        {
            string pathNPCHeroes = String.Concat(pathJson, "\\npc_heroes.txt");
            if (File.Exists(pathNPCHeroes))
            {
                List<string> lines = File.ReadLines(pathNPCHeroes).ToList();
                ParseNPCHeroes(lines, context);
            }
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

                foreach (Hero h in context.Heroes)
                {
                    List<Hero> lh = o.result.heroes.Where(haux => haux.HeroId == h.HeroId).ToList();
                    if (lh.Count > 0)
                        h.LocalizedName = lh[0].LocalizedName;
                }
            }
            string pathJsonHeroes2 = String.Concat(pathJson, "\\heroes.json");
            if (File.Exists(pathJsonHeroes2))
            {
                string jsonString = File.ReadAllText(pathJsonHeroes2);
                JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString, jsonOptions).RootElement.GetProperty("herodata");
                JsonElement.ObjectEnumerator ae = root.EnumerateObject();//.EnumerateArray();
                NumberFormatInfo fp = new NumberFormatInfo();
                fp.NumberDecimalSeparator = ".";

                while (ae.MoveNext())
                {
                    System.Text.Json.JsonProperty aux = ae.Current;
                    string name = aux.Name;
                    List<Hero> lh = (from h in context.Heroes select h).Where(h => h.Name.IndexOf(name) > -1).ToList();
                    if (lh.Count > 0)
                    {
                        lh[0].Roles = aux.Value.GetProperty("droles").GetString();
                        lh[0].RightClickAttack = aux.Value.GetProperty("dac").GetString();
                    }
                }
            }

            string pathJsonHeroPickerData = String.Concat(pathJson, "\\heropickerdata.json");
            if (File.Exists(pathJsonHeroPickerData))
            {
                string jsonString = File.ReadAllText(pathJsonHeroPickerData);
                JsonElement root = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;
                JsonElement.ObjectEnumerator oe = root.EnumerateObject();

                while (oe.MoveNext())
                {
                    System.Text.Json.JsonProperty aux = oe.Current;
                    string name = aux.Name;
                    List<Hero> lh = (from h in context.Heroes select h).Where(h => h.Name.IndexOf(name) > -1).ToList();
                    if (lh.Count > 0)
                    {
                        lh[0].Biography = aux.Value.GetProperty("bio").GetString();
                    }
                }
            }
            context.SaveChanges();
        }

        private static bool compara (string key, string heroName)
        {
            return (key.IndexOf(heroName) == 0);
        }

        private static string getNPCProperty(string line)
        {
            int firstQuotePosition = line.IndexOf("\"");
            int secondQuotePosition = line.Substring(line.IndexOf("\"") + 1).IndexOf("\"") + firstQuotePosition + 1;

            return line.Substring(firstQuotePosition + 1, secondQuotePosition - firstQuotePosition - 1);
        }

        private static string getNPCValue(string line)
        {
            int lastQuotePosition = line.LastIndexOf("\"");
            int prelastQuotePosition = line.Substring(0, lastQuotePosition - 1).LastIndexOf("\"");
            return line.Substring(prelastQuotePosition + 1, lastQuotePosition - prelastQuotePosition - 1);
        }

        private static void ParseNPCHeroes(List<string> lines, Dota2AppDbContext context)
        {
            Regex reHeroName = new Regex(@"^\t""npc_dota_hero[a-zA-Z0-9_]*""$");
            Regex reHeroId = new Regex(@"^\t\t""HeroID""");
            Regex reArmorPhysical = new Regex(@"^\t\t""ArmorPhysical""");
            Regex reAttackDamageMin = new Regex(@"^\t\t""AttackDamageMin""");
            Regex reAttackDamageMax = new Regex(@"^\t\t""AttackDamageMax""");
            Regex reAttackRange = new Regex(@"^\t\t""AttackRange""");

            Regex reAttributeBaseStrength = new Regex(@"^\t\t""AttributeBaseStrength""");
            Regex reAttributeStrengthGain = new Regex(@"^\t\t""AttributeStrengthGain""");
            Regex reAttributeBaseIntelligence = new Regex(@"^\t\t""AttributeBaseIntelligence""");
            Regex reAttributeIntelligenceGain = new Regex(@"^\t\t""AttributeIntelligenceGain""");
            Regex reAttributeBaseAgility = new Regex(@"^\t\t""AttributeBaseAgility""");
            Regex reAttributeAgilityGain = new Regex(@"^\t\t""AttributeAgilityGain""");

            Regex reStatusHealth = new Regex(@"^\t\t""StatusHealth""");
            Regex reStatusHealthRegen = new Regex(@"^\t\t""StatusHealthRegen""");
            Regex reStatusMana = new Regex(@"^\t\t""StatusMana""");
            Regex reStatusManaRegen = new Regex(@"^\t\t""StatusManaRegen""");

            Regex reMovementSpeed = new Regex(@"^\t\t""MovementSpeed""");
            Regex reMovementTurnRate = new Regex(@"^\t\t""MovementTurnRate""");
            Regex reAttackRate = new Regex(@"^\t\t""AttackRate""");

            Regex reVisionDaytimeRange = new Regex(@"^\t\t""VisionDaytimeRange""");
            Regex reVisionNighttimeRange = new Regex(@"^\t\t""VisionNighttimeRange""");

            Regex reMagicalResistance = new Regex(@"^\t\t""MagicalResistance""");
            Regex reBaseAttackSpeed = new Regex(@"^\t\t""BaseAttackSpeed""");

            Regex reAttackAnimationPoint = new Regex(@"^\t\t""AttackAnimationPoint""");
            Regex reAttackAcquisitionRange = new Regex(@"^\t\t""AttackAcquisitionRange""");

            Regex reAbilityNumber = new Regex(@"^\t\t""Ability\d+""");
            Regex reAbilityTalentStart = new Regex(@"^\t\t""AbilityTalentStart""");

            Hero hAux = null;
            Hero hBase = null;
            CultureInfo provider = new CultureInfo("en-GB");
            System.Text.RegularExpressions.Match match;
            for (var i = 0; i < lines.Count; i++)
            {
                match = reHeroName.Match(lines[i]);
                if (match.Success)
                {
                    if (hAux != null)
                    {
                        if (hAux.HeroId != 0)
                        {
                            // Indicamos las habilidades que son talentos
                            foreach (HeroAbility ha in hAux.HeroAbilities)
                            {
                                ha.IsTalent = ha.Order >= hAux.AbilityTalentStart;
                            }
                            context.Heroes.Add(hAux);
                        } else if (hAux.Name == "npc_dota_hero_base")
                        {
                            hBase = hAux;
                        }
                    }
                    hAux = new Hero();
                    hAux.Name = getNPCProperty(lines[i]);
                    if (hBase != null) { 
                        hAux.MinDamage = hBase.MinDamage;
                        hAux.MaxDamage = hBase.MaxDamage;
                        hAux.AttackRange = hBase.AttackRange;
                        hAux.AttackRate = hBase.AttackRate;
                        hAux.MovementSpeed = hBase.MovementSpeed;
                        hAux.TurnRate = hBase.TurnRate;
                        hAux.Armor = hBase.Armor;
                        hAux.BaseArmor = hBase.BaseArmor;
                        hAux.BaseHpRegen = hBase.BaseHpRegen;
                        hAux.BaseManaRegen = hBase.BaseManaRegen;
                        hAux.BaseHp = hBase.BaseHp;
                        hAux.BaseMana = hBase.BaseMana;
                        hAux.VisionDaytimeRange = hBase.VisionDaytimeRange;
                        hAux.VisionNighttimeRange = hBase.VisionNighttimeRange;
                        hAux.MagicalResistance = hBase.MagicalResistance;
                        hAux.BaseAttackSpeed = hBase.BaseAttackSpeed;
                        hAux.AttackAnimationPoint = hBase.AttackAnimationPoint;
                        hAux.AttackAcquisitionRange = hBase.AttackAcquisitionRange;

                        hAux.AbilityTalentStart = hBase.AbilityTalentStart;
                    }


                }
                // HeroId
                match = reHeroId.Match(lines[i]);
                if (match.Success) { hAux.HeroId = long.Parse(getNPCValue(lines[i])); continue; }

                // Attributes
                match = reArmorPhysical.Match(lines[i]);
                if (match.Success) { hAux.BaseArmor = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttackDamageMin.Match(lines[i]);
                if (match.Success) { hAux.MinDamage = int.Parse(getNPCValue(lines[i])); continue; }

                match = reAttackDamageMax.Match(lines[i]);
                if (match.Success) { hAux.MaxDamage = int.Parse(getNPCValue(lines[i])); continue; }

                match = reAttackRange.Match(lines[i]);
                if (match.Success) { hAux.AttackRange = int.Parse(getNPCValue(lines[i])); continue; }

                match = reAttributeBaseStrength.Match(lines[i]);
                if (match.Success) { hAux.Strength.Initial = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttributeStrengthGain.Match(lines[i]);
                if (match.Success) { hAux.Strength.Gain = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttributeBaseIntelligence.Match(lines[i]);
                if (match.Success) { hAux.Intelligence.Initial = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttributeIntelligenceGain.Match(lines[i]);
                if (match.Success) { hAux.Intelligence.Gain = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttributeBaseAgility.Match(lines[i]);
                if (match.Success) { hAux.Agility.Initial = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttributeAgilityGain.Match(lines[i]);
                if (match.Success) { hAux.Agility.Gain = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reStatusHealth.Match(lines[i]);
                if (match.Success) { hAux.BaseHp = decimal.Parse(getNPCValue(lines[i]), provider); continue; }
                match = reStatusHealthRegen.Match(lines[i]);
                if (match.Success) { hAux.BaseHpRegen = decimal.Parse(getNPCValue(lines[i]), provider); continue; }
                match = reStatusMana.Match(lines[i]);
                if (match.Success) { hAux.BaseMana = decimal.Parse(getNPCValue(lines[i]), provider); continue; }
                match = reStatusManaRegen.Match(lines[i]);
                if (match.Success) { hAux.BaseManaRegen = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reMovementSpeed.Match(lines[i]);
                if (match.Success) { hAux.MovementSpeed = int.Parse(getNPCValue(lines[i])); continue; }

                match = reMovementTurnRate.Match(lines[i]);
                if (match.Success) { hAux.TurnRate = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reAttackRate.Match(lines[i]);
                if (match.Success) { hAux.AttackRate = decimal.Parse(getNPCValue(lines[i]), provider); continue; }

                match = reVisionDaytimeRange.Match(lines[i]);
                if (match.Success) { hAux.VisionDaytimeRange = int.Parse(getNPCValue(lines[i])); continue; }
                match = reVisionNighttimeRange.Match(lines[i]);
                if (match.Success) { hAux.VisionNighttimeRange = int.Parse(getNPCValue(lines[i])); continue; }

                match = reMagicalResistance.Match(lines[i]);
                if (match.Success) { hAux.MagicalResistance = decimal.Parse(getNPCValue(lines[i]), provider); continue; }
                match = reBaseAttackSpeed.Match(lines[i]);
                if (match.Success) { hAux.BaseAttackSpeed = int.Parse(getNPCValue(lines[i])); continue; }

                match = reAttackAnimationPoint.Match(lines[i]);
                if (match.Success) { hAux.AttackAnimationPoint = decimal.Parse(getNPCValue(lines[i]), provider); continue; }
                match = reAttackAcquisitionRange.Match(lines[i]);
                if (match.Success) { hAux.AttackAcquisitionRange = int.Parse(getNPCValue(lines[i])); continue; }

                
                match = reAbilityTalentStart.Match(lines[i]);
                if (match.Success) { hAux.AbilityTalentStart = int.Parse(getNPCValue(lines[i])); continue; }

                match = reAbilityNumber.Match(lines[i]);
                if (match.Success)
                {
                    // Tenemos que asociar la habilidad al héroe
                    List<Ability> la = context.Abilities.Where(a => a.Name.Equals(getNPCValue(lines[i]))).ToList();
                    string abString = getNPCProperty(lines[i]);
                    int intTalent = int.Parse(abString.Substring(abString.IndexOf("Ability") + 7));
                    if (la.Count > 0)
                    {
                        HeroAbility ha = new HeroAbility();
                        ha.Hero = hAux;
                        ha.Ability = la[0];
                        ha.Order = intTalent;
                        hAux.HeroAbilities.Add(ha);
                    }
                }
            }
            context.SaveChanges();
        }

        private static void ParseNPCAbilities(List<string> lines, Dota2AppDbContext context)
        {
            Regex reAbilityName = new Regex(@"^\t""[a-zA-Z0-9_]*""$");
            Regex reAbilityId = new Regex(@"^\t\t""ID""");

            Regex reAbilityCastRange = new Regex(@"^\t\t""AbilityCastRange""");
            Regex reAbilityCastRangeBuffer = new Regex(@"^\t\t""AbilityCastRangeBuffer""");
            Regex reAbilityCastPoint = new Regex(@"^\t\t""AbilityCastPoint""");
            Regex reAbilityChannelTime = new Regex(@"^\t\t""AbilityChannelTime""");
            Regex reAbilityCooldown = new Regex(@"^\t\t""AbilityCooldown""");
            Regex reAbilityDuration = new Regex(@"^\t\t""AbilityDuration""");
            Regex reAbilityDamage = new Regex(@"^\t\t""AbilityDamage""");
            Regex reAbilityManaCost = new Regex(@"^\t\t""AbilityManaCost""");
            Regex reHasScepterUpgrade = new Regex(@"^\t\t""HasScepterUpgrade""");
            Regex reIsGrantedByScepter = new Regex(@"^\t\t""IsGrantedByScepter""");

            Regex reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_HIDDEN = new Regex(@"^\t\t""AbilityBehavior"".+DOTA_ABILITY_BEHAVIOR_HIDDEN");
            Regex reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_NOT_LEARNABLE = new Regex(@"^\t\t""AbilityBehavior"".+DOTA_ABILITY_BEHAVIOR_NOT_LEARNABLE");
            Regex reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_PASSIVE = new Regex(@"^\t\t""AbilityBehavior"".+DOTA_ABILITY_BEHAVIOR_PASSIVE");

            Regex reValue = new Regex(@"^\t\t\t\t""value""");
            Regex reAbilityType_DOTA_ABILITY_TYPE_ULTIMATE = new Regex(@"^\t\t""AbilityType"".+DOTA_ABILITY_TYPE_ULTIMATE");
            Regex reAbilityType_DOTA_ABILITY_TYPE_BASIC = new Regex(@"^\t\t""AbilityType"".+DOTA_ABILITY_TYPE_BASIC");
            Regex reAbilityType_DOTA_ABILITY_TYPE_ATTRIBUTES = new Regex(@"^\t\t""AbilityType"".+DOTA_ABILITY_TYPE_ATTRIBUTES");

            Regex reAbilitySpecialBonusStrength = new Regex(@"^\t""special_bonus_strength");
            Regex reAbilitySpecialBonusAgility = new Regex(@"^\t""special_bonus_agility");
            Regex reAbilitySpecialBonusIntelligence = new Regex(@"^\t""special_bonus_intelligence");
            Regex reAbilitySpecialBonusAllStats = new Regex(@"^\t""special_bonus_all_stats");

            Regex reMaxLevel = new Regex(@"^\t\t""MaxLevel""");


            Ability aAux = null;
            Ability aBase = null;
            CultureInfo provider = new CultureInfo("en-GB");
            System.Text.RegularExpressions.Match match;
            for (var i = 0; i < lines.Count; i++)
            {
                match = reAbilityName.Match(lines[i]);
                if (match.Success)
                {
                    if (aAux != null)
                    {
                        if (aAux.AbilityId != 0)
                        {
                            context.Abilities.Add(aAux);
                        }
                        else if (aAux.Name == "ability_base")
                        {
                            aBase = aAux;
                        }
                    }
                    aAux = new Ability();
                    aAux.Name = getNPCProperty(lines[i]);
                    if (aBase != null)
                    {
                        aAux.CastRange = aBase.CastRange;
                        aAux.CastRangeBuffer = aBase.CastRangeBuffer;
                        aAux.CastPoint = aBase.CastPoint;
                        aAux.ChannelTime = aBase.ChannelTime;
                        aAux.Cooldown = aBase.Cooldown;
                        aAux.Duration = aBase.Duration;
                        aAux.Damage = aBase.Damage;
                        aAux.ManaCost = aBase.ManaCost;
                        aAux.MaxLevel = aBase.MaxLevel;
                    }
                }
                // SpecialBonus
                match = reAbilitySpecialBonusAllStats.Match(lines[i]);
                if (match.Success) { aAux.IsAgilityBonus = true; aAux.IsStrengthBonus = true; aAux.IsIntelligenceBonus = true; continue; }
                match = reAbilitySpecialBonusStrength.Match(lines[i]);
                if (match.Success) { aAux.IsStrengthBonus = true; continue; }
                match = reAbilitySpecialBonusAgility.Match(lines[i]);
                if (match.Success) { aAux.IsAgilityBonus = true; continue; }
                match = reAbilitySpecialBonusIntelligence.Match(lines[i]);
                if (match.Success) { aAux.IsIntelligenceBonus = true; continue; }

                // HeroId
                match = reAbilityId.Match(lines[i]);
                if (match.Success) { aAux.AbilityId = long.Parse(getNPCValue(lines[i])); continue; }

                match = reAbilityCastRange.Match(lines[i]);
                if (match.Success) { aAux.CastRange = getNPCValue(lines[i]); continue; }
                match = reAbilityCastRangeBuffer.Match(lines[i]);
                if (match.Success) { aAux.CastRangeBuffer = int.Parse(getNPCValue(lines[i])); continue; }
                match = reAbilityCastPoint.Match(lines[i]);
                if (match.Success) { aAux.CastPoint = getNPCValue(lines[i]); continue; }
                match = reAbilityChannelTime.Match(lines[i]);
                if (match.Success) { aAux.ChannelTime = getNPCValue(lines[i]); continue; }
                match = reAbilityCooldown.Match(lines[i]);
                if (match.Success) { aAux.Cooldown = getNPCValue(lines[i]); continue; }
                match = reAbilityDuration.Match(lines[i]);
                if (match.Success) { aAux.Duration = getNPCValue(lines[i]); continue; }

                match = reAbilityDamage.Match(lines[i]);
                if (match.Success) { aAux.Damage = getNPCValue(lines[i]); continue; }
                match = reAbilityManaCost.Match(lines[i]);
                if (match.Success) { aAux.ManaCost = getNPCValue(lines[i]); continue; }

                match = reHasScepterUpgrade.Match(lines[i]);
                if (match.Success) { aAux.HasScepterUpgrade = getNPCValue(lines[i]).Equals("1"); continue; }
                match = reIsGrantedByScepter.Match(lines[i]);
                if (match.Success) { aAux.IsGrantedByScepter = getNPCValue(lines[i]).Equals("1"); continue; }

                match = reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_HIDDEN.Match(lines[i]);
                if (match.Success) { aAux.IsHidden = true; }

                match = reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_NOT_LEARNABLE.Match(lines[i]);
                if (match.Success) { aAux.IsHidden = true; }
                match = reAbilityBehavior_DOTA_ABILITY_BEHAVIOR_PASSIVE.Match(lines[i]);
                if (match.Success) { aAux.IsPassive = true; }

                match = reValue.Match(lines[i]);
                if (match.Success) { aAux.Value = getNPCValue(lines[i]); continue; }

                // Si la habilidad es de tipo ULTIMATE, el máximo nivel es 3
                match = reAbilityType_DOTA_ABILITY_TYPE_ULTIMATE.Match(lines[i]);
                if (match.Success) { aAux.MaxLevel = 3; continue; }
                // Si la habilidad es de tipo BASIC, el máximo nivel es 4
                match = reAbilityType_DOTA_ABILITY_TYPE_BASIC.Match(lines[i]);
                if (match.Success) { aAux.MaxLevel = 4; continue; }
                // Si la habilidad es de tipo ATTRIBUTES, el máximo nivel es 1 (talentos)
                match = reAbilityType_DOTA_ABILITY_TYPE_ATTRIBUTES.Match(lines[i]);
                if (match.Success) { aAux.MaxLevel = 1; aAux.IsAttributte = true; continue; }
                
                match = reMaxLevel.Match(lines[i]);
                if (match.Success) { aAux.MaxLevel = int.Parse(getNPCValue(lines[i])); continue; }
            }   
            context.SaveChanges();
        }

        public static void InitializeAbilities(string pathJson, Dota2AppDbContext context)
        {
            try
            {
                string pathNPCAbilities = String.Concat(pathJson, "\\npc_abilities.txt");
                if (File.Exists(pathNPCAbilities))
                {
                    List<string> lines = File.ReadLines(pathNPCAbilities).ToList();
                    ParseNPCAbilities(lines, context);

                    GetHeroAbilitiesResponseModel harm = null;
                    string pathJsonHeroAbilities = String.Concat(pathJson, "\\abilities.json");
                    if (File.Exists(pathJsonHeroAbilities))
                    {
                        string jsonString = File.ReadAllText(pathJsonHeroAbilities);
                        var serializeOptions2 = new JsonSerializerOptions
                        {
                            ReadCommentHandling = JsonCommentHandling.Skip
                        };
                        serializeOptions2.Converters.Add(new GetHeroAbilitiesJsonConverter());
                        harm = JsonSerializer.Deserialize<GetHeroAbilitiesResponseModel>(jsonString, serializeOptions2);

                        foreach (Ability ha in context.Abilities)
                        {
                            List<KeyValuePair<string, Ability>> keys = harm.abilitydata.Where(hi => hi.Key == ha.Name).ToList();

                            if (keys.Count > 0)
                            {
                                KeyValuePair<string, Ability> kvp = keys[0];
                                ha.LocalizedName = kvp.Value.LocalizedName;
                                ha.Affects = kvp.Value.Affects;
                                ha.Description = kvp.Value.Description;
                                ha.Notes = kvp.Value.Notes;
                                ha.Attrib = kvp.Value.Attrib;
                                ha.Cmb = kvp.Value.Cmb;
                                ha.Lore = kvp.Value.Lore;
                                ha.Hurl = kvp.Value.Hurl;
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {

            }
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
                InitializeAbilities(pathJson, context);
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
