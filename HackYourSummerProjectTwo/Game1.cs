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
        #region Monogame Specific Variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D placeholderTexture, charactersTexture, taskbarTexture, programIconTextures, levelSelectIconTexture, levelSelectMenuTexture, animatedBackgroundTexture,
            animatedForegroundTexture, cursorTexture, gameModeWindowTexture, buttonTexture, assessmentBarTexture;
        private SpriteFont placeholderFont;
        #endregion

        #region Homebrew Type Variables
        private ApplicationClient currentClient;
        private Button acceptButton, denyButton, notepadButton, returnToLevelSelectButton, levelResetButton;
        private Notepad notepad;
        private LevelSelector currentLevel;
        private GameState currentGameState;
        private AnimatedBackground animatedBackground;
        private Cursor cursor;
        private GameModeWindow programModeWindow;
        private Textbox stopwatch, level;
        #endregion

        #region Container Variables
        private ArrayList clientList;
        private List<Notification> notificationArrayList;
        private List<LevelSelector> levels;
        private ArrayList assessmentBar;
        private List<Notification> timeNotificationArrayList;
        #endregion

        #region Miscellaneous (Mostly Primitive) Variables
        private int assessmentMeter;
        private bool levelFinished;
        private int minimumAssessmentNum;
        private int timer;
        private int levelTimeAllotment;
        private DateTime timerFormatter;
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            currentGameState = GameState.TitleScreen;
            notificationArrayList = new List<Notification>();
            timeNotificationArrayList = new List<Notification>();
            levels = new List<LevelSelector>();
            clientList = new ArrayList();
            assessmentBar = new ArrayList();
            assessmentMeter = 0;
            levelFinished = false;
            timer = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Texture Assignments
            placeholderTexture = Content.Load<Texture2D>("pixel");
            placeholderFont = Content.Load<SpriteFont>("arial12");
            animatedBackgroundTexture = Content.Load<Texture2D>("The Night Shift- Title Card v2");
            animatedForegroundTexture = Content.Load<Texture2D>("The Night Shift- Title Text v2");
            cursorTexture = Content.Load<Texture2D>("The Night Shift- Cursor");
            gameModeWindowTexture = Content.Load<Texture2D>("The Night Shift- Game Mode Window");
            buttonTexture = Content.Load<Texture2D>("The Night Shift- Buttons");
            levelSelectIconTexture = Content.Load<Texture2D>("The Night Shift- Level Select Icon");
            levelSelectMenuTexture = Content.Load<Texture2D>("The Night Shift- Level Select Menu");
            programIconTextures = Content.Load<Texture2D>("The Night Shift- Program Icons");
            taskbarTexture = Content.Load<Texture2D>("The Night Shift- Taskbar");
            charactersTexture = Content.Load<Texture2D>("The Night Shift- Characters");
            assessmentBarTexture = Content.Load<Texture2D>("The Night Shift- AssessmentBarFillers");
            #endregion

            acceptButton = new Button(placeholderTexture, 275, 380, 75, 25, Color.Green);
            denyButton = new Button(placeholderTexture, 450, 380, 75, 25, Color.Red);
            notepadButton = new Button(placeholderTexture, 750, 50, 20, 20, Color.White);
            returnToLevelSelectButton = new Button(placeholderTexture, 445, 320, 100, 25, Color.LightPink);
            levelResetButton = new Button(placeholderTexture, 225, 320, 100, 25, Color.LightBlue);

            programModeWindow = new GameModeWindow(gameModeWindowTexture, buttonTexture, new Vector2(253, 67));
            notepad = new Notepad(placeholderTexture, 200, 120, 300, 240, placeholderFont);
            animatedBackground = new AnimatedBackground(animatedBackgroundTexture, animatedForegroundTexture);
            cursor = new Cursor(cursorTexture);

            stopwatch = new Textbox("", charactersTexture, new Vector2(717, 448));
            level = new Textbox("", charactersTexture, new Vector2(679, 448));

            #region Button Subscriptions
            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
            returnToLevelSelectButton.OnButtonClick += NavigateToLevelSelect;
            levelResetButton.OnButtonClick += NavigateToPrimaryGameScreen;

            programModeWindow.Buttons[0].OnButtonClick += NavigateToLevelSelect;
            #endregion

            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    levels.Add(new LevelSelector(levelSelectIconTexture, new Vector2(189 + (i * 90), 165), true));
                }
                else
                {
                    levels.Add(new LevelSelector(levelSelectIconTexture, new Vector2(189 + ((i - 5) * 90), 255), true));
                }
            }

            levels[0].IsLocked = false;
            currentLevel = levels[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            cursor.Update();

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    animatedBackground.Update(gameTime);
                    if (animatedBackground.ForegroundOpacity == 0)
                    {
                        currentGameState = GameState.MainMenu;
                    }
                    break;
                case GameState.MainMenu:
                    programModeWindow.Update();
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
                        returnToLevelSelectButton.Update();
                        levelResetButton.Update();
                    }

                    for (int i = 0; i < notificationArrayList.Count; i++)
                    {
                        notificationArrayList[i].Update();
                        if (notificationArrayList[i].IsDismissed)
                        {
                            notificationArrayList.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < timeNotificationArrayList.Count; i++)
                    {
                        timeNotificationArrayList[i].Update();
                        if (timeNotificationArrayList[i].IsDismissed)
                        {
                            timeNotificationArrayList.RemoveAt(i);
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            animatedBackground.Draw(_spriteBatch);

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    break;
                case GameState.MainMenu:
                    programModeWindow.Draw(_spriteBatch);
                    break;
                case GameState.Settings:
                    break;
                case GameState.LevelSelect:
                    _spriteBatch.Draw(levelSelectMenuTexture, new Rectangle(125, 119, 550, 242), Color.White);
                    foreach (LevelSelector selector in levels)
                    {
                        selector.Draw(_spriteBatch);
                    }
                    break;
                case GameState.PrimaryGameScreen:
                    _spriteBatch.Draw(taskbarTexture, new Rectangle(0, 0, 800, 480), Color.White);
                    currentClient.Draw(_spriteBatch);
                    notepadButton.Draw(_spriteBatch);
                    acceptButton.Draw(_spriteBatch);
                    denyButton.Draw(_spriteBatch);
                    notepad.Draw(_spriteBatch);

                    foreach (Notification notification in notificationArrayList)
                    {
                        notification.Draw(_spriteBatch);
                    }

                    foreach (Notification notification in timeNotificationArrayList)
                    {
                        notification.Draw(_spriteBatch);
                    }

                    if (levelFinished || (levelTimeAllotment - (timer / 60) == 0))
                    {
                        _spriteBatch.Draw(placeholderTexture, new Rectangle(175, 77, 450, 275), new Color(Color.Gray, 0.75f));
                        returnToLevelSelectButton.Draw(_spriteBatch);
                        levelResetButton.Draw(_spriteBatch);
                        _spriteBatch.DrawString(placeholderFont, (assessmentMeter >= minimumAssessmentNum) ? "Level Passed" : "Level Failed", new Vector2(175, 77), Color.Black);
                    }

                    timerFormatter = new DateTime(2026, 8, 3, 0, 0, levelTimeAllotment - (timer / 60));
                    stopwatch.Phrase = timerFormatter.ToString("mm:ss");
                    stopwatch.Draw(_spriteBatch);

                    level.Phrase = (levels.IndexOf(currentLevel) + 1).ToString();
                    level.Draw(_spriteBatch);

                    foreach (Cursor tracker in assessmentBar)
                    {
                        int yValue;
                        if (assessmentBar.IndexOf(tracker) == 0)
                        {
                            yValue = 0;
                        }
                        else if (assessmentBar.IndexOf(tracker) == assessmentBar.Count - 1)
                        {
                            yValue = 52;
                        }
                        else
                        {
                            yValue = 26;
                        }

                        _spriteBatch.Draw(assessmentBarTexture, new Rectangle(142 + (64 * assessmentBar.IndexOf(tracker)), 442, 64, 26), new Rectangle(0, yValue, 64, 26), Color.White);
                    }
                    break;
                default:
                    break;
            }

            cursor.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptClient()
        {
            notificationArrayList.Add(new Notification(new Textbox((currentClient.IsPhony) ? "Bad Program Accepted" : "Good Program Accepted", charactersTexture, new Vector2(10, 412))));

            if (!currentClient.IsPhony)
            {
                assessmentBar.Add(new Cursor(placeholderTexture));
            }
            else
            {
                assessmentBar.RemoveAt(assessmentBar.Count - 1);
            }

            assessmentMeter += (currentClient.IsPhony) ? -1 : 1;
            timer += (currentClient.IsPhony) ? 60 : -60;
            timeNotificationArrayList.Add(new Notification(new Textbox((currentClient.IsPhony) ? "-1" : "+1", charactersTexture, new Vector2(717, 412))));

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
            notificationArrayList.Add(new Notification(new Textbox((currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied", charactersTexture, new Vector2(10, 412))));

            if (currentClient.IsPhony)
            {
                assessmentBar.Add(new Cursor(placeholderTexture));//MAYDAY: Eventually you'll remove this placeholderTexture and when you do you'll either have to make another constructor for the cursor class or find something else unique you can add to this ArrayList
            }
            else
            {
                assessmentBar.RemoveAt(assessmentBar.Count - 1);
            }

            assessmentMeter += (currentClient.IsPhony) ? 1 : -1;
            timer += (currentClient.IsPhony) ? -60 : 60;
            timeNotificationArrayList.Add(new Notification(new Textbox((currentClient.IsPhony) ? "+1" : "-1", charactersTexture, new Vector2(717, 412))));

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
            assessmentBar.Clear();
            currentGameState = GameState.LevelSelect;

            if (levels[levels.IndexOf(currentLevel)].IsCompleted)
            {
                levels[levels.IndexOf(currentLevel) + 1].IsLocked = false; //MAYDAY: this will throw an error if it runs on the 9th level
            }
            notificationArrayList.Clear();
        }

        protected void NavigateToPrimaryGameScreen()
        {
            assessmentBar.Clear();
            timer = 0;
            levelFinished = false;
            assessmentMeter = 0;
            PopulateClientList();
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

        public void PopulateClientList()
        {
            clientList.Clear();
            minimumAssessmentNum = 8;
            for (int i = 0; i < 8; i++)
            {
                clientList.Add(new ApplicationClient(programIconTextures, 272, 112, 256, 256, 0, placeholderFont));
            }
            currentClient = (ApplicationClient)clientList[0];
        }
    }
}
