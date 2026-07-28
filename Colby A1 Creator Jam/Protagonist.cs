using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Colby_A1_Creator_Jam
{
    internal class Protagonist
    {
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Texture2D spritesheet;

        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;

        private double timeCounter;
        private double fps;
        private double secondsPerFrame;

        private bool isDead;

        public bool IsDead { get { return isDead; } set { isDead = value; } }
        public Rectangle DestinationRectangle { get { return destinationRectangle; } }

        public Protagonist(Texture2D spritesheet)
        {
            this.spritesheet = spritesheet;
            destinationRectangle = new Rectangle(400, 240, 128, 64);
            sourceRectangle = new Rectangle(0, 0, 64, 32);

            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = currKeyboardState;

            timeCounter = 0.0;
            fps = 6.0;

            secondsPerFrame = 1.0 / fps;

            isDead = false;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isDead == false)
            {
                sb.Draw(spritesheet, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            currKeyboardState = Keyboard.GetState();

            if (isDead == false)
            {
                // down
                if (currKeyboardState.IsKeyDown(Keys.S))
                {
                    destinationRectangle.Y += 5;
                    sourceRectangle.Y = 0;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeCounter >= secondsPerFrame)
                    {
                        sourceRectangle.X += 64;

                        if (sourceRectangle.X >= 256)
                        {
                            sourceRectangle.X = 0;
                        }

                        timeCounter -= secondsPerFrame;
                    }
                }

                // up
                else if (currKeyboardState.IsKeyDown(Keys.W))
                {
                    destinationRectangle.Y -= 5;
                    sourceRectangle.Y = 96;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeCounter >= secondsPerFrame)
                    {
                        sourceRectangle.X += 64;

                        if (sourceRectangle.X >= 256)
                        {
                            sourceRectangle.X = 0;
                        }

                        timeCounter -= secondsPerFrame;
                    }
                }

                // left
                else if (currKeyboardState.IsKeyDown(Keys.A))
                {
                    destinationRectangle.X -= 5;
                    sourceRectangle.Y = 32;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeCounter >= secondsPerFrame)
                    {
                        sourceRectangle.X += 64;

                        if (sourceRectangle.X >= 256)
                        {
                            sourceRectangle.X = 0;
                        }

                        timeCounter -= secondsPerFrame;
                    }
                }

                // right
                else if (currKeyboardState.IsKeyDown(Keys.D))
                {
                    destinationRectangle.X += 5;
                    sourceRectangle.Y = 64;
                    timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeCounter >= secondsPerFrame)
                    {
                        sourceRectangle.X += 64;

                        if (sourceRectangle.X >= 256)
                        {
                            sourceRectangle.X = 0;
                        }

                        timeCounter -= secondsPerFrame;
                    }
                }
            }

            prevKeyboardState = Keyboard.GetState();
        }
    }
}
