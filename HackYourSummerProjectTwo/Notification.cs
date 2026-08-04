using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class Notification
    {
        private Textbox text;
        private bool isDismissed;

        public bool IsDismissed { get { return isDismissed; } }

        public Notification(Textbox text)
        {
            this.text = text;
            isDismissed = false;
        }

        public void Update()
        {
            if (text.Y > 240 && !isDismissed)
            {
                text.Y -= 1;
                text.Opacity -= 2;
                text.RGB -= 2;
            }
            else if (text.Y == 240)
            {
                isDismissed = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            text.Draw(sb);
        }
    }
}
