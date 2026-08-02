using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class AnimatedBackground
    {
        private Texture2D sprite;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;

        private double timeCounter, fps, secondsPerFrame;

        public AnimatedBackground(Texture2D sprite)
        {
            this.sprite = sprite;
            sourceRectangle = new Rectangle(0, 0, 400, 240);
            destinationRectangle = new Rectangle(0, 0, 800, 480);

            timeCounter = 0.0;
            fps = 1.0;

            secondsPerFrame = 1.0 / fps;
        }

        public void Update(GameTime gt)
        {
            timeCounter += gt.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= secondsPerFrame)
            {
                sourceRectangle.Y += 240;

                if (sourceRectangle.Y >= 480)
                {
                    sourceRectangle.Y = 0;
                }

                timeCounter -= secondsPerFrame;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
