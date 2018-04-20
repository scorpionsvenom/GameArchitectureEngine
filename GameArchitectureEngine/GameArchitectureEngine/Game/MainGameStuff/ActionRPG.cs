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

        private GameTime gameTime;

        //Master list of entities in the scene
        private List<GameObjectBase> entities;

        private GraphicsDeviceManager graphics;
        const int ScreenHeight = 600;
        const int ScreenWidth = 800;

        private ResourceManager Resources;
        private CommandManager Commands;
        private MapManager mapManager;

        private CollisionManager collisionManager;

        private FSM fsm;
        private IntroScreenState introState;
        private MainGameState mainGameState;
        private GameOverState gameOverState;

        private bool spacePressed = false;
        public bool SpacePressed
        {
            get { return spacePressed; } 
            set { spacePressed = value; }
        }

        public GameState gameState = GameState.IntroState;

        //Used for specific operations on these types of objects
        private List<PotionGameObject> potions = new List<PotionGameObject>();
        private List<EnemyGameObject> enemies = new List<EnemyGameObject>();
        private List<Tree> trees = new List<Tree>();
        private List<Rock> rocks = new List<Rock>();

        private PlayerGameObject player;
        public PlayerGameObject Player
        {
            get { return player; }
        }

        public MousePointer mousePointer;

        private Camera camera;
        private float zoom = 1.0f;
        private float rotation = 0.0f;

        Map currentMap;
        private int currentMapIndex = 0;
        private int currentMapWidth;
        private int currentMapHeight;
        private bool loadNewMap = false;

        private SpriteBatch spriteBatch;

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
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();

            Commands = new CommandManager();

            InitialiseBindings();
        }

        public void InitialiseMainGameState()
        {
            entities = new List<GameObjectBase>();
            
            mapManager = new MapManager(); 
            collisionManager = new CollisionManager();

            player = new PlayerGameObject();
            player.Initialise();            
            
            Commands = new CommandManager();   
            
            mousePointer = new MousePointer(Commands, player);            

            InitialiseBindings();            
        }

        public void InitialiseGameOverState()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();

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

        public void LoadMainGameContent(bool loadGameFromDefault)
        {
            Resources.LoadContent(Content, GraphicsDevice);

            if (loadGameFromDefault)
            {
                LoadGame(string.Format("Content/DefaultData/level{0}.xml", currentMapIndex));
            }

            //Check the list of entities is empty to avoid populating twice
            if (entities.Count == 0)
            {
                entities.Add(player);
                foreach (EnemyGameObject enemy in enemies)
                    entities.Add(enemy);

                foreach (PotionGameObject potion in potions)
                    entities.Add(potion);

                foreach (Tree tree in trees)
                    entities.Add(tree);

                foreach (Rock rock in rocks)
                    entities.Add(rock);

                foreach (GameObjectBase gameObject in entities)
                {
                    gameObject.LoadContent(Resources);
                }
            }
            //TODO: manage Testings
            player.CollideWithEnemy += CollideWithEnemyTest;            
            player.DamageEnemy += player.Damage;

            //mousePointer.SelectEnemy += mousePointer.MouseSelectEntity;

            foreach (EnemyGameObject enemy in enemies)
            {
                enemy.EnemyDies += SpawnPotion;
                enemy.DamagePlayer += HurtPlayerTest;
            }
            //foreach (HealthPotionGameObject potion in potions)
            //   potion.HealPlayer += HealPlayerTest;
            //mousePointer.SelectEnemy += MouseSelectEnemyToAttack;
            //player.DamageEnemy += HurtEnemyTest;
            //player.CollideWithPotion += CollideWithPotionTest;

            ClearMapTypes();            
            mapManager.LoadContent(Resources);
            LoadMapTypes();

            if (currentMapIndex < Resources.Maps.Count)
                currentMap = Resources.Maps[string.Format("Maps/{0}", currentMapIndex)];
            else currentMap = Resources.Maps["Maps/0"];

            currentMapWidth = currentMap.MapList[0].Length * 64;
            currentMapHeight = currentMap.MapList.Count * 64;

            //InitialiseCollidableObjects();

            //entities.Add(player);
            //foreach (EnemyGameObject enemy in enemies)
            //    entities.Add(enemy);

            //foreach (HealthPotionGameObject potion in potions)
            //    entities.Add(potion);

            //foreach (Tree tree in trees)
            //    entities.Add(tree);

            //foreach (Rock rock in rocks)
            //    entities.Add(rock);

            //foreach (GameObjectBase gameObject in entities)
            //{
            //    gameObject.LoadContent(Resources);
            //}            

            mousePointer.LoadContent(Resources);

            InitialiseCollidableObjects();

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = currentMapHeight;
            graphics.PreferredBackBufferWidth = currentMapWidth;
            graphics.ApplyChanges();

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
            //clear registered events
            player.CollideWithEnemy -= CollideWithEnemyTest;
            player.DamageEnemy -= player.Damage;

            //mousePointer.SelectEnemy -= mousePointer.MouseSelectEntity;

            foreach (EnemyGameObject enemy in enemies)
            {
                enemy.DamagePlayer -= HurtPlayerTest;
                enemy.EnemyDies -= SpawnPotion;
            }
                        
            potions.Clear();
            enemies.Clear();
            player = null;
            entities.Clear();
            collisionManager = null;
            Commands = null;
            ClearMapTypes();
            Resources.UnloadContent(Content);
            MediaPlayer.Stop();

            #region Constrain mouse to window - not implemented

            //if (Mouse.GetState().X < 0)

            //    Mouse.SetPosition(0, Mouse.GetState().Y);

            //if (Mouse.GetState().X > ScreenWidth)

            //    Mouse.SetPosition(ScreenHeight, Mouse.GetState().Y);

            //if (Mouse.GetState().Y < 0)

            //    Mouse.SetPosition(Mouse.GetState().X, 0);

            //if (Mouse.GetState().Y > ScreenWidth)

            //    Mouse.SetPosition(Mouse.GetState().X, ScreenHeight);

            #endregion
            base.UnloadContent();
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
            //Nothing currently required here
        }

        private void UpdateMainGame(GameTime gameTime)
        {
            //Update all the entities
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
            }            

            //camera.Update(Player.Position, rotation, zoom, currentMapWidth, currentMapHeight);

            mousePointer.Update(gameTime);

            ResolveRemovals();
            collisionManager.Update();
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            //Nothing here now, but could be filled in with various actions to choose at a game over screen.
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
            spriteBatch.Begin(); //SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            {
                //if (loadNewMap)
                //{
                //    //if (currentMapIndex < Resources.Maps.Count)
                //    //    currentMap = Resources.Maps[string.Format("Maps/{0}", currentMapIndex)];
                //    //else currentMap = Resources.Maps["Maps/0"];

                //    loadNewMap = false;
                //}

                mapManager.Draw(currentMap, Resources.TileSheets["TileSheet/0"], spriteBatch);

                entities = Utilities.SelectionSortList(entities);

                foreach (GameObjectBase entity in entities)
                {                    
                    entity.Draw(gameTime, spriteBatch);
                }

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

            //Vector2 hudLocation = new Vector2(player.Position.X - GraphicsDevice.Viewport.Width / 2, player.Position.Y - GraphicsDevice.Viewport.Height / 2);
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
            Commands.AddKeyboardBindings(Keys.Escape, OnStopGame);
            Commands.AddKeyboardBindings(Keys.Space, OnGoToNextScreen);
            Commands.AddKeyboardBindings(Keys.N, OnGoToNextMap);
            Commands.AddKeyboardBindings(Keys.S, OnSaveGame);
            Commands.AddKeyboardBindings(Keys.L, OnLoadGame);

            if (gameState == GameState.MainGameState)
            {
                Commands.AddMouseBinding(MouseButton.LEFT, player.MoveTowardUnoccupiedMapArea);                
            }
        }

        public void OnStopGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                Exit();
            }
        }        

        public void InitialiseCollidableObjects()
        {
            collisionManager.ClearCollidables();

            collisionManager.AddCollidable(player);

            foreach (GameObjectBase col in entities)
                collisionManager.AddCollidable(col);

            collisionManager.AddCollidable(mousePointer);

            //foreach (HealthPotionGameObject potion in potions)
            //    collisionManager.AddCollidable(potion);

            //foreach (EnemyGameObject enemy in enemies)
            //    collisionManager.AddCollidable(enemy);
        }

        public void ResolveRemovals()
        {
            List<Collidable> toRemove = new List<Collidable>();            

            for (int i = 0; i < entities.Count; i++)
            {
                PotionGameObject potion = entities[i] as PotionGameObject;

                if (potion != null)
                {
                    if (potion.flagForRemoval)
                    {
                        toRemove.Add(potion);
                        entities.RemoveAt(i);
                    }
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                for (int j = 0; j < potions.Count; j++)
                {
                    if (toRemove[i].Equals(potions[j]))
                    {
                        potions.RemoveAt(i);
                        GameInfo.Instance.HealthPotionInfoArray.RemoveAt(i); //both arrays populated at the same time, so indexes should be the same
                    }
                }   
            }

            collisionManager.RemoveCollidable(toRemove);            
        }

        #region event testing
        //TODO: put these methods into appropriate managing class
        private void HurtEnemyTest(object attacker, object receiver, EventArgs e)
        {
            PlayerGameObject player = attacker as PlayerGameObject;
        }

        private void HurtPlayerTest(object sender, EventArgs e)
        {            
            player.HurtPlayer(1);
        }

        //TODO: wire this up
        //private void HealPlayerTest(object sender, EventArgs e)
        //{
        //    player.HealPlayer(50);
        //}

        //private void CollideWithPotionTest(object sender, object passedInPotion, CollisionEventArgs e)
        //{
        //    HealthPotionGameObject potion = passedInPotion as HealthPotionGameObject;

        //    if (potion != null)
        //        potion.OnHealPlayer();
        //}

        private void CollideWithEnemyTest(object sender, object passedInEnemy, CollisionEventArgs e)
        {
            EnemyGameObject enemy = passedInEnemy as EnemyGameObject;

            if (enemy != null)
            {
                player.OnCollisionWithEnemy(enemy);
            }
        }

        /// <summary>
        /// Response to enemy dying event. Random chance that the enemy drops a potion at their position        
        /// </summary>
        /// <param name="position"></param>
        public void SpawnPotion(object sender, EventArgs e)
        {
            EnemyGameObject enemy = sender as EnemyGameObject;

            if (enemy != null && !enemy.droppedPotion)
            {
                int random = Utilities.Rand();
                PotionGameObject potion;

                if (random % 2 == 0)
                    potion = new HealthPotion(enemy.Position);
                else
                    potion = new StrengthPotion(enemy.Position);

                potion.LoadContent(Resources);

                potions.Add(potion);
                entities.Add(potion);
                GameInfo.Instance.HealthPotionInfoArray.Add(new HealthPotionInfo(enemy.Position));

                InitialiseCollidableObjects();

                enemy.droppedPotion = true;
            }
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

        private void ClearMapTypes()
        {
            mapManager.ClearMapTypes();
        }

        private void OnGoToNextScreen(eButtonState buttonState, Vector2 amount)
        {

            if (buttonState == eButtonState.PRESSED)
            {
                nextScreenCalled++;
                spacePressed = true;
            }
        }

        private void OnGoToNextMap(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                loadNewMap = true;

                currentMapIndex++;

                if (currentMapIndex > Resources.Maps.Count)
                    currentMapIndex = 0;

                UnloadMainGameContent();

                InitialiseMainGameState();

                LoadMainGameContent(true);
                
                nextMapCalled++;
            }         
        }

        public void OnSaveGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                GameInfo.Instance.PlayerInfo.Position = player.Position;
                GameInfo.Instance.PlayerInfo.Health = player.Health;
                GameInfo.Instance.PlayerInfo.AttackPower = player.AttackPower;

                GameInfo.Instance.LevelIndex = currentMapIndex;

                for (int i = 0; i < enemies.Count; i++)
                {
                    GameInfo.Instance.EnemyInfoArray[i].Position = enemies[i].Position;
                    GameInfo.Instance.EnemyInfoArray[i].Health = enemies[i].Health;
                }

                for (int i = 0; i < potions.Count; i++)
                {
                    GameInfo.Instance.HealthPotionInfoArray[i].Position = potions[i].Position;
                }

                Resources.WriteSaveFile(@"save.xml");
            }
        }
        
        public void OnLoadGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.PRESSED)
            {
                GameInfo.ClearGameInfoForLoading();

                UnloadMainGameContent();

                InitialiseMainGameState();

                LoadGame("save.xml");
            }
        }

        public void LoadGame(string filename)
        {
            Resources.ReadSaveFile(filename);
            
            Player.Position = GameInfo.Instance.PlayerInfo.Position;
            Player.Health = GameInfo.Instance.PlayerInfo.Health;
            player.AttackPower = GameInfo.Instance.PlayerInfo.AttackPower;

            currentMapIndex = GameInfo.Instance.LevelIndex;

            for (int i = 0; i < GameInfo.Instance.EnemyInfoArray.Count; i++)
                enemies.Add(new EnemyGameObject(GameInfo.Instance.EnemyInfoArray[i].Position, player, GameInfo.Instance.EnemyInfoArray[i].Health, 100));

            for (int i = 0; i < GameInfo.Instance.HealthPotionInfoArray.Count; i++)
                potions.Add(new HealthPotion(GameInfo.Instance.HealthPotionInfoArray[i].Position));

            for (int i = 0; i < GameInfo.Instance.StrengthPotionInfoArray.Count; i++)
                potions.Add(new StrengthPotion(GameInfo.Instance.StrengthPotionInfoArray[i].Position));

            for (int i = 0; i < GameInfo.Instance.TreeInfoArray.Count; i++)
                trees.Add(new Tree(GameInfo.Instance.TreeInfoArray[i].Position));

            for (int i = 0; i < GameInfo.Instance.RockInfoArray.Count; i++)
                rocks.Add(new Rock(GameInfo.Instance.RockInfoArray[i].Position));

            Player.IsAlive = true;

            LoadMainGameContent(false);
        }     
    }
}
