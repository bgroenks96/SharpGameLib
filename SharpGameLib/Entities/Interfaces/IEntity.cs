using System;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;
using SharpGameLib.Collision.Interfaces;
using SharpGameLib.Collision;

namespace SharpGameLib.Entities.Interfaces
{
    public interface IEntity : IDrawable, IUpdatable, IMovable, IStageActor
    {
        ISprite Sprite { get; }

        ICollidable Collidable { get; }

        RectangleF Bounds { get; }

        Guid Id { get; }

        bool HasGravity { get; set; }

        bool IsCollisionEnabled { get; set; }

        Vector2 StartVelocity();
    }
}

