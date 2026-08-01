using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackYourSummerProjectTwo
{
    internal class Notepad
    {
        private Texture2D sprite;
        private SpriteFont font;
        private Rectangle destinationRectangle;
        private MouseState currMouse;
        private MouseState prevMouse;
        private Vector2 clickLocation;
        private Vector2 difference;
        private Vector2 previousLocation;
        private bool isShowing;
        private string applicationText;
        private string notepadTitle;
        private bool clickedInRectangle;
        private Rectangle[] tabs;
        private Rectangle currentTab;

        public bool IsShowing { get { return isShowing; } set { isShowing = value; } }

        public Notepad(Texture2D sprite, int x, int y, int width, int height, SpriteFont font)
        {
            this.sprite = sprite;
            this.font = font;
            tabs = new Rectangle[3];
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = new Rectangle(x + (i * 100), y, 90, 20);
            }

            currentTab = tabs[0];
            destinationRectangle = new Rectangle(x, y, width, height);
            applicationText = "Identify malicious applications through:\n - Poor icon resolution (White)";
            notepadTitle = "Malicious Application Identification: Icon Resolution";
        }

        public void Update()
        {
            currMouse = Mouse.GetState();

            if (currMouse.LeftButton == ButtonState.Pressed)
            {
                if (prevMouse.LeftButton == ButtonState.Released)
                {
                    if (destinationRectangle.Contains(currMouse.Position))
                    {
                        clickLocation = new Vector2(currMouse.Position.X, currMouse.Position.Y);
                        previousLocation = new Vector2(destinationRectangle.X, destinationRectangle.Y);
                        clickedInRectangle = true;
                    }
                    else
                    {
                        clickedInRectangle = false;
                    }

                    foreach (Rectangle tab in tabs)
                    {
                        if (tab.Contains(currMouse.Position))
                        {
                            currentTab = tab;
                        }
                    }
                }
                else if (clickedInRectangle == true)
                {
                    difference = new Vector2(currMouse.X - clickLocation.X, currMouse.Y - clickLocation.Y);
                    destinationRectangle.X = (int)(difference.X + previousLocation.X);
                    destinationRectangle.Y = (int)(difference.Y + previousLocation.Y);
                    for (int i = 0; i < tabs.Length; i++)
                    {
                        tabs[i].X = (int)destinationRectangle.X + (i * 100);
                        tabs[i].Y = (int)destinationRectangle.Y;
                    }
                }
            }

            if (tabs[0].Equals(currentTab))
            {
                applicationText = "Identify malicious applications through:\n - Poor icon resolution (White)";
                notepadTitle = "Malicious Application Identification: Icon Resolution";
            }
            else if (tabs[1].Equals(currentTab))
            {
                applicationText = "Identify malicious applications through:\n - Suspicious property information(475GB)";
                notepadTitle = "Malicious Application Identification: Suspicious property information";
            }
            else
            {
                applicationText = "Identify malicious applications through:\n - Inconsistent tooltip information";
                notepadTitle = "Malicious Application Identification: Inconsistent tooltip information";
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isShowing)
            {
                sb.Draw(sprite, destinationRectangle, Color.White);

                sb.DrawString(font, notepadTitle, new Vector2(destinationRectangle.X, destinationRectangle.Y + 20), Color.Black);
                sb.DrawString(font, applicationText, new Vector2(destinationRectangle.X + 5, destinationRectangle.Y + 36), Color.Black);
                foreach (Rectangle tab in tabs)
                {
                    sb.Draw(sprite, tab, (tab.Equals(currentTab) ? Color.Blue : Color.Gray));
                }
            }
        }
    }
}
