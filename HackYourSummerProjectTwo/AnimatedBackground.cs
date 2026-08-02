using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    internal class AnimatedBackground
    {
        private Texture2D background;
        private Texture2D foreground;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;

        private double timeCounter, fps, secondsPerFrame;
        private int rgb, foregroundOpacity;

        private bool isFading;

        public int ForegroundOpacity { get { return foregroundOpacity; } }

        public AnimatedBackground(Texture2D background, Texture2D foreground)
        {
            this.background = background;
            this.foreground = foreground;
            sourceRectangle = new Rectangle(0, 0, 400, 240);
            destinationRectangle = new Rectangle(0, 0, 800, 480);
            isFading = false;

            timeCounter = 0.0;
            fps = 1.0;

            secondsPerFrame = 1.0 / fps;

            foregroundOpacity = 224;
            rgb = 224;
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

            if (Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                isFading = true;

            }

            if (isFading)
            {
                foregroundOpacity -= 2;
                rgb -= 2;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, destinationRectangle, Color.White);
            sb.Draw(foreground, destinationRectangle, sourceRectangle, new Color(rgb, rgb, rgb, foregroundOpacity));
        }
    }
}
