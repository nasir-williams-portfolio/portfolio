using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NYT_Tiles_Dupe.Content;
using System;
using System.Collections;

namespace NYT_Tiles_Dupe
{
    public delegate void onCellClickDelegate(int x, int y);

    internal class Cell
    {
        private ArrayList tilesArray;
        private Rectangle destinationRectangle;
        private MouseState currMouseState;
        private MouseState prevMouseState;
        private Rectangle mouseRectangle;

        public event onCellClickDelegate ClickedCell;

        private Random rng;

        private int x;
        private int y;

        public ArrayList TilesArray { get { return tilesArray; } }
        public Rectangle DestinationRectangle { get { return destinationRectangle; } }

        public Cell(Rectangle destinationRectangle, Texture2D sprite, int x, int y)
        {
            this.destinationRectangle = destinationRectangle;

            rng = new Random();
            mouseRectangle = new Rectangle(0, 0, 0, 0);
            currMouseState = Mouse.GetState();
            prevMouseState = currMouseState;
            this.x = x;
            this.y = y;

            tilesArray = new ArrayList();



            // you might get the same pattern twice in one cell, make it so the randomized number can only appear once
            tilesArray.Add(new Tile(rng.Next(0, 6), destinationRectangle, sprite));
            //tilesArray.Add(new Tile(rng.Next(0, 6), destinationRectangle, sprite));
            //tilesArray.Add(new Tile(rng.Next(0, 6), destinationRectangle, sprite));
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in tilesArray)
            {
                tile.Draw(sb);
            }
        }

        public void Update()
        {
            currMouseState = Mouse.GetState();
            mouseRectangle.X = currMouseState.X;
            mouseRectangle.Y = currMouseState.Y;

            if (destinationRectangle.Contains(mouseRectangle))
            {
                if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    ClickedCell(x, y);
                }
            }

            prevMouseState = currMouseState;
        }
    }
}
