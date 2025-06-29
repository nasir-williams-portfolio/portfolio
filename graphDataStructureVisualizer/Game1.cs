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

        private Button[] button_array;
        private List<Vertex> vertices;

        private Vertex currentVertex;

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

            up_arrow_key = Content.Load<Texture2D>("up_arrow_key");
            one_by_one = Content.Load<Texture2D>("one_by_one");
            verticle_one_by_two = Content.Load<Texture2D>("verticle_one_by_two");
            verticle_one_by_three = Content.Load<Texture2D>("verticle_one_by_three");
            horizontal_one_by_two = Content.Load<Texture2D>("horizontal_one_by_two");
            horizontal_one_by_three = Content.Load<Texture2D>("horizontal_one_by_three");

            button_array = new Button[8];
            button_array[0] = new Button(up_arrow_key, Vector2.Zero, Direction.North);
            button_array[1] = new Button(up_arrow_key, Vector2.Zero, Direction.NorthEast);
            button_array[2] = new Button(up_arrow_key, Vector2.Zero, Direction.East);
            button_array[3] = new Button(up_arrow_key, Vector2.Zero, Direction.SouthEast);
            button_array[4] = new Button(up_arrow_key, Vector2.Zero, Direction.South);
            button_array[5] = new Button(up_arrow_key, Vector2.Zero, Direction.SouthWest);
            button_array[6] = new Button(up_arrow_key, Vector2.Zero, Direction.West);
            button_array[7] = new Button(up_arrow_key, Vector2.Zero, Direction.NorthWest);

            vertices.Add(new Vertex("kitchen", "Large enough to prepare a feast.", verticle_one_by_two, new Vector2(400, 240)));
            vertices.Add(new Vertex("dining", "A huge table for sixteen has gold place settings.", horizontal_one_by_two, new Vector2(410, 250)));
            vertices.Add(new Vertex("library", "This library is packed with floor-to-ceiling bookshelves.", horizontal_one_by_two, new Vector2(410, 240)));
            vertices.Add(new Vertex("conservatory", "The glass wall allows sunlight to reach the plants here.", horizontal_one_by_two, new Vector2(410, 230)));
            vertices.Add(new Vertex("hall", "The main hall is central to the house.", verticle_one_by_three, new Vector2(430, 230)));
            vertices.Add(new Vertex("deck", "This covered deck looks over the landscaped grounds.", horizontal_one_by_three, new Vector2(410, 220)));
            vertices.Add(new Vertex("exit", "Cobblestone pathway leads you to the gardens.", one_by_one, new Vector2(420, 210)));

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
                    room.Color = Color.Green;
                }
                room.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
