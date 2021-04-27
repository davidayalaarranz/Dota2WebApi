using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.Current.GetAbilities
{
    public class AbilitylistJsonConverter : JsonConverter<Ability>
    {
        public override Ability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Ability a = new Ability();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return a;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "id":
                            a.AbilityId = reader.GetInt64();
                            break;
                        case "name":
                            a.Name = reader.GetString();
                            break;
                        case "name_loc":
                            a.LocalizedName = reader.GetString();
                            break;
                        case "neutral_item_tier":
                            a.NeutralItemTier = reader.GetInt32();
                            break;
                        default:
                            break;
                    }
                }
            }
            return a;
        }

        public override void Write(Utf8JsonWriter writer, Ability value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //This will never be called since CanWrite is false
        }
    }
}
