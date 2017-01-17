using System;

using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Sprites.Interfaces
{
    public interface ISpriteSheet : IDisposable
    {
		Rectangle Bounds { get; }

        void Draw(ICanvas canvas, ISprite sprite, bool flipX = false, int strideFactor = 0);
    }
}

