using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace QuestsOverCoffee_Digitization
{
    public delegate void OnItemCardClickDelegate(string attribute, int value);

    internal class ItemCard
    {
        private string attribute;
        private string name;
        private int value;
        private int price;
        private bool inInventory;
        private bool isUsed;

        private Rectangle destinationRectangle;
        private Color color;
        private MouseState currMouse;
        private MouseState prevMouse;

        private SpriteFont font;
        private Texture2D sprite;
        public OnItemCardClickDelegate OnItemCardClick;
        public OnButtonClickDelegate OnButtonClick;

        public float X { set { destinationRectangle.X = (int)value; } }
        public float Y { set { destinationRectangle.Y = (int)value; } }
        public int Price { get { return price; } }
        public bool IsUsed { get { return isUsed; } }

        public bool InInventory { set { inInventory = value; } }
        public Rectangle DestinationRectangle { set { destinationRectangle = value; } get { return destinationRectangle; } }

        public ItemCard(string attribute, string name, int value, SpriteFont font, Texture2D sprite, bool inInventory)
        {
            this.name = name;
            this.attribute = attribute;
            this.value = value;
            this.font = font;
            this.sprite = sprite;
            this.inInventory = inInventory;
            destinationRectangle = new Rectangle(0, 0, 135, 45);

            currMouse = Mouse.GetState();
            prevMouse = currMouse;

            //statusEffect = new Dictionary<string, int>();
            //statusEffect.Add(attribute, value);

            Random rng = new Random();
            price = rng.Next(1, 7);
            color = Color.White;
            isUsed = false;
        }

        public void Draw(SpriteBatch sb)
        {
            string text = $"{name}\n{attribute}, {value}";
            if (!inInventory)
            {
                text += $"\nCost: {price}";
            }
            sb.Draw(sprite, destinationRectangle, color);
            sb.DrawString(font, text, new Vector2(destinationRectangle.X, destinationRectangle.Y), Color.Black);
        }

        public void Update(GameTime gt)
        {
            currMouse = Mouse.GetState();

            if (destinationRectangle.Contains(new Vector2(currMouse.X, currMouse.Y)))
            {
                if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                {
                    if (inInventory && OnItemCardClick != null)
                    {
                        color = Color.PowderBlue;
                        isUsed = true;
                        OnItemCardClick(attribute, value);
                    }

                    else if (!inInventory && OnButtonClick != null)
                    {
                        OnButtonClick();
                    }
                }
            }
            prevMouse = currMouse;
        }
    }
}
