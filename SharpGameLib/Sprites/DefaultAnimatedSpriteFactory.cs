using System;
using SharpGameLib.Sprites.Interfaces;

namespace SharpGameLib.Sprites
{
    public class DefaultAnimatedSpriteFactory : ISpriteFactory
    {
        private readonly ISpriteSheet spriteSheet;

        public DefaultAnimatedSpriteFactory(ISpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public SpriteConfig DefaultConfig { get; set; } = new SpriteConfig();

        public ISprite Create(SpriteConfig config = null)
        {
            return new DefaultAnimatedSprite(this.spriteSheet, config ?? this.DefaultConfig);
        }
    }
}

