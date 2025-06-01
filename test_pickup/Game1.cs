using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace test_pickup
{
    public enum GameState
    {
        MainMenu,
        OptionsMenu,
        PauseMenu,
        Level
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D player_spritesheet;
        private Texture2D item_spritesheet;
        private Texture2D key_spritesheet;
        private Texture2D tile_spritesheet;
        private Texture2D cursor_sprite;
        private Texture2D button_spritesheet;
        private Texture2D volume_buttons_spritesheet;

        private SoundEffectInstance sfx;
        private SoundEffect collect_sfx_one;
        private SoundEffect collect_sfx_two;
        private SoundEffect walk_sfx;
        private Song song;
        private SoundEffect[] sound_effects;

        private SpriteFont font;

        private Character player;
        private List<Pickup> fruits;
        private Tile[,] map;
        private Cursor cursor;

        private Button play_button;
        private Button pause_button;
        private Button exit_button;
        private Button options_button;
        private Button continue_button;
        private Button back_button;

        private GameState curr_state;
        private Stack<GameState> state_history;

        private KeyboardState currKbState;
        private KeyboardState prevKbState;

        private Random rng;
        private int fruit_count;
        private bool toggleDebug;
        private const string TitleBar = "Fruit Collector";
        public static int scale;
        public int window_width;
        public int window_height;
        private int fruit_capacity;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            currKbState = prevKbState;
            fruit_count = 0;
            toggleDebug = false;
            Window.Title = TitleBar;
            curr_state = GameState.MainMenu;
            state_history = new Stack<GameState>();
            window_height = _graphics.PreferredBackBufferHeight;
            window_width = _graphics.PreferredBackBufferWidth;
            scale = window_width / 400;
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
            button_spritesheet = Content.Load<Texture2D>("ui-large-buttons-horizontal");
            volume_buttons_spritesheet = Content.Load<Texture2D>("volume-large-buttons-horizontal");

            font = Content.Load<SpriteFont>("daydream_8");

            collect_sfx_one = Content.Load<SoundEffect>("446129__justinvoke__collect-1");
            collect_sfx_two = Content.Load<SoundEffect>("446134__justinvoke__collect-2");
            walk_sfx = Content.Load<SoundEffect>("326543__sqeeeek__wetfootsteps");
            song = Content.Load<Song>("[no copyright music] 'Taiyaki' cute background music");
            sound_effects = [collect_sfx_one, collect_sfx_two];
            sfx = walk_sfx.CreateInstance();
            sfx.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;

            player = new Character(player_spritesheet, _graphics, 12, 4);
            fruits = new List<Pickup>();
            rng = new Random();
            cursor = new Cursor(cursor_sprite);

            // can probably turn this into a method that populates an array of button objects
            play_button = new Button(
                button_spritesheet,
                new Vector2(
                    window_width / 2,
                    window_height / 2),
                ButtonStates.Play,
                6,
                2);
            continue_button = new Button(
                button_spritesheet,
                new Vector2(
                    window_width / 2,
                    window_height / 2),
                ButtonStates.Continue,
                6,
                2);
            options_button = new Button(
                button_spritesheet,
                new Vector2(
                    window_width / 2,
                    play_button.Y + Button.Height + 2 * Game1.scale),
                ButtonStates.Options,
                6,
                2);
            exit_button = new Button(
                button_spritesheet,
                new Vector2(
                    (window_width / 2),
                    options_button.Y + Button.Height + 2 * Game1.scale),
                ButtonStates.Quit,
                6,
                2);
            pause_button = new Button(
                button_spritesheet,
                new Vector2(
                    window_width / 2,
                    10 * Game1.scale),
                ButtonStates.Pause,
                6,
                2);
            back_button = new Button(
                button_spritesheet,
                new Vector2(
                    window_width / 2,
                    exit_button.Y - Button.Height + 2 * Game1.scale),
                ButtonStates.Back,
                6,
                2);

            play_button.OnButtonClick += NavigateToLevel;
            continue_button.OnButtonClick += NavigateToLevel;
            pause_button.OnButtonClick += NavigateToPauseMenu;
            exit_button.OnButtonClick += NavigateToExit;
            options_button.OnButtonClick += NavigateToOptionsMenu;
            back_button.OnButtonClick += NavigateToPreviousMenu;

            map = new Tile[_graphics.PreferredBackBufferHeight / 15, _graphics.PreferredBackBufferWidth / 16];
            int[] column = { 3, 5, 7, 3, 3, 5, 3, 3, 3, 3, 3, 3 };

            // populate the map array with a random assortment of grass tiles; could probably be a method
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y] = new Tile(tile_spritesheet, new Vector2(scale * 16 * (y), scale * 16 * (x)), 0, column[rng.Next(0, column.Length)]);
                }
            }

            // populate the fruit list with a random number and assortment of pickup type objects; could definately be a method
            fruit_capacity = rng.Next(10, 26);
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
                base.Exit();
            currKbState = Keyboard.GetState();
            cursor.Update();

            switch (curr_state)
            {
                case GameState.MainMenu:
                    play_button.Update();
                    options_button.Update();
                    exit_button.Update();

                    MediaPlayer.Stop();
                    break;
                case GameState.OptionsMenu:
                    back_button.Update();
                    exit_button.Update();

                    MediaPlayer.Stop();
                    break;
                case GameState.PauseMenu:
                    continue_button.Update();
                    options_button.Update();
                    exit_button.Update();

                    MediaPlayer.Stop();
                    break;
                case GameState.Level:
                    pause_button.Update();
                    player.Update(gameTime);

                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(song);
                    }

                    for (int i = 0; i < fruits.Count; i++)
                    {
                        fruits[i].Colliding = fruits[i].Bounds.Intersects(player.Bounds);

                        if (fruits[i].Colliding && currKbState.IsKeyDown(Keys.E) && !prevKbState.IsKeyDown(Keys.E))
                        {
                            fruit_count++;
                            sound_effects[rng.Next(0, 2)].Play();
                            fruits.Remove(fruits[i]);
                        }
                    }

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

                    if (currKbState.IsKeyDown(Keys.P) && !prevKbState.IsKeyDown(Keys.P))
                    {
                        toggleDebug = !toggleDebug;
                    }
                    break;
                default:
                    break;
            }

            prevKbState = currKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            switch (curr_state)
            {
                case GameState.MainMenu:
                    play_button.Draw(_spriteBatch);
                    options_button.Draw(_spriteBatch);
                    exit_button.Draw(_spriteBatch);
                    break;
                case GameState.OptionsMenu:

                    back_button.Draw(_spriteBatch);
                    break;
                case GameState.PauseMenu:

                    continue_button.Draw(_spriteBatch);
                    options_button.Draw(_spriteBatch);
                    exit_button.Draw(_spriteBatch);
                    break;
                case GameState.Level:
                    // both of these foreach loops could probably be a single method
                    foreach (Tile tile in map)
                    {
                        tile.Draw(_spriteBatch);
                    }

                    foreach (Pickup fruit in fruits)
                    {
                        fruit.Draw(_spriteBatch);
                    }

                    player.Draw(_spriteBatch);

                    if (toggleDebug)
                    {
                        foreach (Pickup fruit in fruits)
                        {
                            DebugLib.DrawRectOutline(_spriteBatch, fruit.Bounds, 1, Color.Black);
                        }

                        DebugLib.DrawRectOutline(_spriteBatch, player.Bounds, 1, Color.Black);

                        _spriteBatch.DrawString(font, $"DEBUG ACTIVATED", new Vector2(1, 16), Color.Black);
                    }

                    pause_button.Draw(_spriteBatch);
                    _spriteBatch.DrawString(font, $"Fruits Collected: {fruit_count} / {fruit_capacity}", new Vector2(1, 1), Color.Black);

                    break;
                default:
                    break;
            }

            _spriteBatch.DrawString(
                font,
                $"Currrent State: {curr_state}",
                new Vector2(1, _graphics.PreferredBackBufferHeight - font.MeasureString($"Currrent State: {curr_state}").Y),
                Color.Black);
            cursor.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void NavigateToLevel()
        {
            state_history.Push(curr_state);
            curr_state = GameState.Level;
        }

        protected void NavigateToPauseMenu()
        {
            state_history.Push(curr_state);
            curr_state = GameState.PauseMenu;
        }

        protected void NavigateToOptionsMenu()
        {
            state_history.Push(curr_state);
            curr_state = GameState.OptionsMenu;
        }

        protected void NavigateToExit()
        {
            if (curr_state == GameState.MainMenu)
            {
                base.Exit();
            }

            else
            {
                curr_state = GameState.MainMenu;
            }
        }

        protected void NavigateToPreviousMenu()
        {
            curr_state = state_history.Pop();
        }

        protected void MuteAudio()
        {
            sfx.Volume = 0f;
        }

        protected void UnMuteAudio()
        {

        }
    }
}
