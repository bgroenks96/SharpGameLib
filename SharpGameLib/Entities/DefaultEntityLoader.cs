
using System.Collections.Generic;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Entities
{
    public class DefaultEntityLoader : IEntityLoader
    {
        private IDictionary<string, IEntityFactory> loaders = new Dictionary<string, IEntityFactory>();

        public void Register(string typeId, IEntityFactory factory)
        {
            this.loaders[typeId] = factory;
        }

        public IEntity Create(IEntityData data)
        {
            return this.loaders[data.Type].Create(data);
        }

        public bool Has(string typeId)
        {
            return this.loaders.ContainsKey(typeId);
        }
    }
}

