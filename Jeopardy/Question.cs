using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Jeopardy
{
    internal class Question
    {
        protected String text;
        private SpriteFont font;
        private Vector2 position;

        public Question(String text, SpriteFont font, Vector2 position)
        {
            this.text = text;
            this.font = font;
            this.position = position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, text, position, Color.White);
        }
    }
}
