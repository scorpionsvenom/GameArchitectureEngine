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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Resources = new ResourceManager();

            Resources.LoadContent(Content, GraphicsDevice);

            using (Stream fileStream = TitleContainer.OpenStream(@"Content/XML/info.xml"))
            {
                FileLoader fileLoader = new FileLoader(fileStream);

                fileLoader.ReadXML(@"Content/XML/info.xml");

                Console.WriteLine("Position: {0}, {1}", GameInfo.Instance.GameObjectBase.Position.X, GameInfo.Instance.GameObjectBase.Position.Y);
                Console.WriteLine("Rotation: {0}", GameInfo.Instance.GameObjectBase.Rotation);
            }
            // TODO: use this.Content to load your game content here
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
            Resources.SprBatch.Begin();

            DrawHUD();

            Resources.SprBatch.End();


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

            DrawShadowedString(Resources.HudFont, message, hudLocation, messageColour);

        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color colour)
        {
            Resources.SprBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            Resources.SprBatch.DrawString(font, value, position, colour);
        }
    }
}
