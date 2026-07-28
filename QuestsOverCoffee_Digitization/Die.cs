using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace QuestsOverCoffee_Digitization
{
    internal class Die
    {
        protected bool isExtra;
        protected bool isSelected;
        protected int clickCounter;
        protected int face;

        protected Rectangle destinationRectangle;
        protected Color color;
        protected MouseState currMouse;
        protected MouseState prevMouse;

        protected Random rng;
        protected Texture2D sprite;
        protected SpriteFont font;

        public int Face { get { return face; } }
        public bool IsExtra { get { return isExtra; } set { isExtra = value; } }
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
        public Vector2 Location { get { return new Vector2(destinationRectangle.X, destinationRectangle.Y); } }

        public Die(Texture2D sprite, Rectangle destinationRectangle, SpriteFont font, bool isExtra)
        {
            rng = new Random();
            this.sprite = sprite;
            this.destinationRectangle = destinationRectangle;
            this.font = font;
            this.isExtra = isExtra;
            isSelected = !isExtra;

            face = 0;

            clickCounter = 0;
            color = Color.White;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isSelected)
            {
                color = Color.White;
            }
            else
            {
                color = Color.Gray;
            }
            sb.Draw(sprite, destinationRectangle, color);
            sb.DrawString(font, face.ToString(), new Vector2(destinationRectangle.X + (font.MeasureString(face.ToString()).X / 2), (int)destinationRectangle.Y), Color.Black);
        }

        public void Update(GameTime gt)
        {
            currMouse = Mouse.GetState();

            if (face != 0)
            {
                if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                {
                    if (destinationRectangle.Contains(currMouse.Position.ToVector2()))
                    {
                        clickCounter++;
                        if (clickCounter % 2 != 0)
                        {
                            isSelected = true;
                        }
                        else
                        {
                            isSelected = false;
                        }
                    }
                }
            }

            prevMouse = currMouse;
        }

        public int Roll()
        {
            face = rng.Next(1, 7);
            return face;
        }

        public void Reset()
        {
            face = 0;
            isSelected = true;
        }
    }
}
