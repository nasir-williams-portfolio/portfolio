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
        private Vector2 position;

        private bool isColliding;
        private bool isCollected;

        public bool Colliding { get { return isColliding; } set { isColliding = value; } }
        public bool Collected { get { return isCollected; } set { isCollected = value; } }
        public Rectangle Bounds { get { return bounds; } }
        public int X { get { return destination_rectangle.X; } set { destination_rectangle.X = value; } }
        public int Y { get { return destination_rectangle.Y; } set { destination_rectangle.Y = value; } }

        public int Height { get { return source_rectangle.Height; } }
        public int Width { get { return source_rectangle.Width; } }

        public Pickup(Texture2D spritesheet, Texture2D key_spritesheet, Vector2 position)
        {
            this.spritesheet = spritesheet;
            this.key_spritesheet = key_spritesheet;

            Random rng = new Random();

            // this will change once I fix the spritesheet
            Rectangle[] rectangles = {
                new Rectangle(213, 4, 7, 7),
                new Rectangle(163, 195, 10, 10),
                new Rectangle(293, 4, 7, 7),
                new Rectangle(294, 35, 9, 9),
                new Rectangle(164, 4, 8, 8)};

            this.position = position;

            source_rectangle = rectangles[rng.Next(0, rectangles.Length)];

            destination_rectangle = new Rectangle(
                (int)position.X,
                (int)position.Y,
                source_rectangle.Width * Game1.scale,
                source_rectangle.Height * Game1.scale);

            bounds = new Rectangle(
                (int)position.X - 2,
                (int)position.Y - 2,
                destination_rectangle.Width,
                destination_rectangle.Height);
        }

        public void Draw(SpriteBatch sb, int x, int y)
        {
            bounds.X = (int)position.X - x;
            bounds.Y = (int)position.Y - y;

            sb.Draw(
            spritesheet,
            new Rectangle(
                (int)position.X - x,
                (int)position.Y - y,
                destination_rectangle.Width,
                destination_rectangle.Height),
            source_rectangle,
            Color.White,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            0f);

            if (Colliding)
            {
                sb.Draw(
                    key_spritesheet,
                    // if you change the hardcoded values (which you should eventually) this will blow up
                    new Rectangle(
                        bounds.X + (bounds.Width / 2),
                        bounds.Y - 7 * Game1.scale,
                        13 * Game1.scale,
                        12 * Game1.scale),
                    new Rectangle(65, 34, 13, 12),
                    Color.White,
                    0f,
                    new Vector2(7, 6),
                    SpriteEffects.None,
                    0f);
            }

        }

        public void Resize()
        {
            // can't fix this until you make a new, uniform spritesheet
            destination_rectangle.Width = source_rectangle.Width * Game1.scale;
            destination_rectangle.Height = source_rectangle.Height * Game1.scale;

            bounds = new Rectangle(
                (int)destination_rectangle.X - 2,
                (int)destination_rectangle.Y - 2,
                destination_rectangle.Width + 4,
                destination_rectangle.Height + 4);
        }
    }
}
