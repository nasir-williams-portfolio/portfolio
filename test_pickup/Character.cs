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

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        public Rectangle Bounds { get { return destinationRectangle; } }

        public Character(Texture2D spritesheet)
        {
            scale = 1;
            this.spritesheet = spritesheet;
            sourceRectangle = new Rectangle(0, 0, 12, 14);
            destinationRectangle = new Rectangle(388, 236, (12 * (int)scale), (14 * (int)scale));
            flip = SpriteEffects.None;

            timeCounter = 0.0;
            fps = 4.0;
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

            if (currKbState.IsKeyDown(Keys.D))
            {
                sourceRectangle.Y = 56;
                destinationRectangle.X += 1;
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
                destinationRectangle.X -= 1;
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
                destinationRectangle.Y -= 1;
            }

            else if (prevKbState.IsKeyDown(Keys.W))
            {
                sourceRectangle.Y = 28;
            }

            else if (currKbState.IsKeyDown(Keys.S))
            {
                sourceRectangle.Y = 42;
                destinationRectangle.Y += 1;
            }

            else if (prevKbState.IsKeyDown(Keys.S))
            {
                sourceRectangle.Y = 0;
            }

            prevKbState = currKbState;
        }
    }
}
