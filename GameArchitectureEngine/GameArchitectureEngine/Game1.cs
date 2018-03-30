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

        PlayerGameObject player;

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

            player = new PlayerGameObject();

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

            //using (Stream fileStream = TitleContainer.OpenStream(@"Content/XML/info.xml"))
            //{
            //    FileLoader fileLoader = new FileLoader(fileStream);

            //    fileLoader.ReadXML(@"Content/XML/info.xml");

            //    Console.WriteLine("Position: {0}, {1}", GameInfo.Instance.GameObjectBase.Position.X, GameInfo.Instance.GameObjectBase.Position.Y);
            //    Console.WriteLine("Rotation: {0}", GameInfo.Instance.GameObjectBase.Rotation);
            //}
            // TODO: use this.Content to load your game content here
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
            Commands.Update();
            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here
            //Resources.SprBatch.Begin();
            spriteBatch.Begin();

            player.Draw(gameTime, spriteBatch);

            DrawHUD();

            //Resources.SprBatch.End();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHUD()
        {
            Rectangle TitleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(TitleSafeArea.X, TitleSafeArea.Y);
            Vector2 centre = new Vector2(TitleSafeArea.X + TitleSafeArea.Width / 2.0f,
                                         TitleSafeArea.Y + TitleSafeArea.Height / 2.0f);

            string message = "Some stuffs";
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
            Commands.AddKeyboardBindings(Keys.Up, MoveUp);
            Commands.AddMouseBinding(MouseButton.LEFT, player.MoveTowards);
        }

        public void StopGame(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                Exit();
            }
        }

        public void MoveUp(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {

            }
        }
    }
}
