using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SharpGameLib.Graphics.Interfaces
{
    public interface ICanvas
    {
        Rectangle Bounds { get; }

        SpriteFont Font { get; set; }

		Vector4 ShadeFactor { get; set; }

        /// <summary>
        /// Gets the underlying resource for this ICanvas.
        /// </summary>
        /// <returns>The canvas graphics resource</returns>
        TGraphics GetCanvasGraphics<TGraphics>() where TGraphics : GraphicsResource;

		void Begin(SpriteSortMode sortMode = SpriteSortMode.BackToFront, BlendState blendState = null, Matrix? transform = null);

		void End();

        void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle = null,
            Color? color = null,
            float rotation = 0,
            Vector2? origin = null,
            Vector2? scale = null,
            SpriteEffects? effects = null,
            float layerDepth = 1f);

        void DrawRect(Rectangle rectangle, Color? color = null, float layerDepth = 0);

        void DrawLine(Vector2 start, Vector2 end, Color? color = null, float layerDepth = 0);

        void DrawString(string text, Vector2 position, Color? color = null, float scale = 0.5f, float layerDepth = 0);
    }
}

