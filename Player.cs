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
        public float mJumpSpeed;
        public float mWalkSpeed;
        public double timer;
        public Player(Vector2 initialPosition)
        {
            mPosition = initialPosition;
        }

        private void Jump(GameTime gameTime)
        {
            if(timer <= 0)
            {
                //refresh jump timer
                timer = 5000;
                mSpeed.Y = -100;
            }
            //if jump button is not held
            //if (!KeyState(KeyInput.Jump) && mSpeed.Y < 0.0f)
            //{
             //   Debug.WriteLine("Jump no longer held.");
             //   mSpeed.Y = Math.Max(mSpeed.Y, 50.0f);
            //}
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
            //KeyboardState state = Keyboard.GetState();
            //mSpeed = Vector2.Zero; // Reset speed each frame

            //if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            //    mSpeed.X = -speed; // Move left
            //if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            //    mSpeed.X = speed; // Move right
            //if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            //    mSpeed.Y = -speed; // Move up (if applicable, like in a jump)
            //if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S) && !mOnGround)
            //    mSpeed.Y = speed; // Move down (if applicable)

            //ground collision handling
            if (mPosition.Y >= preferredBackBufferHeight - Sprite.Height - 50)
            {
                mPosition.Y = preferredBackBufferHeight - Sprite.Height - 50;
                mOnGround = true;
            }
            else
                mOnGround = false;

        }

        int debugCounter = 0;
        public void CharacterUpdate(GameTime gameTime)
        {
            switch (mCurrentState)
            {
                case CharacterState.Stand:
                    Debug.WriteLine("STANDING " + debugCounter++ + " On Ground: " + mOnGround);
                    mSpeed = Vector2.Zero;
                    //mAnimator.Play("Stand");

                    //jumping
                    if (!mOnGround)
                    {
                        mCurrentState = CharacterState.Jump;
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
                        Debug.WriteLine("JUMP input detected " + debugCounter++);
                        Jump(gameTime);
                        mCurrentState = CharacterState.Jump;
                        break;
                    }
                    break;

                case CharacterState.Walk:
                    Debug.WriteLine("WALKING " + debugCounter++ );
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
                        Debug.WriteLine("JUMPING (walk switch) " + debugCounter++);
                        mSpeed.Y -= mJumpSpeed;
                        //mAudioSource.PlayOneShot(mJumpSfx, 1.0f);
                        mCurrentState = CharacterState.Jump;
                        break;
                    }
                    else if (!mOnGround)
                    {
                        mCurrentState = CharacterState.Jump;
                        break;
                    }
                    break;

                case CharacterState.Jump:
                    //Debug.WriteLine("IN JUMP STATE " + debugCounter++);
                    //mAnimator.Play("Jump");
                    mSpeed.Y += 300.8F * (float)gameTime.ElapsedGameTime.TotalSeconds; //hardcoded gravity 9.8F
                    //mSpeed.Y = Math.Max(mSpeed.Y, 20F);
                    

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

            //jump handling
            if(timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer <= 0)
                {
                    mSpeed.Y += 20; // Adjust speed after jump
                                    // Reset jump conditions or handle landing logic here
                }
            }

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
