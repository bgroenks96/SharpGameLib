using System;
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

