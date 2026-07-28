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
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private bool isShowingProperties;
        private Color propertiesColor;
        private Random rng;

        public Application(Texture2D sprite)
        {
            this.sprite = sprite;
            destinationRectangle = new Rectangle(388, 228, 25, 25);
            rng = new Random();
            propertiesColor = new Color(rng.Next(0, 2) * 225, 0, 0);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, Color.White);
            if (isShowingProperties)
            {
                sb.Draw(sprite, new Rectangle(400, 228, 25, 100), propertiesColor);
            }
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            Rectangle cursorPosition = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            if (
                currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released &&
                destinationRectangle.Contains(cursorPosition))
            {
                isShowingProperties = true;
            }

            previousMouseState = currentMouseState;
        }
    }
}
