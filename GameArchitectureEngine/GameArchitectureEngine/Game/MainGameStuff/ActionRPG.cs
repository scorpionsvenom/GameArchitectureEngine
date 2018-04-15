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
        static int nextScreenCalled = 0;
        static int nextMapCalled = 0;

        private const double eventCoolDownTime = 0.25;
        private double nextScreenCurrentTime = 0.25;
        private double nextMapCurrentTime = 0.25;
        GameTime gameTime;

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

        private bool spacePressed = false;
        public bool SpacePressed
        {
            get { return spacePressed; } 
            set { spacePressed = value; }
        }

        public GameState gameState = GameState.IntroState;

        List<HealthPotionGameObject> potions = new List<HealthPotionGameObject>();
        List<EnemyGameObject> enemies = new List<EnemyGameObject>();

        private PlayerGameObject player;
        public PlayerGameObject Player
        {
            get { return player; }
        }

        public MousePointer mousePointer;

        Camera camera;
        float zoom = 1.0f;
        float rotation = 0.0f;

        Map currentMap;
        int currentMapIndex = 0;
        int currentMapWidth;
        int currentMapHeight;

        SpriteBatch spriteBatch;

        public ActionRPG()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #region initialisation
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
            InitialiseBindings();

            //First time initialising, so start with intro state
            gameState = GameState.IntroState;
            InitialiseIntroState();

            camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        public void InitialiseIntroState()
        {
            Commands = new CommandManager();

            InitialiseBindings();
        }

        public void InitialiseMainGameState()
        {
            characters = new List<GameObjectBase>();
            
            mapManager = new MapManager(); 
            collisionManager = new CollisionManager();

            player = new PlayerGameObject();
            player.Initialise();
            characters.Add(player);

            mousePointer = new MousePointer();
            characters.Add(mousePointer);

            //TODO: this should be added to the list from a file
            potions.Add(new HealthPotionGameObject(new Vector2(250, 200)));//TODO: set this position from the map
            enemies.Add(new EnemyGameObject(new Vector2(350, 350), player, 15, 100));
            enemies.Add(new EnemyGameObject(new Vector2(500, 400), player, 100, 100));

            foreach (EnemyGameObject enemy in enemies)
            {
                characters.Add(enemy);
            }

            //TODO: manage Testings
            foreach (EnemyGameObject enemy in enemies)
                enemy.DamagePlayer += HurtPlayerTest;

            foreach (HealthPotionGameObject potion in potions)
                potion.HealPlayer += HealPlayerTest;

            mousePointer.SelectEnemy += MouseSelectEnemyToAttack;
            player.DamageEnemy += HurtEnemyTest;
            player.CollideWithPotion += CollideWithPotionTest;
            player.CollideWithEnemy += CollideWithEnemyTest;
            //TODO Remove testings

            InitialiseCollidableObjects();

            Commands = new CommandManager();            

            InitialiseBindings();
        }

        public void InitialiseGameOverState()
        {
            Commands = new CommandManager();
            InitialiseBindings();
           
        }
        #endregion

        #region load content
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Resources.LoadContent(Content, GraphicsDevice);

            //First time loading so start with intro state
            gameState = GameState.IntroState;
            LoadIntroScreenContent();

            base.LoadContent();            
        }

        public void LoadIntroScreenContent()
        {
            if (Resources == null)
                Resources = new ResourceManager();

            Resources.LoadContent(Content, GraphicsDevice);

            MediaPlayer.Play(Resources.Songs["Sounds/Songs/Leprosy-Death-Leprosy"]);
        }

        public void LoadMainGameContent()
        {
            Resources.LoadContent(Content, GraphicsDevice);
            Player.HealPlayer(50);
            Player.IsAlive = true;
            //Player.Position = new Vector2(120f, 200f);

            foreach (GameObjectBase gameObject in characters)
            {
                gameObject.LoadContent(Resources);
            }

            foreach (HealthPotionGameObject potion in potions)
            {
                potion.LoadContent(Resources);
            }

            mapManager.LoadContent(Resources);
            LoadMapTypes();


            if (currentMapIndex < Resources.Maps.Count)
                currentMap = Resources.Maps[string.Format("Maps/{0}",currentMapIndex)];
            
            currentMapWidth = currentMap.MapList[0].Length * 64;
            currentMapHeight = currentMap.MapList.Count * 64;
            
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Resources.Songs["Sounds/Songs/Transilvanian Hunger-Darkthrone-Transilvanian Hunger"]);
        }

        public void LoadGameOverContent()
        {
            Resources.LoadContent(Content, GraphicsDevice);
            MediaPlayer.Play(Resources.Songs["Sounds/Songs/The Wanderer-Emperor-Anthems to the Welkin at Dusk"]);
        }
        #endregion

        #region unload content
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Resources.UnloadContent(Content);
            MediaPlayer.Stop();
            base.UnloadContent();
        }

        public void UnloadIntroContent()
        {
            MediaPlayer.Stop();
        }

        public void UnloadMainGameContent()
        {
            potions.Clear();
            enemies.Clear();
            player = null;
            characters.Clear();
            collisionManager = null;
            Commands = null;
            Resources.UnloadContent(Content);
            MediaPlayer.Stop();
            //#region Constrain mouse to window

            //if (Mouse.GetState().X < 0)

            //    Mouse.SetPosition(0, Mouse.GetState().Y);

            //if (Mouse.GetState().X > ScreenWidth)

            //    Mouse.SetPosition(ScreenHeight, Mouse.GetState().Y);

            //if (Mouse.GetState().Y < 0)

            //    Mouse.SetPosition(Mouse.GetState().X, 0);

            //if (Mouse.GetState().Y > ScreenWidth)

            //    Mouse.SetPosition(Mouse.GetState().X, ScreenHeight);

            //#endregion
        }

        public void UnloadGameOverContent()
        {
            MediaPlayer.Stop();
        }

        #endregion
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            fsm.Update(gameTime);            
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
                    UpdateIntro(gameTime);
                    break;
            }

            this.gameTime = gameTime;

            base.Update(gameTime);
        }

        private void UpdateIntro(GameTime gameTime)
        {
            //GraphicsDevice.Viewport = new Viewport(0, 0, ScreenWidth, ScreenHeight);
        }

        private void UpdateMainGame(GameTime gameTime)
        {
            ResolveRemovals();
            collisionManager.Update();

            //want to update everything except potions
            foreach (GameObjectBase gameObject in characters)
            {
                gameObject.Update(gameTime);
            }
                        
            camera.Update(Player.Position, rotation, zoom, currentMapWidth, currentMapHeight);
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            {
                mapManager.Draw(currentMap, Resources.TileSheets[@"TileSheet/0"], spriteBatch);                

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

            Vector2 hudLocation = new Vector2(player.Position.X - GraphicsDevice.Viewport.Width / 2, player.Position.Y - GraphicsDevice.Viewport.Height / 2);
            Vector2 hudLocation2 = new Vector2(TitleSafeArea.X, TitleSafeArea.Y);
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
            Commands.AddKeyboardBindings(Keys.N, GoToNextMap);

            if (gameState == GameState.MainGameState)
            {
                Commands.AddMouseBinding(MouseButton.LEFT, player.MoveTowards);
                Commands.AddMouseBinding(MouseButton.LEFT, mousePointer.SelectEnemy);
            }
        }

        public void StopGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                Exit();
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

        #region event testing
        //TODO: put these methods into appropriate managing class
        private void HurtEnemyTest(object player, object enemy, EventArgs e)
        {
            
        }

        private void HurtPlayerTest(object sender, EventArgs e)
        {            
            player.HurtPlayer(25);
        }

        //TODO: wire this up
        private void HealPlayerTest(object sender, EventArgs e)
        {
            player.HealPlayer(50);
        }

        private void CollideWithPotionTest(object sender, object passedInPotion, CollisionEventArgs e)
        {
            HealthPotionGameObject potion = passedInPotion as HealthPotionGameObject;

            if (potion != null)
                potion.OnHealPlayer();
            //throw new NotImplementedException();
        }

        private void CollideWithEnemyTest(object sender, object passedInEnemy, CollisionEventArgs e)
        {
            EnemyGameObject enemy = passedInEnemy as EnemyGameObject;

            if (enemy != null)
            {
                player.OnCollisionWithEnemy(enemy);
            }
        }

        private void MouseSelectEnemyToAttack(object sender, CollisionEventArgs e)
        {
            EnemyGameObject enemy = sender as EnemyGameObject;

            player.CanAttack = true;
        }
        #endregion


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

        private void GoToNextScreen(eButtonState buttonState, Vector2 amount)
        {
            if (nextScreenCurrentTime >= eventCoolDownTime)
            {
                nextScreenCurrentTime = 0.0f;

                if (buttonState == eButtonState.DOWN)
                {
                    nextScreenCalled++;
                    spacePressed = true;
                }
            }
            else
            {
                nextScreenCurrentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void GoToNextMap(eButtonState buttonState, Vector2 amount)
        {
            if (nextMapCurrentTime >= eventCoolDownTime)
            {
                if (buttonState == eButtonState.DOWN)
                {
                    nextMapCurrentTime = 0.0f;

                    currentMapIndex++;

                    UnloadMainGameContent();

                    InitialiseMainGameState();

                    LoadMainGameContent();
                    spacePressed = true;

                    nextMapCalled++;
                }
            }
            else
            {
                nextMapCurrentTime += gameTime.ElapsedGameTime.TotalSeconds;
            }            
        }
    }
}
