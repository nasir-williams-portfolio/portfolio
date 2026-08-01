using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    internal class Notepad
    {
        private Texture2D sprite;
        private SpriteFont font;
        private Rectangle destinationRectangle;
        private MouseState currMouse;
        private MouseState prevMouse;
        private Vector2 clickLocation;
        private Vector2 difference;
        private Vector2 previousLocation;
        private bool isShowing;
        private string applicationText;
        private bool clickedInRectangle;

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

            if (currMouse.LeftButton == ButtonState.Pressed)
            {
                if (prevMouse.LeftButton == ButtonState.Released)
                {
                    if (destinationRectangle.Contains(currMouse.Position))
                    {
                        clickLocation = new Vector2(currMouse.Position.X, currMouse.Position.Y);
                        previousLocation = new Vector2(destinationRectangle.X, destinationRectangle.Y);
                        clickedInRectangle = true;
                    }
                    else
                    {
                        clickedInRectangle = false;
                    }
                }
                else if (clickedInRectangle == true)
                {
                    difference = new Vector2(currMouse.X - clickLocation.X, currMouse.Y - clickLocation.Y);
                    destinationRectangle.X = (int)(difference.X + previousLocation.X);
                    destinationRectangle.Y = (int)(difference.Y + previousLocation.Y);
                }
            }

            prevMouse = currMouse;
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
