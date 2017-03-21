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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SharpGameLib.Graphics
{
    public static class Fonts
    {
        private const string FontContentDir = "Fonts";

        private static readonly IDictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static SpriteFont Get(string fontName)
        {
            if (Scene.Current == null)
            {
                throw new Exception("No active scene!");
            }

            if (!fonts.ContainsKey(fontName))
            {
                fonts[fontName] = Load(fontName);
            }

            return fonts[fontName];
        }

        private static SpriteFont Load(string fontName)
        {
            return Scene.Current.Context.LoadContent<SpriteFont>($"{FontContentDir}/{fontName}");
        }
    }
}

