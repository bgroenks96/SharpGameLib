using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Graphics.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using SharpGameLib.Graphics;

namespace SharpGameLib.Gui
{
    public delegate string TextFunc();

    public class GuiText : GuiElementBase
    {
        public GuiText(TextFunc textFunc, float x = 0, float y = 0, int options = 0)
            : base(options)
        {
            this.TextFunc = textFunc;
            this.Position = new Vector2(x, y);
        }

        public GuiText(string text, float x = 0, float y = 0, int options = 0)
            : this(() => text, x, y, options)
        {
        }

        public string Text
        {
            get
            {
                return this.TextFunc?.Invoke();
            }

            set
            {
                this.TextFunc = () => value;
            }
        }

        public TextFunc TextFunc { get; set; }

		public SpriteFont Font { get; set; }

        protected override void DrawElement(ICanvas canvas)
        {
			var oldFont = canvas.Font;
			if (this.Font != null)
			{
				canvas.Font = this.Font;
			}

            var pos = this.GetAbsolutePosition(canvas.Bounds);
            canvas.DrawString(this.Text, pos, this.Shade, this.Scale, 0);

			// restore previous font
			canvas.Font = oldFont;
        }

        protected override void UpdateElement(GameTime gameTime)
        {
        }
    }
}

