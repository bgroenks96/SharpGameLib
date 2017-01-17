using System;
using SharpGameLib.Sprites.Interfaces;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Sprites
{
    public class SpriteColorAnimator : SpriteAnimatorBase
    {
        private readonly IDictionary<ISprite, Color> initialColors = new Dictionary<ISprite, Color>();

        private readonly double period;

        private readonly Color start, end;

        public SpriteColorAnimator(double period = 1000, Color start = default(Color), Color end = default(Color))
        {
            this.period = period;
            this.start = start;
            this.end = end;
        }

        protected override void Begin(ISprite sprite)
        {
            this.initialColors[sprite] = sprite.Shade;
        }

        protected override void Apply(ISprite sprite, double milliTime)
        {
            var alpha = 1.0 / 2.0 * (Math.Sin(milliTime * 2 * Math.PI / this.period) + 1);
            sprite.Shade = Interpolate(this.start, this.end, (float)alpha);
        }

        protected override void End(ISprite sprite)
        {
            if (!this.initialColors.ContainsKey(sprite))
            {
                return;
            }

            var initial = this.initialColors[sprite];
            sprite.Shade = initial;
            this.initialColors.Remove(sprite);
        }

        private static Color Interpolate(Color c0, Color c1, float alpha)
        {
            var cvec0 = c0.ToVector4();
            var cvec1 = c1.ToVector4();
            return new Color(Vector4.SmoothStep(cvec0, cvec1, alpha));
        }
    }
}

