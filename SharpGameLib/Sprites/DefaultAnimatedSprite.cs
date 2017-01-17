using System;
using SharpGameLib.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Sprites
{
    public class DefaultAnimatedSprite : SpriteBase
    {
        private int currentFrame = 0;
        private double elapsedAnimationTime = 0;

        public DefaultAnimatedSprite(ISpriteSheet spriteSheet, SpriteConfig config)
            : base(spriteSheet, config)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.currentFrame = (int)Math.Floor(this.Config.FrameCount * this.elapsedAnimationTime / this.Config.AnimationDuration);
            this.elapsedAnimationTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsedAnimationTime > this.Config.AnimationDuration)
            {
                this.elapsedAnimationTime = 0;
            }
        }

        public override void Draw(ICanvas canvas)
        {
            this.SpriteSheet.Draw(canvas, this, this.FlipX, this.currentFrame);
        }
    }
}

