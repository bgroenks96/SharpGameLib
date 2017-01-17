using System;
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

