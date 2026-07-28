using System.Collections.Generic;

namespace QuestsOverCoffee_Digitization
{
    internal class Player
    {
        protected int numberOfDice;
        protected static int maxHealth = 5; // [-]
        protected static int maxLuck = 5;   // [-]
        protected static int maxMoney = 10; // [-]
        protected static int maxDice = 3;   // [-]
        protected static int maxItems = 3;  // [X]

        protected Dictionary<string, int> playerStats;
        protected List<ItemCard> items;

        public Dictionary<string, int> PlayerStats { get { return playerStats; } }
        public List<ItemCard> Items { get { return items; } }
        public int NumberOfDice { get { return numberOfDice; } set { numberOfDice = value; } }
        public static int MAXHEALTH { get { return maxHealth; } set { maxHealth = value; } }
        public static int MAXLUCK { get { return maxLuck; } set { maxLuck = value; } }
        public static int MAXMONEY { get { return maxMoney; } set { maxMoney = value; } }
        public static int MAXITEMS { get { return maxItems; } set { maxItems = value; } }

        public Player()
        {
            playerStats = new Dictionary<string, int>();
            playerStats.Add("health", 0);
            playerStats.Add("luck", 0);
            playerStats.Add("money", 0);
            playerStats.Add("stars", 0);

            numberOfDice = 3;

            items = new List<ItemCard>();
        }

        public void SetPlayerStatistics(int health, int luck, int money)
        {
            playerStats["health"] = health;
            playerStats["luck"] = luck;
            playerStats["money"] = money;
        }

        public void ResetPlayer()
        {
            playerStats["health"] = 0;
            playerStats["luck"] = 0;
            playerStats["money"] = 0;
            playerStats["stars"] = 0;

            items.Clear();
        }
    }
}
