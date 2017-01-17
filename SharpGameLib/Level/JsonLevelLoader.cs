using System;
using System.Linq;
using System.IO;

using SharpGameLib.Level.Interfaces;

using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;

namespace SharpGameLib.Level
{
    public class JsonLevelLoader<TLevel> : ILevelLoader<TLevel> where TLevel : ILevel
	{
        private JsonConverter[] converters = new JsonConverter[] { new JsonEntityDataConverter(), new JsonVector2DataConverter() };

        /// <summary>
        /// Initializes a new instance of JsonLevelLoader. Note: if the assigned derived ILevel type is an interface
        /// or abstract class, you will need to provide a JsonConverter implementation to deserialize it.
        /// </summary>
        /// <param name="additionalConverters">Additional converters.</param>
        public JsonLevelLoader(params JsonConverter[] additionalConverters)
        {
            this.converters = this.converters.Concat(additionalConverters).ToArray();
        }

        public TLevel Load(Stream resourceStream)
        {
            using (var reader = new StreamReader(resourceStream))
            {
                var json = reader.ReadToEnd();
                var level = JsonConvert.DeserializeObject<TLevel>(json, this.converters);

                // process entity replication
                var entities = level.Entities.ToList();
                foreach (var entityData in level.Entities)
                {
                    var repeat = entityData.Repeat;
                    var count = repeat[0];
                    var offset = new Vector2(repeat[1], repeat[2]);
                    var basePos = entityData.Position;
                    for (var i = 1; i <= count; i++)
                    {
                        var newEntityData = entityData.Clone();
                        var position = basePos + i * offset;
                        newEntityData.Position = position;
                        entities.Add(newEntityData);
                    }

					foreach (var key in entityData.Properties.Keys.ToList())
					{
						var value = entityData.Properties[key] as JToken;
						if (value == null)
						{
							continue;
						}

						object newValue;
						newValue = value.Type == JTokenType.Array ? value.ToObject<object[]> () : value.ToObject<object> ();

						entityData.Properties [key] = newValue;
					}
                }

                level.Entities = entities.ToArray();
                return level;
            }   
        }
	}
}

