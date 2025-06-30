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
        private Color color;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public Color Color { set { color = value; } }

        public Vertex(string name, string description, Texture2D spritesheet, Rectangle destinationRectangle)
        {
            this.spritesheet = spritesheet;
            this.description = description;
            this.name = name;

            this.destinationRectangle = destinationRectangle;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                destinationRectangle,
                color * 0.5f);
        }
    }
}
