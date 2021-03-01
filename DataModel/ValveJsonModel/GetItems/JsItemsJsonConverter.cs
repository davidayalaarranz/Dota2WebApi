using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.GetItems
{
    public class JsItemsJsonConverter : JsonConverter<HeroItem>
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
                        case "desc":
                            hi.Description = reader.GetString();
                            break;
                        case "cost":
                            hi.Cost = reader.GetInt32();
                            break;
                        case "cd":
                            int valueCD = 0;
                            try
                            {
                                if (reader.TryGetInt32(out valueCD))
                                hi.Cooldown = valueCD;
                            }
                            catch (InvalidOperationException e)
                            {

                            }
                            break;
                        case "mc":
                            int valueMC = 0;
                            try { 
                                if (reader.TryGetInt32(out valueMC))
                                    hi.ManaCost = valueMC;
                            }catch (InvalidOperationException e)
                            {

                            }
                            break;
                        case "notes":
                            hi.Notes = reader.GetString();
                            break;
                        case "lore":
                            hi.Lore = reader.GetString();
                            break;
                        case "attrib":
                            hi.Attrib = reader.GetString();
                            break;
                        case "created":
                            hi.Created = reader.GetBoolean();
                            break;
                        case "dname":
                            hi.LocalizedName = reader.GetString();
                            break;
                        case "components":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    hi.ComponentsName.Add(reader.GetString());
                                }
                            }
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
