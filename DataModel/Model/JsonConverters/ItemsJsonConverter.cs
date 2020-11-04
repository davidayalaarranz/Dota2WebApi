using DataModel.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataModel.ValveJsonModel.GetItems
{
    public class ItemsJsonConverter : JsonConverter<HeroItem>
    {
        public override HeroItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, HeroItem value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

}
