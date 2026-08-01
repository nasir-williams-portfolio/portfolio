using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    public delegate void OnButtonClickDelegate();
    internal class Button
    {
        public OnButtonClickDelegate OnButtonClick;
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private MouseState currMouse;
        private MouseState prevMouse;
        private Color color;

        public Button(Texture2D sprite, int x, int y, int width, int height, Color color)
        {
            this.sprite = sprite;
            this.color = color;
            destinationRectangle = new Rectangle(x, y, width, height);
        }

        public void Update()
        {
            currMouse = Mouse.GetState();

            if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                if (destinationRectangle.Contains(currMouse.Position) && OnButtonClick != null)
                {
                    OnButtonClick();
                }
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, color);
        }
    }
}
