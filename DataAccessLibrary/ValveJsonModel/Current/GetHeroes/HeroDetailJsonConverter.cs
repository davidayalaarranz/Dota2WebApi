using DataAccessLibrary.Data;
using DataAccessLibrary.ValveJsonModel.Current.GetAbilities;
using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.ValveJsonModel.Current.GetHeroes
{
    public class HeroDetailJsonConverter : JsonConverter<Hero>
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
                        case "name_loc":
                            h.LocalizedName = reader.GetString();
                            break;
                        case "bio_loc":
                            h.Biography = reader.GetString();
                            break;
                        case "hype_loc":
                            //h.Hype = reader.GetString();
                            break;
                        case "npe_desc_loc":
                            //h.NpeDesc = reader.GetString();
                            break;
                        case "str_base":
                            h.Strength.Initial = reader.GetDecimal();
                            break;
                        case "str_gain":
                            h.Strength.Gain = reader.GetDecimal();
                            break;
                        case "agi_base":
                            h.Agility.Initial = reader.GetDecimal();
                            break;
                        case "agi_gain":
                            h.Agility.Gain = reader.GetDecimal();
                            break;
                        case "int_base":
                            h.Intelligence.Initial = reader.GetDecimal();
                            break;
                        case "int_gain":
                            h.Intelligence.Gain = reader.GetDecimal();
                            break;
                        //case "primary_attr":
                        //    h.PrincipalAttribute = reader.GetInt32();
                        //    break;
                        //case "complexity":
                        //    h.PrincipalAttribute = reader.GetInt32();
                        //    break;
                        case "attack_capability":
                            int rightClickAttack = reader.GetInt32();
                            if (rightClickAttack == 1) h.RightClickAttack = "Melee";
                            else h.RightClickAttack = "Range";

                            break;
                        case "role_levels":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    //if (reader.TokenType == JsonTokenType.False) continue;
                                    //AbilityNote anl = new AbilityNote();
                                    //anl.Value = reader.GetString();
                                    //a.NotesList.Add(anl);
                                }
                            }
                            break;
                        case "damage_min":
                            h.MinDamage = reader.GetInt32();
                            break;
                        case "damage_max":
                            h.MaxDamage = reader.GetInt32();
                            break;
                        case "attack_rate":
                            h.AttackRate = reader.GetDecimal();
                            break;
                        case "attack_range":
                            h.AttackRange = reader.GetInt32();
                            break;
                        case "projectile_speed":
                            h.ProjectileSpeed = reader.GetInt32();
                            break;
                        case "armor":
                            h.BaseArmor = reader.GetDecimal();
                            break;
                        case "magic_resistance":
                            h.MagicalResistance = reader.GetDecimal();
                            break;
                        case "movement_speed":
                            h.MovementSpeed = reader.GetInt32();
                            break;
                        case "turn_rate":
                            h.TurnRate = reader.GetDecimal();
                            break;
                        case "sight_range_day":
                            h.VisionDaytimeRange = reader.GetInt32();
                            break;
                        case "sight_range_night":
                            h.VisionNighttimeRange = reader.GetInt32();
                            break;
                        case "max_health":
                            h.BaseHp = reader.GetInt32();
                            break;
                        case "health_regen":
                            h.BaseHpRegen = reader.GetDecimal();
                            break;
                        case "max_mana":
                            h.BaseMana = reader.GetInt32();
                            break;
                        case "mana_regen":
                            h.BaseManaRegen = reader.GetDecimal();
                            break;
                        case "abilities":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    var serializeOptions = new JsonSerializerOptions
                                    {
                                        ReadCommentHandling = JsonCommentHandling.Skip
                                    };
                                    serializeOptions.Converters.Add(new AbilitydetailJsonConverter());
                                    Ability a = JsonSerializer.Deserialize<Ability>(ref reader, serializeOptions);
                                    HeroAbility ha = new HeroAbility
                                    {
                                        AbilityId = a.AbilityId,
                                        HeroId = h.HeroId,
                                        AbilityPatchVersionId = AppConfiguration.CurrentDotaPatchVersion.PatchVersionId,
                                        HeroPatchVersionId = AppConfiguration.CurrentDotaPatchVersion.PatchVersionId,
                                        IsTalent = false,
                                        Order = h.HeroAbilities.Count,
                                    };
                                    h.HeroAbilities.Add(ha);
                                }
                            }
                            break;
                        case "talents":
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                while (reader.Read())
                                {
                                    if (reader.TokenType == JsonTokenType.EndArray) break;
                                    var serializeOptions = new JsonSerializerOptions
                                    {
                                        ReadCommentHandling = JsonCommentHandling.Skip
                                    };
                                    serializeOptions.Converters.Add(new AbilitydetailJsonConverter());
                                    Ability a = JsonSerializer.Deserialize<Ability>(ref reader, serializeOptions);
                                    HeroAbility ha = new HeroAbility
                                    {
                                        AbilityId = a.AbilityId,
                                        HeroId = h.HeroId,
                                        AbilityPatchVersionId = AppConfiguration.CurrentDotaPatchVersion.PatchVersionId,
                                        HeroPatchVersionId = AppConfiguration.CurrentDotaPatchVersion.PatchVersionId,
                                        IsTalent = true,
                                        Order = h.HeroAbilities.Count,
                                    };
                                    h.HeroAbilities.Add(ha);
                                }
                            }
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
