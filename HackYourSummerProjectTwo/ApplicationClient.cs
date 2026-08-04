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
        private Texture2D tooltipBackground;
        private Texture2D propertiesBackground;
        private Random rng;
        private Rectangle clientRectangle;
        private Rectangle clientSourceRectangle;
        private Rectangle propertiesMenuRectangle;
        private string propertiesText;
        private string tooltipText;
        private MouseState currMouse;
        private MouseState prevMouse;
        private bool isShowingProperties;
        private bool isPhony;
        private bool isShowingTooltip;
        private int timer;
        private int tooltipX;
        private int tooltipY;

        public bool IsPhony { get { return isPhony; } }

        public ApplicationClient(Texture2D sprite, int x, int y, int width, int height, DifficultyLevel difficultyLevel, Texture2D characters, Texture2D tooltipBackground, Texture2D propertiesBackground)
        {
            this.sprite = sprite;
            this.characters = characters;
            this.tooltipBackground = tooltipBackground;
            this.propertiesBackground = propertiesBackground;
            isShowingProperties = false;
            isShowingTooltip = false;
            rng = new Random();
            clientRectangle = new Rectangle(x, y, width, width);
            clientSourceRectangle = new Rectangle(0, rng.Next(0, 6) * 128, 128, 128);

            propertiesMenuRectangle = new Rectangle(x + width + 12, y - 12, 150, 250);//properties menu needs to be fixed :(
            timer = 0;
            tooltipX = 0;
            tooltipY = 0;

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
            sb.Draw(sprite, clientRectangle, clientSourceRectangle, Color.White);

            if (isShowingProperties)
            {
                Textbox properties = new Textbox(propertiesText, characters, new Vector2(propertiesMenuRectangle.X + 1, propertiesMenuRectangle.Y));
                sb.Draw(propertiesBackground, propertiesMenuRectangle, Color.White);
                properties.Draw(sb);
            }

            if (isShowingTooltip)
            {
                Textbox tooltip = new Textbox(tooltipText, characters, new Vector2(tooltipX + 1, tooltipY));
                sb.Draw(tooltipBackground, new Rectangle(tooltipX - 2, tooltipY - 3, (tooltip.Phrase.Length * 14) + 3, 20), Color.White);
                tooltip.Draw(sb);
            }
        }
    }
}
