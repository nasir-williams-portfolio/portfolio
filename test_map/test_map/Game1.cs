using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_map
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D pixel;

        private Rectangle[] squareSides;
        private Rectangle player;

        private Rectangle baseSquare;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            System.Diagnostics.Debug.WriteLine(
                $"Window Width: {_graphics.PreferredBackBufferWidth}" +
                $"\nWindow Height: {_graphics.PreferredBackBufferHeight}");

            squareSides = new Rectangle[4];

            squareSides[0] = new Rectangle(350, 240, 100, 1); // top
            squareSides[1] = new Rectangle(350, 240, 1, 100); // left
            squareSides[2] = new Rectangle(450, 240, 1, 100); // right
            squareSides[3] = new Rectangle(350, 340, 100, 1); // bottom

            baseSquare = new Rectangle(350, 240, 100, 100);

            player = new Rectangle(360, 250, 5, 5);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Content.Load<Texture2D>("single_pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (player.Top >= baseSquare.Top)
                {
                    player.Y -= 1;
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (player.Left >= baseSquare.Left)
                {
                    player.X -= 1;
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (player.Bottom <= baseSquare.Bottom)
                {
                    player.Y += 1;
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (player.Right <= baseSquare.Right)
                {
                    player.X += 1;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            //foreach (Rectangle side in squareSides)
            //{
            //    _spriteBatch.Draw(pixel, side, Color.Black);
            //}

            _spriteBatch.Draw(pixel, baseSquare, Color.White);

            _spriteBatch.Draw(pixel, player, Color.Red);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
