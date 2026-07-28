using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace HackYourSummerProjectTwo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D placeholderTexture;
        private Queue applicationQueue;
        private Application currentApplication;
        private Button acceptButton;
        private Button denyButton;

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

            placeholderTexture = Content.Load<Texture2D>("pixel");
            acceptButton = new Button(placeholderTexture, new Vector2(288, 350), Color.Green);
            denyButton = new Button(placeholderTexture, new Vector2(438, 350), Color.Red);
            applicationQueue = new Queue();

            applicationQueue.Enqueue(new Application(placeholderTexture));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentApplication = (Application)applicationQueue.Peek();

            currentApplication.Update();
            acceptButton.Update();
            denyButton.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            currentApplication.Draw(_spriteBatch);
            acceptButton.Draw(_spriteBatch);
            denyButton.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
