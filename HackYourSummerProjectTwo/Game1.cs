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

        private ArrayList notificationArrayList;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            notificationArrayList = new ArrayList();

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

            for (int i = 0; i < notificationArrayList.Count; i++)
            {
                Notification currentNotification = (Notification)notificationArrayList[i];
                currentNotification.Update();
                if (currentNotification.IsDismissed)
                {
                    notificationArrayList.Remove(i);
                }
            }

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

            foreach (Notification notification in notificationArrayList)
            {
                notification.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptClient()
        {
            notificationArrayList.Add(new Notification(placeholderFont, (currentClient.IsPhony) ? "Bad Program Accepted :(" : "Good Program Accepted"));
            clientQueue.Dequeue();
            clientQueue.Enqueue(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ApplicationClient)clientQueue.Peek();
        }

        protected void DenyClient()
        {
            notificationArrayList.Add(new Notification(placeholderFont, (currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied :("));
            clientQueue.Dequeue();
            clientQueue.Enqueue(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            currentClient = (ApplicationClient)clientQueue.Peek();
        }

        protected void ToggleNotepad()
        {
            notepad.IsShowing = !notepad.IsShowing;
        }
    }

}
