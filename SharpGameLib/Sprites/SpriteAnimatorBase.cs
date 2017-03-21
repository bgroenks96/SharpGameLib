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

