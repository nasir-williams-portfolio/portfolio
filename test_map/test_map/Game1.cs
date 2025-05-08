using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace test_map
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D pixel;
        private SpriteFont font;

        private Rectangle player;

        private Rectangle firstRect;
        private Rectangle secondRect;

        private List<Rectangle> boundaries;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Rectangle(395, 235, 5, 5);

            firstRect = new Rectangle(350, 190, 100, 100);
            secondRect = new Rectangle(450, 215, 50, 50);

            boundaries = new List<Rectangle>();
            boundaries.Add(new Rectangle(350, 165, 100, 25));
            boundaries.Add(new Rectangle(325, 190, 25, 100));
            boundaries.Add(new Rectangle(350, 290, 100, 25));
            boundaries.Add(new Rectangle(450, 265, 50, 25));
            boundaries.Add(new Rectangle(500, 215, 25, 50));
            boundaries.Add(new Rectangle(450, 190, 50, 25));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = Content.Load<Texture2D>("single_pixel");
            font = Content.Load<SpriteFont>("arial12");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Collide();
            Move();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(pixel, firstRect, Color.Red);
            _spriteBatch.Draw(pixel, secondRect, Color.Red);

            foreach (Rectangle boundary in boundaries)
            {
                _spriteBatch.Draw(pixel, boundary, Color.Yellow);
            }

            _spriteBatch.Draw(pixel, player, Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                player.Y -= 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.X -= 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                player.Y += 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.X += 1;
            }
        }

        public void Collide()
        {
            List<Rectangle> collisions = new List<Rectangle>();

            foreach (Rectangle boundary in boundaries)
            {
                if (boundary.Intersects(player))
                {
                    collisions.Add(boundary);
                }
            }

            foreach(Rectangle boundary in collisions)
            {
                Rectangle overlap = Rectangle.Intersect(boundary, player);

                if (overlap.Height >= overlap.Width)
                {
                    if (player.X < boundary.X)
                    {
                        player.X -= overlap.Width;
                    }

                    else
                    {
                        player.X += overlap.Width;
                    }
                }

                else
                {
                    if (player.Y < boundary.Y)
                    {
                        player.Y -= overlap.Height;
                    }

                    else
                    {
                        player.Y += overlap.Height;
                    }
                }
            }
        }
    }
}
