using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Colby_A1_Creator_Jam
{
    internal class Collideable
    {
        private Texture2D spritesheet;
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;

        private Random rng;

        private int spriteHeight;
        private int spriteWidth;
        private float rotation;

        public Rectangle DestinationRectangle { get { return destinationRectangle; } }

        public Collideable(Texture2D spritesheet)
        {
            this.spritesheet = spritesheet;
            spriteHeight = spritesheet.Height;
            spriteWidth = spritesheet.Width;

            rng = new Random();
            rotation = 0f;

            destinationRectangle = new Rectangle(
                800,
                rng.Next(0, 14) * 30,
                30,
                30);

            sourceRectangle = new Rectangle(
                0,
                0,
                spriteWidth,
                spriteHeight);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                destinationRectangle,
                sourceRectangle,
                Color.White,
                rotation,
                new Vector2(90, 90),
                SpriteEffects.None, 0f);
        }

        public void Update()
        {
            if (destinationRectangle.X + 30 <= 0)
            {
                destinationRectangle.X = 800;
                destinationRectangle.Y = rng.Next(0, 480 - spriteHeight);
            }
            else
            {
                rotation += 0.1f;
                destinationRectangle.X -= 2;
            }
        }
    }
}
