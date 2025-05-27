using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    public delegate void OnButtonClickDelegate();

    public enum ButtonStates
    {
        Play,
        Continue,
        Quit,
        Options,
        Pause,
        Back
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

        private int sprite_width;
        private int sprite_height;

        public OnButtonClickDelegate OnButtonClick;

        public Rectangle Rectangle { get { return dest_rectangle; } set { dest_rectangle = value; } }

        public Button(Texture2D spritesheet, Vector2 dest_vector, ButtonStates function, int rows, int columns)
        {
            this.spritesheet = spritesheet;
            sprite_width = spritesheet.Width / columns;
            sprite_height = spritesheet.Height / rows;
            prev_mouse = curr_mouse;

            this.dest_rectangle = new Rectangle(
                (int)dest_vector.X,
                (int)dest_vector.Y,
                sprite_width,
                sprite_height);
            source_rectangle = new Rectangle(0, 0, sprite_width, sprite_height);
            click_position = new Rectangle(0, 0, 1, 1);
            mouse_rectangle = new Rectangle(0, 0, 1, 1);

            switch (function)
            {
                case ButtonStates.Play:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Play);
                    break;
                case ButtonStates.Continue:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Continue);
                    break;
                case ButtonStates.Quit:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Quit);
                    break;
                case ButtonStates.Options:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Options);
                    break;
                case ButtonStates.Pause:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Pause);
                    break;
                case ButtonStates.Back:
                    source_rectangle.Y = sprite_height * ((int)ButtonStates.Back);
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spritesheet, dest_rectangle, source_rectangle, Color.White);
        }

        public void Update()
        {
            curr_mouse = Mouse.GetState();
            mouse_rectangle = new Rectangle(curr_mouse.X, curr_mouse.Y, 1, 1);

            if (curr_mouse.LeftButton == ButtonState.Pressed && prev_mouse.LeftButton == ButtonState.Released)
            {
                click_position.X = (int)curr_mouse.X;
                click_position.Y = (int)curr_mouse.Y;
            }

            if (dest_rectangle.Contains(mouse_rectangle) && dest_rectangle.Contains(click_position))
            {
                if (curr_mouse.LeftButton == ButtonState.Pressed)
                {
                    source_rectangle.X = sprite_width;
                }

                else if (OnButtonClick != null && prev_mouse.LeftButton == ButtonState.Pressed)
                {
                    OnButtonClick();
                }

                else
                {
                    source_rectangle.X = 0;
                }
            }

            else
            {
                source_rectangle.X = 0;
            }

            prev_mouse = curr_mouse;
        }
    }
}
