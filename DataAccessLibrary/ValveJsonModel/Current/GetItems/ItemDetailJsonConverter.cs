using DataAccessLibrary.ValveJsonModel.Current.GetAbilities;
using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.Current.GetItems
{
    public class HeroItemSpecialValueJsonConverter : JsonConverter<HeroItemSpecialValue>
    {
        public override HeroItemSpecialValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            HeroItemSpecialValue asv = new HeroItemSpecialValue();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return asv;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "name":
                            asv.Name = reader.GetString();
                            break;
                        case "values_float":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    asv.ValuesFloat.Add(new SpecialFloatValue(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "values_int":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    asv.ValuesInt.Add(new SpecialIntValue(reader.GetInt32()));
                                }
                            }
                            break;
                        case "is_percentage":
                            asv.IsPercentage = reader.GetBoolean();
                            break;
                        case "heading_loc":

                            break;
                        default:
                            break;
                    }
                }
            }
            return asv;
        }

        public override void Write(Utf8JsonWriter writer, HeroItemSpecialValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemDetailJsonConverter : JsonConverter<HeroItem>
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
                            hi.HeroItemId = reader.GetInt64();
                            break;
                        case "name":
                            hi.Name = reader.GetString();
                            break;
                        case "name_loc":
                            hi.LocalizedName = reader.GetString();
                            break;
                        case "desc_loc":
                            hi.Description = reader.GetString();
                            break;
                        case "lore_loc":
                            hi.Lore = reader.GetString();
                            break;
                        case "notes_loc":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.NotesList.Add(new HeroItemNote(reader.GetString()));
                                }
                            }
                            break;
                        case "cast_ranges":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.CastRangeList.Add(new HeroItemCastRange(reader.GetInt32()));
                                }
                            }
                            break;
                        case "cast_points":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.CastPointList.Add(new HeroItemCastPoint(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "channel_times":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.ChannelTimeList.Add(new HeroItemChannelTime(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "cooldowns":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.CooldownList.Add(new HeroItemCooldown(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "durations":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.DurationList.Add(new HeroItemDuration(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "damages":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.DamageList.Add(new HeroItemDamage(reader.GetInt32()));
                                }
                            }
                            break;
                        case "mana_costs":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.ManaCostList.Add(new HeroItemManaCost(reader.GetInt32()));
                                }
                            }
                            break;
                        case "gold_costs":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    hi.GoldCostList.Add(new HeroItemGoldCost(reader.GetInt32()));
                                }
                            }
                            break;
                        case "special_values":

                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    var serializeOptions = new JsonSerializerOptions
                                    {
                                        ReadCommentHandling = JsonCommentHandling.Skip
                                    };
                                    serializeOptions.Converters.Add(new HeroItemSpecialValueJsonConverter());
                                    hi.SpecialValues.Add(JsonSerializer.Deserialize<HeroItemSpecialValue>(ref reader, serializeOptions));
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
