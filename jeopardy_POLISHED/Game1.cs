using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace jeopardy_POLISHED
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Color backgroundColor;
        private Texture2D addButtonSprite;
        private Random rng;


        private Button addButton;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            backgroundColor = Color.CornflowerBlue;
            rng = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            addButtonSprite = Content.Load<Texture2D>("addButton");
            addButton = new Button(addButtonSprite, new Vector2(400, 240));
            addButton.onSingleButtonPress += ChangeBackgroundColor;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            addButton.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch.Begin();

            addButton.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void ChangeBackgroundColor()
        {
            backgroundColor = new Color(rng.Next(0, 257), rng.Next(0, 257), rng.Next(0, 257));
        }
    }
}
