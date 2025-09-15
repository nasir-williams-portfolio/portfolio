using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeopardy_POLISHED
{
    public delegate void SingleButtonPressDelegate();

    internal class Button
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private Rectangle clickPosition;

        public SingleButtonPressDelegate onSingleButtonPress;

        public Button(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                sprite.Width,
                sprite.Height / 2);
            this.sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height / 2);
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                sprite,                // the Texture2D object being used for display
                destinationRectangle,  // the way the Texture2D object will look on the screen
                sourceRectangle,       // the way the Texture2D object will look in the image file
                Color.White,           // the color overlay for the object
                0f,                    // the rotation of the object
                Vector2.Zero,          // the origin point of the Texture2D object (as per the image file)
                SpriteEffects.None,    // the sprite effects on the object (whether its flipped horizontally, vertically, etc)
                0f                     // the layer depth of the displayed object
                );
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (previousMouseState.LeftButton == ButtonState.Released)
                {
                    clickPosition = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

                    if (destinationRectangle.Contains(clickPosition) && onSingleButtonPress != null)
                    {
                        sourceRectangle.Y = sprite.Height / 2;
                        onSingleButtonPress();
                    }
                }

            }
            else
            {
                sourceRectangle.Y = 0;
            }

            previousMouseState = currentMouseState;
        }
    }
}
