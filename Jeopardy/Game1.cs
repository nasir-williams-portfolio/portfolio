using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Question currQuestion;
        private State currState;
        private SpriteFont font;

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
            currState = State.Start;
            font = Content.Load<SpriteFont>("arial12");
            currQuestion = new Question("What day is it", font, new Vector2(400, 240));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                currState = State.Options;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                currState = State.Start;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                currState = State.Question;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (currState)
            {
                case State.Start:
                    _spriteBatch.DrawString(font, "Start Screen", Vector2.Zero, Color.Black);
                    break;
                case State.Options:
                    _spriteBatch.DrawString(font, "Options Screen", Vector2.Zero, Color.Black);
                    break;
                case State.Question:
                    _spriteBatch.DrawString(font, "Question Screen", Vector2.Zero, Color.Black);
                    currQuestion.Draw(_spriteBatch);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
