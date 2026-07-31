using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace HackYourSummerProjectTwo
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    internal class ApplicationClient
    {
        private Texture2D sprite;
        private SpriteFont font;
        private Random rng;
        private Rectangle clientRectangle;
        private Rectangle propertiesMenuRectangle;
        private string propertiesText;
        private string tooltipText;
        private MouseState currMouse;
        private MouseState prevMouse;
        private Rectangle cursor;
        private Color color;
        private bool isShowingProperties;
        private bool isPhony;
        private bool isShowingTooltip;
        private int timer;
        private int tooltipX;
        private int tooltipY;

        public bool IsPhony { get { return isPhony; } set { isPhony = value; } }

        public ApplicationClient(Texture2D sprite, int x, int y, int width, int height, DifficultyLevel difficultyLevel, SpriteFont font)
        {
            this.sprite = sprite;
            this.font = font;
            isShowingProperties = false;
            isShowingTooltip = false;
            rng = new Random();
            clientRectangle = new Rectangle(x, y, width, height);
            propertiesMenuRectangle = new Rectangle(x + width + 12, y - 12, 263, 150);
            timer = 0;
            tooltipX = 0;
            tooltipY = 0;

            isPhony = (rng.Next(0, 2) == 0) ? true : false;

            color = Color.Black;
            propertiesText = "Name: Application.Ink\nType: Shortcut\nLocation: C:\\Users\\JohnDoe\\Downloads\nSize: 1.70 KB";
            tooltipText = "Location: C:\\Users\\JohnDoe\\Downloads";

            switch (difficultyLevel)
            {
                case DifficultyLevel.Easy:
                    color = (isPhony) ? Color.White : Color.Black;
                    break;
                case DifficultyLevel.Medium:
                    propertiesText = (isPhony) ? "Name: Phony.Ink\nType: Text Document\nLocation: OS (C:)\nSize: 475GB" : "Name: Application.Ink\nType: Shortcut\nLocation: C:\\Users\\JohnDoe\\Downloads\nSize: 1.70 KB";
                    break;
                case DifficultyLevel.Hard:
                    tooltipText = (isPhony) ? "Location: C:\\Users\\JohnDoe\\Desktop" : "Location: C:\\Users\\JohnDoe\\Downloads";
                    break;
                default:
                    break;
            }
        }

        public void Update()
        {
            currMouse = Mouse.GetState();
            cursor = new Rectangle(currMouse.X, currMouse.Y, 1, 1);

            if (currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                if (clientRectangle.Contains(cursor))
                {
                    isShowingProperties = !isShowingProperties;
                }
                else
                {
                    isShowingProperties = false;
                }
            }

            if (clientRectangle.Contains(cursor))
            {
                timer++;
                if (timer == 49)
                {
                    tooltipX = Mouse.GetState().X;
                    tooltipY = Mouse.GetState().Y;
                }
                if (timer >= 50)
                {
                    if (isShowingProperties == false)
                    {
                        isShowingTooltip = true;
                    }
                    else
                    {
                        timer = 0;
                        isShowingTooltip = false;
                    }
                }
            }
            else
            {
                timer = 0;
                isShowingTooltip = false;
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, clientRectangle, color);

            if (isShowingProperties)
            {
                sb.Draw(sprite, propertiesMenuRectangle, Color.White);
                sb.DrawString(font, propertiesText, new Vector2(propertiesMenuRectangle.X + 1, propertiesMenuRectangle.Y), Color.Black);
            }

            if (isShowingTooltip)
            {
                sb.Draw(sprite, new Rectangle(tooltipX, tooltipY, (int)font.MeasureString(tooltipText).X + 1, (int)font.MeasureString(tooltipText).Y), Color.White);
                sb.DrawString(font, tooltipText, new Vector2(tooltipX + 1, tooltipY), Color.Black);
            }
        }
    }
}
