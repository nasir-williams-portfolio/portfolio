using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace test_pickup
{
    public delegate void OnButtonClickDelegate();
    public enum UIButtonStates
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

        private Rectangle position_rect;
        private Rectangle source_rect;
        private Vector2 click_position;
        private Rectangle mouse_rect;
        private Rectangle button_bounds;

        UIButtonStates state;

        private int sprite_width;
        private int sprite_height;

        public OnButtonClickDelegate OnButtonClick;
        public int X { get { return (int)position_rect.X; } }
        public int Y { get { return (int)position_rect.Y; } }
        public Rectangle Boundary { get { return button_bounds; } }
        public Button(Texture2D spritesheet, Vector2 position, UIButtonStates state, int rows, int columns)
        {
            this.spritesheet = spritesheet;
            sprite_width = (spritesheet.Width / columns);
            sprite_height = (spritesheet.Height / rows);
            prev_mouse = curr_mouse;
            this.state = state;

            position_rect = new Rectangle((int)position.X, (int)position.Y, sprite_width * Game1.scale, sprite_height * Game1.scale);
            source_rect = new Rectangle(0, sprite_height * (int)state, sprite_width, sprite_height);
            click_position = new Vector2(0, 0);
            // this whole "mouse_rect" business is a great example of why you need better architecture (an observer would probably make it easier)
            mouse_rect = new Rectangle(curr_mouse.X, curr_mouse.Y, 1, 1);

            // the buttons are drawn from the center, so the position_rect coordinates are not accurate, "button_bounds" fixes that
            button_bounds = new Rectangle(position_rect.X - (position_rect.Width / 2), position_rect.Y - (position_rect.Height / 2), position_rect.Width, position_rect.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spritesheet,
                position_rect,
                source_rect,
                Color.White,
                0f,
                new Vector2(sprite_width / 2, sprite_height / 2),
                SpriteEffects.None,
                0f);
        }

        public void Update()
        {
            curr_mouse = Mouse.GetState();

            // could possibly turn this into its own method
            mouse_rect.X = curr_mouse.X;
            mouse_rect.Y = curr_mouse.Y;

            if (curr_mouse.LeftButton == ButtonState.Pressed && prev_mouse.LeftButton == ButtonState.Released)
            {
                click_position.X = (int)curr_mouse.X;
                click_position.Y = (int)curr_mouse.Y;
            }

            if (button_bounds.Contains(mouse_rect) && button_bounds.Contains(click_position)) // you could do the AABB collision for the "contains" condition and cut down on rectangle usage
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

        public int GetHeight()
        {
            return sprite_height * Game1.scale;
        }

        public int GetWidth()
        {
            return sprite_width * Game1.scale;
        }

        public void Resize()
        {
            position_rect.Width = sprite_width * Game1.scale;
            position_rect.Height = sprite_height * Game1.scale;
            button_bounds = new Rectangle(position_rect.X - (position_rect.Width / 2), position_rect.Y - (position_rect.Height / 2), position_rect.Width, position_rect.Height);
        }
    }
}
