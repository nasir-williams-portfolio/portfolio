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
        private SpriteFont placeholderFont;
        private int correctAssessments;
        private int incorrectAssessments;
        private bool showNotepad;
        private Button notepadButton;

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

            placeholderFont = Content.Load<SpriteFont>("arial12");
            placeholderTexture = Content.Load<Texture2D>("pixel");
            acceptButton = new Button(placeholderTexture, new Vector2(288, 430), Color.Green, 75, 25);
            acceptButton.OnButtonClick += AcceptApplication;
            denyButton = new Button(placeholderTexture, new Vector2(438, 430), Color.Red, 75, 25);
            denyButton.OnButtonClick += DenyApplication;
            applicationQueue = new Queue();
            notepadButton = new Button(placeholderTexture, new Vector2(770, 5), Color.White, 25, 25);
            notepadButton.OnButtonClick += ToggleNotepad;

            applicationQueue.Enqueue(new Application(placeholderTexture));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            notepadButton.Update();

            if (showNotepad == false)
            {
                currentApplication = (Application)applicationQueue.Peek();

                currentApplication.Update();
                acceptButton.Update();
                denyButton.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            notepadButton.Draw(_spriteBatch);
            _spriteBatch.DrawString(placeholderFont, $"Correct: {correctAssessments}\nIncorrect: {incorrectAssessments}", Vector2.Zero, Color.White);
            currentApplication.Draw(_spriteBatch);
            acceptButton.Draw(_spriteBatch);
            denyButton.Draw(_spriteBatch);

            if (showNotepad)
            {
                _spriteBatch.Draw(placeholderTexture, new Rectangle(150, 90, 500, 300), Color.White);
                _spriteBatch.DrawString(placeholderFont, $"If red, deny entry\nIf green, grant entry", new Vector2(155, 95), Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AcceptApplication()
        {
            if (currentApplication.IsCorrupted)
            {
                incorrectAssessments++;
            }
            else
            {
                correctAssessments++;
            }

            applicationQueue.Enqueue(new Application(placeholderTexture));
            applicationQueue.Dequeue();
        }

        protected void DenyApplication()
        {
            if (currentApplication.IsCorrupted)
            {
                correctAssessments++;
            }
            else
            {
                incorrectAssessments++;
            }

            applicationQueue.Enqueue(new Application(placeholderTexture));
            applicationQueue.Dequeue();
        }

        protected void ToggleNotepad()
        {
            showNotepad = !showNotepad;
        }
    }
}
