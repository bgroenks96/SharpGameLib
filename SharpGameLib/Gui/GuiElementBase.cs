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
using SharpGameLib.Gui.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Gui
{
    public abstract class GuiElementBase : IGuiElement
    {
        protected GuiElementBase(int options = 0)
        {
            this.Options = options;
        }

        public int Options { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool IsVisible { get; set; } = true;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }

		public float Scale { get; set; } = 1.0f;

		public Color Shade { get; set; } = Color.White;

        public int DrawPriority { get; set; }

        public void Draw(ICanvas canvas)
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.DrawElement(canvas);
        }

        public void Update(GameTime gameTime)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            this.UpdateElement(gameTime);
        }

        public Vector2 GetAbsolutePosition(Rectangle bounds)
        {
            return (this.Options & GuiOptions.OptionPositionProportional) != 0 ? this.FromProportional(this.Position, bounds) : this.Position;

        }

        protected abstract void DrawElement(ICanvas canvas);

        protected abstract void UpdateElement(GameTime gameTime);

        protected Vector2 FromProportional(Vector2 pos, Rectangle bounds)
        {
            return new Vector2(pos.X * bounds.Width, pos.Y * bounds.Height);
        }
    }
}

