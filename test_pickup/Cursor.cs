using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    internal class Cursor
    {
        private Texture2D sprite;
        private Vector2 position;

        public Cursor(Texture2D sprite)
        {
            this.sprite = sprite;
            position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
        }

        public void Update()
        {
            position.X = Mouse.GetState().X;
            position.Y = Mouse.GetState().Y;
        }
    }
}
