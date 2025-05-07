using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_movement
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

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        public Character(Texture2D spritesheet)
        {
            this.spritesheet = spritesheet;
            sourceRectangle = new Rectangle(0, 0, 32, 32);
            destinationRectangle = new Rectangle(384, 234, 32, 32);
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
                destinationRectangle,
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                flip,
                0f);
        }

        public void Update(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            int pixelsInRow = 64;

            if (timeCounter >= secondsPerFrame)
            {
                sourceRectangle.X += 32;
                if (sourceRectangle.X >= pixelsInRow)
                {
                    sourceRectangle.X = 0;
                }

                timeCounter -= secondsPerFrame;
            }

            currKbState = Keyboard.GetState();

            if (currKbState.IsKeyDown(Keys.D))
            {
                pixelsInRow = 128;
                sourceRectangle.Y = 128;
                destinationRectangle.X += 1;
                flip = SpriteEffects.None;
            }

            else if (prevKbState.IsKeyDown(Keys.D))
            {
                pixelsInRow = 64;
                sourceRectangle.Y = 32;
                flip = SpriteEffects.None;
            }

            else if (currKbState.IsKeyDown(Keys.A))
            {
                pixelsInRow = 128;
                sourceRectangle.Y = 128;
                destinationRectangle.X -= 1;
                flip = SpriteEffects.FlipHorizontally;
            }

            else if (prevKbState.IsKeyDown(Keys.A))
            {
                pixelsInRow = 64;
                sourceRectangle.Y = 32;
                flip = SpriteEffects.FlipHorizontally;
            }

            else if (currKbState.IsKeyDown(Keys.W))
            {
                pixelsInRow = 128;
                sourceRectangle.Y = 160;
                destinationRectangle.Y -= 1;
            }

            else if (prevKbState.IsKeyDown(Keys.W))
            {
                pixelsInRow = 64;
                sourceRectangle.Y = 64;
            }

            else if (currKbState.IsKeyDown(Keys.S))
            {
                pixelsInRow = 128;
                sourceRectangle.Y = 96;
                destinationRectangle.Y += 1;
            }

            else if (prevKbState.IsKeyDown(Keys.S))
            {
                pixelsInRow = 64;
                sourceRectangle.Y = 0;
            }

            prevKbState = currKbState;
        }
    }
}
