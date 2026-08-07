using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackYourSummerProjectTwo
{
    internal class GameModeWindow
    {
        private Texture2D windowTexture;
        private Rectangle destinationRectangle;
        private Button resetGameModeButton;
        private Button selectGameModeButton;
        private Button[] buttons;

        public Button[] Buttons { get { return buttons; } }

        public GameModeWindow(Texture2D windowTexture, Texture2D buttonTexture, Vector2 location)
        {
            this.windowTexture = windowTexture;
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, windowTexture.Width * 2, windowTexture.Height * 2);
            resetGameModeButton = new Button(buttonTexture, (int)location.X + 196, (int)location.Y + 306, 58, 54, new Rectangle(0, 270, 58, 54));
            selectGameModeButton = new Button(buttonTexture, (int)location.X + 46, (int)location.Y + 306, 128, 54, new Rectangle(0, 108, 128, 54));

            buttons = new Button[2];
            buttons[0] = selectGameModeButton;
            buttons[1] = resetGameModeButton;
        }

        public void Update()
        {
            selectGameModeButton.Update();
            resetGameModeButton.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(windowTexture, destinationRectangle, Color.White);
            selectGameModeButton.Draw(sb);
            resetGameModeButton.Draw(sb);
        }
    }
}
