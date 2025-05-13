using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace random_password_generator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;
        private Texture2D sprite;
        private Texture2D sprite_hover;

        private Button button;

        private string text;
        private string[] letters;
        private int[] numbers;
        private Random rng;

        private int window_height;
        private int window_width;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            letters = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
            numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
            text = "";
            rng = new Random();
            window_height = _graphics.PreferredBackBufferHeight;
            window_width = _graphics.PreferredBackBufferWidth;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("daydream_8");
            sprite = Content.Load<Texture2D>("button");
            sprite_hover = Content.Load<Texture2D>("button_hover");

            button = new Button(
                sprite,
                sprite_hover,
                new Vector2(
                    window_width / 2 - 31,
                    window_height / 2 + 35));

            button.OnButtonClick += this.RandomizePassword;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            button.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 position = new Vector2(
                window_width / 2 - font.MeasureString(text).X / 2,
                window_height / 2 - font.MeasureString(text).Y);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(
                font,
                text,
                position,
                Color.Black);
            button.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void RandomizePassword()
        {
            text = "";

            for (int i = 0; i < 10; i++)
            {
                text += letters[rng.Next(0, 26)];
            }
        }
    }
}
