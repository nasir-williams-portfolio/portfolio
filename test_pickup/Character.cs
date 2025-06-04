using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    internal class Character
    {
        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;
        private Vector2 sprite_center;
        private Texture2D spritesheet;
        private SpriteEffects flip;

        private double timeCounter;
        private double fps;
        private double secondsPerFrame;
        private int speed;
        private int sprite_width;
        private int sprite_height;

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        public Rectangle Bounds { get { return destinationRectangle; } }

        public Character(Texture2D spritesheet, GraphicsDeviceManager graphics, int rows, int columns)
        {
            this.spritesheet = spritesheet;

            flip = SpriteEffects.None;
            speed = 1; // use a vector for this (reference the mario finite state machine practice exercise)
            timeCounter = 0.0;
            fps = 6.0;

            secondsPerFrame = 1.0 / fps;
            sprite_width = (spritesheet.Width / columns);
            sprite_height = (spritesheet.Height / rows);
            sprite_center = new Vector2(sprite_width / 2, sprite_height / 2);

            sourceRectangle = new Rectangle(
                0,
                0,
                sprite_width,
                sprite_height);

            destinationRectangle = new Rectangle(
                graphics.PreferredBackBufferWidth / 2 - (int)sprite_center.X,
                graphics.PreferredBackBufferHeight / 2 - (int)sprite_center.Y,
                sprite_width * Game1.scale,
                sprite_height * Game1.scale);

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
                speed = 2 * Game1.scale;
                fps = 12.0;
                secondsPerFrame = 1.0 / fps;
            }

            else
            {
                speed = 1 * Game1.scale;
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

        public void Resize()
        {
            destinationRectangle.Width = destinationRectangle.Width * Game1.scale;
            destinationRectangle.Height = destinationRectangle.Height * Game1.scale;
        }
    }
}