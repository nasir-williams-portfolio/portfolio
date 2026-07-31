using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    internal class Notepad
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private MouseState currMouse;
        private SpriteFont font;
        private bool isShowing;
        private string applicationText;

        public bool IsShowing { get { return isShowing; } set { isShowing = value; } }

        public Notepad(Texture2D sprite, int x, int y, int width, int height, SpriteFont font)
        {
            this.sprite = sprite;
            destinationRectangle = new Rectangle(x, y, width, height);
            this.font = font;
            applicationText = "Identify malicious applications through:\n - Poor icon resolution (White)\n - Suspicious property information(475GB)\n - Inconsistent tooltip information";
        }

        public void Update()
        {
            currMouse = Mouse.GetState();
            Rectangle cursor = new Rectangle(currMouse.X, currMouse.Y, 1, 1);

            if (currMouse.LeftButton == ButtonState.Pressed && destinationRectangle.Contains(cursor))
            {
                destinationRectangle.X = cursor.X - 50;
                destinationRectangle.Y = cursor.Y - 50;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (isShowing)
            {
                sb.Draw(sprite, destinationRectangle, Color.White);
                sb.DrawString(font, "Malicious Application Identification", new Vector2(destinationRectangle.X + 70, destinationRectangle.Y), Color.Black);
                sb.DrawString(font, applicationText, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y + 20), Color.Black);
            }
        }
    }
}
