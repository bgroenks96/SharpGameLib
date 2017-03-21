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
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Graphics
{
    public class TextureBackground : IBackground
    {
        private readonly Texture2D texture;

		private Vector2? scaleVector = null;

        public TextureBackground(Texture2D texture)
        {
            this.texture = texture;
        }

        public int DrawPriority { get; set; } = int.MinValue;

        public void Begin(ICanvas canvas, Matrix? transformMatrix = default(Matrix?))
        {
            var batch = canvas.GetCanvasGraphics<SpriteBatch>();
            var transform = transformMatrix ?? Matrix.Identity;
            batch.Begin(SpriteSortMode.Deferred, blendState: BlendState.Opaque, samplerState: SamplerState.LinearWrap, transformMatrix: transform);
        }

        public void Draw(ICanvas canvas)
        {
			this.CalculateTextureScaling(canvas.Bounds, Scene.Current.LevelBounds);
			var levelWidth = Scene.Current.LevelBounds.Width;
			var srcRect = new Rectangle(0, 0, Math.Max(this.texture.Width, levelWidth), this.texture.Height);
			canvas.Draw(this.texture, Vector2.Zero, srcRect, color: Color.White, scale: this.scaleVector, effects: SpriteEffects.None, layerDepth: 1);
        }

        public void End(ICanvas canvas)
        {
            var batch = canvas.GetCanvasGraphics<SpriteBatch>();
            batch.End();
        }

		private void CalculateTextureScaling(Rectangle canvasBounds, Rectangle levelBounds)
		{
			if (this.scaleVector.HasValue)
			{
				return;
			}

			var ratioY = Math.Max(canvasBounds.Height, levelBounds.Height) / (float)this.texture.Height;
			//var scaledHeight = ratioY * this.texture.Height;
			//var nearestPo2Height = Math.Pow(2, Math.Ceiling(Math.Log(scaledHeight) / Math.Log(2)));
			//var powOf2RatioY = nearestPo2Height / this.texture.Height;
			this.scaleVector = new Vector2((float)ratioY, (float)ratioY);
		}
    }
}

