using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ShootingPacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        float playerMoveSpeed;
        // Image for static BG
        Texture2D mainBackground;
        //Parallaxing Layers
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        // Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;
        // Rate for enemies appearing
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        Random random;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Player
            player = new Player();
            playerMoveSpeed = 8.0f;

            // BG
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            //Enemies
            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.5f);
            random = new Random();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 playerPosition = new
            Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            player.Initialize(playerAnimation, playerPosition);

            //Load background
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);
            mainBackground = Content.Load<Texture2D>("mainbackground");

            // Load enemies
            enemyTexture = Content.Load<Texture2D>("mineAnimation");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Save keyboardstate, read keyboardstate
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // Update player
            UpdatePlayer(gameTime);

            // Update parallax BG
            bgLayer1.Update();
            bgLayer2.Update();    

            // Update enemies
            UpdateEnemies(gameTime);

            // Update collision check
            UpdateCollision();
                      
            base.Update(gameTime);
        }

        private void AddEnemy()
        {
            // Create and initialize animation object
            Animation enemyAnimation = new Animation();
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            // Generate random position
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2,
                random.Next(100, GraphicsDevice.Viewport.Height - 100));
            // Create and initialize enemy and add it to list of enemies
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyAnimation, position);
            enemies.Add(enemy);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn new enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                AddEnemy();
            }
            //Update enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Active == false)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            //Keyboard controls
            if(currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.Position.X -= playerMoveSpeed;
            }
            if(currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.Position.X += playerMoveSpeed;
            }
            if(currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if(currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Check out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X,
                player.Width/2, GraphicsDevice.Viewport.Width - player.Width/2);
            player.Position.Y = MathHelper.Clamp(player.Position.Y,
                player.Height/2, GraphicsDevice.Viewport.Height - player.Height/2);
        }

        private void UpdateCollision()
        {
            // Use Rectangles built-in intersect to determine overlapping objects
            Rectangle rectangle1;
            Rectangle rectangle2;
            // Create once for player
            rectangle1 = new Rectangle((int)player.Position.X-20, (int)player.Position.Y,
                player.Width, player.Height);

            // Detect collision between player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y,
                    enemies[i].Width-30, enemies[i].Height-15);
                // Determine if collision occurs
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract health from player
                    player.Health -= enemies[i].Damage;
                    // Destroy enemy
                    enemies[i].Health = 0;
                    // Check to see if player died
                    if (player.Health <= 0)
                        player.Active = false;
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Start drawing
            spriteBatch.Begin();

            // Draw BG
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw player            
            player.Draw(spriteBatch);

            // Draw enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

                // End drawing
                spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
