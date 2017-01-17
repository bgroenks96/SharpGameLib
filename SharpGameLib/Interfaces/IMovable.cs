using System;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Interfaces
{
    public interface IMovable
    {
        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }

        Vector2 Acceleration { get; set; }
    }
}

