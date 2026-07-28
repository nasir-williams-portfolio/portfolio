using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NYT_Tiles_Dupe.Content
{
    public enum Pattern
    {
        red,
        orange,
        yellow,
        green,
        blue,
        purple
    }

    internal class Tile
    {
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Texture2D sprite;
        private Pattern tilePattern;

        public Pattern TilePattern { get { return tilePattern; } }

        public Tile(int num, Rectangle destinationRectangle, Texture2D sprite)
        {
            sourceRectangle = new Rectangle(0, (32 * num), 32, 32);
            this.destinationRectangle = destinationRectangle;
            this.sprite = sprite;
            tilePattern = (Pattern)num;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
