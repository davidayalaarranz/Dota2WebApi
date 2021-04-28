using DataModel.Model;
using DataModel.Model.Common;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.Current.GetAbilities
{
    public class AbilitySpecialValueJsonConverter : JsonConverter<AbilitySpecialValue>
    {
        public override AbilitySpecialValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            AbilitySpecialValue asv = new AbilitySpecialValue();
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
                                    asv.ValuesFloat.Add(new AbilitySpecialFloatValue(reader.GetDecimal()));
                                }
                            }
                            break;
                        case "values_int":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    asv.ValuesInt.Add(new AbilitySpecialIntValue(reader.GetInt32()));
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

        public override void Write(Utf8JsonWriter writer, AbilitySpecialValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
    public class AbilitydetailJsonConverter : JsonConverter<Ability>
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
                        case "desc_loc":
                            a.Description = reader.GetString();
                            break;
                        case "lore_loc":
                            a.Lore = reader.GetString();
                            break;
                        case "notes_loc":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    a.NotesList.Add(new AbilityNote(reader.GetString()));
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
                                    a.CastRangeList.Add(new AbilityCastRange(reader.GetInt32()));
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
                                    a.CastPointList.Add(new AbilityCastPoint(reader.GetDecimal()));
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
                                    a.ChannelTimeList.Add(new AbilityChannelTime(reader.GetDecimal()));
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
                                    a.CooldownList.Add(new AbilityCooldown(reader.GetDecimal()));
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
                                    a.DurationList.Add(new AbilityDuration(reader.GetDecimal()));
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
                                    a.DamageList.Add(new AbilityDamage(reader.GetInt32()));
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
                                    a.ManaCostList.Add(new AbilityManaCost(reader.GetInt32()));
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
                                    a.GoldCostList.Add(new AbilityGoldCost(reader.GetInt32()));
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
                                    serializeOptions.Converters.Add(new AbilitySpecialValueJsonConverter());
                                    a.SpecialValues.Add(JsonSerializer.Deserialize<AbilitySpecialValue>(ref reader, serializeOptions));
                                }
                            }
                            break;
                        case "shard_loc":
                            a.ShardDescription = reader.GetString();
                            break;
                        case "scepter_loc":
                            a.ScepterDescription = reader.GetString();
                            break;
                        //case "type":
                        //    a.GetType = reader.GetInt32();
                        //    break;
                        //case "behavior":
                        //    a.Behavior = reader.GetString();
                        //    break;
                        //case "target_team":
                        //    a.TargetTeam = reader.GetInt32();
                        //    break;
                        //case "target_type":
                        //    a.TargetType = reader.GetInt32();
                        //    break;
                        //case "flags":
                        //    a.Flags = reader.GetInt32();
                        //    break;
                        case "damage":
                            a.Damage = reader.GetInt32().ToString();
                            break;
                        //case "immunity":
                        //    a.Immunity = reader.GetInt32();
                        //    break;
                        case "dispellable":
                            a.Dispellable = reader.GetInt32();
                            break;
                        case "max_level":
                            a.MaxLevel = reader.GetInt32();
                            break;
                        case "is_item":
                            a.IsItem = reader.GetBoolean();
                            break;
                        case "ability_has_scepter":
                            a.HasScepterUpgrade = reader.GetBoolean();
                            break;
                        case "ability_has_shard":
                            a.HasShardUpgrade = reader.GetBoolean();
                            break;
                        case "ability_is_granted_by_scepter":
                            a.IsGrantedByScepter = reader.GetBoolean();
                            break;
                        case "ability_is_granted_by_shard":
                            a.IsGrantedByShard = reader.GetBoolean();
                            break;
                        //case "item_cost":
                        //    a.ItemCost = reader.GetInt32();
                        //    break;
                        case "item_initial_charges":
                            a.ItemInitialCharges = reader.GetInt32();
                            break;
                        case "item_neutral_tier":
                            a.NeutralItemTier = reader.GetDecimal();
                            break;
                        //case "item_stock_max":
                        //    a.ItemStockMax = reader.GetInt32();
                        //    break;
                        //case "item_stock_item":
                        //    a.ItemStockItem = reader.GetInt32();
                        //    break;
                        //case "item_quality":
                        //    a.ItemQuality = reader.GetInt32();
                        //    break;
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
