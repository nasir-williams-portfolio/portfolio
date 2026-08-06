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
        private Rectangle clientSourceRectangle;
        private string propertiesText;
        private string tooltipText;
        private MouseState currMouse;
        private MouseState prevMouse;
        private bool isShowingProperties;
        private bool isPhony;
        private bool isShowingTooltip;
        private int timer;
        private Frame tooltipFrame;
        private Frame propertiesFrame;
        private Vector2 tooltipLocation;
        private Vector2 propertiesLocation;

        public bool IsPhony { get { return isPhony; } }

        public ApplicationClient(Texture2D sprite, int x, int y, int width, int height, DifficultyLevel difficultyLevel, SpriteFont font, Texture2D textBackground)
        {
            this.font = font;
            this.sprite = sprite;
            isShowingProperties = false;
            isShowingTooltip = false;
            rng = new Random();
            clientRectangle = new Rectangle(x, y, width, width);
            clientSourceRectangle = new Rectangle(0, rng.Next(0, 6) * 128, 128, 128);

            timer = 0;

            isPhony = (rng.Next(0, 2) == 0) ? true : false;

            propertiesText = "Name: Application.Ink\nType: Shortcut\nLocation: \nC:\\Users\\JohnDoe\\Downloads\nSize: 1.70 KB";
            tooltipText = "Location: C:\\Users\\JohnDoe\\Downloads";

            switch (difficultyLevel)
            {
                case DifficultyLevel.Easy:
                    clientSourceRectangle.X = (isPhony) ? 128 : 0;
                    break;
                case DifficultyLevel.Medium:
                    propertiesText = (isPhony) ? "Name: Phony.Ink\nType: Text Document\nLocation: OS (C:)\nSize: 475GB" : "Name: Application.Ink\nType: Shortcut\nLocation: \nC:\\Users\\JohnDoe\\Downloads\nSize: 1.70 KB";
                    break;
                case DifficultyLevel.Hard:
                    tooltipText = (isPhony) ? "Location: C:\\Users\\JohnDoe\\Desktop" : "Location: C:\\Users\\JohnDoe\\Downloads";
                    break;
                default:
                    break;
            }




            propertiesFrame = new Frame(textBackground, 490, 140, (int)font.MeasureString(propertiesText).Length() + 4, 200);
            tooltipFrame = new Frame(textBackground, 0, 0, (int)font.MeasureString(tooltipText).Length() + 10, 32);

            propertiesLocation = new Vector2(propertiesFrame.X + (Math.Abs(propertiesFrame.Width - (font.MeasureString(propertiesText).X)) / 2), propertiesFrame.Y + 10);
            tooltipLocation = new Vector2(0, 0);
        }

        public void Update()
        {
            currMouse = Mouse.GetState();

            if (currMouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                if (clientRectangle.Contains(currMouse.Position))
                {
                    isShowingProperties = !isShowingProperties;
                }
                else
                {
                    isShowingProperties = false;
                }
            }

            if (clientRectangle.Contains(currMouse.Position))
            {
                timer++;
                if (timer == 49)
                {
                    tooltipFrame.X = Mouse.GetState().X;
                    tooltipFrame.Y = Mouse.GetState().Y;

                    tooltipLocation.X = tooltipFrame.X + (Math.Abs(tooltipFrame.Width - (font.MeasureString(tooltipText).X)) / 2);
                    tooltipLocation.Y = tooltipFrame.Y + 10;
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

            tooltipFrame.Update();

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, clientRectangle, clientSourceRectangle, Color.White);

            if (isShowingProperties)
            {
                propertiesFrame.Draw(sb);
                sb.DrawString(font, propertiesText, propertiesLocation, Color.White);
            }

            if (isShowingTooltip)
            {
                tooltipFrame.Draw(sb);
                sb.DrawString(font, tooltipText, tooltipLocation, Color.White);
            }
        }
    }
}
