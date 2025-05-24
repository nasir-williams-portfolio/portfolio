using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace test_pickup
{
    internal class Tile
    {
        private Texture2D spritesheet;

        private Rectangle source_rectangle;
        private Vector2 position;

        public Tile(Texture2D spritesheet, Vector2 position, int row, int column)
        {
            this.spritesheet = spritesheet;
            source_rectangle = new Rectangle(16 * column, 16 * row, 16, 16);
            this.position = position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                position,
                source_rectangle,
                Color.White);
        }
    }
}
