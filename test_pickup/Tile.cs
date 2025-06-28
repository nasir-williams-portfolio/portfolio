using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace test_pickup
{
    internal class Tile
    {
        private Texture2D spritesheet;

        private Rectangle source_rectangle;

        private int width;
        private int height;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Tile(Texture2D spritesheet, int row, int column)
        {
            this.spritesheet = spritesheet;
            source_rectangle = new Rectangle(16 * column, 16 * row, 16, 16);
            width = source_rectangle.Width * Game1.scale;
            height = source_rectangle.Height * Game1.scale;
        }

        public void Draw(SpriteBatch sb, int x, int y)
        {
            sb.Draw(
                spritesheet,
                new Rectangle(
                    x,
                    y,
                    width,
                    height),
                source_rectangle,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f);
        }

        public void Resize()
        {
            // again, this is gonna look janky until you fix the spritesheet
            width = source_rectangle.Width * Game1.scale;
            height = source_rectangle.Width * Game1.scale;
        }
    }
}
