﻿using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.Current.GetItems
{
    public class ItemlistJsonConverter : JsonConverter<HeroItem>
    {
        public override HeroItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            HeroItem hi = new HeroItem();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return hi;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "id":
                            hi.HeroItemId = reader.GetInt32();
                            break;
                        case "name":
                            hi.Name = reader.GetString();
                            break;
                        case "name_loc":
                            hi.LocalizedName = reader.GetString();
                            break;
                        case "neutral_item_tier":
                            hi.NeutralItemTier = reader.GetInt32();
                            break;
                    }
                }
            }
            return hi;
        }

        public override void Write(Utf8JsonWriter writer, HeroItem value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //This will never be called since CanWrite is false
        }
    }
}
