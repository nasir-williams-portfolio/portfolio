using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private Texture2D cursor_sprite;

        private SoundEffect soundEffect;
        private SoundEffect walk_sound;
        private Song song;
        private SpriteFont font;

        private SoundEffectInstance sfx;

        private Character player;
        private List<Pickup> fruits;
        private Tile[,] map;
        private Cursor cursor;

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        private Random rng;
        private int fruit_count;
        private bool toggleDebug;
        private const string TitleBar = "Fruit Collector";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            currKbState = prevKbState;
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            fruit_count = 0;
            toggleDebug = false;
            Window.Title = TitleBar;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player_spritesheet = Content.Load<Texture2D>("Prototype_Character_Red");
            item_spritesheet = Content.Load<Texture2D>("TheBanquet_SpriteAtlas_Master");
            key_spritesheet = Content.Load<Texture2D>("Keyboard Letters and Symbols");
            tile_spritesheet = Content.Load<Texture2D>("zeo254-completed-commission");
            cursor_sprite = Content.Load<Texture2D>("tile_0200");

            font = Content.Load<SpriteFont>("daydream_8");

            soundEffect = Content.Load<SoundEffect>("446129__justinvoke__collect-1");
            walk_sound = Content.Load<SoundEffect>("326543__sqeeeek__wetfootsteps");
            song = Content.Load<Song>("[no copyright music] 'Taiyaki' cute background music");
            sfx = walk_sound.CreateInstance();
            sfx.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
            MediaPlayer.Play(song);

            player = new Character(player_spritesheet, _graphics);
            fruits = new List<Pickup>();
            rng = new Random();
            cursor = new Cursor(cursor_sprite);

            map = new Tile[_graphics.PreferredBackBufferHeight / 15, _graphics.PreferredBackBufferWidth / 16];
            int[] column = { 3, 5, 7, 3, 3, 5, 3, 3, 3, 3, 3, 3 };

            // populate the map array with a random assortment of grass tiles
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y] = new Tile(tile_spritesheet, new Vector2(16 * (y), 16 * (x)), 0, column[rng.Next(0, column.Length)]);
                }
            }

            // populate the fruit list with a random number and assortment of pickup type objects
            int fruit_capacity = rng.Next(10, 26);
            for (int i = 0; i < fruit_capacity; i++)
            {
                fruits.Add(new Pickup(
                    item_spritesheet,
                    key_spritesheet,
                    new Vector2(rng.Next(231, _graphics.PreferredBackBufferWidth - 20), rng.Next(15, _graphics.PreferredBackBufferHeight - 20))));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            currKbState = Keyboard.GetState();

            Collide();

            foreach (Pickup fruit in fruits)
            {
                if (fruit.Colliding == true && currKbState.IsKeyDown(Keys.E) && !prevKbState.IsKeyDown(Keys.E))
                {
                    fruit.Scale = 0;
                    fruit_count++;
                    soundEffect.Play();
                }
            }

            if (currKbState.IsKeyDown(Keys.P) && !prevKbState.IsKeyDown(Keys.P))
            {
                toggleDebug = !toggleDebug;
            }

            cursor.Update();

            if (currKbState.IsKeyDown(Keys.W) || currKbState.IsKeyDown(Keys.A) || currKbState.IsKeyDown(Keys.S) || currKbState.IsKeyDown(Keys.D))
            {
                if (currKbState.IsKeyDown(Keys.LeftShift))
                {
                    sfx.Pitch = 1;
                }

                else
                {
                    sfx.Pitch = 0;
                }

                sfx.Play();
            }

            else
            {
                sfx.Stop();
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

            foreach (Pickup fruit in fruits)
            {
                fruit.Draw(_spriteBatch);
            }

            player.Draw(_spriteBatch);

            _spriteBatch.DrawString(font, $"Fruits Collected: {fruit_count} / {fruits.Count}", new Vector2(1, 1), Color.Black);

            if (toggleDebug)
            {
                foreach (Pickup fruit in fruits)
                {
                    DebugLib.DrawRectOutline(_spriteBatch, fruit.Bounds, 1, Color.Black);
                }

                DebugLib.DrawRectOutline(_spriteBatch, player.Bounds, 1, Color.Black);

                _spriteBatch.DrawString(font, $"DEBUG ACTIVATED", new Vector2(1, 16), Color.Black);
            }

            cursor.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Collide()
        {
            foreach (Pickup fruit in fruits)
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
