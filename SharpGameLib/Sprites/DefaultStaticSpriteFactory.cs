using System;
using SharpGameLib.Sprites.Interfaces;

namespace SharpGameLib.Sprites
{
    public class DefaultStaticSpriteFactory : ISpriteFactory
    {
        private readonly ISpriteSheet spriteSheet;

        public DefaultStaticSpriteFactory(ISpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public SpriteConfig DefaultConfig { get; set; } = new SpriteConfig();

        public ISprite Create(SpriteConfig config = null)
        {
            return new DefaultStaticSprite(this.spriteSheet, config ?? this.DefaultConfig);
        }
    }
}

