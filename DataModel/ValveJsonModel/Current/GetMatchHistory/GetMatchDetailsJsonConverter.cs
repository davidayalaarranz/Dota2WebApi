using DataModel.Common;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.Current.GetMatchHistory
{
    public class GetMatchDetailsJsonConverter : JsonConverter<Match>
    {
        public override Match Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Match m = new Match();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return m;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "radiant_win":
                            m.RadiantWin = reader.GetBoolean();
                            break;
                        case "duration":
                            m.Duration = reader.GetInt32();
                            break;
                        case "pre_game_duration":
                            m.PreGameDuration = reader.GetInt32();
                            break;
                        case "tower_status_radiant":
                            m.TowerStatusRadiant = reader.GetInt32();
                            break;
                        case "tower_status_dire":
                            m.TowerStatusDire = reader.GetInt32();
                            break;
                        case "barracks_status_radiant":
                            m.BarracksStatusRadiant = reader.GetInt32();
                            break;
                        case "barracks_status_dire":
                            m.BarracksStatusDire = reader.GetInt32();
                            break;
                        case "first_blood_time":
                            m.FirstBloodTime = reader.GetInt32();
                            break;
                        case "game_mode":
                            m.GameMode = reader.GetInt32();
                            break;
                        case "radiant_score":
                            m.RadiantScore = reader.GetInt32();
                            break;
                        case "dire_score":
                            m.DireScore = reader.GetInt32();
                            break;


                        case "match_id":
                            m.MatchId = reader.GetInt64();
                            break;
                        case "match_seq_num":
                            m.MatchSeqNum = reader.GetInt64();
                            break;
                        case "start_time":
                            m.StartTime = DataImportationMethods.UnixTimeStampToDateTime(reader.GetInt64());
                            break;
                        case "players":
                            // Leemos el array de players
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                m.MatchPlayers = new List<MatchPlayer>();
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;

                                    if (reader.TokenType == JsonTokenType.StartObject)
                                    {
                                        MatchPlayer mp = new MatchPlayer();
                                        mp.Match = m;
                                        mp.MatchId = m.MatchId;
                                        mp.Player = new Player();
                                        mp.Hero = new Hero();
                                        m.MatchPlayers.Add(mp);
                                        MatchPlayerHeroItemUpgrade mphiu;
                                        while (reader.Read())
                                        {
                                            if (reader.TokenType == JsonTokenType.EndObject) break;

                                            propertyName = reader.GetString();
                                            reader.Read();
                                            switch (propertyName)
                                            {
                                                case "account_id":
                                                    mp.PlayerId = reader.GetInt64();
                                                    mp.Player.PlayerId = mp.PlayerId;
                                                    break;
                                                case "player_slot":
                                                    mp.PlayerSlot = reader.GetInt32();
                                                    break;
                                                case "hero_id":
                                                    mp.HeroId = reader.GetInt64();
                                                    mp.Hero.HeroId = mp.HeroId;
                                                    break;
                                                case "kills":
                                                    mp.Kills = reader.GetInt32();
                                                    break;
                                                case "deaths":
                                                    mp.Deaths = reader.GetInt32();
                                                    break;
                                                case "assists":
                                                    mp.Assists = reader.GetInt32();
                                                    break;
                                                case "last_hits":
                                                    mp.LastHits = reader.GetInt32();
                                                    break;
                                                case "denies":
                                                    mp.Denies = reader.GetInt32();
                                                    break;
                                                case "gold_per_min":
                                                    mp.GPM = reader.GetInt32();
                                                    break;
                                                case "xp_per_min":
                                                    mp.XPM = reader.GetInt32();
                                                    break;
                                                case "hero_damage":
                                                    mp.HeroDamage = reader.GetInt32();
                                                    break;
                                                case "tower_damage":
                                                    mp.TowerDamage = reader.GetInt32();
                                                    break;
                                                case "hero_healing":
                                                    mp.HeroHealing = reader.GetInt32();
                                                    break;
                                                case "gold":
                                                    mp.Gold = reader.GetInt32();
                                                    break;
                                                case "level":
                                                    mp.Level = reader.GetInt32();
                                                    break;

                                                case "item_0":
                                                    int itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    { 
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 0;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_1":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 1;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_2":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 2;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_3":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 3;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_4":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 4;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_5":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 5;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    break;
                                                case "item_neutral":
                                                    itemId = reader.GetInt32();
                                                    if (itemId != 0)
                                                    {
                                                        mphiu = new MatchPlayerHeroItemUpgrade();
                                                        mphiu.HeroItemId = itemId;
                                                        mphiu.HeroItemSlot = 6;
                                                        mphiu.StartLevel = 1;
                                                        mphiu.IsSold = false;
                                                        mp.HeroItemUpgrades.Add(mphiu);
                                                    }
                                                    //NeutralItem
                                                    break;


                                                case "ability_upgrades":
                                                    // Leemos el array de players
                                                    if (reader.TokenType == JsonTokenType.StartArray)
                                                    {
                                                        while (reader.Read())
                                                        {
                                                            if (reader.TokenType == JsonTokenType.EndArray) break;

                                                            if (reader.TokenType == JsonTokenType.StartObject)
                                                            {
                                                                MatchPlayerAbilityUpgrade mpau = new MatchPlayerAbilityUpgrade();
                                                                mp.HeroUpgrades.Add(mpau);
                                                                while (reader.Read())
                                                                {
                                                                    if (reader.TokenType == JsonTokenType.EndObject) break;
                                                                    propertyName = reader.GetString();
                                                                    reader.Read();
                                                                    switch (propertyName)
                                                                    {
                                                                        case "ability":
                                                                            mpau.AbilityId = reader.GetInt32();
                                                                            break;
                                                                        case "time":
                                                                            mpau.Time = new TimeSpan(0, 0, reader.GetInt32());
                                                                            break;
                                                                        case "level":
                                                                            mpau.Level = reader.GetInt32();
                                                                            break;
                                                                        default:
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    break;
                                                
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "picks_bans":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;

                                    if (reader.TokenType == JsonTokenType.StartObject)
                                    {
                                        //MatchPlayerAbilityUpgrade mpau = new MatchPlayerAbilityUpgrade();
                                        //mp.HeroUpgrades.Add(mpau);
                                        while (reader.Read())
                                        {
                                            if (reader.TokenType == JsonTokenType.EndObject) break;
                                            propertyName = reader.GetString();
                                            reader.Read();
                                            switch (propertyName)
                                            {
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return m;
        }

        public override void Write(Utf8JsonWriter writer, Match value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
