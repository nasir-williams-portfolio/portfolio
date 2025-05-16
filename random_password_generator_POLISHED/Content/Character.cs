using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace random_password_generator_POLISHED.Content
{
    internal class Character
    {
        private Rectangle source_rectangle;
        private Rectangle destination_rectangle;
        private Texture2D spritesheet;
        private char character;
        private string[,] characters;

        public Rectangle Rectangle { get { return destination_rectangle; } }

        public Character(char character, Texture2D spritesheet, Vector2 destination_vector)
        {
            this.spritesheet = spritesheet;
            this.character = character;
            this.destination_rectangle = new Rectangle(
                (int)destination_vector.X,
                (int)destination_vector.Y,
                10,
                12);

            characters = new string[6, 6]
            {
                { "A","B","C","D","E","F"},
                { "G","H","I","J","K","L"},
                { "M","N","O","P","Q","R"},
                { "S","T","U","V","W","X"},
                { "Y","Z","0","1","2","3"},
                { "4","5","6","7","8","9"}
            };

            source_rectangle = new Rectangle(TranslateCharacter()[0], TranslateCharacter()[1], 10, 12);
        }
        public int[] TranslateCharacter()
        {
            int a = 0;
            int b = 0;

            for (int x = 0; x < characters.GetLength(0); x++)
            {
                for (int y = 0; y < characters.GetLength(1); y++)
                {
                    if (characters[x, y] == character.ToString())
                    {
                        a += y * 10;
                        b += x * 12;

                        y = characters.GetLength(1);
                        x = characters.GetLength(0);
                    }
                }
            }

            int[] coordinates = [a, b];

            return coordinates;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                destination_rectangle,
                source_rectangle,
                Color.White);
        }
    }
}
