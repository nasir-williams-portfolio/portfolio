using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace HackYourSummerProjectTwo
{
    internal class Application
    {
        private Texture2D sprite;
        private Rectangle applicationRectangle;
        private Rectangle propertiesRectangle;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private bool isShowingProperties;
        private Color propertiesColor;
        private Random rng;
        private Color applicationColor;

        public Application(Texture2D sprite)
        {
            this.sprite = sprite;
            rng = new Random();
            applicationRectangle = new Rectangle(363, 203, 75, 75);
            propertiesRectangle = new Rectangle(450, 228, 100, 200);
            applicationColor = new Color(rng.Next(0, 226), rng.Next(0, 226), rng.Next(0, 226));

            propertiesColor = (rng.Next(0, 2) == 1) ? Color.Red : Color.Blue;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, applicationRectangle, applicationColor);
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
                if (applicationRectangle.Contains(cursorPosition))
                {
                    isShowingProperties = !isShowingProperties;
                }
            }

            previousMouseState = currentMouseState;
        }
    }
}
