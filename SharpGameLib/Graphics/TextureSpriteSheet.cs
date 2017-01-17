using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SharpGameLib.Interfaces;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Graphics.Interfaces;
using SharpGameLib.Collision;

namespace SharpGameLib.Graphics
{
    internal class TextureSpriteSheet : ISpriteSheet
    {
        private readonly Texture2D texture;

        internal TextureSpriteSheet(Texture2D texture)
        {
            this.texture = texture;
        }

		public Rectangle Bounds
		{
			get
			{
				return this.texture.Bounds;
			}
		}

        public void Draw(ICanvas canvas, ISprite sprite, bool flipX = false, int strideFactor = 0)
        {
            var config = sprite.Config;
            var position = sprite.Position;
            var scale = sprite.Scale;
            var xpos = config.XOffs + config.XStride * strideFactor;
            var ypos = config.YOffs + config.YStride * strideFactor;
			var sourceRect = ComputeClippedTextureRegion (sprite, xpos, ypos);
            var effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            canvas?.Draw(this.texture, position, color: sprite.Shade, sourceRectangle: sourceRect, scale: scale, effects: effects);
        }

        public void Dispose()
        {
            this.texture.Dispose();
        }

		/// <summary>
		/// Applies the clipping region for this sprite in texture space and returns the
		/// appropriate source rectangle.
		/// </summary>
		/// <returns>The clipped texture region.</returns>
		/// <param name="sprite">Sprite</param>
		/// <param name="tx">texture x pos</param>
		/// <param name="ty">texture y pos</param>
		private static Rectangle ComputeClippedTextureRegion(ISprite sprite, int tx, int ty)
		{
			if (sprite.ClipRegion.IsEmpty)
			{
				return new Rectangle(tx, ty, sprite.Config.Width, sprite.Config.Height);
			}

			var clipRegion = sprite.ClipRegion;
			var spriteBounds = sprite.Config.BoundsWith (sprite.Position);
			var overlap = RectangleF.Intersect (spriteBounds, clipRegion);
			if (overlap.IsEmpty)
			{
				return Rectangle.Empty;
			}

			var wtRatio = overlap.Width / spriteBounds.Width;
			var htRatio = overlap.Height / spriteBounds.Height;
			var texSpaceX = (int)Math.Floor(overlap.X - sprite.Position.X + tx);
			var texSpaceY = (int)Math.Floor(overlap.Y - sprite.Position.Y + ty);
			var texSpaceWt = (int)Math.Ceiling (wtRatio * sprite.Config.Width);
			var texSpaceHt = (int)Math.Ceiling (htRatio * sprite.Config.Height);
			return new Rectangle (texSpaceX, texSpaceY, texSpaceWt, texSpaceHt);
		}
    }
}

