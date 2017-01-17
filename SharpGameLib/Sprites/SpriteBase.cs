using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Graphics.Interfaces;
using SharpGameLib.Collision;

namespace SharpGameLib.Sprites
{
    public abstract class SpriteBase : ISprite
    {
        private SpriteConfig config;

        protected SpriteBase(ISpriteSheet spriteSheet, SpriteConfig config)
        {
            this.SpriteSheet = spriteSheet;
            this.config = config;
        }

        public ISpriteSheet SpriteSheet { get; set; }

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public Vector2 Acceleration { get; set; } = Vector2.Zero;

        public Color Shade { get; set; } = new Color(255, 255, 255);

		public RectangleF ClipRegion { get; set; } = RectangleF.Empty;

        public SpriteConfig Config
        {
            get
            {
                return this.config;
            }

            set
            {
                var newConfig = value;
                if (this.config?.Equals(newConfig) ?? false)
                {
                    return;
                }

				if (newConfig.Height > 0 && this.config.Height > 0)
				{
					var heightDiff = newConfig.Height - this.config.Height;
					this.Position -= new Vector2(0, heightDiff * this.Scale.Y);
				}

                this.config = newConfig;
            }
        }

        public int DrawPriority { get; set; } = 0;

        public bool FlipX { get; set; } = false;

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(ICanvas canvas)
        {
            this.SpriteSheet.Draw(canvas, this, this.FlipX);
        }
    }
}

