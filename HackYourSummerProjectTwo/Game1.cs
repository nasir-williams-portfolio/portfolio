using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HackYourSummerProjectTwo
{
    public enum GameState
    {
        TitleScreen,
        MainMenu,
        Settings,
        LevelSelect,
        PrimaryGameScreen
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D placeholderTexture;
        private SpriteFont placeholderFont;
        private ArrayList clientList;
        private ApplicationClient currentClient;
        private Button acceptButton;
        private Button denyButton;
        private Button notepadButton;
        private Notepad notepad;
        private Random rng;

        private ArrayList notificationArrayList;
        private GameState currentGameState;
        private Button playButton;
        private Button settingsButton;
        private List<LevelSelector> levels;
        private LevelSelector currentLevel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            notificationArrayList = new ArrayList();
            currentGameState = GameState.TitleScreen;
            levels = new List<LevelSelector>();


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
            playButton = new Button(placeholderTexture, 368, 228, 75, 25, Color.White);
            settingsButton = new Button(placeholderTexture, 368, 260, 75, 25, Color.Gray);
            notepad = new Notepad(placeholderTexture, 200, 120, 350, 240, placeholderFont);
            rng = new Random();
            clientList = new ArrayList();

            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
            playButton.OnButtonClick += NavigateToLevelSelect;
            settingsButton.OnButtonClick += NavigateToSettingsMenu;

            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    levels.Add(new LevelSelector(placeholderTexture, new Vector2(50, 130), false));
                }
                else if (i < 5 && i > 0)
                {
                    levels.Add(new LevelSelector(placeholderTexture, new Vector2(50 + (150 * i), 130), true));
                }
                else
                {
                    levels.Add(new LevelSelector(placeholderTexture, new Vector2(50 + (150 * (i - 5)), 280), true));
                }
            }

            currentLevel = levels[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    if (Keyboard.GetState().GetPressedKeyCount() > 0)
                    {
                        currentGameState = GameState.MainMenu;
                    }
                    break;
                case GameState.MainMenu:
                    playButton.Update();
                    settingsButton.Update();
                    break;
                case GameState.Settings:
                    break;
                case GameState.LevelSelect:
                    foreach (LevelSelector selector in levels)
                    {
                        selector.Update();
                        if (selector.BeenClicked == true)
                        {
                            currentLevel = selector;
                            NavigateToPrimaryGameScreen();
                        }
                    }
                    break;
                case GameState.PrimaryGameScreen:
                    if (clientList.Count == 0)
                    {
                        currentLevel.IsCompleted = true;
                        NavigateToLevelSelect();
                    }

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
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(placeholderFont, $"{currentGameState.ToString()}", Vector2.One, Color.Black);

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    _spriteBatch.DrawString(placeholderFont, "Welcome to Midnight at Club Key", new Vector2(400 - (placeholderFont.MeasureString("Welcome to Midnight at Club Key").X / 2), 240), Color.Black);
                    break;
                case GameState.MainMenu:
                    playButton.Draw(_spriteBatch);
                    settingsButton.Draw(_spriteBatch);
                    break;
                case GameState.Settings:
                    break;
                case GameState.LevelSelect:
                    foreach (LevelSelector selector in levels)
                    {
                        selector.Draw(_spriteBatch);
                    }
                    break;
                case GameState.PrimaryGameScreen:
                    _spriteBatch.DrawString(placeholderFont, $"Selected Level {levels.IndexOf(currentLevel) + 1}", new Vector2(0, 15), Color.Black);
                    currentClient.Draw(_spriteBatch);
                    notepadButton.Draw(_spriteBatch);
                    acceptButton.Draw(_spriteBatch);
                    denyButton.Draw(_spriteBatch);
                    notepad.Draw(_spriteBatch);

                    foreach (Notification notification in notificationArrayList)
                    {
                        notification.Draw(_spriteBatch);
                    }
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptClient()
        {
            notificationArrayList.Add(new Notification(placeholderFont, (currentClient.IsPhony) ? "Bad Program Accepted :(" : "Good Program Accepted"));
            clientList.RemoveAt(0);
            if (clientList.Count > 0)
            {
                currentClient = (ApplicationClient)clientList[0];
            }
        }

        protected void DenyClient()
        {
            notificationArrayList.Add(new Notification(placeholderFont, (currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied :("));
            clientList.RemoveAt(0);
            if (clientList.Count > 0)
            {
                currentClient = (ApplicationClient)clientList[0];
            }
        }

        protected void ToggleNotepad()
        {
            notepad.IsShowing = !notepad.IsShowing;
        }

        protected void NavigateToLevelSelect()
        {
            currentGameState = GameState.LevelSelect;

            if (levels[levels.IndexOf(currentLevel)].IsCompleted)
            {
                levels[levels.IndexOf(currentLevel) + 1].IsLocked = false; //this will throw an error if it runs on the 9th level
            }
            notificationArrayList.Clear();
        }

        protected void NavigateToSettingsMenu()
        {
            currentGameState = GameState.Settings;
        }

        protected void NavigateToPrimaryGameScreen()
        {
            PopulateClientList((levels.IndexOf(currentLevel) + 1) * 2);
            currentGameState = GameState.PrimaryGameScreen;
            foreach (LevelSelector selector in levels)
            {
                if (selector.BeenClicked)
                {
                    selector.BeenClicked = false;
                }
            }
        }

        public void PopulateClientList(int numberOfClients)
        {
            for (int i = 0; i < numberOfClients; i++)
            {
                clientList.Add(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            }
            currentClient = (ApplicationClient)clientList[0];
        }
    }

}
