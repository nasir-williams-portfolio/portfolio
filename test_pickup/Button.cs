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

        private Vector2 position;
        private Rectangle position_rect;
        private Rectangle source_rect;
        private Rectangle click_position;
        private Rectangle mouse_rect;

        private int sprite_width;
        private int sprite_height;

        public OnButtonClickDelegate OnButtonClick;

        public int X { get { return (int)position.X; } }
        public int Y { get { return (int)position.Y; } }
        public static int Width { get { return 46 * Game1.scale; } }
        public static int Height { get { return 14 * Game1.scale; } }

        public Button(Texture2D spritesheet, Vector2 position, ButtonStates function, int rows, int columns)
        {
            this.spritesheet = spritesheet;
            this.position = position;
            sprite_width = (spritesheet.Width / columns); //fundamentally, doing the calculation when the variable is hard-coded in the static variable is redundant; so maybe make a method that positions it?
            sprite_height = (spritesheet.Height / rows);
            prev_mouse = curr_mouse;

            position_rect = new Rectangle((int)position.X, (int)position.Y, sprite_width * Game1.scale, sprite_height * Game1.scale);
            source_rect = new Rectangle(0, 0, sprite_width, sprite_height);
            click_position = new Rectangle(0, 0, 1, 1);
            mouse_rect = new Rectangle(0, 0, 1, 1);

            switch (function)
            {
                case ButtonStates.Play:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Play);
                    break;
                case ButtonStates.Continue:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Continue);
                    break;
                case ButtonStates.Quit:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Quit);
                    break;
                case ButtonStates.Options:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Options);
                    break;
                case ButtonStates.Pause:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Pause);
                    break;
                case ButtonStates.Back:
                    source_rect.Y = sprite_height * ((int)ButtonStates.Back);
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                position_rect,
                source_rect,
                Color.White,
                0f,
                new Vector2(source_rect.Width / 2, source_rect.Height / 2),
                SpriteEffects.None,
                0f);
        }

        public void Update()
        {
            curr_mouse = Mouse.GetState();
            mouse_rect = new Rectangle(curr_mouse.X, curr_mouse.Y, 1, 1);
            Rectangle button_bounds = new Rectangle(position_rect.X - (position_rect.Width / 2), position_rect.Y - (position_rect.Height / 2), position_rect.Width, position_rect.Height);

            if (curr_mouse.LeftButton == ButtonState.Pressed && prev_mouse.LeftButton == ButtonState.Released)
            {
                click_position.X = (int)curr_mouse.X;
                click_position.Y = (int)curr_mouse.Y;
            }

            if (button_bounds.Contains(mouse_rect) && button_bounds.Contains(click_position))
            {
                if (curr_mouse.LeftButton == ButtonState.Pressed)
                {
                    source_rect.X = sprite_width;
                }

                else if (OnButtonClick != null && prev_mouse.LeftButton == ButtonState.Pressed)
                {
                    source_rect.X = 0;
                    OnButtonClick();
                }
            }

            else
            {
                source_rect.X = 0;
            }

            prev_mouse = curr_mouse;
        }
    }
}
