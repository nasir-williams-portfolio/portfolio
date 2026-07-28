using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace QuestsOverCoffee_Digitization
{
    public enum GameStates
    {
        Title,
        Selection,
        Questing,
        Shopping,
        Grading,
        Death
    }

    public class Game1 : Game
    {
        private string playerStats;
        private string questResult;
        private string p1Stats;
        private string p2Stats;
        private string finalMessage;
        private int result;
        private int mouseX;
        private int mouseY;
        private int rounds;
        private int starGrade;
        private bool canRoll;
        private bool questCalculated;
        private bool debugActive;

        private KeyboardState currKeyboard;
        private KeyboardState prevKeyboard;

        private GameStates currState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont arial12;
        private Texture2D square;
        private Button diceRollButton;
        private Button initiateQuest;
        private Button playerPreset1;
        private Button playerPreset2;
        private Button restartButton;
        private QuestCard activeQuest;
        private Player player1;
        private List<QuestCard> questCards;
        private List<QuestCard> pulledQuestCards;
        private List<Die> diceSet;
        private List<ItemCard> shopItems;
        private Random rng;

        //press Q to open the debug menu

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();

            currState = GameStates.Title;

            result = 0;
            rounds = 0;
            starGrade = 0;
            canRoll = true;

            rng = new Random();
            player1 = new Player();

            p1Stats = $"health: 3\nluck: 1\nmoney: 2";
            p2Stats = $"health: 2\nluck: 3\nmoney: 3";

            debugActive = false;
            questResult = "";
            questCalculated = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            arial12 = Content.Load<SpriteFont>("arial12");
            square = Content.Load<Texture2D>("square");

            #region card stuff
            questCards = new List<QuestCard>();
            pulledQuestCards = new List<QuestCard>();

            PopulateQuestCardsList();
            ShuffleCards();
            DrawCards();
            #endregion

            diceSet = new List<Die>();
            shopItems = new List<ItemCard>();

            shopItems.Add(new ItemCard("health", "Birthday Cake", 5, arial12, square, false));
            shopItems.Add(new ItemCard("health", "Gourd Pie", 2, arial12, square, false));
            shopItems.Add(new ItemCard("luck", "Uncut PNP Sheets", 5, arial12, square, false));

            foreach (ItemCard card in shopItems)
            {
                card.OnButtonClick += PurchaseItem;
            }

            diceRollButton = new Button(square, new Vector2(350, 400), arial12, "ROLL DICE");
            initiateQuest = new Button(square, new Vector2(350, 425), arial12, "QUEST");
            playerPreset1 = new Button(square, new Vector2(290, 230), arial12, "PLAYER ONE");
            playerPreset2 = new Button(square, new Vector2(410, 230), arial12, "PLAYER TWO");
            restartButton = new Button(square, new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, _graphics.PreferredBackBufferHeight / 2), arial12, "RESTART");

            for (int i = 0; i < player1.NumberOfDice; i++)
            {
                diceSet.Add(new Die(square, new Rectangle(350 + (40 * i), 375, 20, 20), arial12, false));
            }

            diceRollButton.OnButtonClick += RollDice;
            initiateQuest.OnButtonClick += QuestCalculation;
            playerPreset1.OnButtonClick += SelectPlayer1;
            playerPreset2.OnButtonClick += SelectPlayer2;
            restartButton.OnButtonClick += RestartGame;

            player1.Items.Add(new ItemCard("luck", "CALENDAR WITH CIRCLE", 3, arial12, square, true));
            player1.Items[0].OnItemCardClick += ActivateItemCard;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            playerStats = $"Player Stats\nHEALTH: {player1.PlayerStats["health"]}\nLUCK: {player1.PlayerStats["luck"]}\nMONEY: {player1.PlayerStats["money"]}\nSTARS: {player1.PlayerStats["stars"]}";

            currKeyboard = Keyboard.GetState();

            switch (currState)
            {
                case GameStates.Title:
                    playerPreset1.Update(gameTime);
                    playerPreset2.Update(gameTime);
                    break;
                case GameStates.Selection:
                    if (rounds == 5)
                    {
                        currState = GameStates.Shopping;
                    }
                    else if (rounds == 11)
                    {
                        currState = GameStates.Grading;
                    }
                    else if (pulledQuestCards.Count == 0 && rounds != 5 && rounds != 11)
                    {
                        DrawCards();
                    }

                    foreach (QuestCard card in pulledQuestCards)
                    {
                        card.Update(gameTime);
                    }
                    for (int i = 0; i < diceSet.Count; i++)
                    {
                        if (diceSet[i].IsExtra)
                        {
                            diceSet.Remove(diceSet[i]);
                        }
                    }

                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].Update(gameTime);
                    }
                    break;
                case GameStates.Questing:
                    if (canRoll)
                    {
                        foreach (Die die in diceSet)
                        {
                            die.Update(gameTime);
                        }
                        diceRollButton.Update(gameTime);
                    }
                    else
                    {
                        initiateQuest.Update(gameTime);
                    }
                    // i want to change the questing button to return you back to the selection screen
                    if (currKeyboard.IsKeyDown(Keys.Back))
                    {
                        if (player1.PlayerStats["health"] <= 0)
                        {
                            currState = GameStates.Death;
                        }
                        else if (questCalculated)
                        {
                            currState = GameStates.Selection;
                            pulledQuestCards.Remove(activeQuest);
                            questCards.Remove(activeQuest);
                            foreach (QuestCard card in questCards)
                            {
                                card.Reset();
                            }
                        }
                    }
                    //this should be a button
                    if (currKeyboard.IsKeyDown(Keys.M) && prevKeyboard.IsKeyUp(Keys.M) && diceSet.Count < 6 && player1.PlayerStats["money"] >= activeQuest.Stars && !questCalculated)
                    {
                        player1.PlayerStats["money"] -= activeQuest.Stars;

                        diceSet.Add(new Die(square, new Rectangle((int)diceSet[diceSet.Count - 1].Location.X + 40, 375, 20, 20), arial12, true));
                        if (canRoll)
                        {
                            diceSet[diceSet.Count - 1].IsSelected = true;
                        }
                        else
                        {
                            diceSet[diceSet.Count - 1].Roll();
                            result += diceSet[diceSet.Count - 1].Face;
                        }

                    }
                    //this should be a button
                    if (currKeyboard.IsKeyDown(Keys.L) && prevKeyboard.IsKeyUp(Keys.L) && player1.PlayerStats["luck"] > 0 && !canRoll && !questCalculated)
                    {
                        canRoll = true;
                        player1.PlayerStats["luck"]--;
                    }

                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].Update(gameTime);
                    }

                    break;
                case GameStates.Shopping:
                    for (int i = 0; i < shopItems.Count; i++)
                    {
                        shopItems[i].Update(gameTime);
                    }
                    //this should be a button
                    if (currKeyboard.IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                    {
                        rounds++;
                        currState = GameStates.Selection;
                    }
                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].X = 20;
                        player1.Items[i].Y = 310 + (i * 55);
                        player1.Items[i].Update(gameTime);
                    }
                    break;
                case GameStates.Grading:
                    restartButton.Update(gameTime);
                    break;
                case GameStates.Death:
                    restartButton.Update(gameTime);
                    break;
            }

            if (currKeyboard.IsKeyDown(Keys.Q) && prevKeyboard.IsKeyUp(Keys.Q))
            {
                debugActive = !debugActive;
            }

            prevKeyboard = currKeyboard;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            MouseState currMouse = Mouse.GetState();
            MouseState prevMouse = currMouse;
            mouseX = currMouse.X;
            mouseY = currMouse.Y;

            if (debugActive)
            {
                _spriteBatch.DrawString(arial12, $"X:{mouseX}\nY:{mouseY}\ncurrent game state: {currState}\ncurrent: {currMouse.LeftButton}\nprevious: {prevMouse.LeftButton}", Vector2.Zero, Color.Black);
            }

            _spriteBatch.DrawString(arial12, playerStats, new Vector2(_graphics.PreferredBackBufferWidth - arial12.MeasureString(playerStats).X, 0), Color.Black);

            switch (currState)
            {
                case GameStates.Title:
                    playerPreset1.Draw(_spriteBatch);
                    playerPreset2.Draw(_spriteBatch);
                    _spriteBatch.DrawString(arial12, p1Stats, new Vector2(315, 255), Color.Black);
                    _spriteBatch.DrawString(arial12, p2Stats, new Vector2(435, 255), Color.Black);
                    break;
                case GameStates.Selection:
                    foreach (QuestCard card in pulledQuestCards)
                    {
                        card.Draw(_spriteBatch);
                    }
                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].X = 20;
                        player1.Items[i].Y = 310 + (i * 55);
                        player1.Items[i].Draw(_spriteBatch);
                    }
                    break;
                case GameStates.Questing:
                    _spriteBatch.DrawString(arial12, $"Roll Total: {result}", new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString($"Roll Total: {result}").X / 2, 40), Color.Black);
                    if (questCalculated)
                    {
                        _spriteBatch.DrawString(arial12, questResult, new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString(questResult).X / 2, 55), Color.White);
                    }
                    activeQuest.Draw(_spriteBatch);
                    foreach (Die d6 in diceSet)
                    {
                        d6.Draw(_spriteBatch);
                    }
                    if (canRoll)
                    {
                        diceRollButton.Draw(_spriteBatch);
                    }
                    else
                    {
                        initiateQuest.Draw(_spriteBatch);
                    }
                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].X = 20;
                        player1.Items[i].Y = 310 + (i * 55);
                        player1.Items[i].Draw(_spriteBatch);
                    }
                    break;
                case GameStates.Shopping:
                    _spriteBatch.DrawString(arial12, "Shop", new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("Shop").X, 80), Color.Black);
                    for (int i = 0; i < shopItems.Count; i++)
                    {
                        shopItems[i].X = 200 + (i * 140);
                        shopItems[i].Y = 150;
                        shopItems[i].Draw(_spriteBatch);
                    }
                    for (int i = 0; i < player1.Items.Count; i++)
                    {
                        player1.Items[i].X = 20;
                        player1.Items[i].Y = 310 + (i * 55);
                        player1.Items[i].Draw(_spriteBatch);
                    }
                    break;
                case GameStates.Grading:
                    CalculateGrading();
                    string calculationMessage = $"Star Grade: {starGrade}, Victory Points: {player1.PlayerStats["stars"]}";
                    _spriteBatch.DrawString(arial12, calculationMessage, new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString($"Star Grade: {starGrade}, Victory Points: {player1.PlayerStats["stars"]}").X / 2, 180), Color.Black);
                    _spriteBatch.DrawString(arial12, finalMessage, new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString(finalMessage).X / 2, 210), Color.Black);
                    restartButton.Draw(_spriteBatch);
                    break;
                case GameStates.Death:
                    _spriteBatch.DrawString(arial12, "YOU DIED", new Vector2(_graphics.PreferredBackBufferWidth / 2 - arial12.MeasureString("YOU DIED").X / 2, (_graphics.PreferredBackBufferHeight / 2) - 20), Color.Red);
                    restartButton.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void RollDice()
        {
            if (canRoll)
            {
                result = 0;
                foreach (Die d6 in diceSet)
                {
                    if (d6.IsSelected)
                    {
                        d6.Roll();
                    }
                    d6.IsSelected = false;
                    result += d6.Face;
                }
                canRoll = false;
            }
        }

        protected void SelectCard()
        {
            if (currState == GameStates.Selection)
            {
                foreach (QuestCard card in questCards)
                {
                    if (card.IsActiveQuest == true)
                    {
                        activeQuest = card;
                        ActivateQuesting();
                    }
                }
            }
        }

        protected void ActivateQuesting()
        {
            activeQuest.DestinationRectangle = new Rectangle(330, 100, 135, 200);

            if (currState != GameStates.Questing)
            {
                canRoll = true;
                questCalculated = false;
                result = 0;
                currState = GameStates.Questing;
                foreach (Die d6 in diceSet)
                {
                    d6.Reset();
                }
                rounds++;
            }
        }

        protected void QuestCalculation()
        {
            if (!questCalculated)
            {
                if (result <= activeQuest.FailValue)
                {
                    questResult = "Quest Failure";
                    foreach (var attribute in activeQuest.FailEvents)
                    {
                        if (attribute.Key == "item")
                        {
                            player1.Items.Remove(player1.Items[0]);
                        }
                        else
                        {
                            player1.PlayerStats[attribute.Key] += attribute.Value;
                        }
                    }
                }
                else if (result >= activeQuest.SuccessValue)
                {
                    questResult = "Quest Success";
                    foreach (var attribute in activeQuest.SuccessEvents)
                    {
                        if (attribute.Key == "item" && player1.Items.Count < 3)
                        {
                            player1.Items.Add(new ItemCard("money", "JOKES KIT", 2, arial12, square, true));
                            player1.Items[player1.Items.Count - 1].OnItemCardClick += ActivateItemCard;
                        }
                        else
                        {
                            player1.PlayerStats[attribute.Key] += attribute.Value;
                        }
                    }
                    player1.PlayerStats["stars"] += activeQuest.Stars;
                }
                else
                {
                    questResult = "Quest Abandoned";
                }
                if (activeQuest.CalculateDieConditional(diceSet))
                {
                    foreach (var attribute in activeQuest.SpecialConditionEvents)
                    {
                        if (attribute.Key == "item" && player1.Items.Count < 3)
                        {
                            player1.Items.Add(new ItemCard("money", "JOKES KIT", 2, arial12, square, true));
                            player1.Items[player1.Items.Count - 1].OnItemCardClick += ActivateItemCard;
                        }
                        else
                        {
                            player1.PlayerStats[attribute.Key] += attribute.Value;
                        }
                    }
                    questResult += "\nSpecial Condition Criteria Satisfied";
                }
            }

            questCalculated = true;
        }

        protected void SelectPlayer1()
        {
            player1.SetPlayerStatistics(3, 1, 2);
            currState = GameStates.Selection;
        }

        protected void SelectPlayer2()
        {
            player1.SetPlayerStatistics(2, 3, 3);
            currState = GameStates.Selection;
        }

        protected void ShuffleCards()
        {
            List<QuestCard> tempList = new List<QuestCard>();

            int i = 0;
            while (tempList.Count != pulledQuestCards.Count)
            {
                QuestCard tempCard = pulledQuestCards[rng.Next(0, pulledQuestCards.Count)];
                if (!tempList.Contains(tempCard))
                {
                    tempList.Add(tempCard);
                    i++;
                }
            }
            pulledQuestCards = tempList;
        }

        protected void DrawCards()
        {
            for (int i = 0; i < 5; i++)
            {
                pulledQuestCards.Add(questCards[i]);
                pulledQuestCards[i].DestinationRectangle = new Rectangle(20 + (i * 155), 100, 135, 200);
                starGrade += pulledQuestCards[i].Stars;
            }
        }

        protected void PurchaseItem()
        {
            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].DestinationRectangle.Contains(new Vector2(mouseX, mouseY)))
                {
                    if (player1.PlayerStats["money"] >= shopItems[i].Price && player1.Items.Count < Player.MAXITEMS)
                    {
                        player1.PlayerStats["money"] -= shopItems[i].Price;
                        player1.Items.Add(shopItems[i]);
                        player1.Items[player1.Items.Count - 1].OnItemCardClick += ActivateItemCard;
                        player1.Items[player1.Items.Count - 1].InInventory = true;
                        shopItems.Remove(shopItems[i]);
                    }
                }
            }
        }

        protected void ActivateItemCard(string attribute, int value)
        {
            player1.PlayerStats[attribute] += value;
            for (int i = 0; i < player1.Items.Count; i++)
            {
                if (player1.Items[i].IsUsed)
                {
                    player1.Items.Remove(player1.Items[i]);
                }
            }
        }

        protected void RestartGame()
        {
            result = 0;
            rounds = 0;
            starGrade = 0;
            canRoll = true;

            currState = GameStates.Title;

            player1.ResetPlayer();

            player1.Items.Add(new ItemCard("luck", "CALENDAR WITH CIRCLE", 3, arial12, square, true));
            player1.Items.Add(new ItemCard("health", "DUSTY UNOPENED BOOK", 3, arial12, square, true));

            foreach (ItemCard card in player1.Items)
            {
                card.OnItemCardClick += ActivateItemCard;
            }

            pulledQuestCards.Clear();

            PopulateQuestCardsList();
            ShuffleCards();
            DrawCards();
        }

        protected void PopulateQuestCardsList()
        {
            if (questCards.Count >= 1)
            {
                questCards.Clear();
            }

            #region Quest Card Populating
            questCards.Add(new QuestCard(7, 11, 2, square, arial12, SpecialCondition.Pair)); //run
            questCards.Add(new QuestCard(5, 13, 2, square, arial12, SpecialCondition.Match)); //spy vs spy
            questCards.Add(new QuestCard(7, 12, 2, square, arial12, SpecialCondition.Match)); //surprise
            questCards.Add(new QuestCard(4, 9, 1, square, arial12, SpecialCondition.Straight)); //guitar
            questCards.Add(new QuestCard(8, 11, 1, square, arial12, SpecialCondition.Match)); //hey get down here

            questCards.Add(new QuestCard(9, 12, 2, square, arial12, SpecialCondition.Pair)); //treasure here
            questCards.Add(new QuestCard(6, 10, 2, square, arial12, SpecialCondition.Straight)); //looks edible
            questCards.Add(new QuestCard(4, 8, 2, square, arial12, SpecialCondition.Pair)); //this street looks busy
            questCards.Add(new QuestCard(10, 13, 3, square, arial12, SpecialCondition.Straight)); //an orc here
            questCards.Add(new QuestCard(9, 12, 2, square, arial12, SpecialCondition.Match)); //is this actually your car


            questCards[0].AddFailEvent("health", -1);
            questCards[0].AddSuccessEvent("luck", 2);
            questCards[0].AddSpecialConditionEvent("luck", 1);

            questCards[1].AddFailEvent("health", -2);
            questCards[1].AddSuccessEvent("health", 2);
            questCards[1].AddSpecialConditionEvent("money", 1);

            questCards[2].AddFailEvent("health", -1);
            questCards[2].AddSuccessEvent("luck", 3);
            questCards[2].AddSpecialConditionEvent("item", 1);

            questCards[3].AddFailEvent("item", -1);
            questCards[3].AddSuccessEvent("luck", 1);
            questCards[3].AddSpecialConditionEvent("money", 1);
            questCards[3].AddSpecialConditionEvent("luck", 1);

            questCards[4].AddFailEvent("health", -1);
            questCards[4].AddSuccessEvent("luck", 1);
            questCards[4].AddSpecialConditionEvent("health", -1);

            questCards[5].AddFailEvent("health", -2);
            questCards[5].AddSuccessEvent("item", 1);
            questCards[5].AddSpecialConditionEvent("money", 1);

            questCards[6].AddFailEvent("health", -2);
            questCards[6].AddSuccessEvent("health", 2);
            questCards[6].AddSpecialConditionEvent("luck", 1);

            questCards[7].AddFailEvent("health", -1);
            questCards[7].AddSuccessEvent("luck", 1);
            questCards[7].AddSpecialConditionEvent("money", 1);

            questCards[8].AddFailEvent("health", -3);
            questCards[8].AddSuccessEvent("item", 1);
            questCards[8].AddSpecialConditionEvent("money", 2);

            questCards[9].AddFailEvent("luck", -1);
            questCards[9].AddSuccessEvent("luck", 1);
            questCards[9].AddSpecialConditionEvent("money", 2);
            #endregion

            foreach (QuestCard card in questCards)
            {
                card.OnCardSelect += SelectCard;
            }
        }

        protected void CalculateGrading()
        {
            if (starGrade - player1.PlayerStats["stars"] <= 0)
            {
                finalMessage = "Fantastic job! Absolute perfection. You are an amazing adventurer!";
            }
            else if (starGrade - player1.PlayerStats["stars"] <= 5 && starGrade - player1.PlayerStats["stars"] >= 1)
            {
                finalMessage = "Good job! Be proud of yourself!";
            }
            else if (starGrade - player1.PlayerStats["stars"] <= 10 && starGrade - player1.PlayerStats["stars"] >= 6)
            {
                finalMessage = "Needs some work, but decent! Still worth hanging on your refrigerator.";
            }
            else
            {
                finalMessage = "Hey, we're open tomorrow! Come in early, grab a free cup of coffee, and we'll sort through the back for some items that may help you.";
            }
        }
    }
}
