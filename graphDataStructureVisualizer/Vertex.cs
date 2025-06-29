using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphDataStructureVisualizer
{
    internal class Vertex
    {
        private string name;
        private string description;
        private Texture2D spritesheet;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private Color color;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public Color Color { set { color = value; } }

        public Vertex(string name, string description, Texture2D spritesheet, Vector2 position)
        {
            this.spritesheet = spritesheet;
            this.description = description;

            sourceRectangle = new Rectangle(
                0,
                0,
                spritesheet.Width,
                spritesheet.Height);
            destinationRectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                sourceRectangle.Width,
                sourceRectangle.Height);
            color = Color.White;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                destinationRectangle,
                sourceRectangle,
                color);
        }
    }
}
