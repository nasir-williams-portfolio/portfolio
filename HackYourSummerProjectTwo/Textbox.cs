using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class Textbox
    {
        private string phrase;
        private Texture2D spritesheet;
        private string[,] characters;
        private Rectangle[] sourceRectangles;
        private Vector2 location;
        private Color color;

        public string Phrase { set { phrase = value; TranslateString(); } }

        public Textbox(string phrase, Texture2D spritesheet, Color color, Vector2 location)
        {
            this.phrase = phrase;
            this.spritesheet = spritesheet;
            this.color = color;
            this.location = location;

            characters = new string[4, 10]
            {
                { "0","1","2","3","4","5", "6", "7", "8", "9"},
                { "A","B","C","D","E","F", "G", "H", "I", "J"},
                { "K","L","M","N","O","P", "Q", "R", "S", "T"},
                { "U","V","W","X","Y","Z", ":", "\\", "-", "+"}
            };

            TranslateString();
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < sourceRectangles.Length; i++)
            {
                sb.Draw(spritesheet, new Vector2(location.X + (((sourceRectangles[i].Width) * i)), location.Y), sourceRectangles[i], color);
            }
        }

        public void TranslateString()
        {
            sourceRectangles = new Rectangle[phrase.Length];

            for (int i = 0; i < phrase.Length; i++)
            {
                for (int x = 0; x < characters.GetLength(0); x++)
                {
                    for (int y = 0; y < characters.GetLength(1); y++)
                    {
                        if (phrase[i].ToString() == " ")
                        {
                            sourceRectangles[i] = new Rectangle(0, 14, 1, 1);
                        }

                        else if (phrase[i].ToString() == ":")
                        {
                            sourceRectangles[i] = new Rectangle(84, 42, 14, 14);
                        }

                        else if (phrase[i].ToString() == "\\")
                        {
                            sourceRectangles[i] = new Rectangle(98, 42, 14, 14);
                        }

                        else if (phrase[i].ToString() == "-")
                        {
                            sourceRectangles[i] = new Rectangle(112, 42, 14, 14);
                        }

                        else if (phrase[i].ToString() == "+")
                        {
                            sourceRectangles[i] = new Rectangle(126, 42, 14, 14);
                        }

                        else if (characters[x, y] == phrase[i].ToString().ToUpper())
                        {
                            sourceRectangles[i] = new Rectangle(y * 14, x * 14, 14, 14);
                        }
                    }
                }
            }
        }
    }
}
