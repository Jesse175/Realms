using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Realms
{
    public class Game1 : Game
    {
        Player player;
        Texture2D ballTexture;
        Texture2D groundTexture; // For a solid color ground
        Rectangle groundRectangle;
        Vector2 ballPosition;
        float ballSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2));
            Texture2D ballTexture;


            //ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
            //    _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            player.Sprite = Content.Load<Texture2D>("player");

            // Create a 1x1 white texture for drawing colored rectangles
            groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            groundTexture.SetData(new Color[] { Color.White });

            // Define the size of the ground rectangle
            groundRectangle = new Rectangle(0, _graphics.PreferredBackBufferHeight - 50, _graphics.PreferredBackBufferWidth, 50); // Example size
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //bounds handling


            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                ballPosition.X = ballTexture.Width / 2;
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                ballPosition.Y = ballTexture.Height / 2;
            }


            base.Update(gameTime);
            
            player.UpdatePhysics(gameTime, _graphics.PreferredBackBufferHeight);
            player.Update(_graphics.PreferredBackBufferHeight);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //_spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), 
            //    Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.Draw(player.Sprite, player.mPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(groundTexture, groundRectangle, Color.Brown); // Draw the ground as a brown rectangle
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}