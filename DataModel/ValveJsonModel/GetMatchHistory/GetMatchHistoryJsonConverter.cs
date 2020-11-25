﻿using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.GetMatchHistory
{
    public class GetMatchHistoryJsonConverter : JsonConverter<Match>
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
                        case "match_id":
                            m.MatchId = reader.GetInt64();
                            break;
                        case "match_seq_num":
                            m.MatchSeqNum = reader.GetInt64();
                            break;
                        case "start_time":
                            m.StartTime = UnixTimeStampToDateTime(reader.GetInt64());
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

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public override void Write(Utf8JsonWriter writer, Match value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
