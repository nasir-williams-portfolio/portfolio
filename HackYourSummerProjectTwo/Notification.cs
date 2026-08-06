using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class Notification
    {
        private SpriteFont font;
        private Vector2 location;
        private string text;
        private int opacity;
        private int rgb;
        private bool isDismissed;

        public bool IsDismissed { get { return isDismissed; } }

        public Notification(SpriteFont font, string text, Vector2 location)
        {
            this.font = font;
            this.text = text;
            this.location = location;
            isDismissed = false;

            opacity = 255;
            rgb = 255;
        }

        public void Update()
        {
            if (location.Y > 240 && !isDismissed)
            {
                location.Y -= 1;
                opacity -= 2;
                rgb -= 2;
            }
            else if (location.Y == 240)
            {
                isDismissed = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, text, location, new Color(rgb, rgb, rgb, opacity));
        }
    }
}
