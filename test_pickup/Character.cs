using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    internal class Character
    {
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Texture2D spritesheet;
        private SpriteEffects flip;

        private double timeCounter;
        private double fps;
        private double secondsPerFrame;
        private float scale;
        private int speed;

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        public Rectangle Bounds { get { return destinationRectangle; } }

        public Character(Texture2D spritesheet, GraphicsDeviceManager graphics)
        {
            this.spritesheet = spritesheet;

            flip = SpriteEffects.None;
            speed = 1;
            scale = 1;

            sourceRectangle = new Rectangle(
                0,
                0,
                12,
                14);

            destinationRectangle = new Rectangle(
                graphics.PreferredBackBufferWidth / 2 - sourceRectangle.Width / 2,
                graphics.PreferredBackBufferHeight / 2 - sourceRectangle.Height / 2,
                (12 * (int)scale),
                (14 * (int)scale));

            timeCounter = 0.0;
            fps = 6.0;
            secondsPerFrame = 1.0 / fps;

            currKbState = Keyboard.GetState();
            prevKbState = currKbState;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                new Vector2(destinationRectangle.X, destinationRectangle.Y),
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                flip,
                0f);
        }

        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= secondsPerFrame)
            {
                sourceRectangle.X += 12;

                if (sourceRectangle.X >= 48)
                {
                    sourceRectangle.X = 0;
                }

                timeCounter -= secondsPerFrame;
            }

            MovePlayer();
        }

        public void MovePlayer()
        {
            currKbState = Keyboard.GetState();

            if (currKbState.GetPressedKeyCount() > 1 && currKbState.IsKeyDown(Keys.LeftShift))
            {
                speed = 2;
                fps = 12.0;
                secondsPerFrame = 1.0 / fps;
            }

            else
            {
                speed = 1;
                fps = 6.0;
                secondsPerFrame = 1.0 / fps;
            }

            if (currKbState.IsKeyDown(Keys.D))
            {
                sourceRectangle.Y = 56;
                destinationRectangle.X += speed;
                flip = SpriteEffects.None;
            }

            else if (prevKbState.IsKeyDown(Keys.D))
            {
                sourceRectangle.Y = 14;
                flip = SpriteEffects.None;
            }

            else if (currKbState.IsKeyDown(Keys.A))
            {
                sourceRectangle.Y = 56;
                destinationRectangle.X -= speed;
                flip = SpriteEffects.FlipHorizontally;
            }

            else if (prevKbState.IsKeyDown(Keys.A))
            {
                sourceRectangle.Y = 14;
                flip = SpriteEffects.FlipHorizontally;
            }

            else if (currKbState.IsKeyDown(Keys.W))
            {
                sourceRectangle.Y = 70;
                destinationRectangle.Y -= speed;
            }

            else if (prevKbState.IsKeyDown(Keys.W))
            {
                sourceRectangle.Y = 28;
            }

            else if (currKbState.IsKeyDown(Keys.S))
            {
                sourceRectangle.Y = 42;
                destinationRectangle.Y += speed;
            }

            else if (prevKbState.IsKeyDown(Keys.S))
            {
                sourceRectangle.Y = 0;
            }

            prevKbState = currKbState;
        }
    }
}

// https://freesound.org/people/JustInvoke/sounds/446134/