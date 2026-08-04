using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class GameModeWindow
    {
        private Texture2D windowTexture;
        private Texture2D buttonTexture;
        private Rectangle destinationRectangle;
        private Button restartGameModeButton;
        private Button selectGameModeButton;
        private Button[] buttons;

        public Button[] Buttons { get { return buttons; } }

        public GameModeWindow(Texture2D windowTexture, Texture2D buttonTexture, Vector2 location)
        {
            this.windowTexture = windowTexture;
            this.buttonTexture = buttonTexture;
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, windowTexture.Width * 2, windowTexture.Height * 2);
            restartGameModeButton = new Button(buttonTexture, (int)location.X + 196, (int)location.Y + 306, 58, 54, new Rectangle(0, 270, 58, 54));
            selectGameModeButton = new Button(buttonTexture, (int)location.X + 46, (int)location.Y + 306, 128, 54, new Rectangle(0, 108, 128, 54));

            buttons = new Button[2];
            buttons[0] = selectGameModeButton;
            buttons[1] = restartGameModeButton;
        }

        public void Update()
        {
            selectGameModeButton.Update();
            restartGameModeButton.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(windowTexture, destinationRectangle, Color.White);
            selectGameModeButton.Draw(sb);
            restartGameModeButton.Draw(sb);
        }
    }
}
