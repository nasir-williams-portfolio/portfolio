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
        private bool clickedInRectangle;
        private Rectangle[] tabs;
        private Rectangle currentTab;
        private List<string> instructions;
        private Vector2 instructionsVector;

        public bool IsShowing { get { return isShowing; } set { isShowing = value; } }

        public Notepad(Texture2D sprite, int x, int y, int width, int height, SpriteFont font)
        {
            this.font = font;
            this.sprite = sprite;
            this.sourceRectangle = new Rectangle(0, 0, 296, 240);
            tabs = new Rectangle[3];
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i] = new Rectangle(18 + (22 * i) + x, y + 2, 14, 14);
            }
            exitButton = new Rectangle(x + 274, y, 22, 24);

            currentTab = tabs[0];
            destinationRectangle = new Rectangle(x, y, width, height);

            instructions = new List<string>();
            instructions.Add("deny programs that look \nglitchy");
            instructions.Add("deny programs with suspicious \ntooltips");
            instructions.Add("deny programs with \ndiscrepancies between the \ntooltip and properties menu");
            instructionsVector = new Vector2(destinationRectangle.X + 20, destinationRectangle.Y + 20);
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
                            tabs[i].X = (int)destinationRectangle.X + 18 + (22 * i);
                            tabs[i].Y = (int)destinationRectangle.Y + 2;
                        }
                        exitButton.X = (int)destinationRectangle.X + 274;
                        exitButton.Y = (int)destinationRectangle.Y;

                        instructionsVector.X = (int)destinationRectangle.X + 20;
                        instructionsVector.Y = (int)destinationRectangle.Y + 20;
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
                        sb.DrawString(font, instructions[0], instructionsVector, Color.White);
                        break;
                    case 240:
                        sb.DrawString(font, instructions[1], instructionsVector, Color.White);
                        break;
                    case 480:
                        sb.DrawString(font, instructions[2], instructionsVector, Color.White);
                        break;
                }
            }
        }
    }
}
