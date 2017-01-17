using System;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Collision.Interfaces
{
    public interface IEntityCollidable<TEntity> : ICollidable where TEntity : IEntity
    {
        TEntity Entity { get; }
    }
}

