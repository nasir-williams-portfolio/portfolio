using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{


    internal class LevelSelector
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private bool isLocked;
        private bool beenClicked;

        public bool BeenClicked { get { return beenClicked; } set { beenClicked = value; } }

        public LevelSelector(Texture2D sprite, Vector2 location, bool isLocked)
        {
            this.sprite = sprite;
            this.isLocked = isLocked;
            beenClicked = false;
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 30, 30);
        }

        public void Update()
        {
            if (destinationRectangle.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed && !isLocked)
            {
                beenClicked = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, (isLocked) ? Color.Gray : Color.Blue);
        }
    }
}
