using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace test_FileIO
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;
        private List<Rectangle> squares;
        private Texture2D sprite;
        private Color[] colors;

        private Random rng;
        private string text;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            rng = new Random();
            squares = new List<Rectangle>();
            colors = new Color[7];
            for (int i = 0; i < 7; i++)
            {
                colors[i] = new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble());
            }

            StreamReader textReader = null;
            textReader = new StreamReader("C:\\Users\\QuizM\\Desktop\\Personal\\Programming Portfolio\\portfolio\\test_FileIO\\text.txt");
            while ((text = textReader.ReadLine()!) != null)
            {
                string[] rectangle_info = text.Split(",");
                squares.Add(new Rectangle(
                    int.Parse(rectangle_info[0]),
                    int.Parse(rectangle_info[1]),
                    int.Parse(rectangle_info[2]),
                    int.Parse(rectangle_info[3])));
            }
            textReader.Close();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("arial_8");
            sprite = Content.Load<Texture2D>("single_pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            int color = 0;

            foreach (Rectangle square in squares)
            {
                _spriteBatch.Draw(sprite, square, colors[color]);
                color++;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
