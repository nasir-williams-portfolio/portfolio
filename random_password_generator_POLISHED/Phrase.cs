using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace random_password_generator_POLISHED
{
    internal static class Phrase
    {
        public static Rectangle[] TranslateString(string phrase, Texture2D spritesheet)
        {
            Rectangle[] source_rectangles = new Rectangle[phrase.Length];

            string[,] characters = new string[6, 6]
            {
                { "A","B","C","D","E","F"},
                { "G","H","I","J","K","L"},
                { "M","N","O","P","Q","R"},
                { "S","T","U","V","W","X"},
                { "Y","Z","0","1","2","3"},
                { "4","5","6","7","8","9"}
            };

            for (int i = 0; i < phrase.Length; i++)
            {
                for (int x = 0; x < characters.GetLength(0); x++)
                {
                    for (int y = 0; y < characters.GetLength(1); y++)
                    {
                        if (characters[x, y] == phrase[i].ToString().ToUpper())
                        {
                            source_rectangles[i] = new Rectangle(
                                y * 10,
                                x * 12,
                                10,
                                12);
                        }

                        else if (phrase[i].ToString().ToUpper() == " ")
                        {
                            source_rectangles[i] = new Rectangle(
                            0,
                            72,
                            10,
                            12);
                        }
                    }
                }
            }

            return source_rectangles;
        }
        public static void Draw(SpriteBatch sb, Texture2D spritesheet, Rectangle[] phrase_sources, Vector2 location, bool isCentered)
        {
            if (isCentered == false)
            {
                for (int i = 0; i < phrase_sources.Length; i++)
                {
                    sb.Draw(
                    spritesheet,
                    new Rectangle(
                        (int)location.X + (phrase_sources[i].Width * i),
                        (int)location.Y,
                        phrase_sources[i].Width,
                        phrase_sources[i].Height),
                    phrase_sources[i],
                    Color.White);
                }
            }

            else
            {
                int half_phrase_width = (phrase_sources.Length * phrase_sources[0].Width) / 2;

                for (int i = 0; i < phrase_sources.Length; i++)
                {
                    sb.Draw(
                    spritesheet,
                    new Rectangle(
                        (int)location.X + (phrase_sources[i].Width * i) - half_phrase_width,
                        (int)location.Y,
                        phrase_sources[i].Width,
                        phrase_sources[i].Height),
                    phrase_sources[i],
                    Color.White);
                }
            }
        }
    }
}
