using System;

namespace SharpGameLib.Sprites.Interfaces
{
    public interface ISpriteFactory
    {
        SpriteConfig DefaultConfig { get; set; }

        ISprite Create(SpriteConfig config = null);
    }
}

