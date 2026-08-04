using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HackYourSummerProjectTwo
{
    internal class Notepad
    {
        private Texture2D sprite;
        private SpriteFont font;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private Rectangle exitButton;
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
        private List<Textbox> instructions;

        public bool IsShowing { get { return isShowing; } set { isShowing = value; } }

        public Notepad(Texture2D sprite, int x, int y, int width, int height, Rectangle sourceRectangle, Texture2D characters)
        {
            this.sprite = sprite;
            this.sourceRectangle = sourceRectangle;
            tabs = new Rectangle[3];
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = new Rectangle((10 + (i * 8)) + destinationRectangle.X, destinationRectangle.Y, 6, 4);
            }
            exitButton = new Rectangle(x + 282, y + 6, 14, 14);

            currentTab = tabs[0];
            destinationRectangle = new Rectangle(x, y, width, height);

            instructions = new List<Textbox>();
            for (int i = 0; i < 3; i++)
            {
                instructions.Add(new Textbox("", characters, new Vector2(x + 20, y + 20)));
            }

            new Textbox("", characters, new Vector2(x + 20, y + 20));
        }

        public void Update()
        {
            currMouse = Mouse.GetState();

            if (isShowing)
            {
                if (currMouse.LeftButton == ButtonState.Pressed)
                {
                    if (prevMouse.LeftButton == ButtonState.Released)
                    {
                        if (exitButton.Contains(currMouse.Position))
                        {
                            isShowing = false;
                        }

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
                            tabs[i].X = (int)destinationRectangle.X + (10 + (i * 8));
                            tabs[i].Y = (int)destinationRectangle.Y;
                        }
                        exitButton.X = (int)destinationRectangle.X + 282;
                        exitButton.Y = (int)destinationRectangle.Y + 6;

                        foreach (Textbox line in instructions)
                        {
                            line.X = (int)destinationRectangle.X + 20;
                            line.Y = (int)destinationRectangle.Y + (20 * (instructions.IndexOf(line) + 1));
                        }
                    }
                }

                for (int i = 0; i < tabs.Length; i++)
                {
                    if (tabs[i].Equals(currentTab))
                    {
                        sourceRectangle.Y = i * 240;
                    }
                }
            }

            switch (sourceRectangle.Y)
            {
                case 0:
                    instructions[0].Phrase = "deny glitches";
                    break;
                case 240:
                    instructions[0].Phrase = "deny tooltips with";
                    instructions[1].Phrase = "odd locations";
                    break;
                case 480:
                    instructions[0].Phrase = "deny location";
                    instructions[1].Phrase = "discrepancies in";
                    instructions[2].Phrase = "tooltip-properties";
                    break;
                default:
                    break;
            }

            prevMouse = currMouse;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isShowing)
            {
                sb.Draw(sprite, destinationRectangle, sourceRectangle, Color.White);
                switch (sourceRectangle.Y)
                {
                    case 0:
                        instructions[0].Draw(sb);
                        break;
                    case 240:
                        instructions[0].Draw(sb);
                        instructions[1].Draw(sb);
                        break;
                    case 480:
                        instructions[0].Draw(sb);
                        instructions[1].Draw(sb);
                        instructions[2].Draw(sb);
                        break;
                }
                foreach (Textbox line in instructions)
                {
                    line.Draw(sb);
                }
            }
        }
    }
}
