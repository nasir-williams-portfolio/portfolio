using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace test_pickup
{
    internal class Toggle : Button
    {
        public Toggle(Texture2D spritesheet, Vector2 position, ButtonStates function, int rows, int columns) : base(spritesheet, position, function, rows, columns)
        {

        }
    }
}
