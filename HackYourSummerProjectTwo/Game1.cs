using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;

namespace HackYourSummerProjectTwo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D placeholderTexture;
        private SpriteFont placeholderFont;
        private Queue clientQueue;
        private ClubClient currentClient;
        private Button acceptButton;
        private Button denyButton;
        private Button notepadButton;
        private Notepad notepad;
        private Random rng;

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
            placeholderFont = Content.Load<SpriteFont>("arial12");

            acceptButton = new Button(placeholderTexture, 275, 380, 75, 25, Color.Green);
            denyButton = new Button(placeholderTexture, 450, 380, 75, 25, Color.Red);
            notepadButton = new Button(placeholderTexture, 750, 50, 20, 20, Color.White);
            notepad = new Notepad(placeholderTexture, 200, 120, 400, 240);
            rng = new Random();
            clientQueue = new Queue();
            clientQueue.Enqueue(new ClubClient(placeholderTexture, 350, 190, 100, 100, DifficultyLevel.Easy, placeholderFont));

            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentClient = (ClubClient)clientQueue.Peek();
            currentClient.Update();
            notepadButton.Update();
            acceptButton.Update();
            denyButton.Update();
            notepad.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            currentClient.Draw(_spriteBatch);
            notepadButton.Draw(_spriteBatch);
            acceptButton.Draw(_spriteBatch);
            denyButton.Draw(_spriteBatch);
            notepad.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptClient()
        {
            System.Diagnostics.Debug.WriteLine((currentClient.IsPhony) ? "Bad" : "Good");

            clientQueue.Dequeue();
            clientQueue.Enqueue(new ClubClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ClubClient)clientQueue.Peek();
        }

        protected void DenyClient()
        {
            System.Diagnostics.Debug.WriteLine((currentClient.IsPhony) ? "Good" : "Bad");

            clientQueue.Dequeue();
            clientQueue.Enqueue(new ClubClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ClubClient)clientQueue.Peek();
        }

        protected void ToggleNotepad()
        {
            notepad.IsShowing = !notepad.IsShowing;
        }
    }
}
