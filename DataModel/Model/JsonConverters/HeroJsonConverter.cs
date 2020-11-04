using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.Model.JsonConverters
{
    public class HeroPrincipalAttributeJsonConverter : JsonConverter<HeroPrincipalAttribute>
    {
        public override HeroPrincipalAttribute Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, HeroPrincipalAttribute value, JsonSerializerOptions options)
        {
            switch(value)
            {
                case (HeroPrincipalAttribute.Strength):
                    writer.WriteStringValue("Strength");
                    break;
                case (HeroPrincipalAttribute.Agility):
                    writer.WriteStringValue("Agility");
                    break;
                default:
                    writer.WriteStringValue("Inteligence");
                    break;
            }
            
        }
    }
}
