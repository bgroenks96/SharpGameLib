using System;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Sprites.Interfaces
{
    /// <summary>
    /// Utility type for animating sprite properties.
    /// </summary>
    public interface ISpriteAnimator : IUpdatable
    {
        void Start(ISprite sprite, TimeSpan duration);

        void Cancel(ISprite sprite);
    }
}

