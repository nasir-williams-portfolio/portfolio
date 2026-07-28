using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NYT_Tiles_Dupe.Content;

namespace NYT_Tiles_Dupe
{
    public class Game1 : Game
    {


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D tiles;
        private Texture2D cellHighlight;

        private Cell[,] gameBoard;
        private Cell[] selectedPair;
        private bool firstSelection;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            selectedPair = new Cell[2];
            firstSelection = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tiles = Content.Load<Texture2D>("Test_Tiles");
            cellHighlight = Content.Load<Texture2D>("Cell_Highlight");
            gameBoard = new Cell[5, 6];

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    gameBoard[x, y] = new Cell(new Rectangle(0 + (32 * x), 0 + (32 * y), 32, 32), tiles, x, y);
                    gameBoard[x, y].ClickedCell += SelectCells;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (gameBoard[x, y].TilesArray.Count != 0)
                    {
                        gameBoard[x, y].Update();
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();




            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (gameBoard[x, y].TilesArray.Count != 0)
                    {
                        gameBoard[x, y].Draw(_spriteBatch);
                    }
                }
            }

            if (selectedPair[0] != null)
            {
                _spriteBatch.Draw(cellHighlight, selectedPair[0].DestinationRectangle, Color.White);
            }
            if (selectedPair[1] != null)
            {
                _spriteBatch.Draw(cellHighlight, selectedPair[1].DestinationRectangle, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SelectCells(int x, int y)
        {
            if (firstSelection == true)
            {
                selectedPair[0] = gameBoard[x, y];
                firstSelection = false;
            }
            else if (firstSelection == false)
            {
                if (selectedPair[1] == null)
                {
                    selectedPair[1] = gameBoard[x, y];
                }
                else
                {
                    selectedPair[0] = selectedPair[1];
                    selectedPair[1] = gameBoard[x, y];
                }
            }

            CheckPair();
        }

        public bool CheckPair()
        {
            if (selectedPair[0] != null && selectedPair[1] != null)
            {
                foreach (Tile patternTile1 in selectedPair[0].TilesArray)
                {
                    foreach (Tile patternTile2 in selectedPair[1].TilesArray)
                    {
                        if (patternTile1.TilePattern == patternTile2.TilePattern)
                        {
                            System.Diagnostics.Debug.WriteLine($"{patternTile1.TilePattern.ToString()}, {patternTile2.TilePattern.ToString()}");
                            selectedPair[1].TilesArray.Remove(patternTile2);
                            selectedPair[0].TilesArray.Remove(patternTile1);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
