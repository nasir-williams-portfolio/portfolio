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
        private MouseState prevMouse;
        private bool isShowing;

        public bool IsShowing { get { return isShowing; } set { isShowing = value; } }

        public Notepad(Texture2D sprite, int x, int y, int width, int height)
        {
            this.sprite = sprite;
            destinationRectangle = new Rectangle(x, y, width, height);
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

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isShowing)
            {
                sb.Draw(sprite, destinationRectangle, Color.White);
            }
        }
    }
}
