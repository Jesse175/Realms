using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realms
{
    internal class Player : Actor
    {
        public const float cWalkSpeed = 160.0f;
        public const float cJumpSpeed = 110.0f;
        public const float cMinJumpSpeed = 200.0f;
        public const float cHalfSizeY = 20.0f;
        public const float cHalfSizeX = 6.0f;
        public const float cTerminalJumpSpeed = 300f;

        protected bool[] mInputs;
        protected bool[] mPrevInputs;

        public Texture2D Sprite;
        private int speed = 200;

        public CharacterState mCurrentState = CharacterState.Stand;
        public bool gravity;
        public float mJumpSpeed;
        public float mWalkSpeed;
        public double jumpTime;
        public Player(Vector2 initialPosition)
        {
            mPosition = initialPosition;
        }

        public void CharacterInit(bool[] inputs, bool[] prevInputs, Vector2 initialPosition)
        {
            mPosition = initialPosition;
            mAABB.halfSize = new Vector2(cHalfSizeX, cHalfSizeY);
            mAABBOffset.Y = mAABB.halfSize.Y;

            mInputs = inputs;
            mPrevInputs = prevInputs;

            mJumpSpeed = cJumpSpeed;
            mWalkSpeed = cWalkSpeed;

            mScale = Vector2.One;
        }

        protected bool Released(KeyInput key)
        {
            return (!mInputs[(int)key] && mPrevInputs[(int)key]);
        }
        protected bool KeyState(KeyInput key)
        {
            return (mInputs[(int)key]);
        }
        protected bool Pressed(KeyInput key)
        {
            return (mInputs[(int)key] && !mPrevInputs[(int)key]);
        }

        public void Update(int preferredBackBufferHeight)
        {

            //ground collision handling
            if (mPosition.Y >= preferredBackBufferHeight - Sprite.Height - 50)
            {
                mPosition.Y = preferredBackBufferHeight - Sprite.Height - 50;
                mOnGround = true;
            }
            else
                mOnGround = false;

        }

        private void Jump()
        {
            mSpeed.Y = cJumpSpeed;
            if(jumpTime > 0)
            {
                gravity = false;
            }
            
        }

        int debugCounter = 0;
        public void CharacterUpdate(GameTime gameTime)
        {
            switch (mCurrentState)
            {
                case CharacterState.Stand:
                    mSpeed = Vector2.Zero;

                    //in air
                    if (!mOnGround)
                    {
                        mCurrentState = CharacterState.InAir;
                        break;
                    }

                    //walking
                    if (KeyState(KeyInput.GoRight) != KeyState(KeyInput.GoLeft))
                    {
                        mCurrentState = CharacterState.Walk;
                        break;
                    }
                    else if (Pressed(KeyInput.Jump))
                    {
                        Jump();
                        mCurrentState = CharacterState.InAir;
                        break;
                    }
                    break;

                case CharacterState.Walk:
                    //mAnimator.Play("Walk");
                    if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                    {
                        mCurrentState = CharacterState.Stand;
                        mSpeed = Vector2.Zero;
                        break;
                    }
                    else if (KeyState(KeyInput.GoRight))
                    {
                        if (mPushesRightWall)
                            mSpeed.X = 0.0f;
                        else
                            mSpeed.X = mWalkSpeed;

                        mScale.X = Math.Abs(mScale.X);
                    }
                    else if (KeyState(KeyInput.GoLeft))
                    {
                        if (mPushesLeftWall)
                            mSpeed.X = 0.0f;
                        else
                            mSpeed.X = -mWalkSpeed;
                        mScale.X = -Math.Abs(mScale.X);
                    }

                    //yump
                    if (KeyState(KeyInput.Jump))
                    {
                        mSpeed.Y -= mJumpSpeed;
                        mCurrentState = CharacterState.InAir;
                        break;
                    }
                    else if (!mOnGround)
                    {
                        mCurrentState = CharacterState.InAir;
                        break;
                    }
                    break;

                case CharacterState.InAir:
                    //is there a way to check how character ended up in the air? was space pressed?

                    if (gravity)
                    {
                        mSpeed.Y += 300.8F * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    

                    if (KeyState(KeyInput.GoRight) == KeyState(KeyInput.GoLeft))
                    {
                        mSpeed.X = 0.0f;
                    }
                    else if (KeyState(KeyInput.GoRight))
                    {
                        if (mPushesRightWall)
                            mSpeed.X = 0.0f;
                        else
                            mSpeed.X = mWalkSpeed;
                        mScale.X = Math.Abs(mScale.X);
                    }
                    else if (KeyState(KeyInput.GoLeft))
                    {
                        if (mPushesLeftWall)
                            mSpeed.X = 0.0f;
                        else
                            mSpeed.X = -mWalkSpeed;
                        mScale.X = -Math.Abs(mScale.X);
                    }
                    
                    //if we hit the ground 
                    if (mOnGround)
                    {
                        //if there's no movement change state to standing 
                        if (mInputs[(int)KeyInput.GoRight] == mInputs[(int)KeyInput.GoLeft])
                        {
                            mCurrentState = CharacterState.Stand;
                            mSpeed = Vector2.Zero;
                            //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                        }
                        else    //either go right or go left are pressed so we change the state to walk 
                        {
                            mCurrentState = CharacterState.Walk;
                            mSpeed.Y = 0.0f;
                            //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);
                        }
                    }
                    break;

                case CharacterState.GrabLedge:
                    break;
            }

            Debug.WriteLine(mCurrentState + " " + debugCounter++);
            //UpdatePhysics();

            //if ((!mWasOnGround && mOnGround)
            //|| (!mWasAtCeiling && mAtCeiling)
            //|| (!mPushedLeftWall && mPushesLeftWall)
            //|| (!mPushedRightWall && mPushesRightWall))
                //mAudioSource.PlayOneShot(mHitWallSfx, 0.5f);

            UpdatePrevInputs();
        }
        public void UpdatePrevInputs()
        {
            var count = (byte)KeyInput.Count;
            for (byte i = 0; i < count; ++i)
                mPrevInputs[i] = mInputs[i];
        }
    }
}
