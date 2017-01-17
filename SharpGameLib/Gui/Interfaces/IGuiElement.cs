using System;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Gui.Interfaces
{
    public interface IGuiElement : IDrawable, IUpdatable, IMovable
    {
        bool IsEnabled { get; set; }

        bool IsVisible { get; set; }

        float Scale { get; set; }

        Color Shade { get; set; }

        int Options { get; set; }

        Vector2 GetAbsolutePosition(Rectangle bounds);
    }
}

