using System;
using SharpGameLib.Effects.Interfaces;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Effects
{
    public class DefaultParticle : IParticle, IMovable, IDrawable, IUpdatable
    {
        public DefaultParticle(ISprite sprite, TimeSpan duration)
        {
            this.Sprite = sprite;
            this.Duration = duration;

            this.Id = Guid.NewGuid();
        }

        public ISprite Sprite { get; }

        public TimeSpan Duration { get; }

        public Guid Id { get; }

        public void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);
        }

        public void Draw(ICanvas canvas)
        {
            this.Sprite.Draw(canvas);
        }

        public Vector2 Position
        {
            get
            {
                return this.Sprite.Position;
            }
            set
            {
                this.Sprite.Position = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return this.Sprite.Velocity;
            }
            set
            {
                this.Sprite.Velocity = value;
            }
        }

        public Vector2 Acceleration
        {
            get
            {
                return this.Sprite.Acceleration;
            }
            set
            {
                this.Sprite.Acceleration = value;
            }
        }

        public int DrawPriority
        {
            get
            {
                return this.Sprite.DrawPriority;
            }

            set
            {
                this.Sprite.DrawPriority = value;
            }
        }

        public virtual void OnExpired()
        {
        }

        public virtual void OnEmit(IParticleEmitter emitter)
        {
        }

        public virtual void OnEnter(IStage stage)
        {
        }

        public virtual void OnExit(IStage stage)
        {
        }
    }
}

