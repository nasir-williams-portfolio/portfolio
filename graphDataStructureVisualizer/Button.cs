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
        public OnButtonClickDelegate OnButtonClick;
        private Direction direction;

        private Texture2D spritesheet;
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Rectangle mouseRectangle;

        private Color color;
        private MouseState currMouse;
        private MouseState prevMouse;

        private bool isActive;
        public static int HEIGHT = 18;
        public static int WIDTH = 18;
        private int x;
        private int y;

        public Direction Direction { get { return direction; } }
        public bool IsActive { set { isActive = value; } }
        public int Height { get { return HEIGHT; } }
        public int Width { get { return WIDTH; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }

        public Button(Texture2D spritesheet, Vector2 position, Direction direction)
        {
            x = (int)position.X;
            y = (int)position.Y;

            sourceRectangle = new Rectangle(
                0,
                HEIGHT * (int)direction,
                spritesheet.Width,
                spritesheet.Height / 8);

            destinationRectangle = new Rectangle(
                x,
                y,
                WIDTH,
                HEIGHT);

            this.spritesheet = spritesheet;

            isActive = true;
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

            sb.Draw(spritesheet, destinationRectangle, sourceRectangle, color);
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
