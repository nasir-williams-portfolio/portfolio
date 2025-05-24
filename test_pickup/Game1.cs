using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace test_pickup
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D player_spritesheet;
        private Texture2D item_spritesheet;
        private Texture2D key_spritesheet;
        private Texture2D tile_spritesheet;

        private Character player;

        private List<Pickup> oranges;
        private Tile[,] map;

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        private Random rng;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currKbState = prevKbState;
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player_spritesheet = Content.Load<Texture2D>("Prototype_Character_Red");
            item_spritesheet = Content.Load<Texture2D>("SpriteAtlas_TheBanquet_pt1_Outline_BigWander");
            key_spritesheet = Content.Load<Texture2D>("Keyboard Letters and Symbols");
            tile_spritesheet = Content.Load<Texture2D>("zeo254-completed-commission");

            player = new Character(player_spritesheet);
            oranges = new List<Pickup>();
            rng = new Random();

            map = new Tile[125, 125];

            int[] column = { 3, 5, 7, 3, 3, 5, 3, 3, 3, 3, 3, 3 };

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y] = new Tile(tile_spritesheet, new Vector2(16 * (y), 16 * (x)), 0, column[rng.Next(0, column.Length)]);
                }
            }

            for (int i = 0; i < 15; i++)
            {
                oranges.Add(new Pickup(
                    item_spritesheet,
                    key_spritesheet,
                    new Vector2(rng.Next(1, 125) * 16, rng.Next(1, 125) * 16)));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            currKbState = Keyboard.GetState();

            Collide();

            foreach (Pickup fruit in oranges)
            {
                if (fruit.Colliding == true && currKbState.IsKeyDown(Keys.E) && !prevKbState.IsKeyDown(Keys.E))
                {
                    fruit.Scale = 0;
                }
            }

            prevKbState = currKbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            foreach (Tile tile in map)
            {
                tile.Draw(_spriteBatch);
            }

            foreach (Pickup fruit in oranges)
            {
                fruit.Draw(_spriteBatch);
                //DebugLib.DrawRectOutline(_spriteBatch, fruit.Bounds, 1f, Color.Red);
            }
            player.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Collide()
        {
            foreach (Pickup fruit in oranges)
            {
                if (fruit.Bounds.Intersects(player.Bounds))
                {
                    fruit.Colliding = true;
                }

                else
                {
                    fruit.Colliding = false;
                }
            }
        }
    }
}
