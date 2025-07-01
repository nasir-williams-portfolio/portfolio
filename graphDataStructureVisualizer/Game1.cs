using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

/*
 * https://otterisk.itch.io/hana-caraka-fantasy-interior - link for the room spritesheet
 */

public enum GameState
{
    TitleScreen,
    GameScreen
}

namespace graphDataStructureVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D compass_needles;
        private Texture2D asset_map;
        private Texture2D single_pixel;
        private Texture2D ui_buttons;
        private Texture2D title_screen_menu;

        private Vertex currentVertex;
        private GameState currState;
        private UserInterfaceButton playButton;
        private UserInterfaceButton quitButton;

        private TraversalButton[] buttonArray;
        private List<Vertex> vertices;
        private Dictionary<string, Dictionary<Vertex, Direction>> adjacencyDictionary;
        private Graph map;

        private Dictionary<Vertex, Direction> kitchenDoors;
        private Dictionary<Vertex, Direction> diningDoors;
        private Dictionary<Vertex, Direction> libraryDoors;
        private Dictionary<Vertex, Direction> conservatoryDoors;
        private Dictionary<Vertex, Direction> hallDoors;
        private Dictionary<Vertex, Direction> deckDoors;
        private Dictionary<Vertex, Direction> exitDoors;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            currState = GameState.TitleScreen;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            vertices = new List<Vertex>();
            adjacencyDictionary = new Dictionary<string, Dictionary<Vertex, Direction>>();
            buttonArray = new TraversalButton[8];

            #region textures
            compass_needles = Content.Load<Texture2D>("compass_needles");
            asset_map = Content.Load<Texture2D>("map");
            single_pixel = Content.Load<Texture2D>("single_pixel");
            title_screen_menu = Content.Load<Texture2D>("title_screen_menu");
            ui_buttons = Content.Load<Texture2D>("ui_buttons");
            #endregion

            #region buttons
            buttonArray[0] = new TraversalButton(compass_needles, new Vector2(200, 212), Direction.North);
            buttonArray[1] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X - 16, buttonArray[0].Y + 00), Direction.NorthWest);
            buttonArray[2] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X + 16, buttonArray[0].Y + 00), Direction.NorthEast);

            buttonArray[3] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X + 16, buttonArray[0].Y + 16), Direction.East);
            buttonArray[4] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X - 16, buttonArray[0].Y + 16), Direction.West);

            buttonArray[5] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X - 16, buttonArray[0].Y + 32), Direction.SouthWest);
            buttonArray[6] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X + 00, buttonArray[0].Y + 32), Direction.South);
            buttonArray[7] = new TraversalButton(compass_needles, new Vector2(buttonArray[0].X + 16, buttonArray[0].Y + 32), Direction.SouthEast);

            playButton = new UserInterfaceButton(ui_buttons, new Vector2(354, 250), ButtonUse.Play);
            quitButton = new UserInterfaceButton(ui_buttons, new Vector2(354, 290), ButtonUse.Quit);

            playButton.OnButtonClick += ScreenTransition;
            quitButton.OnButtonClick += Exit;

            foreach (TraversalButton btn in buttonArray)
            {
                btn.OnButtonClick += MoveRoom;
            }
            #endregion

            #region vertices
            Vertex kitchen = new("kitchen", "Large enough to prepare a feast.", single_pixel, new Rectangle(306, 291, 74, 175));
            Vertex dining = new("dining", "A huge table for sixteen has gold place settings.", single_pixel, new Rectangle(386, 383, 150, 83));
            Vertex library = new("library", "This library is packed with floor-to-ceiling bookshelves.", single_pixel, new Rectangle(386, 291, 150, 83));
            Vertex conservatory = new("conservatory", "The glass wall allows sunlight to reach the plants here.", single_pixel, new Rectangle(386, 199, 150, 83));
            Vertex hall = new("hall", "The main hall is central to the house.", single_pixel, new Rectangle(542, 199, 74, 267));
            Vertex deck = new("deck", "This covered deck looks over the landscaped grounds.", single_pixel, new Rectangle(386, 107, 230, 83));
            Vertex exit = new("exit", "Cobblestone pathway leads you to the gardens.", single_pixel, new Rectangle(473, 15, 50, 83));

            vertices.Add(kitchen);
            vertices.Add(dining);
            vertices.Add(library);
            vertices.Add(conservatory);
            vertices.Add(hall);
            vertices.Add(deck);
            vertices.Add(exit);
            #endregion

            #region adjacencies
            kitchenDoors = new Dictionary<Vertex, Direction>();
            kitchenDoors.Add(dining, Direction.SouthEast);
            kitchenDoors.Add(library, Direction.NorthEast);

            diningDoors = new Dictionary<Vertex, Direction>();
            diningDoors.Add(kitchen, Direction.West);
            diningDoors.Add(hall, Direction.East);

            libraryDoors = new Dictionary<Vertex, Direction>();
            libraryDoors.Add(kitchen, Direction.West);
            libraryDoors.Add(conservatory, Direction.North);

            conservatoryDoors = new Dictionary<Vertex, Direction>();
            conservatoryDoors.Add(library, Direction.South);
            conservatoryDoors.Add(deck, Direction.North);
            conservatoryDoors.Add(hall, Direction.East);

            hallDoors = new Dictionary<Vertex, Direction>();
            hallDoors.Add(dining, Direction.SouthWest);
            hallDoors.Add(conservatory, Direction.NorthWest);
            hallDoors.Add(deck, Direction.North);

            deckDoors = new Dictionary<Vertex, Direction>();
            deckDoors.Add(exit, Direction.North);
            deckDoors.Add(hall, Direction.SouthEast);
            deckDoors.Add(conservatory, Direction.SouthWest);

            exitDoors = new Dictionary<Vertex, Direction>();
            exitDoors.Add(deck, Direction.South);

            adjacencyDictionary.Add("kitchen", kitchenDoors);
            adjacencyDictionary.Add("dining", diningDoors);
            adjacencyDictionary.Add("library", libraryDoors);
            adjacencyDictionary.Add("conservatory", conservatoryDoors);
            adjacencyDictionary.Add("hall", hallDoors);
            adjacencyDictionary.Add("deck", deckDoors);
            adjacencyDictionary.Add("exit", exitDoors);
            #endregion

            map = new Graph(vertices, adjacencyDictionary);

            currentVertex = kitchen;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currState)
            {
                case GameState.TitleScreen:
                    playButton.Update();
                    quitButton.Update();
                    break;
                case GameState.GameScreen:
                    foreach (TraversalButton btn in buttonArray)
                    {
                        btn.Update();
                    }
                    break;
                default:
                    break;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51, 60, 58, 1));

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            switch (currState)
            {
                case GameState.TitleScreen:
                    _spriteBatch.Draw(title_screen_menu, new Rectangle(175, 120, title_screen_menu.Width * 3, title_screen_menu.Height * 3), Color.White);
                    playButton.Draw(_spriteBatch);
                    quitButton.Draw(_spriteBatch);
                    break;
                case GameState.GameScreen:
                    _spriteBatch.Draw(asset_map, new Vector2(300, 6), Color.White);

                    foreach (TraversalButton btn in buttonArray)
                    {
                        if (map.GetAdjacentDictionary(currentVertex.Name).Values.Contains(btn.Direction))
                        {
                            btn.IsActive = true;
                        }

                        else
                        {
                            btn.IsActive = false;
                        }

                        btn.Draw(_spriteBatch);
                    }

                    foreach (Vertex room in map.Vertices)
                    {
                        if (currentVertex == room)
                        {
                            room.Color = Color.LightGreen;
                        }

                        else if (map.GetAdjacentDictionary(currentVertex.Name).ContainsKey(room))
                        {
                            room.Color = Color.Yellow;
                        }

                        else
                        {
                            room.Color = Color.Red;
                        }

                        room.Draw(_spriteBatch);
                    }
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void MoveRoom(Direction movementDirection)
        {
            foreach (Direction dir in map.GetAdjacentDictionary(currentVertex.Name).Values)
            {
                if (movementDirection == dir)
                {
                    currentVertex = map.GetAdjacentDictionary(currentVertex.Name).FirstOrDefault(x => x.Value == dir).Key;
                }
            }
        }

        protected void ScreenTransition()
        {
            currState = GameState.GameScreen;
        }
    }
}
