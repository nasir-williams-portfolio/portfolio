using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace graphDataStructureVisualizer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D up_arrow_key;
        private Texture2D one_by_one;
        private Texture2D verticle_one_by_two;
        private Texture2D verticle_one_by_three;
        private Texture2D horizontal_one_by_two;
        private Texture2D horizontal_one_by_three;

        private Vertex currentVertex;

        private Button[] button_array;
        private List<Vertex> vertices;
        private Dictionary<string, List<Vertex>> adjacencyList;
        private Graph map;

        private List<Vertex> kitchenDoors;
        private List<Vertex> diningDoors;
        private List<Vertex> libraryDoors;
        private List<Vertex> conservatoryDoors;
        private List<Vertex> hallDoors;
        private List<Vertex> deckDoors;
        private List<Vertex> exitDoors;



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

            vertices = new List<Vertex>();
            adjacencyList = new Dictionary<string, List<Vertex>>();
            button_array = new Button[8];

            up_arrow_key = Content.Load<Texture2D>("up_arrow_key");
            one_by_one = Content.Load<Texture2D>("one_by_one");
            verticle_one_by_two = Content.Load<Texture2D>("verticle_one_by_two");
            verticle_one_by_three = Content.Load<Texture2D>("verticle_one_by_three");
            horizontal_one_by_two = Content.Load<Texture2D>("horizontal_one_by_two");
            horizontal_one_by_three = Content.Load<Texture2D>("horizontal_one_by_three");

            button_array[0] = new Button(up_arrow_key, Vector2.Zero, Direction.North);
            button_array[1] = new Button(up_arrow_key, Vector2.Zero, Direction.NorthEast);
            button_array[2] = new Button(up_arrow_key, Vector2.Zero, Direction.East);
            button_array[3] = new Button(up_arrow_key, Vector2.Zero, Direction.SouthEast);
            button_array[4] = new Button(up_arrow_key, Vector2.Zero, Direction.South);
            button_array[5] = new Button(up_arrow_key, Vector2.Zero, Direction.SouthWest);
            button_array[6] = new Button(up_arrow_key, Vector2.Zero, Direction.West);
            button_array[7] = new Button(up_arrow_key, Vector2.Zero, Direction.NorthWest);

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

            kitchenDoors = new List<Vertex>();
            kitchenDoors.Add(dining);
            kitchenDoors.Add(library);

            diningDoors = new List<Vertex>();
            diningDoors.Add(kitchen);
            diningDoors.Add(hall);

            libraryDoors = new List<Vertex>();
            libraryDoors.Add(kitchen);
            libraryDoors.Add(conservatory);

            conservatoryDoors = new List<Vertex>();
            conservatoryDoors.Add(library);
            conservatoryDoors.Add(deck);
            conservatoryDoors.Add(hall);

            hallDoors = new List<Vertex>();
            hallDoors.Add(dining);
            hallDoors.Add(conservatory);
            hallDoors.Add(deck);

            deckDoors = new List<Vertex>();
            deckDoors.Add(exit);
            deckDoors.Add(hall);
            deckDoors.Add(conservatory);

            exitDoors = new List<Vertex>();
            exitDoors.Add(deck);

            adjacencyList.Add("kitchen", kitchenDoors);
            adjacencyList.Add("dining", diningDoors);
            adjacencyList.Add("library", libraryDoors);
            adjacencyList.Add("conservatory", conservatoryDoors);
            adjacencyList.Add("hall", hallDoors);
            adjacencyList.Add("deck", deckDoors);
            adjacencyList.Add("exit", exitDoors);

            map = new Graph(vertices, adjacencyList);

            currentVertex = vertices[0];
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

            foreach (Button btn in button_array)
            {
                btn.Draw(_spriteBatch);
            }

            foreach (Vertex room in vertices)
            {
                if (currentVertex == room)
                {
                    room.Color = Color.LightGreen;
                }
                room.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void MoveRoom()
        {

        }
    }
}
