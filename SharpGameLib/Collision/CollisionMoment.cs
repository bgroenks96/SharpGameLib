using System;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Collision
{
    public struct CollisionMoment
    {
        public static readonly CollisionMoment NullCollision = new CollisionMoment(
                                                                   CollisionEdge.None, 
                                                                   CollisionEdge.None,
                                                                   float.NaN);

        public CollisionMoment(CollisionEdge thisEdge, CollisionEdge otherEdge, float timeAlpha)
        {
            this.ThisEdge = thisEdge;
            this.OtherEdge = otherEdge;
            this.TimeAlpha = timeAlpha;
        }

        public CollisionEdge ThisEdge { get; }

        public CollisionEdge OtherEdge { get; }

        public float TimeAlpha { get; }

        public bool IsNullCollision()
        {
            return this.ThisEdge.Equals(CollisionEdge.None) &&
                this.OtherEdge.Equals(CollisionEdge.None) &&
                float.IsNaN(this.TimeAlpha);
        }

        /// <summary>
        /// Creates a new CollisionMoment with 'ThisEdge' and 'OtherEdge' reversed.
        /// </summary>
        /// <returns>The edge polarity.</returns>
        public CollisionMoment ReversePolarity()
        {
            return new CollisionMoment(this.OtherEdge, this.ThisEdge, this.TimeAlpha);
        }
    }
}

