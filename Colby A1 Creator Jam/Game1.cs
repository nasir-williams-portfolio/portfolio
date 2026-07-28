using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Colby_A1_Creator_Jam
{
    public enum GameState
    {
        TitleScreen,
        GameplayScreen,
        EndScreen
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;

        private Texture2D raccoonSprite;
        private Texture2D daggerSprite;
        private SpriteFont arial12;

        private Protagonist petRaccoon;
        private Collideable[] daggers;
        private GameState gameState;

        private string titleScreenText;
        private string losingText;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //_graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = currKeyboardState;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreenText = "Welcome to\nCOLBY A1 COMMUNITY GAME!";
            losingText = "You Lost\nWhomp Whomp\n\nPress enter to return to start screen";

            raccoonSprite = Content.Load<Texture2D>("halloween_raccoon");
            daggerSprite = Content.Load<Texture2D>("Dagger");
            arial12 = Content.Load<SpriteFont>("arial12");

            petRaccoon = new Protagonist(raccoonSprite);
            daggers = new Collideable[5];

            for (int i = 0; i < daggers.Length; i++)
            {
                daggers[i] = new Collideable(daggerSprite);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currKeyboardState = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.TitleScreen:
                    if (currKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.GameplayScreen;
                    }
                    break;
                case GameState.GameplayScreen:
                    if (petRaccoon.IsDead == true && currKeyboardState.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
                    {
                        petRaccoon.IsDead = false;
                    }

                    petRaccoon.Update(gameTime);
                    foreach (Collideable dagger in daggers)
                    {
                        dagger.Update();
                    }

                    foreach (Collideable dagger in daggers)
                    {
                        if (petRaccoon.DestinationRectangle.Contains(dagger.DestinationRectangle))
                        {
                            petRaccoon.IsDead = true;
                            gameState = GameState.EndScreen;
                        }
                    }
                    break;
                case GameState.EndScreen:
                    if (currKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.TitleScreen;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            switch (gameState)
            {
                case GameState.TitleScreen:
                    _spriteBatch.DrawString(
                        arial12,
                        titleScreenText,
                        new Vector2(400 - (arial12.MeasureString(titleScreenText).X / 2), 240 - (arial12.MeasureString(titleScreenText).Y / 2)),
                        Color.White);
                    break;
                case GameState.GameplayScreen:
                    petRaccoon.Draw(_spriteBatch);

                    foreach (Collideable dagger in daggers)
                    {
                        dagger.Draw(_spriteBatch);
                    }
                    break;
                case GameState.EndScreen:
                    _spriteBatch.DrawString(
                        arial12,
                        losingText,
                        new Vector2(400 - arial12.MeasureString(losingText).X / 2, 240 - (arial12.MeasureString(losingText).Y / 2)),
                        Color.White);
                    break;
                default:
                    break;
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
