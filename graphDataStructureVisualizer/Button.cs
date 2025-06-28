using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace graphDataStructureVisualizer
{
    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    internal class Button
    {
        private Texture2D spritesheet;
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private bool isActive;
        private Color color;
        private Direction direction;
        private Vector2 spriteCenter;
        private float rotation;

        public float Rotation { get { return rotation; } set { rotation = value; } }

        public Button(Texture2D spritesheet, Vector2 position, Direction direction)
        {
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

            spriteCenter = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);

            this.spritesheet = spritesheet;
            this.direction = direction;

            isActive = true;
            rotation = (int)direction * (MathHelper.Pi / 4);
        }

        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                color = Color.White;
            }
            else
            {
                color = Color.Gray;
            }

            sb.Draw(spritesheet, destinationRectangle, sourceRectangle, color, rotation, spriteCenter, SpriteEffects.None, 0f);
        }

        public void Update()
        {

        }
    }
}
