using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace chess_playground
{
    internal class ChessBoard
    {
        // i'd probably give the chessboard object the whole spritesheet then use like an enumeration to split between the pawns, rooks, knights, king, queen, and bishops
        private Pawn[,] chessBoard;

        private KeyboardState currentKBState;
        private KeyboardState previousKBState;

        public ChessBoard(Pawn soldier)
        {
            Pawn[,] chessBoard = { { soldier, null }, { null, null } };
            this.chessBoard = chessBoard;

            chessBoard[0, 0].X = 200;
            chessBoard[0, 0].Y = 200;

            currentKBState = Keyboard.GetState();
            previousKBState = currentKBState;
        }

        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < chessBoard.GetLength(0); x++)
            {
                for (int y = 0; y < chessBoard.GetLength(1); y++)
                {
                    if (chessBoard[x, y] != null)
                    {
                        chessBoard[x, y].X = x * 18;
                        chessBoard[x, y].Y = y * 18;
                        chessBoard[x, y].Draw(sb);
                    }
                }
            }
        }

        public void Update()
        {
            currentKBState = Keyboard.GetState();

            if (currentKBState.IsKeyDown(Keys.Down) && previousKBState.IsKeyUp(Keys.Down))
            {
                try
                {
                    chessBoard[0, 1] = chessBoard[0, 0];
                    chessBoard[0, 0] = null;
                }
                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("Can't move there");
                }
            }

            previousKBState = currentKBState;
        }
    }
}
