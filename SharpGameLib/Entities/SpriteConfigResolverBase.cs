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
using System.Reflection;
using System.Collections.Generic;
using SharpGameLib.Entities.Interfaces;
using SharpGameLib.States.Interfaces;

namespace SharpGameLib.Entities
{
    public abstract class SpriteConfigResolverBase : ISpriteConfigResolver
    {
        private readonly IDictionary<uint, SpriteConfig> stateIdsToConfigs = new Dictionary<uint, SpriteConfig>();

        protected SpriteConfigResolverBase(Type spriteConfigDefinitionType)
        {
            var props = spriteConfigDefinitionType.GetRuntimeProperties();
            var configProps = props.Where(prop => prop.PropertyType.Equals(typeof(SpriteConfig)));
            foreach (var prop in configProps)
            {
                var config = prop.GetValue(null) as SpriteConfig;
                stateIdsToConfigs[config.StateCode] = config;
            }
        }

        public SpriteConfig Resolve(params IState[] states)
        {
            uint stateCode = 0;
            foreach (var state in states)
            {
                stateCode |= state.Id;
            }

            if (!stateIdsToConfigs.ContainsKey(stateCode))
            {
                var stateStr = string.Join(",", states.Select(s => s.GetType()));
                throw new Exception($"No sprite configuration mapping for {stateStr}");
            }

            return stateIdsToConfigs[stateCode];
        }
    }
}

