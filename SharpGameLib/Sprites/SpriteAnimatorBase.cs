using System;
using SharpGameLib.Sprites.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Sprites
{
    public abstract class SpriteAnimatorBase : ISpriteAnimator
    {
        private IDictionary<ISprite, TimeSpan> spriteAnimations = new Dictionary<ISprite, TimeSpan>();

        public void Start(ISprite sprite, TimeSpan duration)
        {
            this.spriteAnimations[sprite] = duration;
            this.Begin(sprite);
        }

        public void Cancel(ISprite sprite)
        {
            this.spriteAnimations.Remove(sprite);
            this.End(sprite);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var sprite in this.spriteAnimations.Keys.ToList())
            {
                var remaining = this.spriteAnimations[sprite];
                remaining -= gameTime.ElapsedGameTime;
                if (remaining <= TimeSpan.Zero)
                {
                    this.Cancel(sprite);
                    continue;
                }

                this.Apply(sprite, gameTime.TotalGameTime.TotalMilliseconds);
                this.spriteAnimations[sprite] = remaining;
            }
        }

        protected abstract void Begin(ISprite sprite);

        protected abstract void Apply(ISprite sprite, double milliTime);

        protected abstract void End(ISprite sprite);
    }
}

