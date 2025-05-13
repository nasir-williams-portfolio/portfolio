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
        private Texture2D button_blue;
        private Texture2D button_blue_hover;
        private Texture2D button_red;
        private Texture2D button_red_hover;
        private Texture2D button_green;
        private Texture2D button_green_hover;

        private Button rng_button;
        private Button increase_button;
        private Button decrease_button;

        private string text;
        private string[] letters;
        private int[] numbers;
        private Random rng;
        private int window_height;
        private int window_width;
        private int password_length;

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
            password_length = 10;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("daydream_8");

            button_blue = Content.Load<Texture2D>("button");
            button_blue_hover = Content.Load<Texture2D>("button_hover");
            button_red = Content.Load<Texture2D>("button_red");
            button_red_hover = Content.Load<Texture2D>("button_red_hover");
            button_green = Content.Load<Texture2D>("button_green");
            button_green_hover = Content.Load<Texture2D>("button_green_hover");

            rng_button = new Button(
                button_blue,
                button_blue_hover,
                new Vector2(
                    window_width / 2 - button_blue.Width / 2,
                    window_height / 2 + 10));
            decrease_button = new Button(
                button_red,
                button_red_hover,
                new Vector2(rng_button.Rectangle.X - 35, rng_button.Rectangle.Y));
            increase_button = new Button(
                button_green,
                button_green_hover,
                new Vector2(rng_button.Rectangle.X + 67, rng_button.Rectangle.Y));

            rng_button.OnButtonClick += this.RandomizePassword;
            decrease_button.OnButtonClick += this.ShortenPassword;
            increase_button.OnButtonClick += this.LengthenPassword;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            rng_button.Update();
            decrease_button.Update();
            increase_button.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 position = new Vector2(
                window_width / 2 - font.MeasureString(text).X / 2,
                window_height / 2 - font.MeasureString(text).Y);

            _spriteBatch.Begin();

            rng_button.Draw(_spriteBatch);
            increase_button.Draw(_spriteBatch);
            decrease_button.Draw(_spriteBatch);

            _spriteBatch.DrawString(
                font,
                text,
                position,
                Color.Black);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void RandomizePassword()
        {
            text = "";

            for (int i = 0; i < password_length; i++)
            {
                text += letters[rng.Next(0, 26)];
            }
        }

        public void LengthenPassword()
        {
            password_length++;
        }

        public void ShortenPassword()
        {
            if (password_length > 1)
            {
                password_length--;
            }
        }
    }
}
