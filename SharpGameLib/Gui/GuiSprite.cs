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
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Gui
{
	public class GuiSprite : GuiElementBase
	{
		public GuiSprite(ISprite sprite, float x = 0, float y = 0, int options = 0)
			: base (options)
		{
			this.Sprite = sprite;
			this.Position = new Vector2(x, y);
		}

		public ISprite Sprite { get; }

		public int RepeatX { get; set; }

		public int RepeatY { get; set; }

		public int RepeatMargin { get; set; } = 1;

		protected override void DrawElement(ICanvas canvas)
		{
			var pos = this.GetAbsolutePosition(canvas.Bounds);
			this.Sprite.Position = pos;
			this.Sprite.Shade = this.Shade;
			this.Sprite.Scale = new Vector2(this.Scale, this.Scale);
			this.Sprite.Draw(canvas);

			// process repeat x option
			var spriteWidth = this.Sprite.Config.Width * this.Scale;
			for (var i = 1; i <= this.RepeatX; i++)
			{
				this.Sprite.Position = pos + new Vector2(i * (spriteWidth + this.RepeatMargin), 0);
				this.Sprite.Draw(canvas);
			}

			// process repeat y option
			var spriteHeight = this.Sprite.Config.Height * this.Scale;
			for (var i = 1; i <= this.RepeatY; i++)
			{
				this.Sprite.Position = pos + new Vector2(0, i * (spriteHeight + this.RepeatMargin));
				this.Sprite.Draw(canvas);
			}

			// reset position to base value
			this.Sprite.Position = pos;
		}

		protected override void UpdateElement(GameTime gameTime)
		{
			this.Sprite.Update(gameTime);
		}
	}
}

