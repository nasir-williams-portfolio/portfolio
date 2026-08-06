using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class Frame
    {
        private Texture2D texture;
        private Rectangle[,] sourceRectangles;
        private Rectangle[,] destinationRectangles;
        private int x;
        private int y;
        private int width;
        private int height;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }


        public Frame(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;

            sourceRectangles = new Rectangle[3, 3]
            {{ new Rectangle(0,0,3,5),new Rectangle(3,0,26,5),new Rectangle(29,0,3,5)},
            { new Rectangle(0,5,2,24),new Rectangle(2,5,28,24),new Rectangle(30,5,2,24)},
            { new Rectangle(0,29,3,3),new Rectangle(3,29,26,3),new Rectangle(29,29,3,3)}};

            destinationRectangles = new Rectangle[3, 3]
            {{new Rectangle(x,y,6,10), new Rectangle(x+6,y,width-12,10), new Rectangle(x+58+(width-64),y,6,10) },
            { new Rectangle(x,y+10,4,height-16), new Rectangle(x+4,y+10,width-8,height-16), new Rectangle(x+60+(width-64),y+10,4,height-16) },
            { new Rectangle(x,y+58+(height-64),6,6), new Rectangle(x+6,y+58+(height-64),width-12,6), new Rectangle(x+58+(width-64),y+58+(height-64),6,6) } };
        }

        public void Update()
        {
            destinationRectangles[0, 0] = new Rectangle(x, y, 6, 10);
            destinationRectangles[0, 1] = new Rectangle(x + 6, y, width - 12, 10);
            destinationRectangles[0, 2] = new Rectangle(x + 58 + (width - 64), y, 6, 10);

            destinationRectangles[1, 0] = new Rectangle(x, y + 10, 4, height - 16);
            destinationRectangles[1, 1] = new Rectangle(x + 4, y + 10, width - 8, height - 16);
            destinationRectangles[1, 2] = new Rectangle(x + 60 + (width - 64), y + 10, 4, height - 16);

            destinationRectangles[2, 0] = new Rectangle(x, y + 58 + (height - 64), 6, 6);
            destinationRectangles[2, 1] = new Rectangle(x + 6, y + 58 + (height - 64), width - 12, 6);
            destinationRectangles[2, 2] = new Rectangle(x + 58 + (width - 64), y + 58 + (height - 64), 6, 6);
        }

        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    sb.Draw(texture, destinationRectangles[x, y], sourceRectangles[x, y], Color.White);
                }

            }
        }
    }
}
