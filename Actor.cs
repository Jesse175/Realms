using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Realms.Physics;

namespace Realms
{
    internal class Actor
    {
        public AABB mAABB;
        public Vector2 mAABBOffset;
        public Vector2 mOldPosition;
        public Vector2 mPosition;

        public Vector2 mOldSpeed;
        public Vector2 mSpeed;

        public Vector2 mScale;

        public bool mPushedRightWall;
        public bool mPushesRightWall;
        public bool mPushedLeftWall;
        public bool mPushesLeftWall;
        public bool mWasOnGround;
        public bool mOnGround;
        public bool mWasAtCeiling;
        public bool mAtCeiling;

        public void UpdatePhysics(GameTime gameTime, int preferredBackBufferHeight)
        {
            mOldPosition = mPosition;
            mOldSpeed = mSpeed;

            mWasOnGround = mOnGround;
            mPushedRightWall = mPushesRightWall;
            mPushedLeftWall = mPushesLeftWall;
            mWasAtCeiling = mAtCeiling;

            //update position using current speed
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            mPosition += mSpeed * deltaTime;

            //temporary ground detection
            //if (mPosition.Y > preferredBackBufferHeight - 50)
            //{
            //    mPosition.Y = preferredBackBufferHeight - 50;
            //    mOnGround = true;
            //}
            //else
            //    mOnGround = false;

            mAABB.center = mPosition + mAABBOffset;
        }
    }
}
