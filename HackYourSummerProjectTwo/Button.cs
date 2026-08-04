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
        private Rectangle sourceRectangle;
        private MouseState currMouse;
        private MouseState prevMouse;
        private Vector2 clickLocation;
        private int width;

        public Button(Texture2D sprite, int x, int y, int width, int height, Rectangle sourceRectangle)
        {
            this.sprite = sprite;
            this.width = width;
            destinationRectangle = new Rectangle(x, y, width, height);
            this.sourceRectangle = sourceRectangle;
            clickLocation = new Vector2();
        }

        public void Update()
        {
            currMouse = Mouse.GetState();

            if (currMouse.LeftButton == ButtonState.Pressed)
            {
                if (prevMouse.LeftButton == ButtonState.Released)
                {
                    clickLocation = currMouse.Position.ToVector2();
                }

                if (destinationRectangle.Contains(clickLocation))
                {
                    sourceRectangle.X = 240;
                }
                else
                {
                    sourceRectangle.X = 0;
                }
            }
            else
            {
                sourceRectangle.X = 0;
            }

            if (currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed && destinationRectangle.Contains(currMouse.Position) && OnButtonClick != null)
            {
                OnButtonClick();
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
