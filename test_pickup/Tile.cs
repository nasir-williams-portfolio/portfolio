using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace test_pickup
{
    internal class Tile
    {
        private Texture2D spritesheet;

        private Rectangle source_rectangle;
        private Rectangle destination_rectangle;

        public Tile(Texture2D spritesheet, Vector2 position, int row, int column)
        {
            this.spritesheet = spritesheet;
            source_rectangle = new Rectangle(16 * column, 16 * row, 16, 16);
            destination_rectangle = new Rectangle((int)position.X, (int)position.Y, source_rectangle.Width * Game1.scale, source_rectangle.Height * Game1.scale);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                destination_rectangle,
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
            destination_rectangle.Width = 16 * Game1.scale;
            destination_rectangle.Height = 16 * Game1.scale;
        }
    }
}
