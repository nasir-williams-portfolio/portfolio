using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace graphDataStructureVisualizer
{
    public delegate void OnButtonClickDelegate(Direction movementDirection);
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
        private Vector2 spriteCenter;
        private float rotation;
        private Direction direction;
        public OnButtonClickDelegate OnButtonClick;

        private MouseState currMouse;
        private MouseState prevMouse;

        private Rectangle mouseRectangle;


        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Direction Direction { get { return direction; } }

        public bool IsActive { set { isActive = value; } }

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

            isActive = true;
            rotation = (int)direction * (MathHelper.Pi / 4);
            this.direction = direction;
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

            DebugLib.DrawRectOutline(sb, destinationRectangle, 1f, Color.Red);
        }

        public void Update()
        {
            currMouse = Mouse.GetState();
            mouseRectangle = new Rectangle(currMouse.X, currMouse.Y, 1, 1);

            if (destinationRectangle.Contains(mouseRectangle) && currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (OnButtonClick != null && isActive)
                {
                    OnButtonClick(direction);
                }
            }

            prevMouse = currMouse;
        }
    }
}
