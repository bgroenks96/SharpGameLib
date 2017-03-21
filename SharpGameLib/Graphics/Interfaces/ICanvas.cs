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
using Microsoft.Xna.Framework;

namespace SharpGameLib.Graphics.Interfaces
{
    public interface ICanvas
    {
        Rectangle Bounds { get; }

        SpriteFont Font { get; set; }

		Vector4 ShadeFactor { get; set; }

        /// <summary>
        /// Gets the underlying resource for this ICanvas.
        /// </summary>
        /// <returns>The canvas graphics resource</returns>
        TGraphics GetCanvasGraphics<TGraphics>() where TGraphics : GraphicsResource;

		void Begin(SpriteSortMode sortMode = SpriteSortMode.BackToFront, BlendState blendState = null, Matrix? transform = null);

		void End();

        void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle = null,
            Color? color = null,
            float rotation = 0,
            Vector2? origin = null,
            Vector2? scale = null,
            SpriteEffects? effects = null,
            float layerDepth = 1f);

        void DrawRect(Rectangle rectangle, Color? color = null, float layerDepth = 0);

        void DrawLine(Vector2 start, Vector2 end, Color? color = null, float layerDepth = 0);

        void DrawString(string text, Vector2 position, Color? color = null, float scale = 0.5f, float layerDepth = 0);
    }
}

