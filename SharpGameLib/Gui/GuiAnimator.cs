/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
using System.Linq;
using SharpGameLib.Gui.Interfaces;
using System.Collections.Generic;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace SharpGameLib.Gui
{
    internal delegate void ApplyAnimation(IGuiElement element, double t);

    public class GuiAnimator : IUpdatable
    {
        private ISet<AnimationInstance> animations = new HashSet<AnimationInstance>();

        public void Update(GameTime gameTime)
        {
            var toRemove = new List<AnimationInstance>();
			foreach (var animation in animations.ToList())
            {
                animation.Update(gameTime);
                if (animation.IsExpired())
                {
                    toRemove.Add(animation);
                }
            }

            foreach (var expiredAnimation in toRemove)
            {
                animations.Remove(expiredAnimation);
            }
        }

        public Task BeginScale(IGuiElement element, float targetScale, TimeSpan duration)
        {
            var initialScale = element.Scale;
            ApplyAnimation scaleFunc = (e, t) => e.Scale = (float)(initialScale + (targetScale - initialScale) * t);
            this.animations.Add(new AnimationInstance(element, duration, scaleFunc));
            return Task.Delay(duration);
        }

        public Task BeginMove(IGuiElement element, Vector2 targetPos, TimeSpan duration)
        {
            var initialPos = element.Position;
            ApplyAnimation scaleFunc = (e, t) => e.Position = initialPos + (targetPos - initialPos) * (float)t;
            this.animations.Add(new AnimationInstance(element, duration, scaleFunc));
            return Task.Delay(duration);
        }

		public Task BeginColorChange(IGuiElement element, Color targetColor, TimeSpan duration)
		{
			var initialColor = element.Shade;
			ApplyAnimation colorFunc = (e, t) => e.Shade = InterpolateColors(initialColor, targetColor, (float)t);
			this.animations.Add(new AnimationInstance(element, duration, colorFunc));
			return Task.Delay(duration);
		}

		public Task BeginSineColorChange(IGuiElement element, Color startColor, Color targetColor, TimeSpan duration, float freq = 1)
		{
			ApplyAnimation colorFunc = (e, t) =>
			{
					var alpha = 1.0 / 2.0 * (Math.Sin(t * 2 * Math.PI * freq) + 1);
					e.Shade = InterpolateColors(startColor, targetColor, (float)alpha);
			};
			this.animations.Add(new AnimationInstance(element, duration, colorFunc));
			return Task.Delay(duration);
		}

		public void CancelAnimationsFor(IGuiElement element)
		{
			foreach (var animation in this.animations.Where(inst => inst.Element == element).ToList())
			{
				animations.Remove(animation);
			}
		}

		public void CancelAll()
		{
			animations.Clear();
		}

		private static Color InterpolateColors(Color c0, Color c1, float alpha)
		{
			var cvec0 = c0.ToVector4();
			var cvec1 = c1.ToVector4();
			return new Color(Vector4.SmoothStep(cvec0, cvec1, alpha));
		}

        private class AnimationInstance
        {
            internal AnimationInstance(IGuiElement element, TimeSpan duration, ApplyAnimation func)
            {
                this.Element = element;
                this.Initial = duration;
                this.TimeRemaining = duration;
                this.Func = func;
            }

            internal IGuiElement Element { get; }

            internal ApplyAnimation Func { get; }

            internal TimeSpan Initial { get; }

            internal TimeSpan TimeRemaining { get; set; }

            internal void Update(GameTime gameTime)
            {
                this.TimeRemaining -= gameTime.ElapsedGameTime;
                this.Func.Invoke(this.Element, 1 - this.TimeRemaining.TotalMilliseconds / this.Initial.TotalMilliseconds);
            }

            internal bool IsExpired()
            {
                return this.TimeRemaining <= TimeSpan.Zero;
            }
        }
    }
}

