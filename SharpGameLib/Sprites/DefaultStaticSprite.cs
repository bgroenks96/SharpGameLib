using System;
using SharpGameLib.Sprites.Interfaces;

namespace SharpGameLib.Sprites
{
    public class DefaultStaticSprite : SpriteBase
    {
        public DefaultStaticSprite(ISpriteSheet spriteSheet, SpriteConfig config)
            : base(spriteSheet, config)
        {
        }
    }
}

