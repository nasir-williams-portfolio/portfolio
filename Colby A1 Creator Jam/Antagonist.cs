using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colby_A1_Creator_Jam
{
    internal class Antagonist
    {
        private Texture2D spritesheet;
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;

        public Antagonist(Texture2D spritesheet)
        {
            this.spritesheet = spritesheet;
            sourceRectangle = new Rectangle(
                0,
                0,
                23,
                14);

            destinationRectangle = new Rectangle(
                0,
                0,
                23,
                14);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spritesheet, destinationRectangle, sourceRectangle, Color.White);
        }

        public void Update()
        {

        }
    }
}
