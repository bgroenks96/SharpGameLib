/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
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

