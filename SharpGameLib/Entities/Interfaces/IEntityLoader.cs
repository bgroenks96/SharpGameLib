using System;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Entities.Interfaces
{
    public interface IEntityLoader : IEntityFactory<IEntity>
    {
        void Register(string typeId, IEntityFactory factory);
    }
}

