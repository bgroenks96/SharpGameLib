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
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Graphics;

namespace SharpGameLib.Gui
{
    public delegate string TextFunc();

    public class GuiText : GuiElementBase
    {
        public GuiText(TextFunc textFunc, float x = 0, float y = 0, int options = 0)
            : base(options)
        {
            this.TextFunc = textFunc;
            this.Position = new Vector2(x, y);
        }

        public GuiText(string text, float x = 0, float y = 0, int options = 0)
            : this(() => text, x, y, options)
        {
        }

        public string Text
        {
            get
            {
                return this.TextFunc?.Invoke();
            }

            set
            {
                this.TextFunc = () => value;
            }
        }

        public TextFunc TextFunc { get; set; }

		public SpriteFont Font { get; set; }

        protected override void DrawElement(ICanvas canvas)
        {
			var oldFont = canvas.Font;
			if (this.Font != null)
			{
				canvas.Font = this.Font;
			}

            var pos = this.GetAbsolutePosition(canvas.Bounds);
            canvas.DrawString(this.Text, pos, this.Shade, this.Scale, 0);

			// restore previous font
			canvas.Font = oldFont;
        }

        protected override void UpdateElement(GameTime gameTime)
        {
        }
    }
}

