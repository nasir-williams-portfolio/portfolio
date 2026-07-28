using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    public delegate void OnButtonClickDelegate();
    internal class Button
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        public OnButtonClickDelegate OnButtonClick;
        private Color color;

        public Button(Texture2D sprite, Vector2 location, Color color, int width, int height)
        {
            this.sprite = sprite;
            this.color = color;
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, color);
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            Rectangle cursor = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (destinationRectangle.Contains(cursor) && OnButtonClick != null)
                {
                    OnButtonClick();
                }
            }

            previousMouseState = currentMouseState;
        }
    }
}
