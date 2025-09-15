using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jeopardy
{
    public delegate void OnButtonClickDelegate(string text, int price, string answer);
    internal class Button
    {
        private SpriteFont font;
        private Texture2D sprite;
        private Rectangle positionRectangle;

        private Rectangle cursor;
        private MouseState currMouseState;
        private MouseState prevMouseState;

        private string question;
        private int price;
        private bool beenClicked;
        private string answer;

        public OnButtonClickDelegate OnButtonClick;

        public Rectangle Position { get { return positionRectangle; } }

        public Button(int price, SpriteFont font, Texture2D sprite, Vector2 position, string question, string answer)
        {
            this.price = price;
            this.font = font;
            this.sprite = sprite;
            this.question = question;
            this.answer = answer;

            positionRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                100,
                50);

            prevMouseState = currMouseState;
            beenClicked = false;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, positionRectangle, Color.White);

            if (beenClicked == false)
            {
                sb.DrawString(
                    font,                                                                                                 // spriteFont
                    "$" + price,                                                                                          // text
                    new Vector2(positionRectangle.Center.X, positionRectangle.Center.Y),                                  // position
                    Color.White,                                                                                          // color
                    0f,                                                                                                   // rotation
                    new Vector2(font.MeasureString("$" + price.ToString()).X / 2, font.MeasureString("$" + price.ToString()).Y / 2),  // origin
                    1f,                                                                                                   // scale
                    SpriteEffects.None,                                                                                   // effects
                    0f);                                                                                                  // layerDepth
            }
        }

        public void Update()
        {
            currMouseState = Mouse.GetState();
            cursor = new Rectangle(currMouseState.X, currMouseState.Y, 1, 1);

            if (beenClicked == false)
            {
                //if (positionRectangle.Contains(cursor) && currMouseState.LeftButton == ButtonState.Released && currMouseState.RightButton == ButtonState.Released)
                //{
                //    buttonColor.A = 112;
                //    textColor.A = 112;
                //}

                //else
                //{
                //    buttonColor.A = 225;
                //    textColor.A = 225;
                //}

                if (positionRectangle.Contains(cursor) && currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released && OnButtonClick != null)
                {
                    OnButtonClick(question, price, answer);
                    beenClicked = true;
                }
            }

            prevMouseState = currMouseState;
        }
    }
}
