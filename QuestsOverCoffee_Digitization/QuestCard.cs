using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestsOverCoffee_Digitization
{
    public enum SpecialCondition
    {
        Straight, // [STRAIGHT] the dice rolled are in sequential order
        Pair,     // [PAIR] at least 2 of the dice are the same number
        Match,    // [#,#,#] the dice match the listed numbers
        Greater,  // [HEALTH/LUCK/MONEY </>/=#] the players listed attribute is less than, greater than, or equal to the listed number
        Lesser,
        Equal
    }

    internal class QuestCard
    {
        protected int failValue;
        protected int successValue;
        protected int stars;
        private int numOfClicks;
        protected int[] numbersToMatch;
        protected bool isActiveQuest;
        private string cardinfo;
        protected string conditional;

        private Color color;
        private MouseState prevMouse;
        private MouseState currMouse;
        private Rectangle cursor;
        protected Rectangle destinationRectangle;
        private Rectangle sourceRectangle;

        protected SpecialCondition condition;

        public OnButtonClickDelegate OnCardSelect;
        protected Dictionary<string, int> failEvents;
        protected Dictionary<string, int> successEvents;
        protected Dictionary<string, int> specialConditionEvents;
        private Texture2D sprite;
        private SpriteFont font;

        public int FailValue { get { return failValue; } }
        public int SuccessValue { get { return successValue; } }
        public int Stars { get { return stars; } }
        public Rectangle DestinationRectangle { get { return destinationRectangle; } set { destinationRectangle = value; } }
        public bool IsActiveQuest { get { return isActiveQuest; } set { isActiveQuest = value; } }
        public int NumberOfClicks { get { return numOfClicks; } }
        public Dictionary<string, int> FailEvents { get { return failEvents; } }
        public Dictionary<string, int> SuccessEvents { get { return successEvents; } }
        public Dictionary<string, int> SpecialConditionEvents { get { return specialConditionEvents; } }

        public QuestCard(int failValue, int successValue, int stars, Texture2D sprite, SpriteFont font, SpecialCondition condition)
        {
            color = Color.White;
            isActiveQuest = false;
            numOfClicks = 0;
            cardinfo = $"fail value: {failValue}\nsuccess value: {successValue}\nstar count: {stars}";

            failEvents = new Dictionary<string, int>();
            successEvents = new Dictionary<string, int>();
            specialConditionEvents = new Dictionary<string, int>();
            this.sourceRectangle = new Rectangle(0, 0, 1, 1);

            this.failValue = failValue;
            this.successValue = successValue;
            this.stars = stars;
            this.sprite = sprite;
            this.font = font;
            this.condition = condition;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, destinationRectangle, sourceRectangle, color);
            sb.DrawString(font, cardinfo, new Vector2(destinationRectangle.X, destinationRectangle.Y), Color.Black);
        }

        public void Update(GameTime gt)
        {
            currMouse = Mouse.GetState();
            cursor = new Rectangle(currMouse.X, currMouse.Y, 1, 1);

            if (isActiveQuest)
            {
                color = Color.Yellow;
            }
            else
            {
                color = Color.White;
            }

            if (currMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                if (destinationRectangle.Contains(cursor) && OnCardSelect != null)
                {
                    isActiveQuest = true;
                    numOfClicks++;
                    if (numOfClicks >= 2)
                    {
                        OnCardSelect();
                    }
                }
                else
                {
                    isActiveQuest = false;
                    numOfClicks = 0;
                }
            }

            prevMouse = currMouse;
        }

        public void Reset()
        {
            isActiveQuest = false;
            numOfClicks = 0;
        }

        public void AddFailEvent(string attribute, int value)
        {
            failEvents.Add(attribute, value);
            cardinfo += $"\nfail: {attribute}, {value}";
        }

        public void AddSuccessEvent(string attribute, int value)
        {
            successEvents.Add(attribute, value);
            cardinfo += $"\nsuccess: {attribute}, {value}\n{condition.ToString().ToUpper()}";
            if (condition == SpecialCondition.Match)
            {
                Random rng = new Random();
                numbersToMatch = new int[rng.Next(1, 4)];
                cardinfo += $": [";
                for (int i = 0; i < numbersToMatch.Length; i++)
                {
                    numbersToMatch[i] = rng.Next(1, 7);
                    cardinfo += $" {numbersToMatch[i]} ";
                }
                cardinfo += $"]";
            }
        }

        public void AddSpecialConditionEvent(string attribute, int value)
        {
            specialConditionEvents.Add(attribute, value);
            cardinfo += $"\n{attribute}, {value}";
        }

        public bool CalculateDieConditional(List<Die> diceSet)
        {
            bool result = false;
            switch (condition)
            {
                case SpecialCondition.Straight:
                    List<Die> sortedDiceSet = diceSet.OrderBy(o => o.Face).ToList();
                    for (int i = 0; i < sortedDiceSet.Count - 1; i++)
                    {
                        if (sortedDiceSet[i].Face + 1 == sortedDiceSet[i + 1].Face)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            i = sortedDiceSet.Count - 1;
                        }
                    }
                    break;
                case SpecialCondition.Pair:
                    List<int> arr = diceSet.ConvertAll<int>(o => o.Face);
                    foreach (int num in arr)
                    {
                        if (arr.IndexOf(num) != arr.LastIndexOf(num))
                        {
                            result = true;
                        }
                    }
                    break;
                case SpecialCondition.Match:
                    List<int> diceSetCopy = diceSet.ConvertAll<int>(o => o.Face);
                    int matches = 0;
                    foreach (int num in numbersToMatch)
                    {
                        if (diceSetCopy.Contains(num))
                        {
                            diceSetCopy.Remove(num);
                            matches++;
                        }
                    }
                    result = (matches == numbersToMatch.Length);
                    break;
                default:
                    break;
            }
            return result;
        }

        public bool CalculateStatConditional(Dictionary<string, int> playerStatistics)
        {
            bool result = false;

            switch (condition)
            {
                case SpecialCondition.Greater:

                    break;
                case SpecialCondition.Lesser:
                    break;
                case SpecialCondition.Equal:
                    break;
            }

            return result;
        }
    }
}
