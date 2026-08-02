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
        private Button returnToLevelSelect;
        private Button levelReset;
        private Notepad notepad;
        private Random rng;
        private ArrayList notificationArrayList;
        private GameState currentGameState;
        private Button playButton;
        private Button settingsButton;
        private List<LevelSelector> levels;
        private LevelSelector currentLevel;

        private int assessmentMeter;
        private bool levelFinished;
        private int minimumAssessmentNum;

        private int timer;
        private int levelTimeAllotment;

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
            assessmentMeter = 0;
            levelFinished = false;

            timer = 0;

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

            returnToLevelSelect = new Button(placeholderTexture, 445, 320, 100, 25, Color.LightPink);
            levelReset = new Button(placeholderTexture, 225, 320, 100, 25, Color.LightBlue);

            notepad = new Notepad(placeholderTexture, 200, 120, 300, 240, placeholderFont);
            rng = new Random();
            clientList = new ArrayList();

            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
            playButton.OnButtonClick += NavigateToLevelSelect;
            returnToLevelSelect.OnButtonClick += NavigateToLevelSelect;
            settingsButton.OnButtonClick += NavigateToSettingsMenu;
            levelReset.OnButtonClick += NavigateToPrimaryGameScreen;

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
                    timer = 0;
                    break;
                case GameState.PrimaryGameScreen:
                    if (levelFinished == false)
                    {
                        if (clientList.Count == 0 || (levelTimeAllotment - (timer / 60) == 0))
                        {
                            if (assessmentMeter >= minimumAssessmentNum)
                            {
                                currentLevel.IsCompleted = true;
                            }

                            levelFinished = true;
                        }

                        currentClient.Update();
                        notepadButton.Update();
                        acceptButton.Update();
                        denyButton.Update();
                        notepad.Update();
                        timer++;
                    }
                    else
                    {
                        returnToLevelSelect.Update();
                        levelReset.Update();
                    }


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

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    _spriteBatch.DrawString(placeholderFont, "Welcome to The Night Shift", new Vector2(302, 240), Color.Black);
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
                    _spriteBatch.DrawString(placeholderFont, $"Selected Level: {levels.IndexOf(currentLevel) + 1}\nAssessment Meter: {assessmentMeter}/{minimumAssessmentNum * 2}\n{levelTimeAllotment - (timer / 60)}", Vector2.One, Color.Black);

                    currentClient.Draw(_spriteBatch);
                    notepadButton.Draw(_spriteBatch);
                    acceptButton.Draw(_spriteBatch);
                    denyButton.Draw(_spriteBatch);
                    notepad.Draw(_spriteBatch);

                    foreach (Notification notification in notificationArrayList)
                    {
                        notification.Draw(_spriteBatch);
                    }

                    if (levelFinished || (levelTimeAllotment - (timer / 60) == 0))
                    {
                        _spriteBatch.Draw(placeholderTexture, new Rectangle(175, 77, 450, 275), new Color(Color.Gray, 0.75f));
                        returnToLevelSelect.Draw(_spriteBatch);
                        levelReset.Draw(_spriteBatch);
                        _spriteBatch.DrawString(placeholderFont, (assessmentMeter >= minimumAssessmentNum) ? "Level Passed" : "Level Failed", new Vector2(175, 77), Color.Black);
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
            assessmentMeter += (currentClient.IsPhony) ? -1 : 1;
            timer += (currentClient.IsPhony) ? 60 : -60;
            if (clientList.Count > 0)
            {
                clientList.RemoveAt(0);

                if (clientList.Count > 0)
                {
                    currentClient = (ApplicationClient)clientList[0];
                }
            }
        }

        protected void DenyClient()
        {
            notificationArrayList.Add(new Notification(placeholderFont, (currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied :("));
            assessmentMeter += (currentClient.IsPhony) ? 1 : -1;
            timer += (currentClient.IsPhony) ? -60 : 60;
            if (clientList.Count > 0)
            {
                clientList.RemoveAt(0);
                if (clientList.Count > 0)
                {
                    currentClient = (ApplicationClient)clientList[0];
                }
            }
        }

        protected void ToggleNotepad()
        {
            notepad.IsShowing = !notepad.IsShowing;
        }

        protected void NavigateToLevelSelect()
        {
            levelFinished = true;
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
            timer = 0;
            levelFinished = false;
            assessmentMeter = 0;
            PopulateClientList((levels.IndexOf(currentLevel) + 1) * 2);
            levelTimeAllotment = 10 + (clientList.Count * 3);
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
            clientList.Clear();
            minimumAssessmentNum = numberOfClients / 2;
            for (int i = 0; i < numberOfClients; i++)
            {
                clientList.Add(new ApplicationClient(placeholderTexture, 350, 190, 100, 100, (DifficultyLevel)rng.Next(0, 3), placeholderFont));
            }
            currentClient = (ApplicationClient)clientList[0];
        }
    }

}
