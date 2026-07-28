using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuestsOverCoffee_Digitization
{
    public delegate void OnButtonClickDelegate();

    internal class Button
    {
        private string text;
        private int numberOfClicks;
        private bool isDoubleClick;

        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Rectangle cursor;
        private MouseState prevMouse;
        private MouseState currMouse;
        private Color color;

        public OnButtonClickDelegate OnButtonClick;
        private Texture2D sprite;
        private SpriteFont font;

        public bool IsDoubleClick { get { return isDoubleClick; } set { isDoubleClick = value; } }

        public Button(Texture2D sprite, Vector2 position, SpriteFont font, string text)
        {
            this.sprite = sprite;
            this.font = font;
            this.text = text;
            isDoubleClick = false;
            color = Color.White;
            numberOfClicks = 0;
            cursor = new Rectangle(0, 0, 1, 1);
            sourceRectangle = new Rectangle(0, 0, 1, 1);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 100, 20);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, color);
            sb.DrawString(font, text, new Vector2(destinationRectangle.X, (int)destinationRectangle.Y), Color.Black);
        }

        public void Update(GameTime gt)
        {
            currMouse = Mouse.GetState();
            cursor.X = currMouse.X;
            cursor.Y = currMouse.Y;

            if (destinationRectangle.Contains(cursor))
            {
                if (currMouse.LeftButton == ButtonState.Pressed)
                {
                    color = Color.Red;
                }

                if (currMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
                {
                    numberOfClicks++;
                    color = Color.White;
                    if (isDoubleClick == true && numberOfClicks == 2)
                    {
                        OnButtonClick();
                    }

                    else if (isDoubleClick == false)
                    {
                        OnButtonClick();
                    }
                }
            }
            else
            {
                color = Color.White;
            }

            prevMouse = currMouse;
        }
    }
}
