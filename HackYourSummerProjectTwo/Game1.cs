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
        private Texture2D placeholderTexture, charactersTexture, taskbarTexture, programIconTextures, levelSelectIconTexture,
            levelSelectMenuTexture, animatedBackgroundTexture, animatedForegroundTexture, cursorTexture, gameModeWindowTexture, buttonTexture,
             assessmentBarTexture, notepadFrameTexture, textBackgroundTexture;
        private SpriteFont font;
        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;
        #endregion

        #region Homebrew Type Variables
        private Button acceptButton, denyButton, notepadButton, returnToLevelSelectButton, levelRestartButton, backButton;
        private AnimatedBackground animatedBackground;
        private GameModeWindow programModeWindow;
        private ApplicationClient currentClient;
        private LevelSelector currentLevel;
        private GameState currentGameState;
        private string stopwatch;
        private Notepad notepad;
        private Cursor cursor;
        #endregion

        #region Container Variables
        private ArrayList clientList;
        private List<Notification> notificationArrayList;
        private List<LevelSelector> levels;
        private ArrayList assessmentBar;
        private List<Notification> timeNotificationArrayList;
        private Stack screenQueue;
        #endregion

        #region Miscellaneous (Mostly Primitive) Variables
        private bool levelFinished;
        private int minimumAssessmentNum, timer, levelTimeAllotment, level, assessmentMeter;
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
            screenQueue = new Stack();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Texture Assignments
            font = Content.Load<SpriteFont>("font10");
            placeholderTexture = Content.Load<Texture2D>("pixel");
            animatedBackgroundTexture = Content.Load<Texture2D>("The Night Shift- Title Card v2");
            animatedForegroundTexture = Content.Load<Texture2D>("The Night Shift- Title Text v2");
            cursorTexture = Content.Load<Texture2D>("The Night Shift- Cursor");
            gameModeWindowTexture = Content.Load<Texture2D>("The Night Shift- Game Mode Window");
            buttonTexture = Content.Load<Texture2D>("The Night Shift- Labeled Buttons");
            levelSelectIconTexture = Content.Load<Texture2D>("The Night Shift- Level Select Icon");
            levelSelectMenuTexture = Content.Load<Texture2D>("The Night Shift- Level Select Menu");
            programIconTextures = Content.Load<Texture2D>("The Night Shift- Program Icons");
            taskbarTexture = Content.Load<Texture2D>("The Night Shift- Taskbar");
            charactersTexture = Content.Load<Texture2D>("The Night Shift- Characters");
            assessmentBarTexture = Content.Load<Texture2D>("The Night Shift- AssessmentBarFillers");
            notepadFrameTexture = Content.Load<Texture2D>("The Night Shift- Notepad Frame");
            textBackgroundTexture = Content.Load<Texture2D>("The Night Shift- Text Background");
            #endregion

            acceptButton = new Button(buttonTexture, 161, 360, 240, 54, new Rectangle(0, 0, 240, 54));
            denyButton = new Button(buttonTexture, 455, 360, 184, 54, new Rectangle(0, 54, 184, 54));
            notepadButton = new Button(buttonTexture, 740, 2, 58, 54, new Rectangle(0, 324, 58, 54));
            returnToLevelSelectButton = new Button(buttonTexture, 445, 275, 126, 54, new Rectangle(0, 216, 126, 54));
            levelRestartButton = new Button(buttonTexture, 225, 275, 154, 54, new Rectangle(0, 162, 154, 54));
            backButton = new Button(buttonTexture, 106, 102, 58, 54, new Rectangle(0, 378, 58, 54));

            programModeWindow = new GameModeWindow(gameModeWindowTexture, buttonTexture, new Vector2(253, 67));
            notepad = new Notepad(notepadFrameTexture, 200, 120, 296, 240, font);
            animatedBackground = new AnimatedBackground(animatedBackgroundTexture, animatedForegroundTexture);
            cursor = new Cursor(cursorTexture);

            #region Button Subscriptions
            acceptButton.OnButtonClick += AcceptClient;
            denyButton.OnButtonClick += DenyClient;
            notepadButton.OnButtonClick += ToggleNotepad;
            returnToLevelSelectButton.OnButtonClick += NavigateToLevelSelect;
            levelRestartButton.OnButtonClick += NavigateToPrimaryGameScreen;
            backButton.OnButtonClick += NavigateBackwards;


            programModeWindow.Buttons[0].OnButtonClick += NavigateToLevelSelect;
            programModeWindow.Buttons[1].OnButtonClick += ResetGameMode;
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

            currKeyboardState = Keyboard.GetState();

            cursor.Update();

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    animatedBackground.Update(gameTime);
                    if (animatedBackground.ForegroundOpacity == 0)
                    {
                        currentGameState = GameState.MainMenu;
                        screenQueue.Push(currentGameState);
                    }
                    break;
                case GameState.MainMenu:
                    programModeWindow.Update();
                    break;
                case GameState.Settings:
                    break;
                case GameState.LevelSelect:
                    backButton.Update();
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
                        if (clientList.Count == 0 || (levelTimeAllotment - (timer / 60) == 0) || assessmentMeter == minimumAssessmentNum)
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
                        levelRestartButton.Update();
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

            prevKeyboardState = currKeyboardState;
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
                    backButton.Draw(_spriteBatch);
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
                        _spriteBatch.Draw(levelSelectMenuTexture, new Rectangle(125, 119, 550, 242), Color.White);

                        returnToLevelSelectButton.Draw(_spriteBatch);
                        levelRestartButton.Draw(_spriteBatch);
                        string score = (assessmentMeter >= minimumAssessmentNum) ? "Level Passed" : "Level Failed";
                        _spriteBatch.DrawString(font, score, new Vector2(400 - (font.MeasureString(score).X / 2), 220), Color.White);
                    }

                    timerFormatter = new DateTime(2026, 8, 3, 0, 0, levelTimeAllotment - (timer / 60));
                    stopwatch = timerFormatter.ToString("mm:ss");
                    _spriteBatch.DrawString(font, stopwatch, new Vector2(730, 448), Color.White);

                    _spriteBatch.DrawString(font, level.ToString(), new Vector2(668 + Math.Abs(18 - font.MeasureString(level.ToString()).X / 2), 448), Color.White);

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
            notificationArrayList.Add(new Notification(font, (currentClient.IsPhony) ? "Bad Program Accepted" : "Good Program Accepted", new Vector2(10, 412)));

            if (!currentClient.IsPhony)
            {
                assessmentBar.Add(new Cursor(placeholderTexture));
            }
            else
            {
                if (assessmentBar.Count > 0) { assessmentBar.RemoveAt(assessmentBar.Count - 1); }
            }

            assessmentMeter += (currentClient.IsPhony) ? -1 : 1;
            timer += (currentClient.IsPhony) ? 60 : -60;
            timeNotificationArrayList.Add(new Notification(font, (currentClient.IsPhony) ? "-1" : "+1", new Vector2(760, 412)));

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
            notificationArrayList.Add(new Notification(font, (currentClient.IsPhony) ? "Bad Program Denied" : "Good Program Denied", new Vector2(10, 412)));

            if (currentClient.IsPhony)
            {
                assessmentBar.Add(new Cursor(placeholderTexture));//MAYDAY: Eventually you'll remove this placeholderTexture and when you do you'll either have to make another constructor for the cursor class or find something else unique you can add to this ArrayList
            }
            else
            {
                if (assessmentBar.Count > 0) { assessmentBar.RemoveAt(assessmentBar.Count - 1); }
            }

            assessmentMeter += (currentClient.IsPhony) ? 1 : -1;
            timer += (currentClient.IsPhony) ? -60 : 60;
            timeNotificationArrayList.Add(new Notification(font, (currentClient.IsPhony) ? "+1" : "-1", new Vector2(760, 412)));

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
            notepad.IsShowing = true;
        }

        protected void NavigateToLevelSelect()
        {
            levelFinished = true;
            assessmentBar.Clear();
            currentGameState = GameState.LevelSelect;

            if (levels[levels.IndexOf(currentLevel)].IsCompleted && levels.IndexOf(currentLevel) != 9)
            {
                levels[levels.IndexOf(currentLevel) + 1].IsLocked = false;
            }
            notificationArrayList.Clear();
        }

        protected void NavigateToPrimaryGameScreen()
        {
            level = (levels.IndexOf(currentLevel) + 1);
            assessmentBar.Clear();
            timer = 0;
            levelFinished = false;
            assessmentMeter = 0;
            PopulateClientList();
            levelTimeAllotment = 10 + (level * 4);
            currentGameState = GameState.PrimaryGameScreen;
            foreach (LevelSelector selector in levels)
            {
                if (selector.BeenClicked)
                {
                    selector.BeenClicked = false;
                }
            }
        }

        protected void PopulateClientList()
        {
            clientList.Clear();
            minimumAssessmentNum = 8;

            Random rng = new Random();
            int difficultyLevel = 0;

            switch (levels.IndexOf(currentLevel))
            {
                case 0:
                case 1:
                case 2:
                    difficultyLevel = 0;
                    break;
                case 3:
                case 4:
                case 5:
                    difficultyLevel = rng.Next(0, 2);
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    difficultyLevel = rng.Next(0, 3);
                    break;
            }

            for (int i = 0; i < 12; i++)
            {
                clientList.Add(new ApplicationClient(programIconTextures, 272, 112, 256, 256, (DifficultyLevel)difficultyLevel, font, textBackgroundTexture));
            }
            currentClient = (ApplicationClient)clientList[0];
        }

        protected void NavigateBackwards()
        {
            currentGameState = (GameState)screenQueue.Pop();
            screenQueue.Push(currentGameState);
        }

        protected void ResetGameMode()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (i != 0)
                {
                    levels[i].IsLocked = true;
                }
                levels[i].IsCompleted = false;
            }
        }
    }
}
