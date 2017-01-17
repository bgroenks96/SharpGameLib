using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Interfaces;
using SharpGameLib.Collision;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Sprites.Interfaces
{
    public interface ISprite : IDrawable, IUpdatable, IMovable
    {
        ISpriteSheet SpriteSheet { get; set; }

        SpriteConfig Config { get; set; }

        Vector2 Scale { get; set; }

        Color Shade { get; set; }

		RectangleF ClipRegion { get; set; }

        bool FlipX { get; set; }
    }
}

