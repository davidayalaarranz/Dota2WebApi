using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
                        case "img":
                            hi.ImagePath = reader.GetString();
                            if (hi.ImagePath.Contains("?"))
                            {
                                hi.ImagePath = hi.ImagePath.Substring(0, hi.ImagePath.IndexOf("?"));
                            }
                            break;
                        case "desc":
                            hi.Description = reader.GetString();
                            break;
                        case "qual":
                            if (reader.TokenType == JsonTokenType.String)
                            {
                                hi.Qual = reader.GetString();
                            }
                            else if (reader.TokenType == JsonTokenType.False)
                            {
                                hi.Qual = "false";
                            }
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

                            ParseAttribDescription(hi);

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
                                    // La siguiente línea se añadió porque en la 7.27d, el array de componentes podía contener false en lugar de un string.
                                    if (reader.TokenType == JsonTokenType.False) continue;
                                    
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

        private void ParseAttribDescription(HeroItem hi)
        {
            string attribDesc = hi.Attrib;
            //+ <span class=\"attribVal\">4</span>
            //Regex reAttribVal = new Regex(@"\+ <span class=\""attribVal\"">(\d+)</span>");
            //<span class=\"attribValText\">Armor</span>
            //Regex reAttribText = new Regex(@"<span class=\""attribValText\"">([A-Za-z ]+)</span>");
            
            //+ <span class="attribVal">16</span> <span class="attribValText">Strength</span>
            Regex reAttrib = new Regex(@"\+ <span class=\""attribVal\"">(\d+)</span> <span class=\""attribValText\"">([A-Za-z ]+)</span>");
            System.Text.RegularExpressions.MatchCollection mAttrib = reAttrib.Matches(attribDesc);
            List<int> attribVal = new List<int>();
            List<string> attribText = new List<string>();
            foreach (System.Text.RegularExpressions.Match match in mAttrib)
            {
                attribVal.Add(int.Parse(match.Groups[1].Value));
                attribText.Add(match.Groups[2].Value);
            }

            //System.Text.RegularExpressions.MatchCollection mAttribVal = reAttribVal.Matches(attribDesc);
            //System.Text.RegularExpressions.MatchCollection mAttribText = reAttribText.Matches(attribDesc);

            

            

            //foreach (System.Text.RegularExpressions.Match match in mAttribVal)
            //{
            //    attribVal.Add(int.Parse(match.Groups[1].Value));
            //}
            //foreach (System.Text.RegularExpressions.Match match in mAttribText)
            //{
            //    attribText.Add(match.Groups[1].Value);
            //}



            if (attribVal.Count > 0)
            {
                for (int i = 0; i < attribVal.Count; i++)
                {
                    switch (attribText[i])
                    {
                        case "Strength":
                            hi.BonusStrength = attribVal[i];
                            break;
                        case "Agility":
                            hi.BonusAgility = attribVal[i];
                            break;
                        case "Intelligence":
                            hi.BonusIntelligence = attribVal[i];
                            break;
                        case "All Attributes":
                            hi.BonusStrength = attribVal[i];
                            hi.BonusAgility = attribVal[i];
                            hi.BonusIntelligence = attribVal[i];
                            break;
                        case "Primary Attribute":
                            break;
                        case "Attack Speed":
                            hi.BonusAttackSpeed = attribVal[i];
                            break;
                        case "Movement Speed":
                            hi.BonusMovementSpeed = attribVal[i];
                            break;
                        case "HP Regeneration":
                            hi.BonusHPRegeneration = attribVal[i];
                            break;
                        case "Mana Regeneration":
                            hi.BonusManaRegeneration = attribVal[i];
                            break;
                        case "Damage":
                            hi.BonusDamage = attribVal[i];
                            break;
                        case "Health":
                            hi.BonusHealth = attribVal[i];
                            break;
                        case "Mana":
                            hi.BonusMana = attribVal[i];
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, HeroItem value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); //This will never be called since CanWrite is false
        }
    }
}
