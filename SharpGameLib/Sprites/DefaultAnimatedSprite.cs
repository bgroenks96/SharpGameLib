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

