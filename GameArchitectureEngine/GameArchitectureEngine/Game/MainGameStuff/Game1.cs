using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameArchitectureEngine
{
    public enum GameState
    {
        IntroState = 0,
        MainGameState = 1,
        GameOverState = 2,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ActionRPG : Microsoft.Xna.Framework.Game
    {
        List<GameObjectBase> characters;

        GraphicsDeviceManager graphics;
        const int ScreenHeight = 600;
        const int ScreenWidth = 800;

        ResourceManager Resources;
        CommandManager Commands;
        MapManager mapManager;

        CollisionManager collisionManager;

        FSM fsm;
        IntroScreenState introState;
        MainGameState mainGameState;
        GameOverState gameOverState;

        bool spacePressed = false;
        
        public GameState gameState = GameState.IntroState;

        //TODO: have a dictionary of gameobjectbases, so that appropriate methods can be called on each of these in an easier way
        List<HealthPotionGameObject> potions = new List<HealthPotionGameObject>();
        List<EnemyGameObject> enemies = new List<EnemyGameObject>();

        private PlayerGameObject player;
        public PlayerGameObject Player
        {
            get { return player; }
        }

        public MousePointer mousePointer;
        //HealthPotionGameObject potion;

        SpriteBatch spriteBatch;

        public ActionRPG()
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
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();

            characters = new List<GameObjectBase>();

            fsm = new FSM(this);
            introState = new IntroScreenState();
            mainGameState = new MainGameState();
            gameOverState = new GameOverState();

            introState.AddTransitions(new Transition(mainGameState, () => spacePressed));
            mainGameState.AddTransitions(new Transition(gameOverState, () => (!player.IsAlive)));
            gameOverState.AddTransitions(new Transition(introState, () => spacePressed));

            fsm.AddState(introState);
            fsm.AddState(mainGameState);
            fsm.AddState(gameOverState);

            fsm.Initialise("Intro");

            Resources = new ResourceManager();
            Commands = new CommandManager();
            mapManager = new MapManager(); //TODO: don't think MapManager should need Resources passed in
            collisionManager = new CollisionManager();

            player = new PlayerGameObject();
            player.Initialise();
            characters.Add(player);

            mousePointer = new MousePointer();
            characters.Add(mousePointer);

            //TODO: this should be added to the list from the map
            potions.Add(new HealthPotionGameObject(new Vector2(250, 200)));//TODO: set this position from the map
            enemies.Add(new EnemyGameObject(new Vector2(350, 350), player, 15, 100));
            enemies.Add(new EnemyGameObject(new Vector2(500, 400), player, 100, 100));
                        
            foreach (EnemyGameObject enemy in enemies) 
            {
                characters.Add(enemy);
            }

            //TODO: manage TEstings
            foreach (EnemyGameObject enemy in enemies)
                enemy.DamagePlayer += HurtPlayerTest;

            foreach (HealthPotionGameObject potion in potions)
                potion.HealPlayer += HealPlayerTest;

            player.DamageEnemy += HurtEnemyTest;

            InitialiseCollidableObjects();
            
            base.Initialize();

            InitialiseBindings();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.LoadContent(Content, GraphicsDevice);

            foreach(GameObjectBase gameObject in characters)
            {
                gameObject.LoadContent(Resources);
            }

            foreach (HealthPotionGameObject potion in potions)
            {
                potion.LoadContent(Resources);
            }

            mapManager.LoadContent(Resources);
            LoadMapTypes();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Resources.Songs["Sounds/Songs/Leprosy-Death-Leprosy"]);//Content.Load<Song>("Sounds/Music"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Resources.UnloadContent(Content);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            fsm.Update(gameTime);

            ResolveRemovals();
            //collisionManager.Update();
            Commands.Update();

            switch (gameState)
            {
                case (GameState.IntroState):
                    UpdateIntro(gameTime);
                    break;
                case (GameState.MainGameState):
                    UpdateMainGame(gameTime);
                    break;
                case (GameState.GameOverState):
                    UpdateGameOver(gameTime);
                    break;
                default:
                    DrawIntroScreen();
                    break;
            }

            
            //// TODO: Add your update logic here
            //fsm.Update(gameTime);

            //ResolveRemovals();
            //collisionManager.Update();
            //Commands.Update();
            
            ////want to update everything except potions
            //foreach(GameObjectBase gameObject in characters)
            //{
            //    gameObject.Update(gameTime);               
            //}            

            //base.Update(gameTime);
        }

        private void UpdateIntro(GameTime gameTime)
        {
            
        }

        private void UpdateMainGame(GameTime gameTime)
        {
            // TODO: Add your update logic here
            fsm.Update(gameTime);

            ResolveRemovals();
            //collisionManager.Update();
            Commands.Update();

            //want to update everything except potions
            foreach (GameObjectBase gameObject in characters)
            {
                gameObject.Update(gameTime);
            }
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            switch (gameState)
            {
                case (GameState.IntroState):
                    DrawIntroScreen();
                    break;
                case (GameState.MainGameState):
                    DrawMainGame(gameTime); 
                    break;
                case (GameState.GameOverState):
                    DrawGameOver();
                    break;
                default:
                    DrawIntroScreen();
                    break;
            }
        }

        public void DrawIntroScreen()
        {
            spriteBatch.Begin();
            {
                spriteBatch.Draw(Resources.Screens["Screens/TitleScreen"], new Vector2(0.0f, 0.0f), Color.White);
            }
            spriteBatch.End();
        }

        public void DrawMainGame(GameTime gameTime)
        {
            spriteBatch.Begin();
            {
                mapManager.Draw(Resources.Maps["Maps/0"], Resources.TileSheets[@"TileSheet/0"], spriteBatch);                

                foreach(EnemyGameObject enemy in enemies)
                {
                    enemy.Draw(gameTime, spriteBatch);
                }

                foreach(HealthPotionGameObject potion in potions)
                {
                    potion.Draw(gameTime, spriteBatch);
                }

                player.Draw(gameTime, spriteBatch);

                mousePointer.Draw(gameTime, spriteBatch);

                DrawHUD();
            }
            spriteBatch.End();
        }

        public void DrawGameOver()
        {
            spriteBatch.Begin();
            {
                spriteBatch.Draw(Resources.Screens["Screens/GameOverScreen"], new Vector2(0.0f, 0.0f), Color.White);
            }
            spriteBatch.End();
        }

        private void DrawHUD()
        {
            Rectangle TitleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(TitleSafeArea.X, TitleSafeArea.Y);
            Vector2 centre = new Vector2(TitleSafeArea.X + TitleSafeArea.Width / 2.0f,
                                         TitleSafeArea.Y + TitleSafeArea.Height / 2.0f);

            string message = "Health: " + player.Health + "/" + player.MaxHealth;
            Color messageColour;

            messageColour = Color.Gray;

            //Passing in the path is acceptible? seems a bit clunky
            DrawShadowedString(Resources.Fonts["Fonts/Hud"], message, hudLocation, messageColour);

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color colour)
        {            
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, colour);
        }

        private void InitialiseBindings()
        {
            Commands.AddKeyboardBindings(Keys.Escape, StopGame);
            Commands.AddKeyboardBindings(Keys.Space, GoToNextScreen);
            Commands.AddMouseBinding(MouseButton.LEFT, player.MoveTowards);
        }

        public void StopGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                Exit();
            }
        }

        public void GoToNextScreen(eButtonState buttonState, Vector2 amount)
        {
            if (spacePressed) spacePressed = false;

            if (buttonState == eButtonState.DOWN)
            {
                spacePressed = true;
            }
        }

        public void InitialiseCollidableObjects()
        {
            collisionManager.AddCollidable(player);
            foreach (HealthPotionGameObject potion in potions)
                collisionManager.AddCollidable(potion);

            foreach (EnemyGameObject enemy in enemies)
                collisionManager.AddCollidable(enemy);
        }

        public void ResolveRemovals()
        {
            List<Collidable> toRemove = new List<Collidable>();

            foreach (HealthPotionGameObject potion in potions)
            {
                if (potion.flagForRemoval)
                    toRemove.Add(potion);
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                for (int j = 0; j < potions.Count; j++)
                {
                    if (toRemove[i].Equals(potions[j]))
                    {
                        potions.RemoveAt(i);                        
                    }
                }
            }

            collisionManager.RemoveCollidable(toRemove);
        }

        public void HurtEnemyTest(PlayerGameObject player, EnemyGameObject enemy, EventArgs e)
        {
            
        }

        public void HurtPlayerTest(object sender, EventArgs e)
        {            
            player.HurtPlayer(25);
        }

        //TODO: wire this up
        public void HealPlayerTest(object sender, EventArgs e)
        {
            player.HealPlayer(50);
        }

        private void LoadMapTypes()
        {
            mapManager.AddMapTileTypes("GrassTL", (int)enumMapTileType.GrassTL, 0, 0);
            mapManager.AddMapTileTypes("GrassT", (int)enumMapTileType.GrassT, 64, 0);
            mapManager.AddMapTileTypes("GrassTR", (int)enumMapTileType.GrassTR, 128, 0);

            mapManager.AddMapTileTypes("EarthTL", (int)enumMapTileType.EarthTL, 192, 0);
            mapManager.AddMapTileTypes("EarthT", (int)enumMapTileType.EarthT, 256, 0);
            mapManager.AddMapTileTypes("EarthTR", (int)enumMapTileType.EarthTR, 320, 0);

            mapManager.AddMapTileTypes("GrassL", (int)enumMapTileType.GrassL, 0, 64);
            mapManager.AddMapTileTypes("GrassM", (int)enumMapTileType.GrassM, 64, 64);
            mapManager.AddMapTileTypes("GrassR", (int)enumMapTileType.GrassR, 128, 64);

            mapManager.AddMapTileTypes("EarthL", (int)enumMapTileType.EarthL, 192, 64);
            mapManager.AddMapTileTypes("EarthM", (int)enumMapTileType.EarthM, 256, 64);
            mapManager.AddMapTileTypes("EarthR", (int)enumMapTileType.EarthR, 320, 64);

            mapManager.AddMapTileTypes("GrassBL", (int)enumMapTileType.GrassBL, 0, 128);
            mapManager.AddMapTileTypes("GrassB", (int)enumMapTileType.GrassB, 64, 128);
            mapManager.AddMapTileTypes("GrassBR", (int)enumMapTileType.GrassBR, 128, 128);

            mapManager.AddMapTileTypes("EarthBL", (int)enumMapTileType.EarthBL, 192, 128);
            mapManager.AddMapTileTypes("EarthB", (int)enumMapTileType.EarthB, 256, 128);
            mapManager.AddMapTileTypes("EarthBR", (int)enumMapTileType.EarthBR, 320, 128);
        }

        public void ResetScene()
        {
            potions.Clear();
            enemies.Clear();

            potions.Add(new HealthPotionGameObject(new Vector2(250, 200)));//TODO: set this position from the map
            enemies.Add(new EnemyGameObject(new Vector2(350, 350), player, 15, 100));
            enemies.Add(new EnemyGameObject(new Vector2(500, 400), player, 100, 100));
            //Initialize();
        }
    }
}
