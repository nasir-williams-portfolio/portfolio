using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Jeopardy
{
    public enum State
    {
        Start,
        Options,
        Question
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        private MouseState currentMouseState;
        private MouseState prevMouseState;

        private Rectangle flipToReveal;
        private Rectangle revealRectangle;
        private Rectangle addRectangle;
        private Rectangle subtractRectangle;
        private Rectangle continueRectangle;
        private Rectangle answerRectangle;
        private Rectangle questionBackgroundRectangle;

        private Texture2D sprite;
        private Texture2D buttonSprite;
        private Texture2D background;
        private Texture2D teamNameLabel;
        private Texture2D nameChangeIndicator;
        private Texture2D star;
        private Texture2D questionBackground;
        private Texture2D addButton;
        private Texture2D subtractButton;
        private Texture2D continueButton;
        private Texture2D revealButton;
        private Texture2D answerSprite;

        private Button[,] boardClues;
        private List<Team> teams;
        private Team testTeam;
        private Team testTeamTwo;
        private Team currentTeam;
        private Team testTeamThree;
        private Team testTeamFour;
        private Team testTeamFive;
        private State currentState;

        private string line;
        private string currentQuestion;
        private string currentAnswer;
        private string[] categories;
        private string pointUpdate;
        private string currentCategory;
        private int currentPointValue;
        private bool isRevealed;

        private KeyboardState currState;
        private KeyboardState prevState;

        private StreamReader sr;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currentQuestion = "NULL";
            flipToReveal = new Rectangle(361, 300, 78, 30);
            isRevealed = false;
            currentAnswer = "Correct Answer";
            prevMouseState = currentMouseState;

            categories = new string[5];

            categories[0] = "Fairytale Characters";
            categories[1] = "Gods & Spirits";
            categories[2] = "Creation/Destruction";
            categories[3] = "Modern-Day Remakes";
            categories[4] = "Misc.";

            currState = prevState;

            answerRectangle = new Rectangle(flipToReveal.X - 61, flipToReveal.Y + 50, 200, 50);
            revealRectangle = new Rectangle(0, 0, 78, 30);
            addRectangle = new Rectangle(answerRectangle.Left - 35, answerRectangle.Y + 10, 30, 30);
            subtractRectangle = new Rectangle(answerRectangle.Right + 5, answerRectangle.Y + 10, 30, 30);
            questionBackgroundRectangle = new Rectangle(50, 140, 700, 200);

            pointUpdate = "";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("ari-w9500-display");
            sprite = Content.Load<Texture2D>("single_pixel");
            background = Content.Load<Texture2D>("jeopardy_background");
            teamNameLabel = Content.Load<Texture2D>("teamNameLabel");
            nameChangeIndicator = Content.Load<Texture2D>("nameChangeIndicator");
            star = Content.Load<Texture2D>("star");
            buttonSprite = Content.Load<Texture2D>("buttonSprite");
            questionBackground = Content.Load<Texture2D>("questionBackground");
            addButton = Content.Load<Texture2D>("addButton");
            subtractButton = Content.Load<Texture2D>("subtractButton");
            continueButton = Content.Load<Texture2D>("continueButton");
            revealButton = Content.Load<Texture2D>("revealButton");
            answerSprite = Content.Load<Texture2D>("answerSprite");

            currentState = State.Start;

            // add two more teams - make modular at another time
            testTeam = new Team(font, teamNameLabel, new Vector2(600, 105), nameChangeIndicator);
            testTeamTwo = new Team(font, teamNameLabel, new Vector2(600, 155), nameChangeIndicator);
            testTeamThree = new Team(font, teamNameLabel, new Vector2(600, 205), nameChangeIndicator);
            testTeamFour = new Team(font, teamNameLabel, new Vector2(600, 255), nameChangeIndicator);
            testTeamFive = new Team(font, teamNameLabel, new Vector2(600, 305), nameChangeIndicator);

            testTeam.OnTeamClick += SelectTeam;
            testTeamTwo.OnTeamClick += SelectTeam;
            testTeamThree.OnTeamClick += SelectTeam;
            testTeamFour.OnTeamClick += SelectTeam;
            testTeamFive.OnTeamClick += SelectTeam;

            teams = new List<Team>();
            teams.Add(testTeam);
            teams.Add(testTeamTwo);
            teams.Add(testTeamThree);
            teams.Add(testTeamFour);
            teams.Add(testTeamFive);
            currentTeam = teams[0];

            #region Board Generation
            boardClues = new Button[5, 5];
            sr = new StreamReader("C:\\Users\\QuizM\\Desktop\\portfolio\\Jeopardy\\JeopardyCluesAndQuestions.txt");

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if ((line = sr.ReadLine()) != null)
                    {
                        string[] placeholder = line.Split('|');
                        boardClues[x, y] = new Button(int.Parse(placeholder[0]), font, buttonSprite, new Vector2(50 + (y * 105), 105 + (x * 55)), placeholder[1].Replace("*", "\n"), placeholder[2]);
                        boardClues[x, y].OnButtonClick += SelectQuestion;
                    }
                }
            }
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // so...it is staying
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                currentState = State.Options;
            }

            currentMouseState = Mouse.GetState();
            currState = Keyboard.GetState();

            Rectangle cursor = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);

            switch (currentState)
            {
                case State.Start:
                    break;
                case State.Options:
                    if (currState.IsKeyDown(Keys.Up) && prevState.IsKeyUp(Keys.Up))
                    {
                        currentTeam.Score += 100;
                    }

                    if (currState.IsKeyDown(Keys.Down) && prevState.IsKeyUp(Keys.Down))
                    {
                        currentTeam.Score -= 100;
                    }
                    isRevealed = false;
                    pointUpdate = "";
                    for (int x = 0; x < 5; x++)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            boardClues[x, y].Update();
                        }
                    }
                    foreach (Team team in teams)
                    {
                        team.Update();
                    }
                    break;
                case State.Question:
                    if (flipToReveal.Contains(cursor) && currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        isRevealed = !isRevealed;
                    }

                    if (addRectangle.Contains(cursor) && currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        currentTeam.Score += currentPointValue;
                        pointUpdate = currentPointValue + " Points Awarded";
                    }
                    if (subtractRectangle.Contains(cursor) && currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        pointUpdate = currentPointValue + " Points Deducted";
                        currentTeam.Score -= currentPointValue;

                    }


                    if (currState.IsKeyDown(Keys.D1))
                    {
                        currentTeam = teams[0];
                    }
                    else if (currState.IsKeyDown(Keys.D2))
                    {
                        currentTeam = teams[1];
                    }
                    else if (currState.IsKeyDown(Keys.D3))
                    {
                        currentTeam = teams[2];
                    }
                    else if (currState.IsKeyDown(Keys.D4))
                    {
                        currentTeam = teams[3];
                    }
                    break;
            }

            prevMouseState = currentMouseState;
            prevState = currState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            _spriteBatch.Draw(sprite, new Rectangle(0, 480, 800, 300), Color.Black);

            switch (currentState)
            {
                case State.Start:
                    _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
                    break;
                case State.Options:
                    foreach (Team team in teams)
                    {
                        team.Draw(_spriteBatch);
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            boardClues[x, y].Draw(_spriteBatch);
                        }
                    }
                    _spriteBatch.Draw(
                        star,
                        new Rectangle(currentTeam.Position.Right - 10, currentTeam.Position.Top - 9, 20, 18),
                        Color.White);

                    _spriteBatch.DrawString(
                        font,
                        categories[0],
                        new Vector2(boardClues[0, 0].Position.Center.X, boardClues[0, 0].Position.Center.Y - 40),
                        Color.White,
                        0f,
                        new Vector2(font.MeasureString(categories[0]).X / 2, font.MeasureString(categories[0]).Y / 2),
                        1f,
                        SpriteEffects.None,
                        0f);

                    _spriteBatch.DrawString(
                        font,
                        categories[1],
                        new Vector2(boardClues[4, 1].Position.Center.X, boardClues[4, 1].Position.Center.Y + 40),
                        Color.White,
                        0f,
                        new Vector2(font.MeasureString(categories[1]).X / 2, font.MeasureString(categories[1]).Y / 2),
                        1f,
                        SpriteEffects.None,
                        0f);

                    _spriteBatch.DrawString(
                        font,
                        categories[2],
                        new Vector2(boardClues[0, 2].Position.Center.X, boardClues[0, 2].Position.Center.Y - 40),
                        Color.White,
                        0f,
                        new Vector2(font.MeasureString(categories[2]).X / 2, font.MeasureString(categories[2]).Y / 2),
                        1f,
                        SpriteEffects.None,
                        0f);

                    _spriteBatch.DrawString(
                        font,
                        categories[3],
                        new Vector2(boardClues[4, 3].Position.Center.X, boardClues[4, 3].Position.Center.Y + 40),
                        Color.White,
                        0f,
                        new Vector2(font.MeasureString(categories[3]).X / 2, font.MeasureString(categories[3]).Y / 2),
                        1f,
                        SpriteEffects.None,
                        0f);

                    _spriteBatch.DrawString(
                        font,
                        categories[4],
                        new Vector2(boardClues[0, 4].Position.Center.X, boardClues[0, 4].Position.Center.Y - 40),
                        Color.White,
                        0f,
                        new Vector2(font.MeasureString(categories[4]).X / 2, font.MeasureString(categories[4]).Y / 2),
                        1f,
                        SpriteEffects.None,
                        0f);


                    break;
                case State.Question:
                    _spriteBatch.Draw(questionBackground, questionBackgroundRectangle, Color.White);
                    _spriteBatch.DrawString(
                        font,
                        currentQuestion,
                        new Vector2((_graphics.PreferredBackBufferWidth - font.MeasureString(currentQuestion).X) / 2, (_graphics.PreferredBackBufferHeight - font.MeasureString(currentQuestion).Y) / 2),
                        Color.White);

                    // this should show the team and their current points
                    _spriteBatch.DrawString(font, currentTeam.Name + " " + currentTeam.Score, new Vector2((_graphics.PreferredBackBufferWidth - font.MeasureString(currentTeam.Name + " " + currentTeam.Score).X) / 2, 20), Color.White);

                    if (isRevealed)
                    {
                        _spriteBatch.Draw(
                            answerSprite,
                            answerRectangle,
                            new Rectangle(0, 0, 150, 50),
                            Color.White);

                        _spriteBatch.DrawString(
                            font,
                            currentAnswer,
                            new Vector2(answerRectangle.Center.X, answerRectangle.Center.Y),
                            Color.White,
                            0f,
                            new Vector2(font.MeasureString(currentAnswer.ToString()).X / 2, font.MeasureString(currentAnswer.ToString()).Y / 2),
                            1f,
                            SpriteEffects.None,
                            0f);

                        _spriteBatch.Draw(
                            addButton,
                            addRectangle,
                            new Rectangle(0, 0, 30, 30),
                            Color.White);

                        _spriteBatch.Draw(
                            subtractButton,
                            subtractRectangle,
                            new Rectangle(0, 0, 30, 30),
                            Color.White);

                        _spriteBatch.DrawString(
                            font,
                            pointUpdate,
                            new Vector2(questionBackgroundRectangle.Center.X, questionBackgroundRectangle.Center.Y + 200),
                            Color.White,
                            0f,
                            new Vector2(font.MeasureString(pointUpdate).X / 2, font.MeasureString(pointUpdate).Y / 2),
                            1f,
                            SpriteEffects.None,
                            0f);
                    }

                    _spriteBatch.Draw(revealButton, flipToReveal, revealRectangle, Color.White);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void SelectQuestion(string question, int price, string answer)
        {
            currentQuestion = question;
            currentPointValue = price;
            currentAnswer = answer;
            currentState = State.Question;
        }

        protected void SelectTeam(Team team)
        {
            currentTeam = team;
        }

    }
}
