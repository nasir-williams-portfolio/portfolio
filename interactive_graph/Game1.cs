using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace interactive_graph
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D arrow;
        private Texture2D room_blue;
        private SpriteFont font;

        private Vector2 room_origin;
        private Vector2 arrow_origin;

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

            arrow = Content.Load<Texture2D>("arrow");
            room_blue = Content.Load<Texture2D>("button_blue");
            font = Content.Load<SpriteFont>("daydream_12");

            room_origin = new Vector2(room_blue.Width / 2, room_blue.Height / 2); // room_blue.Width / 2, room_blue.Height / 2
            arrow_origin = new Vector2(arrow.Width / 2, arrow.Height / 2); // arrow.Width / 2, arrow.Height / 2
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

            _spriteBatch.DrawString(
                font,
                $"Room Dimensions: {room_blue.Width}, {room_blue.Height}" +
                $"\nArrow Dimensions: {arrow.Width}, {arrow.Height}",
                new Vector2(10, 10),
                Color.White);

            _spriteBatch.Draw(
                room_blue,
                new Rectangle(
                    400,
                    240,
                    room_blue.Width,
                    room_blue.Height),
                new Rectangle(0, 0, room_blue.Width, room_blue.Height),
                Color.White,
                (float)Math.PI / 2,
                room_origin,
                SpriteEffects.None,
                0f);

            _spriteBatch.Draw(
                arrow,
                new Rectangle(
                    421,
                    225,
                    arrow.Width,
                    arrow.Height),
                new Rectangle(0, 0, arrow.Width, arrow.Height),
                Color.White,
                0f,
                arrow_origin,
                SpriteEffects.None,
                0f);

            _spriteBatch.Draw(
                arrow,
                new Rectangle(
                    421,
                    255,
                    arrow.Width,
                    arrow.Height),
                new Rectangle(0, 0, arrow.Width, arrow.Height),
                Color.White,
                0f,
                arrow_origin,
                SpriteEffects.None,
                0f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
