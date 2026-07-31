using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class Notification
    {
        private Vector2 textVector2;
        private Texture2D sprite;
        private SpriteFont font;
        private float opacity;
        private string text;
        private bool isDismissed;

        public bool IsDismissed { get { return isDismissed; } }

        public Notification(SpriteFont font, string text)
        {
            this.font = font;
            this.text = text;
            textVector2 = new Vector2(10, 462);
            opacity = 1f;
            isDismissed = false;
        }

        public void Update()
        {
            if (textVector2.Y > 240)
            {
                textVector2.Y -= 1;
                opacity -= 0.005f;
            }
            else if (textVector2.Y == 240)
            {
                isDismissed = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, text, textVector2, new Color(Color.Black, opacity));
        }
    }
}
