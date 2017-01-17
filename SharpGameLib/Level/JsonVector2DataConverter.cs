using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Level
{
    public class JsonVector2DataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = serializer.Deserialize<float[]>(reader);
            return new Vector2(array[0], array[1]);
            // return arrays.Select(array => new Vector2(array[0], array[1])).ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}

