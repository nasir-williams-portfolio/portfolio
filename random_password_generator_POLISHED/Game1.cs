using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace random_password_generator_POLISHED
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D characters;
        private Texture2D menu_spritesheet;
        private Rectangle source_rectangle;
        private Vector2 scale;

        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            source_rectangle = new Rectangle(0, 0, 100, 60);
            fps = 4;
            secondsPerFrame = 1 / fps;
            timeCounter = 0;

            scale = new Vector2(_graphics.PreferredBackBufferWidth / source_rectangle.Width, _graphics.PreferredBackBufferHeight / source_rectangle.Height);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            characters = Content.Load<Texture2D>("ui letters and numbers spritesheet");
            menu_spritesheet = Content.Load<Texture2D>("application main menu spritesheet");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeCounter >= secondsPerFrame)
            {
                source_rectangle.X += 100;
                if (source_rectangle.X >= 300)
                {
                    source_rectangle.X = 0;
                }
                timeCounter -= secondsPerFrame;
            }

            _spriteBatch.Draw(
                menu_spritesheet,
                new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                source_rectangle,
                Color.White,
                0f,
                new Vector2(((menu_spritesheet.Width / 4) / 2), menu_spritesheet.Height / 2),
                scale,
                SpriteEffects.None,
                0f);

            Phrase.Draw(
                _spriteBatch,
                characters,
                Phrase.TranslateString("welcome to", characters),
                new Vector2(400, 253),
                true);

            Phrase.Draw(
                _spriteBatch,
                characters,
                Phrase.TranslateString("random password", characters),
                new Vector2(400, 266),
                true);

            Phrase.Draw(
                _spriteBatch,
                characters,
                Phrase.TranslateString("generator", characters),
                new Vector2(400, 279),
                true);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
