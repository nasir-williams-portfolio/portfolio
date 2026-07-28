using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace chess_playground
{
    internal class Pawn
    {
        private Texture2D spritesheet;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private Color color;

        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        private int spriteHeight;
        private int spriteWidth;

        public int X { get { return destinationRectangle.X; } set { destinationRectangle.X = value; } }
        public int Y { get { return destinationRectangle.Y; } set { destinationRectangle.Y = value; } }
        public Rectangle DestinationRectangle { get { return destinationRectangle; } }
        public Texture2D Sprite { get { return spritesheet; } }
        public Color Color { get { return color; } }

        public Pawn(Texture2D spritesheet, Color color)
        {
            this.spritesheet = spritesheet;
            this.color = color;
            spriteHeight = spritesheet.Height;
            spriteWidth = spritesheet.Width;

            destinationRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);

            currentKBState = Keyboard.GetState();
            previousKBState = currentKBState;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spritesheet, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
