using System;
using SharpGameLib.Collision.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Interfaces;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Collision
{
    public abstract class CollidableBase : ICollidable
    {
        public Color DebugColor { get; set; }

        public bool IsEnabled { get; set; } = true;

        public abstract Vector2 Position { get; set; }

        public abstract Vector2 Velocity { get; set; }

        public abstract Vector2 Acceleration { get; set; }

        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// <see cref="IUpdatable.Update(GameTime)"/>; called BEFORE any collision processing takes place.
        /// Make sure all input processing and updates to velocity happen here.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public abstract void Update(GameTime gameTime);

        public abstract void OnCollideWith(ICollidable target, CollisionMoment collisionMoment, ICollisionContainer container);

        public abstract void OnContainerExit(Vector2 edgePosition, ICollisionContainer container);

        public void OnProcessingComplete(GameTime gameTime, TimeSpan totalProcessingTime, ICollisionContainer container)
        {
        }

        public virtual void OnCollideWith<TCollidable>(ICollidable target, CollisionMoment collisionMoment, ICollisionContainer container) 
            where TCollidable : class, ICollidable
        {
            var self = this as TCollidable;
            if (self == null)
            {
                return;
            }

            var collisionTarget = target as ICollidable<TCollidable>;
            collisionTarget?.OnCollideWith(self, collisionMoment, container);
        }

        public virtual void OnEnter(IStage stage)
        {
        }

        public virtual void OnExit(IStage stage)
        {
        }

        /// <summary>
        /// Applies the given collision moment to the current collidable's movement vectors.
        /// This method adjusts position, velocity, and acceleration for the current update
        /// cycle according to the collision edges and time alpha value reported by the given
        /// collision information.
        /// </summary>
        /// <param name="collisionMoment">Collision moment.</param>
        protected void ApplyToMovement(CollisionMoment collisionMoment)
        {
            var modifier = Vector2.One;
            if (collisionMoment.ThisEdge.IsTop() || collisionMoment.ThisEdge.IsBottom())
            {
                modifier = new Vector2(modifier.X, collisionMoment.TimeAlpha);
            }
            else if (collisionMoment.ThisEdge.IsLeft() || collisionMoment.ThisEdge.IsRight())
            {
                modifier = new Vector2(collisionMoment.TimeAlpha, modifier.Y);
            }

            this.Acceleration *= modifier;
            this.Velocity *= modifier;
            this.Position += Vector2.Negate(this.Velocity * (Vector2.One - modifier));
        }
    }
}

