using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HackYourSummerProjectTwo
{
    internal class Application
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private Rectangle propertiesRectangle;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private bool isShowingProperties;
        private Color propertiesColor;
        private Random rng;

        public Application(Texture2D sprite)
        {
            this.sprite = sprite;
            destinationRectangle = new Rectangle(388, 228, 25, 25);
            propertiesRectangle = new Rectangle(450, 228, 100, 200);
            rng = new Random();

            propertiesColor = (rng.Next(0, 2) == 1) ? Color.Red : Color.Blue;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, Color.White);
            if (isShowingProperties)
            {
                sb.Draw(sprite, propertiesRectangle, propertiesColor);
            }
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            Rectangle cursorPosition = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                if (destinationRectangle.Contains(cursorPosition))
                {
                    isShowingProperties = !isShowingProperties;
                }
            }

            previousMouseState = currentMouseState;
        }
    }
}
