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
using Microsoft.Xna.Framework;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Graphics.Interfaces;
using SharpGameLib.Collision;

namespace SharpGameLib.Sprites
{
    public abstract class SpriteBase : ISprite
    {
        private SpriteConfig config;

        protected SpriteBase(ISpriteSheet spriteSheet, SpriteConfig config)
        {
            this.SpriteSheet = spriteSheet;
            this.config = config;
        }

        public ISpriteSheet SpriteSheet { get; set; }

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public Vector2 Acceleration { get; set; } = Vector2.Zero;

        public Color Shade { get; set; } = new Color(255, 255, 255);

		public RectangleF ClipRegion { get; set; } = RectangleF.Empty;

        public SpriteConfig Config
        {
            get
            {
                return this.config;
            }

            set
            {
                var newConfig = value;
                if (this.config?.Equals(newConfig) ?? false)
                {
                    return;
                }

				if (newConfig.Height > 0 && this.config.Height > 0)
				{
					var heightDiff = newConfig.Height - this.config.Height;
					this.Position -= new Vector2(0, heightDiff * this.Scale.Y);
				}

                this.config = newConfig;
            }
        }

        public int DrawPriority { get; set; } = 0;

        public bool FlipX { get; set; } = false;

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(ICanvas canvas)
        {
            this.SpriteSheet.Draw(canvas, this, this.FlipX);
        }
    }
}

