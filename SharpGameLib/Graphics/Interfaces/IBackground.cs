using System;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Graphics.Interfaces
{
    public interface IBackground : IDrawable
    {
        void Begin(ICanvas canvas, Matrix? transform = default(Matrix?));

        void End(ICanvas canvas);
    }
}

