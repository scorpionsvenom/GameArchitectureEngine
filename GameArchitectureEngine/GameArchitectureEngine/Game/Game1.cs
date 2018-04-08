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
using System.IO;

namespace GameArchitectureEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ActionRPG : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        
        ResourceManager Resources;
        CommandManager Commands;
        MapManager mapManager;

        CollisionManager collisionManager;

        //TODO: have a dictionary of gameobjectbases, so that appropriate methods can be called on each of these in an easier way
        List<HealthPotionGameObject> potions = new List<HealthPotionGameObject>();
        List<EnemyGameObject> enemies = new List<EnemyGameObject>();

        PlayerGameObject player;
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
            // TODO: Add your initialization logic here
            Resources = new ResourceManager();
            Commands = new CommandManager();
            mapManager = new MapManager(Resources); //TODO: don't think MapManager should need Resources passed in
            collisionManager = new CollisionManager();

            player = new PlayerGameObject();
            player.Initialise();
            

            //TODO: this should be added to the list from the map
            potions.Add(new HealthPotionGameObject(new Vector2(250, 200)));//TODO: set this position from the map
            enemies.Add(new EnemyGameObject(new Vector2(350, 350), player));

            InitialiseCollidableObjects();

            this.IsMouseVisible = true;

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
            player.LoadContent(Resources);
            
            foreach(HealthPotionGameObject potion in potions)
                potion.LoadContent(Resources.SpriteSheets["Sprites/Powerups/Potion"]);

            foreach (EnemyGameObject enemy in enemies)
                enemy.LoadContent(Resources.SpriteSheets["Sprites/Enemies/EnemyIdle"]);

            mapManager.AddMapTileTypes("Earth", (int)enumMapTileType.Earth, 0, 0);
            mapManager.AddMapTileTypes("Grass", (int)enumMapTileType.Grass, 64, 0);
            mapManager.AddMapTileTypes("Water", (int)enumMapTileType.Water, 0, 64);
            mapManager.AddMapTileTypes("Mountain", (int)enumMapTileType.Mountain, 64, 64);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Resources.UnloadContent(Content);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            // TODO: Add your update logic here
            ResolveRemovals();
            collisionManager.Update();
            Commands.Update();

            foreach (EnemyGameObject enemy in enemies)
                enemy.Update(gameTime);

            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin();
            {
                mapManager.Draw(Resources.Maps[@"Maps/0"], spriteBatch);

                foreach(HealthPotionGameObject potion in potions)
                    potion.Draw(gameTime, spriteBatch);

                foreach (EnemyGameObject enemy in enemies)
                    enemy.Draw(gameTime, spriteBatch);

                player.Draw(gameTime, spriteBatch);

                DrawHUD();
            }        
            spriteBatch.End();

            base.Draw(gameTime);
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
            //Resources.SprBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            //Resources.SprBatch.DrawString(font, value, position, colour);
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, colour);
        }

        private void InitialiseBindings()
        {
            Commands.AddKeyboardBindings(Keys.Escape, StopGame);
            Commands.AddMouseBinding(MouseButton.LEFT, player.MoveTowards);
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
    }
}
