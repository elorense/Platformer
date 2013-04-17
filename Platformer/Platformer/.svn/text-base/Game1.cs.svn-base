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

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public World mGameWorld;
        public const int Width = 1280;
        public const int Height = 720;
        public Camera camera;
        private MenuScreen menuScreen;
        public bool gameWin;
        bool gameBegin;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;
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
            // TODO: Add your initialization logic here=
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
            menuScreen = new MenuScreen(this);
            mGameWorld = new World(this);
            camera = new Camera(spriteBatch, this);
            mGameWorld.worldWidth = Width;
            mGameWorld.worldHeight = Height;
            mGameWorld.camera = camera;
            
            mGameWorld.menuScreen = menuScreen;
            mGameWorld.LoadContent(Content);
            menuScreen.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            KeyboardState keystate = Keyboard.GetState();
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                 || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            if (menuScreen.gameStatus == 1)
            {
                gameBegin = false;
                gameWin = true;
            }

            if (menuScreen.restart)
            {
                gameBegin = true;
                menuScreen.restart = false;
            }

            

            menuScreen.Update(gameTime);
            mGameWorld.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            if (gameBegin)
            {
               
                mGameWorld.Draw(spriteBatch);
            }
            else
            {
                if (gameWin)
                    menuScreen.Draw(spriteBatch, 1);
                else
                    menuScreen.Draw(spriteBatch, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
