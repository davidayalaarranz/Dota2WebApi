using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.GetHeroes
{
    public class GetHeroAbilitiesJsonConverter : JsonConverter<HeroAbility>
    {
        public override HeroAbility Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            HeroAbility ha = new HeroAbility();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return ha;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "id":
                            ha.Id = reader.GetInt64();
                            break;
                        case "dname":
                            ha.LocalizedName = reader.GetString();
                            break;
                        case "affects":
                            ha.Affects = reader.GetString();
                            break;
                        case "desc":
                            ha.Description = reader.GetString();
                            break;
                        case "notes":
                            ha.Notes = reader.GetString();
                            break;
                        case "dmg":
                            ha.Damage = reader.GetString();
                            break;
                        case "attrib":
                            ha.Attrib = reader.GetString();
                            break;
                        case "cmb":
                            ha.Cmb = reader.GetString();
                            break;
                        case "lore":
                            ha.Lore = reader.GetString();
                            break;
                        case "hurl":
                            ha.Hurl = reader.GetString();
                            break;
                        default:
                            break;
                    }
                }
            }
            return ha;
        }

        public override void Write(Utf8JsonWriter writer, HeroAbility value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //This will never be called since CanWrite is false
        }
    }
}
