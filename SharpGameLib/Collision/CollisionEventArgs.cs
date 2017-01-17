using System;
using SharpGameLib.Collision.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Collision
{
    public class CollisionEventArgs : EventArgs
    {
        public CollisionEventArgs(ICollidable entityOne, ICollidable entityTwo, CollisionMoment moment)
        {
            this.EntityOne = entityOne;
            this.EntityTwo = entityTwo;
            this.CollisionMoment = moment;
        }

        ICollidable EntityOne { get; }

        ICollidable EntityTwo { get; }

        CollisionMoment CollisionMoment { get; }
    }
}

