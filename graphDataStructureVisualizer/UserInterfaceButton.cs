using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace graphDataStructureVisualizer
{
    public delegate void OnButtonClickDelegate();

    public enum ButtonUse
    {
        Play,
        Quit
    }

    internal class UserInterfaceButton
    {
        private Texture2D spritesheet;
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Rectangle mouseRectangle;
        private Rectangle clickPosition;
        private MouseState currMouse;
        private MouseState prevMouse;

        public OnButtonClickDelegate OnButtonClick;

        private bool isDepressed;

        public UserInterfaceButton(Texture2D spritesheet, Vector2 position, ButtonUse use)
        {
            this.spritesheet = spritesheet;
            sourceRectangle = new Rectangle(
                0,
                15 * (int)use,
                46,
                15);
            destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                46 * 2,
                15 * 2);
            isDepressed = false;
            mouseRectangle = new Rectangle(0, 0, 1, 1);
            clickPosition = new Rectangle(0, 0, 1, 1);
        }

        public void Draw(SpriteBatch sb)
        {
            if (isDepressed)
            {
                sourceRectangle.X = 46;
            }
            else
            {
                sourceRectangle.X = 0;
            }

            sb.Draw(spritesheet, destinationRectangle, sourceRectangle, Color.White);
        }

        public void Update()
        {
            currMouse = Mouse.GetState();
            mouseRectangle.X = currMouse.X;
            mouseRectangle.Y = currMouse.Y;

            if (currMouse.LeftButton == ButtonState.Pressed)
            {
                if (prevMouse.LeftButton == ButtonState.Released)
                {
                    clickPosition.X = currMouse.X;
                    clickPosition.Y = currMouse.Y;
                }

                if (destinationRectangle.Contains(mouseRectangle))
                {
                    isDepressed = true;
                }
            }

            else
            {
                isDepressed = false;
            }

            if (prevMouse.LeftButton == ButtonState.Pressed && currMouse.LeftButton == ButtonState.Released && destinationRectangle.Contains(clickPosition) && OnButtonClick != null)
            {
                OnButtonClick();
            }

            prevMouse = currMouse;
        }
    }
}
