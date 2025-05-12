using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_tileset
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D tileset;

        #region tileset items
        private Rectangle drink1;
        private Rectangle drink2;
        private Rectangle drink3;
        private Rectangle drink4;
        private Rectangle drink5;
        private Rectangle note;
        private Rectangle gameboy;
        private Rectangle tableset;
        private Rectangle bread;
        private Rectangle toaster;
        private Rectangle ducky;
        private Rectangle controller;
        private Rectangle monitor;
        private Rectangle book;
        private Rectangle page;
        private Rectangle trashcan;
        private Rectangle computer;
        private Rectangle window_open;
        private Rectangle window_closed;
        private Rectangle tv_front;
        private Rectangle tv_side;
        private Rectangle tv_back;
        private Rectangle chair;
        private Rectangle dresser_1;
        private Rectangle dresser_2;
        private Rectangle flowers_1;
        private Rectangle flowers_2;
        private Rectangle stove;
        private Rectangle cabinet;
        private Rectangle fridge;
        private Rectangle couch;
        private Rectangle toilet;
        private Rectangle sink;
        private Rectangle bath;
        private Rectangle rug_1;
        private Rectangle rug_2;
        private Rectangle laptop;
        private Rectangle radio;
        private Rectangle table_1;
        private Rectangle table_2;
        private Rectangle endTable;
        private Rectangle wardrobe;
        private Rectangle bookshelf;
        private Rectangle bed;
        private Rectangle floor_1;
        private Rectangle floor_2;
        private Rectangle floor_3;
        private Rectangle floor_4;
        private Rectangle floor_5;
        private Rectangle floor_6;
        #endregion

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

            tileset = Content.Load<Texture2D>("prototype_tileset");
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

            _spriteBatch.Begin();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
