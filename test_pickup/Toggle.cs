﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace test_pickup
{
    public delegate void OnToggleDelegate();

    public enum ToggleStates
    {
        Volume,
        WindowResizing
    }

    internal class Toggle
    {
        private Texture2D spritesheet;

        private Rectangle position_rect;
        private Rectangle source_rect;
        private Rectangle button_bounds;
        private Rectangle mouse_rect;

        private Vector2 click_position;

        private MouseState curr_mouse;
        private MouseState prev_mouse;

        private ToggleStates state;

        private int sprite_width; //make these constants, if you ever solidify the spritesheets
        private int sprite_height; //make these constants, if you ever solidify the spritesheets
        private bool isToggled;

        public OnToggleDelegate OnToggle;

        public Rectangle Boundary { get { return button_bounds; } }
        public Toggle(Texture2D spritesheet, ToggleStates state, Vector2 position, int rows, int columns)
        {
            this.spritesheet = spritesheet;
            sprite_width = (spritesheet.Width / columns);
            sprite_height = (spritesheet.Height / rows);
            isToggled = false;
            curr_mouse = Mouse.GetState();
            prev_mouse = curr_mouse;
            this.state = state;

            position_rect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                sprite_width * Game1.scale,
                sprite_height * Game1.scale);
            source_rect = new Rectangle(
                sprite_height + (sprite_height * Enum.GetNames(typeof(ToggleStates)).Length * (int)state),
                0,
                sprite_width,
                sprite_height);
            button_bounds = new Rectangle(
                position_rect.X - (position_rect.Width / 2),
                position_rect.Y - (position_rect.Height / 2),
                position_rect.Width,
                position_rect.Height);
            mouse_rect = new Rectangle(
                curr_mouse.X,
                curr_mouse.Y,
                1,
                1);

            click_position = new Vector2(0, 0);
        }
        public void Draw(SpriteBatch sb)
        {
            if (isToggled == true)
            {
                // if the audio is currently muted
                source_rect.Y = sprite_height + (sprite_height * Enum.GetNames(typeof(ToggleStates)).Length * (int)state);
            }

            else
            {
                // if the audio is NOT currently muted
                source_rect.Y = 0 + (sprite_height * Enum.GetNames(typeof(ToggleStates)).Length * (int)state);
            }

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

                else if (curr_mouse.LeftButton == ButtonState.Released)
                {
                    source_rect.X = 0;
                }

                if (OnToggle != null && curr_mouse.LeftButton == ButtonState.Pressed && prev_mouse.LeftButton == ButtonState.Released)
                {
                    isToggled = !isToggled;
                    OnToggle();
                }
            }

            else
            {
                source_rect.X = 0;
            }

            prev_mouse = curr_mouse;
        }
        public void Resize()
        {
            position_rect.Width = sprite_width * Game1.scale;
            position_rect.Height = sprite_height * Game1.scale;

            button_bounds = new Rectangle(position_rect.X - (position_rect.Width / 2), position_rect.Y - (position_rect.Height / 2), position_rect.Width, position_rect.Height);
        }
        public void Reposition(int x, int y)
        {
            position_rect.X = x;
            position_rect.Y = y;

            // because you are the furthest thing from gods favorite, the boundaries are slightly to the right; fix them
            button_bounds = new Rectangle(position_rect.X - (position_rect.Width / 2), position_rect.Y - (position_rect.Height / 2), position_rect.Width, position_rect.Height);
        }
    }
}
