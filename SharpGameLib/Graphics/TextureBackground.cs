﻿using System;
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;

namespace SharpGameLib.Graphics
{
    public class TextureBackground : IBackground
    {
        private readonly Texture2D texture;

		private Vector2? scaleVector = null;

        public TextureBackground(Texture2D texture)
        {
            this.texture = texture;
        }

        public int DrawPriority { get; set; } = int.MinValue;

        public void Begin(ICanvas canvas, Matrix? transformMatrix = default(Matrix?))
        {
            var batch = canvas.GetCanvasGraphics<SpriteBatch>();
            var transform = transformMatrix ?? Matrix.Identity;
            batch.Begin(SpriteSortMode.Deferred, blendState: BlendState.Opaque, samplerState: SamplerState.LinearWrap, transformMatrix: transform);
        }

        public void Draw(ICanvas canvas)
        {
			this.CalculateTextureScaling(canvas.Bounds, Scene.Current.LevelBounds);
			var levelWidth = Scene.Current.LevelBounds.Width;
			var srcRect = new Rectangle(0, 0, Math.Max(this.texture.Width, levelWidth), this.texture.Height);
			canvas.Draw(this.texture, Vector2.Zero, srcRect, color: Color.White, scale: this.scaleVector, effects: SpriteEffects.None, layerDepth: 1);
        }

        public void End(ICanvas canvas)
        {
            var batch = canvas.GetCanvasGraphics<SpriteBatch>();
            batch.End();
        }

		private void CalculateTextureScaling(Rectangle canvasBounds, Rectangle levelBounds)
		{
			if (this.scaleVector.HasValue)
			{
				return;
			}

			var ratioY = Math.Max(canvasBounds.Height, levelBounds.Height) / (float)this.texture.Height;
			//var scaledHeight = ratioY * this.texture.Height;
			//var nearestPo2Height = Math.Pow(2, Math.Ceiling(Math.Log(scaledHeight) / Math.Log(2)));
			//var powOf2RatioY = nearestPo2Height / this.texture.Height;
			this.scaleVector = new Vector2((float)ratioY, (float)ratioY);
		}
    }
}

