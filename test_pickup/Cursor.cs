using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    internal class Cursor
    {
        private Texture2D sprite;
        private Rectangle position;

        public Cursor(Texture2D sprite)
        {
            this.sprite = sprite;
            position = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, sprite.Width * Game1.scale, sprite.Height * Game1.scale);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                sprite,
                position,
                new Rectangle(0, 0, sprite.Width, sprite.Height),
                Color.White,
                0f,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                SpriteEffects.None, 0f);
        }

        public void Update()
        {
            position.X = Mouse.GetState().X;
            position.Y = Mouse.GetState().Y;
        }

        public void Resize()
        {
            position.Width = position.Width * Game1.scale;
            position.Height = position.Height * Game1.scale;
        }
    }
}
