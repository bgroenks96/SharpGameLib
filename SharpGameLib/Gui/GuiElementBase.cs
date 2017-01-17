using System;
using SharpGameLib.Gui.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Gui
{
    public abstract class GuiElementBase : IGuiElement
    {
        protected GuiElementBase(int options = 0)
        {
            this.Options = options;
        }

        public int Options { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool IsVisible { get; set; } = true;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }

		public float Scale { get; set; } = 1.0f;

		public Color Shade { get; set; } = Color.White;

        public int DrawPriority { get; set; }

        public void Draw(ICanvas canvas)
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.DrawElement(canvas);
        }

        public void Update(GameTime gameTime)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            this.UpdateElement(gameTime);
        }

        public Vector2 GetAbsolutePosition(Rectangle bounds)
        {
            return (this.Options & GuiOptions.OptionPositionProportional) != 0 ? this.FromProportional(this.Position, bounds) : this.Position;

        }

        protected abstract void DrawElement(ICanvas canvas);

        protected abstract void UpdateElement(GameTime gameTime);

        protected Vector2 FromProportional(Vector2 pos, Rectangle bounds)
        {
            return new Vector2(pos.X * bounds.Width, pos.Y * bounds.Height);
        }
    }
}

