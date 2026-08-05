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
        private Texture2D characters;
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

        private Textbox properties;
        private Textbox tooltip;

        public bool IsPhony { get { return isPhony; } }

        public ApplicationClient(Texture2D sprite, int x, int y, int width, int height, DifficultyLevel difficultyLevel, Texture2D characters, Texture2D textBackground)
        {
            this.sprite = sprite;
            this.characters = characters;
            isShowingProperties = false;
            isShowingTooltip = false;
            rng = new Random();
            clientRectangle = new Rectangle(x, y, width, width);
            clientSourceRectangle = new Rectangle(0, rng.Next(0, 6) * 128, 128, 128);

            timer = 0;

            isPhony = (rng.Next(0, 2) == 0) ? true : false;

            propertiesText = "Location: Downloads";
            tooltipText = "Location: Downloads";

            switch (difficultyLevel)
            {
                case DifficultyLevel.Easy:
                    clientSourceRectangle.X = (isPhony) ? 128 : 0;
                    break;
                case DifficultyLevel.Medium:
                    propertiesText = (isPhony) ? "Location: Desktop" : "Location: Downloads";
                    break;
                case DifficultyLevel.Hard:
                    tooltipText = (isPhony) ? "Location: Desktop" : "Location: Downloads";
                    break;
                default:
                    break;
            }

            properties = new Textbox(propertiesText, characters, new Vector2(506, 162));
            tooltip = new Textbox(tooltipText, characters, new Vector2(0, 0));

            propertiesFrame = new Frame(textBackground, 500, 150, properties.Phrase.Length * 14 + 10, 200);
            tooltipFrame = new Frame(textBackground, 0, 0, properties.Phrase.Length * 14 + 10, 32);
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
                    tooltip.X = Mouse.GetState().X;
                    tooltip.Y = Mouse.GetState().Y;
                    tooltipFrame.X = Mouse.GetState().X - 6;
                    tooltipFrame.Y = Mouse.GetState().Y - 12;
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
                properties.Draw(sb);
            }

            if (isShowingTooltip)
            {
                tooltipFrame.Draw(sb);
                tooltip.Draw(sb);
            }
        }
    }
}
