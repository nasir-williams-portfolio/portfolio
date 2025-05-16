using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace random_password_generator
{

    public delegate void OnButtonClickDelegate();

    internal class Button
    {
        private Texture2D sprite;
        private Texture2D sprite_hover;
        private Texture2D current_sprite;

        private Rectangle dest_rectangle;

        private MouseState curr_mouse;
        private MouseState prev_mouse;

        public event OnButtonClickDelegate OnButtonClick;

        public Rectangle Rectangle { get { return dest_rectangle; } }

        public Button(Texture2D sprite, Texture2D sprite_hover, Vector2 dest_vector)
        {
            this.sprite = sprite;
            this.sprite_hover = sprite_hover;

            this.dest_rectangle = new Rectangle(
                (int)dest_vector.X,
                (int)dest_vector.Y,
                sprite.Width,
                sprite.Height);


            prev_mouse = curr_mouse;
            current_sprite = sprite;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                current_sprite,
                dest_rectangle,
                Color.White);
        }

        public void Update()
        {
            curr_mouse = Mouse.GetState();

            Rectangle mouse_rectangle = new Rectangle(curr_mouse.X, curr_mouse.Y, 1, 1);

            if (dest_rectangle.Contains(mouse_rectangle))
            {
                if ((curr_mouse.LeftButton == ButtonState.Pressed && prev_mouse.LeftButton == ButtonState.Released) && OnButtonClick != null)
                {
                    OnButtonClick();
                }
            }

            if (curr_mouse.LeftButton == ButtonState.Pressed && dest_rectangle.Contains(mouse_rectangle))
            {
                current_sprite = sprite_hover;
            }

            else
            {
                current_sprite = sprite;
            }

            prev_mouse = curr_mouse;
        }
    }
}
