using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace test_pickup
{
    public delegate void OnButtonClickDelegate();

    public enum ButtonStates
    {
        Play,
        Continue,
        Quit,
        Options,
        Pause
    }

    internal class Button
    {
        private Texture2D spritesheet;

        private MouseState curr_mouse;
        private MouseState prev_mouse;

        private Rectangle dest_rectangle;
        private Rectangle source_rectangle;
        private Rectangle click_position;
        private Rectangle mouse_rectangle;

        public OnButtonClickDelegate OnButtonClick;

        public Rectangle Rectangle { get { return dest_rectangle; } set { dest_rectangle = value; } }

        public Button(Texture2D spritesheet, Vector2 dest_vector, ButtonStates button)
        {
            this.spritesheet = spritesheet;
            this.dest_rectangle = new Rectangle(
                (int)dest_vector.X,
                (int)dest_vector.Y,
                46,
                15);
            source_rectangle = new Rectangle(0, 0, 46, 15);
            click_position = new Rectangle(0, 0, 1, 1);
            mouse_rectangle = new Rectangle(0, 0, 1, 1);
            prev_mouse = curr_mouse;

            switch (button)
            {
                case ButtonStates.Play:
                    source_rectangle.Y = 0;
                    break;
                case ButtonStates.Continue:
                    source_rectangle.Y = 15;
                    break;
                case ButtonStates.Quit:
                    source_rectangle.Y = 30;
                    break;
                case ButtonStates.Options:
                    source_rectangle.Y = 45;
                    break;
                case ButtonStates.Pause:
                    source_rectangle.Y = 60;
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spritesheet, dest_rectangle, source_rectangle, Color.White);
        }

        public async Task Update()
        {
            curr_mouse = Mouse.GetState();
            mouse_rectangle = new Rectangle(curr_mouse.X, curr_mouse.Y, 1, 1);

            if (dest_rectangle.Contains(mouse_rectangle))
            {
                if (curr_mouse.LeftButton == ButtonState.Pressed)
                {
                    click_position.X = (int)curr_mouse.X;
                    click_position.Y = (int)curr_mouse.Y;

                    if (dest_rectangle.Contains(click_position))
                    {
                        source_rectangle.X = 46;
                    }

                    else
                    {
                        source_rectangle.X = 0;
                    }
                }

                if (curr_mouse.LeftButton == ButtonState.Released && prev_mouse.LeftButton == ButtonState.Pressed)
                {
                    source_rectangle.X = 0;

                    if (OnButtonClick != null)
                    {
                        await Task.Delay(200);
                        OnButtonClick();
                    }
                }

            }
            prev_mouse = curr_mouse;
        }
    }
}
