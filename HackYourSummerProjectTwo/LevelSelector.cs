using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{


    internal class LevelSelector
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private bool isLocked;
        private bool beenClicked;
        private bool isCompleted;

        public bool BeenClicked { get { return beenClicked; } set { beenClicked = value; } }
        public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
        public bool IsCompleted { get { return isCompleted; } set { isCompleted = value; } }

        public LevelSelector(Texture2D sprite, Vector2 location, bool isLocked)
        {
            this.sprite = sprite;
            this.isLocked = isLocked;
            beenClicked = false;
            isCompleted = false;
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 60, 60);
            sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height / 2);

        }

        public void Update()
        {
            if (destinationRectangle.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed && !isLocked)
            {
                beenClicked = true;
            }

            sourceRectangle.Y = (isLocked) ? sprite.Height / 2 : 0;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
