using System;
using SharpGameLib.Collision.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Entities.Interfaces;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Collision
{
    public abstract class EntityCollidableBase<TEntity> : CollidableBase, IEntityCollidable<TEntity> where TEntity : IEntity
    {
        protected EntityCollidableBase(TEntity entity)
        {
            this.Entity = entity;
            this.DebugColor = Color.Blue;
        }

        public TEntity Entity { get; }

        public override RectangleF Bounds
        {
            get
            {
                return this.Entity.Bounds;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return this.Entity.Position;
            }
            set
            {
                this.Entity.Position = value;
            }
        }
        public override Vector2 Velocity
        {
            get
            {
                return this.Entity.Velocity;
            }
            set
            {
                this.Entity.Velocity = value;
            }
        }
        public override Vector2 Acceleration
        {
            get
            {
                return this.Entity.Acceleration;
            }
            set
            {
                this.Entity.Acceleration = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void OnContainerExit(Vector2 edgePosition, ICollisionContainer container)
        {
        }
    }
}

