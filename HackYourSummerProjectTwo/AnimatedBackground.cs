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
        private Rectangle foregroundSourceRectangle, backgroundSourceRectangle;

        private double timeCounter, fps, secondsPerFrame;
        private int rgb, foregroundOpacity;

        private bool isFading;

        public int ForegroundOpacity { get { return foregroundOpacity; } }

        public AnimatedBackground(Texture2D background, Texture2D foreground)
        {
            this.background = background;
            this.foreground = foreground;
            foregroundSourceRectangle = new Rectangle(0, 0, 400, 240);
            backgroundSourceRectangle = new Rectangle(0, 0, 400, 240);
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
                foregroundSourceRectangle.Y += 240;
                backgroundSourceRectangle.X += 2;

                if (foregroundSourceRectangle.Y >= 480)
                {
                    foregroundSourceRectangle.Y = 0;
                }

                if (backgroundSourceRectangle.X >= 176)
                {
                    backgroundSourceRectangle.X = 0;
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
            sb.Draw(background, destinationRectangle, backgroundSourceRectangle, Color.White);
            sb.Draw(foreground, destinationRectangle, foregroundSourceRectangle, new Color(rgb, rgb, rgb, foregroundOpacity));
        }
    }
}
