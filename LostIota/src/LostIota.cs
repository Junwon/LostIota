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

namespace LostIota
{
    public enum GameState { Loading, Running, Won, GameOver }
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LostIota : Microsoft.Xna.Framework.Game
    {
        int map_level;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        protected ContentManager content;
        protected List<List<string>> attributes, contents;

        Random random;

        GameState currentGameState = GameState.Loading;
        Texture2D splashBackground, gameOver;

        InputManager inputManager;

        Player player;
        List<Enemy> enemies;
        Map map;
        List<string> maps;
        Collision collision;
        List<string> collisions;

        public LostIota()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;

            Content.RootDirectory = "Content";
            random = new Random();
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
            inputManager = new InputManager();
            maps = new List<string>();
            collisions = new List<string>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            map_level = 0;
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("GameFont");
            // TODO: use this.Content to load your game content here
            player = new Player();
            player.LoadContent(Content, inputManager);

            enemies = new List<Enemy>();
            enemies.Add(new Enemy());

            foreach(Enemy e in enemies)
            {
                e.LoadContent(Content, inputManager);
            }

            map = new Map();
            maps.Add("Load/Map.cme");
            maps.Add("Load/Map1.cme");
            maps.Add("Load/Map2.cme");
            map.LoadContent(Content, maps[0]);

            collision = new Collision();          
            collisions.Add("Load/Collision.cme");
            collisions.Add("Load/Collision1.cme");
            collisions.Add("Load/Collision2.cme");
            collision.LoadContent(Content, collisions[0]);

            splashBackground = Content.Load<Texture2D>(@"menu");
            gameOver = Content.Load<Texture2D>(@"gameover");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
            spriteBatch.Dispose();
            player.UnloadContent();

            foreach (Enemy e in enemies)
            {
                e.UnloadContent();
            }

            map.UnloadContent();
            collision.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            // Allows the game to exit
            inputManager.Update();

            if (inputManager.KeyPressed(Keys.Escape))
                this.Exit();

            if (currentGameState == GameState.Loading)
            {
                if (inputManager.KeyPressed(Keys.Up))
                {
                    currentGameState = GameState.Running;
                }
            }

            if (currentGameState == GameState.Running)
            {
                map.resumeBackgroundMusic();
                map.Update();

                if (((player.Position.X + 60) >= GameConstants.windowWidth) && map_level < 2)
                {
                    map_level++;
                    player.Position = new Vector2(1, player.Position.Y);
                    map.LoadContent(Content, maps[map_level]);
                    collision.LoadContent(Content, collisions[map_level]);
                }
                else if(((player.Position.X) < 1) && map_level > 0)
                {
                    map_level--;
                    player.Position = new Vector2(GameConstants.windowWidth-85, player.Position.Y);
                    map.LoadContent(Content, maps[map_level]);
                    collision.LoadContent(Content, collisions[map_level]);
                }

                player.Update(gameTime, inputManager, collision);

                try
                {
                    foreach (Enemy e in enemies)
                    {
                        if (e.Health <= 0)
                        {
                            enemies.Remove(e);
                        }
                        else
                        {
                            e.Update(gameTime, inputManager);

                            foreach (Bullet b in player.getBulletLoad())
                            {
                                if (b.BulletPosX > e.Position.X && b.BulletPosY > e.Position.Y
                                    && b.BulletPosX < e.Position.X + e.Image.Width && b.BulletPosY < e.Position.Y + e.Image.Height && e.Hit == false)
                                {
                                    e.LostHealth(10);
                                    e.Hit = true;
                                }
                                else if (!(b.BulletPosX > e.Position.X && b.BulletPosY > e.Position.Y
                                    && b.BulletPosX < e.Position.X + e.Image.Width && b.BulletPosY < e.Position.Y + e.Image.Height && e.Hit == false))
                                {
                                    e.Hit = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                }

                if (inputManager.KeyPressed(Keys.Down))
                {
                    map.pauseBackgroundMusic();
                    currentGameState = GameState.Won;
                }
            }

            if (currentGameState == GameState.Won)
            {
                if (inputManager.KeyPressed(Keys.Left))
                {
                    currentGameState = GameState.GameOver;
                }
            }

            if (currentGameState == GameState.GameOver)
            {
                if (inputManager.KeyPressed(Keys.Right))
                {
                    currentGameState = GameState.Loading;
                }
            }

            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameState.Loading:
                    DrawSplashScreen();
                    break;
                case GameState.Running:
                    DrawGameplayScreen();
                    break;
                case GameState.Won:
                    DrawWinScreen();
                    break;
                case GameState.GameOver:
                    DrawGameOverScreen();
                    break;
            };
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawText(String s, int x, int y, Color c)
        {
            spriteBatch.DrawString(font, s, new Vector2(x, y), c);
        }

        //SCREEN 1
        private void DrawSplashScreen()
        {
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(splashBackground, new Vector2(0, 0), Color.White);
            
        }

        //SCREEN 2
        private void DrawGameplayScreen()
        {
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            GraphicsDevice.Clear(Color.Blue);

            map.Draw(spriteBatch);

            player.Draw(spriteBatch);

            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }

        //SCREEN 3
        private void DrawWinScreen()
        {
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            GraphicsDevice.Clear(Color.Green);
        }

        //SCREEN 4
        private void DrawGameOverScreen()
        {
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(gameOver, new Vector2(0, 0), Color.White);
            
        }

        private bool keyPressedNotHeld(KeyboardState current, KeyboardState last, Keys k)
        {
            if (last.IsKeyDown(k) && (current.IsKeyUp(k)))
                return true;
            
            return false;
        }

    }
}
