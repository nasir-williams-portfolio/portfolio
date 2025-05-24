using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            source_rectangle = new Rectangle(133, 4, 7, 7);
            destination_rectangle = new Rectangle((int)position.X, (int)position.Y, 7, 7);
            bounds = new Rectangle((int)position.X - 2, (int)position.Y - 2, 11, 11);

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
                    new Vector2(destination_rectangle.X - 3, destination_rectangle.Y - 15),
                    new Rectangle(65, 34, 13, 12),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
