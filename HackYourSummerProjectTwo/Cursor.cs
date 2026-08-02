using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    internal class Cursor
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;

        public Cursor(Texture2D sprite)
        {
            this.sprite = sprite;
            destinationRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public void Update()
        {
            destinationRectangle.X = Mouse.GetState().X;
            destinationRectangle.Y = Mouse.GetState().Y;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, Color.White);
        }
    }
}
