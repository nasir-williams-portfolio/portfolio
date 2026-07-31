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
        private ApplicationClient currentClient;
        private Button acceptButton;
        private Button denyButton;
        private Button notepadButton;
        private Notepad notepad;
        private Random rng;

        private Vector2 notificationVector;
        private float opacity;
        private bool notificationIsMoving;
        private string notificationText;
        private ArrayList notificationList;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            notificationVector = new Vector2(10, 462);
            opacity = 1f;
            notificationList = new ArrayList();

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
            notepad = new Notepad(placeholderTexture, 200, 120, 350, 240, placeholderFont);
            rng = new Random();
            clientQueue = new Queue();
            clientQueue.Enqueue(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, DifficultyLevel.Hard, placeholderFont));

            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentClient = (ApplicationClient)clientQueue.Peek();
            currentClient.Update();
            notepadButton.Update();
            acceptButton.Update();
            denyButton.Update();
            notepad.Update();

            TriggerNotification();

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

            if (notificationIsMoving)
            {
                _spriteBatch.DrawString(placeholderFont, notificationList[notificationList.Count - 1].ToString(), notificationVector, new Color(Color.Black, opacity));
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptClient()
        {
            notificationList.Add(notificationText = (currentClient.IsPhony) ? "Bad Program Accepted :(" : "Good Program Accepted");
            clientQueue.Dequeue();
            clientQueue.Enqueue(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ApplicationClient)clientQueue.Peek();

            notificationVector.Y = 462;
            opacity = 1f;
            notificationIsMoving = true;
        }

        protected void DenyClient()
        {
            notificationList.Add((currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied :(");
            clientQueue.Dequeue();
            clientQueue.Enqueue(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ApplicationClient)clientQueue.Peek();

            notificationVector.Y = 462;
            opacity = 1f;
            notificationIsMoving = true;
        }

        protected void ToggleNotepad()
        {
            notepad.IsShowing = !notepad.IsShowing;
        }

        protected void TriggerNotification()
        {
            if (notificationVector.Y > 240 && notificationIsMoving)
            {
                notificationVector.Y -= 1;
                opacity -= 0.005f;
            }
        }
    }
}
