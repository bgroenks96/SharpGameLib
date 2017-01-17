using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SharpGameLib.Interfaces;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Collision.Interfaces
{
    public delegate void CollisionEventHandler(ICollisionContainer source, CollisionEventArgs collisionEventArgs);

    public delegate void CompleteEventHandler(ICollisionContainer source, ProcessingCompleteEventArgs eventArgs);

    public interface ICollisionContainer : IUpdatable, IDrawable
    {
        event CollisionEventHandler Collision;

        event CompleteEventHandler ProcessingComplete;

        Rectangle ContainerBounds { get; }

        IEnumerable<ICollidable> Collidables { get; }

        void Add(ICollidable collidable);

        void Remove(ICollidable collidable);

        void Clear();
    }
}

