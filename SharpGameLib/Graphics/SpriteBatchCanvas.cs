using System;
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Graphics
{
    public class SpriteBatchCanvas : ICanvas
    {
        private const string DefaultFontName = "Arial-32";

        private readonly SpriteBatch drawBatch;

        private readonly Texture2D blankTexture;

        private readonly SpriteFont defaultFont;

        private readonly IGameContext context;

        public SpriteBatchCanvas(IGameContext context)
        {
            this.context = context;
            this.drawBatch = new SpriteBatch(context.Graphics);
            this.blankTexture = new Texture2D(this.drawBatch.GraphicsDevice, 1, 1);
            this.blankTexture.SetData(new[] { Color.White });
            this.defaultFont = Fonts.Get(DefaultFontName);
        }

        public Rectangle Bounds
        {
            get
            {
                return this.context.Graphics.Viewport.Bounds;
            }
        }

		public Vector4 ShadeFactor { get; set; } = Vector4.One;

        public SpriteFont Font { get; set; } 

        public TGraphics GetCanvasGraphics<TGraphics>() where TGraphics : GraphicsResource
        {
            var result = this.drawBatch as TGraphics;
            if (result == null)
            {
                throw new Exception($"Invalid type parameter {typeof(TGraphics)}; expected {typeof(SpriteBatch)}");
            }

            return result;
        }

		public void Begin(SpriteSortMode sortMode = SpriteSortMode.BackToFront, BlendState blendState = null, Matrix? transform = null)
		{
			blendState = blendState ?? BlendState.AlphaBlend;
			transform = transform ?? Matrix.Identity;
			this.drawBatch.Begin(sortMode, blendState, transformMatrix: transform);
		}

		public void End()
		{
			this.drawBatch.End();
		}

        public void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle = null,
            Color? color = null,
            float rotation = 0f,
            Vector2? origin = null,
            Vector2? scale = null,
            SpriteEffects? effects = null,
            float layerDepth = 1f)
        {
			var colorValue = color.HasValue ? this.ApplyShadingFactor(color.Value) : Color.White;
            var originValue = origin.HasValue ? origin.Value : Vector2.Zero;
            var scaleValue = scale.HasValue ? scale.Value : Vector2.One;
            var effectsValue = effects.HasValue ? effects.Value : SpriteEffects.None;
            this.drawBatch.Draw(
                texture: texture,
                position: position,
                sourceRectangle: sourceRectangle,
                color: colorValue,
                rotation: rotation,
                origin: originValue,
                scale: scaleValue,
                effects: effectsValue,
                layerDepth: layerDepth);
        }

        public void DrawRect(Rectangle rect, Color? color = null, float layerDepth = 0)
        {
            this.DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom), color, layerDepth);
            this.DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, layerDepth);
            this.DrawLine(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Right, rect.Top), color, layerDepth);
            this.DrawLine(new Vector2(rect.Right, rect.Top), new Vector2(rect.Left, rect.Top), color, layerDepth);
        }

        public void DrawLine(Vector2 start, Vector2 end, Color? color = null, float layerDepth = 0)
        {
            var length = (end - start).Length();
            var rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
			var colorValue = color.HasValue ? this.ApplyShadingFactor(color.Value) : Color.White;
            this.drawBatch.Draw(this.blankTexture, start, rotation: rotation, color: colorValue, scale: new Vector2(length, 1), layerDepth: layerDepth);
        }

        public void DrawString(string text, Vector2 position, Color? color = null, float scale = 0.5f, float layerDepth = 0)
        {
			var colorValue = color.HasValue ? this.ApplyShadingFactor(color.Value) : Color.White;
            this.drawBatch.DrawString(this.Font ?? this.defaultFont, text, position, colorValue, 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }

		private Color ApplyShadingFactor(Color color)
		{
			var cvec = color.ToVector4() * this.ShadeFactor;
			return new Color(cvec);
		}
    }
}

