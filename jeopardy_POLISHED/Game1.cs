using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace jeopardy_POLISHED
{
    public enum GameState
    {
        TitleScreen,
        SettingsScreen,
        QuestionScreen,
        BoardScreen,
        LeaderboardScreen
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState currentGameState;

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    break;
                case GameState.SettingsScreen:
                    break;
                case GameState.QuestionScreen:
                    break;
                case GameState.BoardScreen:
                    break;
                case GameState.LeaderboardScreen:
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
                    break;
                case GameState.SettingsScreen:
                    break;
                case GameState.QuestionScreen:
                    break;
                case GameState.BoardScreen:
                    break;
                case GameState.LeaderboardScreen:
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
