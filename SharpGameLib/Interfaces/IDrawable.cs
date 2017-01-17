using System;
using SharpGameLib.Sprites.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Interfaces
{
    public interface IDrawable
    {
        int DrawPriority { get; set; }

        void Draw(ICanvas canvas);
    }
}

