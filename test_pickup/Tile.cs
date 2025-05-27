using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace test_pickup
{
    internal class Tile
    {
        private Texture2D spritesheet;

        private Rectangle source_rectangle;
        private Vector2 position;
        private int rows;
        private int columns;

        public Tile(Texture2D spritesheet, Vector2 position, int row, int column)
        {
            this.spritesheet = spritesheet;
            source_rectangle = new Rectangle(16 * column, 16 * row, 16 * Game1.scale, 16 * Game1.scale);
            this.position = position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                position,
                source_rectangle,
                Color.White,
                0f,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0f);
        }
    }
}
