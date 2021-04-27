using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.v7_28.GetHeroes
{
    public class GetHeroesJsonConverter : JsonConverter<Hero>
    {
        public override Hero Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Hero h = new Hero();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return h;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "id":
                            h.HeroId = reader.GetInt64();
                            break;
                        case "name":
                            h.Name = reader.GetString();
                            break;
                        case "localized_name":
                            h.LocalizedName = reader.GetString();
                            break;
                        default:
                            break;
                    }
                }
            }
            return h;
        }

        public override void Write(Utf8JsonWriter writer, Hero value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //This will never be called since CanWrite is false
        }
    }
}
