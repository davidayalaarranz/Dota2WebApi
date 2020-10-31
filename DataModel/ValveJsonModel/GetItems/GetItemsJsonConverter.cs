using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.GetItems
{
    public class GetItemsJsonConverter : JsonConverter<HeroItem>
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
                        case "localized_name":
                            hi.LocalizedName = reader.GetString();
                            break;
                        case "cost":
                            hi.Cost = reader.GetInt32();
                            break;
                        case "secret_shop":
                            hi.IsSecretShop = reader.GetInt16() == 1;
                            break;
                        case "side_shop":
                            hi.IsSideShop = reader.GetInt16() == 1;
                            break;
                        case "recipe":
                            hi.IsRecipe = reader.GetInt16() == 1;
                            break;
                        default:
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
