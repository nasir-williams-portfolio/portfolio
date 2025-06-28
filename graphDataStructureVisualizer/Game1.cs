using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace graphDataStructureVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D up_arrow_key;
        private Texture2D one_by_one;
        private Texture2D verticle_one_by_three;
        private Texture2D horizontal_one_by_two;
        private Texture2D horizontal_one_by_three;

        private Button button_array;

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

            up_arrow_key = Content.Load<Texture2D>("up_arrow_key");
            one_by_one = Content.Load<Texture2D>("one_by_one");
            verticle_one_by_three = Content.Load<Texture2D>("verticle_one_by_three");
            horizontal_one_by_two = Content.Load<Texture2D>("horizontal_one_by_two");
            horizontal_one_by_three = Content.Load<Texture2D>("horizontal_one_by_three");

            button_array = new Button[8];
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
            base.Draw(gameTime);
        }
    }
}
