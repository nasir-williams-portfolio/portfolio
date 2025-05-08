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

        private Rectangle player;

        private List<Rectangle> tops;
        private List<Rectangle> lefts;
        private List<Rectangle> bottoms;
        private List<Rectangle> rights;

        private Rectangle firstRoom;
        private Rectangle secondRoom;

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

            tops = new List<Rectangle>();
            tops.Add(new Rectangle(350, 240, 100, 1));
            tops.Add(new Rectangle(450, 265, 50, 1));

            lefts = new List<Rectangle>();
            lefts.Add(new Rectangle(350, 240, 1, 100));

            bottoms = new List<Rectangle>();
            bottoms.Add(new Rectangle(350, 340, 100, 1));
            bottoms.Add(new Rectangle(450, 315, 50, 1));

            rights = new List<Rectangle>();
            rights.Add(new Rectangle(450, 240, 1, 25));
            rights.Add(new Rectangle(450, 315, 1, 25));
            rights.Add(new Rectangle(500, 265, 1, 50));

            firstRoom = new Rectangle(350, 240, 100, 100);
            secondRoom = new Rectangle(450, 265, 50, 50);

            player = new Rectangle(360, 250, 1, 1);

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
                foreach(Rectangle side in tops)
                {
                    if (player.Top >= side.Top)
                    {
                        player.Y -= 1;
                    }
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                foreach (Rectangle side in lefts)
                {
                    if (player.Left >= side.Left)
                    {
                        player.X -= 1;
                    }
                }
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                foreach (Rectangle side in bottoms)
                {
                    if (player.Bottom <= side.Bottom)
                    {
                        player.Y += 1;
                    }
                }    
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (
                    (player.Right <= rights[0].Right && 
                    player.Right <= rights[1].Right) ||  
                    player.Right <= rights[2].Right)
                {
                    player.X += 1;
                }
            }

            System.Diagnostics.Debug.WriteLine(
                $"player right: {player.Right}" +
                $"\ntop third right: {rights[0].Right}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(pixel, firstRoom, Color.White);
            _spriteBatch.Draw(pixel, secondRoom, Color.White);

            foreach (Rectangle side in tops)
            {
                _spriteBatch.Draw(pixel, side, Color.Black);
            }

            foreach (Rectangle side in lefts)
            {
                _spriteBatch.Draw(pixel, side, Color.Black);
            }

            foreach (Rectangle side in bottoms)
            {
                _spriteBatch.Draw(pixel, side, Color.Black);
            }

            foreach (Rectangle side in rights)
            {
                _spriteBatch.Draw(pixel, side, Color.Black);
            }

            _spriteBatch.Draw(pixel, player, Color.Red);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
