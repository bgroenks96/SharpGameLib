using System;
using SharpGameLib.Effects.Interfaces;

namespace SharpGameLib.Entities.Interfaces
{
    public interface IProjectileEntity : IEntity, IParticle
    {
        void Remove();
    }
}

