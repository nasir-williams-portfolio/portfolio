using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private Button toggle_capitals;
        private Button toggle_symbols;
        private List<Button> button_list;

        private string text;
        private List<string> letters;
        private List<int> numbers;
        private List<char> symbols;
        private Random rng;
        private int window_height;
        private int window_width;
        private int password_length;
        private bool usingCapitals;
        private bool usingSymbols;

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
            symbols = ['!', '?', '_'];
            text = "";
            rng = new Random();
            window_height = _graphics.PreferredBackBufferHeight;
            window_width = _graphics.PreferredBackBufferWidth;
            password_length = 10;
            button_list = new List<Button>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("arial_8");

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
            toggle_capitals = new Button(button_green, button_green_hover, Vector2.Zero);
            toggle_symbols = new Button(button_red, button_red_hover, new Vector2(0, 32));

            rng_button.OnButtonClick += this.RandomizePassword;
            decrease_button.OnButtonClick += this.ShortenPassword;
            increase_button.OnButtonClick += this.LengthenPassword;
            toggle_capitals.OnButtonClick += this.ToggleCaptials;
            toggle_symbols.OnButtonClick += this.ToggleSymbols;

            button_list.Add(rng_button);
            button_list.Add(decrease_button);
            button_list.Add(increase_button);
            button_list.Add(toggle_capitals);
            button_list.Add(toggle_symbols);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Button btn in button_list)
            {
                btn.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 position = new Vector2(
                window_width / 2 - font.MeasureString(text).X / 2,
                window_height / 2 - font.MeasureString(text).Y);

            _spriteBatch.Begin();

            foreach (Button btn in button_list)
            {
                btn.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(
                font,
                text,
                position,
                Color.Black);

            _spriteBatch.DrawString(
                font,
                $"the password will have capital letters: {usingCapitals}" +
                $"\nthe password will have symbols: {usingSymbols}" +
                $"\nlength of password: {password_length}",
                new Vector2(0, 100), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void RandomizePassword()
        {
            text = "";

            for (int i = 0; i < password_length; i++)
            {
                int probability = rng.Next(0, 101);

                if (probability <= 10 && i > 0 && !symbols.Contains(text[i - 1]) && usingSymbols)
                {
                    text += symbols[rng.Next(0, 3)];
                }

                else if (probability <= 30 && probability >= 11)
                {
                    text += numbers[rng.Next(0, 10)];
                }

                else
                {
                    if (usingCapitals && rng.Next(0, 11) > 7 && usingCapitals)
                    {
                        text += letters[rng.Next(0, 26)].ToUpper();
                    }

                    else
                    {
                        text += letters[rng.Next(0, 26)].ToLower();
                    }
                }
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

        public void ToggleCaptials()
        {
            usingCapitals = !usingCapitals;
        }

        public void ToggleSymbols()
        {
            usingSymbols = !usingSymbols;
        }
    }
}
