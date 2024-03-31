using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realms
{
    internal class Player : Actor
    {
        public Texture2D Sprite;
        private int speed = 200;
        public Player(Vector2 initialPosition)
        {
            mPosition = initialPosition;
        }

        public void Update(int preferredBackBufferHeight)
        {
            KeyboardState state = Keyboard.GetState();
            mSpeed = Vector2.Zero; // Reset speed each frame

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                mSpeed.X = -speed; // Move left
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                mSpeed.X = speed; // Move right
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                mSpeed.Y = -speed; // Move up (if applicable, like in a jump)
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S) && !mOnGround)
                mSpeed.Y = speed; // Move down (if applicable)

            //ground collision handling
            if (mPosition.Y > preferredBackBufferHeight - Sprite.Height - 50)
            {
                mPosition.Y = preferredBackBufferHeight - Sprite.Height - 50;
                mOnGround = true;
                mSpeed.Y = 0; // Stop the downward movement.
            }
            else
                mOnGround = false;

        }
    }
}
