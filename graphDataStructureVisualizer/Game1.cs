using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace graphDataStructureVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D directional_thumbsticks;
        private Texture2D one_by_one;
        private Texture2D verticle_one_by_two;
        private Texture2D verticle_one_by_three;
        private Texture2D horizontal_one_by_two;
        private Texture2D horizontal_one_by_three;

        private Vertex currentVertex;

        private Button[] buttonArray;
        private Direction movementDirection;
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
            movementDirection = Direction.North;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            vertices = new List<Vertex>();
            adjacencyDictionary = new Dictionary<string, Dictionary<Vertex, Direction>>();
            buttonArray = new Button[8];

            #region textures
            directional_thumbsticks = Content.Load<Texture2D>("directional_thumbsticks");
            one_by_one = Content.Load<Texture2D>("one_by_one");
            verticle_one_by_two = Content.Load<Texture2D>("verticle_one_by_two");
            verticle_one_by_three = Content.Load<Texture2D>("verticle_one_by_three");
            horizontal_one_by_two = Content.Load<Texture2D>("horizontal_one_by_two");
            horizontal_one_by_three = Content.Load<Texture2D>("horizontal_one_by_three");
            #endregion

            #region buttons
            buttonArray[0] = new Button(directional_thumbsticks, new Vector2(200, 212), Direction.North);
            buttonArray[1] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X - 18, buttonArray[0].Y + 00), Direction.NorthWest);
            buttonArray[2] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X + 18, buttonArray[0].Y + 00), Direction.NorthEast);

            buttonArray[3] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X + 18, buttonArray[0].Y + 18), Direction.East);
            buttonArray[4] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X - 18, buttonArray[0].Y + 18), Direction.West);

            buttonArray[5] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X - 18, buttonArray[0].Y + 36), Direction.SouthWest);
            buttonArray[6] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X + 00, buttonArray[0].Y + 36), Direction.South);
            buttonArray[7] = new Button(directional_thumbsticks, new Vector2(buttonArray[0].X + 18, buttonArray[0].Y + 36), Direction.SouthEast);

            foreach (Button btn in buttonArray)
            {
                btn.OnButtonClick += MoveRoom;
            }
            #endregion

            #region vertices
            Vertex kitchen = new Vertex("kitchen", "Large enough to prepare a feast.", verticle_one_by_two, new Vector2(400, 240));
            Vertex dining = new Vertex("dining", "A huge table for sixteen has gold place settings.", horizontal_one_by_two, new Vector2(410, 250));
            Vertex library = new Vertex("library", "This library is packed with floor-to-ceiling bookshelves.", horizontal_one_by_two, new Vector2(410, 240));
            Vertex conservatory = new Vertex("conservatory", "The glass wall allows sunlight to reach the plants here.", horizontal_one_by_two, new Vector2(410, 230));
            Vertex hall = new Vertex("hall", "The main hall is central to the house.", verticle_one_by_three, new Vector2(430, 230));
            Vertex deck = new Vertex("deck", "This covered deck looks over the landscaped grounds.", horizontal_one_by_three, new Vector2(410, 220));
            Vertex exit = new Vertex("exit", "Cobblestone pathway leads you to the gardens.", one_by_one, new Vector2(420, 210));

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

            foreach (Button btn in buttonArray)
            {
                btn.Update();
            }

            System.Diagnostics.Debug.WriteLine($"{Mouse.GetState().X}, {Mouse.GetState().Y}");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

            foreach (Button btn in buttonArray)
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
    }
}
