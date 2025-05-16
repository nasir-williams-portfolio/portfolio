using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using random_password_generator_POLISHED.Content;
using System.Collections.Generic;

namespace random_password_generator_POLISHED
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D characters;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            characters = Content.Load<Texture2D>("ui letters and numbers spritesheet");
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

            List<Character> character_list = new List<Character>();

            character_list.Add(new Character('S', characters, Vector2.Zero));
            character_list.Add(new Character('P', characters, new Vector2(10, 0)));
            character_list.Add(new Character('I', characters, new Vector2(20, 0)));
            character_list.Add(new Character('D', characters, new Vector2(30, 0)));
            character_list.Add(new Character('E', characters, new Vector2(40, 0)));
            character_list.Add(new Character('Y', characters, new Vector2(50, 0)));

            foreach (Character letter in character_list)
            {
                letter.Draw(_spriteBatch);
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
