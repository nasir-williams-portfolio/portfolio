using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Jeopardy
{
    internal class Button
    {
        private String text;
        private SpriteFont font;
        private Texture2D sprite;
        private Rectangle rectangle;

        public Button(String text, SpriteFont font, Texture2D sprite, Vector2 position)
        {
            this.text = text;
            this.font = font;
            this.sprite = sprite;

            rectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                75,
                30);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, rectangle, Color.Navy);

            sb.DrawString(
                font,                                                                         // spriteFont
                text,                                                                         // text
                new Vector2(rectangle.Center.X, rectangle.Center.Y),                          // position
                Color.Yellow,                                                                 // color
                0f,                                                                           // rotation
                new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2),  // origin
                1f,                                                                           // scale
                SpriteEffects.None,                                                           // effects
                0f);                                                                          // layerDepth

        }

        public void Update()
        {
            // this is where your past button logic can go
        }
    }
}
