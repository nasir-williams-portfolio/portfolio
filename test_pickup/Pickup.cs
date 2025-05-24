using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace test_pickup
{
    internal class Pickup
    {
        private Texture2D spritesheet;
        private Texture2D key_spritesheet;

        private Rectangle source_rectangle;
        private Rectangle destination_rectangle;
        private Rectangle bounds;

        private float scale;
        private bool isColliding;

        public bool Colliding { get { return isColliding; } set { isColliding = value; } }
        public float Scale { set { scale = value; } }
        public Rectangle Bounds { get { return bounds; } }

        public Pickup(Texture2D spritesheet, Texture2D key_spritesheet, Vector2 position)
        {
            this.spritesheet = spritesheet;
            this.key_spritesheet = key_spritesheet;

            Random rng = new Random();

            Rectangle[] rectangles = {
                new Rectangle(213, 4, 7, 7),
                new Rectangle(163, 195, 10, 10),
                new Rectangle(293, 4, 7, 7),
                new Rectangle(294, 35, 9, 9),
                new Rectangle(164, 4, 8, 8)};

            source_rectangle = rectangles[rng.Next(0, rectangles.Length)];
            destination_rectangle = new Rectangle((int)position.X, (int)position.Y, source_rectangle.Width, source_rectangle.Height);
            bounds = new Rectangle((int)position.X - 2, (int)position.Y - 2, destination_rectangle.Width + 4, destination_rectangle.Height + 4);

            scale = 1;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                new Vector2(destination_rectangle.X, destination_rectangle.Y),
                source_rectangle,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f);

            if (Colliding)
            {
                sb.Draw(
                    key_spritesheet,
                    new Vector2(destination_rectangle.X + source_rectangle.Width / 2, destination_rectangle.Y - 10),
                    new Rectangle(65, 34, 13, 12),
                    Color.White,
                    0f,
                    new Vector2(6, 6),
                    scale,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
