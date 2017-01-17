using System;
using Newtonsoft.Json;

namespace SharpGameLib.Level
{
    public static class JsonReaderExt
    {
        public static bool ReadUntil(this JsonReader reader, JsonToken token)
        {
            while (reader.TokenType != token)
            {
                if (!reader.Read())
                {
                    return false;
                }
            }

            return true;
        }

        public static void AssertValue(this JsonReader reader, string value)
        {
            var token = reader.ReadAsString();
            if (!token.Equals(value))
            {
                throw new FormatException($"found {token} when {value} was expected");
            }
        }
    }
}

