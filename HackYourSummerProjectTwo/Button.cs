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
        private Color color;

        public Button(Texture2D sprite, int x, int y, int width, int height, Color color)
        {
            this.sprite = sprite;
            this.color = color;
            destinationRectangle = new Rectangle(x, y, width, height);
            sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height / 2);
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
                    if (destinationRectangle.Contains(currMouse.Position) && OnButtonClick != null)
                    {
                        OnButtonClick();
                    }
                }

                if (destinationRectangle.Contains(clickLocation))
                {
                    sourceRectangle.Y = 14;
                }
                else
                {
                    sourceRectangle.Y = 0;
                }
            }
            else
            {
                sourceRectangle.Y = 0;
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, color);
        }
    }
}
