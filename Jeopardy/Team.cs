using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jeopardy
{
    public class Team
    {
        public delegate void OnTeamClickDelegate(Team team);

        private string teamName;
        private int teamScore;
        private bool isActive;
        private string value;

        private Rectangle positionRectangle;
        private Rectangle textChangeButton;
        private Rectangle cursor;
        private MouseState currMouseState;
        private MouseState prevMouseState;
        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;

        private SpriteFont font;
        private Texture2D sprite;
        private Texture2D sprite2;

        public OnTeamClickDelegate OnTeamClick;

        public string Name { get { return teamName; } }
        public int Score { get { return teamScore; } set { teamScore = value; } }
        public Rectangle Position { get { return positionRectangle; } }

        public Team(SpriteFont font, Texture2D sprite, Vector2 position, Texture2D sprite2)
        {
            positionRectangle = new Rectangle((int)position.X, (int)position.Y, 175, 30);
            textChangeButton = new Rectangle((int)position.X - 20, (int)position.Y + 6, 16, 16);
            this.font = font;
            this.sprite = sprite;
            this.sprite2 = sprite2;
            teamName = "Team Null";
            value = "";

            prevMouseState = currMouseState;
            prevKeyboardState = currKeyboardState;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isActive == true)
            {
                // this is drawing the button that will allow you to change the name of the selected team
                sb.Draw(
                    sprite2,
                    textChangeButton,
                    new Rectangle(0, 16, 16, 16),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f);
            }

            else
            {
                sb.Draw(
                    sprite2,
                    textChangeButton,
                    new Rectangle(0, 0, 16, 16),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f);
            }

            sb.Draw(sprite, positionRectangle, Color.White);

            sb.DrawString(
                    font,
                    teamName,
                    new Vector2(positionRectangle.Center.X, positionRectangle.Center.Y - 2),
                    Color.White,
                    0f,
                    new Vector2(font.MeasureString(teamName).X / 2, font.MeasureString(teamName).Y / 2),
                    1f,
                    SpriteEffects.None,
                    0f
                    );

            sb.DrawString(font, teamScore.ToString(), new Vector2(positionRectangle.Left - 5, positionRectangle.Bottom - 12), Color.White);
        }

        public void Update()
        {
            currMouseState = Mouse.GetState();
            currKeyboardState = Keyboard.GetState();
            cursor = new Rectangle(currMouseState.X, currMouseState.Y, 1, 1);

            if (textChangeButton.Contains(cursor) && currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                isActive = !isActive;
            }

            if (isActive)
            {

                for (int i = 0; i < Keyboard.GetState().GetPressedKeys().Length; i++)
                {
                    if (Keyboard.GetState().GetPressedKeys()[i] != Keys.Back)
                    {
                        if (font.MeasureString(value).X <= positionRectangle.Width)
                        {
                            if (currKeyboardState.IsKeyDown(Keyboard.GetState().GetPressedKeys()[i]) && prevKeyboardState.IsKeyUp(Keyboard.GetState().GetPressedKeys()[i]))
                            {
                                if (Keyboard.GetState().GetPressedKeys()[i] == Keys.Space)
                                {
                                    value += " ";
                                }
                                else
                                {
                                    value += Keyboard.GetState().GetPressedKeys()[i];
                                }
                            }
                        }
                    }
                    else if (Keyboard.GetState().GetPressedKeys()[i] == Keys.Back && value.Length - 1 >= 0)
                    {
                        if (currKeyboardState.IsKeyDown(Keyboard.GetState().GetPressedKeys()[i]) && prevKeyboardState.IsKeyUp(Keyboard.GetState().GetPressedKeys()[i]))
                        {
                            value = value.Remove(value.Length - 1);
                        }
                    }
                }
                teamName = value;
            }

            if (positionRectangle.Contains(cursor) && currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (OnTeamClick != null)
                {
                    OnTeamClick(this);
                }
            }
            prevMouseState = currMouseState;
            prevKeyboardState = currKeyboardState;
        }
    }
}
