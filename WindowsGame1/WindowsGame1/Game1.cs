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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D shuttle;
        private Texture2D earth;
        private Texture2D pacman;
        private SpriteFont font;
        private int score = 0;
        private float anglePacman = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here

            background = Content.Load<Texture2D>("stars");
            shuttle = Content.Load<Texture2D>("shuttle");
            earth = Content.Load<Texture2D>("earth");
            pacman = Content.Load<Texture2D>("pacman");
            font = Content.Load<SpriteFont>("Score");
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

            // TODO: Add your update logic here
            anglePacman += 0.01f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.Red);
            spriteBatch.Draw(earth, new Vector2(400, 240), Color.White);
            spriteBatch.Draw(shuttle, new Vector2(450, 240), Color.White);
            spriteBatch.DrawString(font, "Angle: " + anglePacman, new Vector2(550, 10), Color.White);

            Vector2 location = new Vector2(100, 100);
            Rectangle sourceRectangle = new Rectangle(0, 0, pacman.Width, pacman.Height);
            Vector2 origin = new Vector2(pacman.Width/2, pacman.Height/2);

            spriteBatch.Draw(pacman, location, sourceRectangle, Color.White, anglePacman, origin, 1.0f, SpriteEffects.None, 1);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
