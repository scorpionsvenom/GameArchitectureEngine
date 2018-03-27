using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Diagnostics;

namespace GameArchitectureEngine
{
    public class ResourceManager
    {
        /// <summary>
        /// TODO
        /// replace individual elements with dictionaries
        /// ie all sprites in a dictionary together keyed off name
        /// which is read from file name and then used to reference them
        /// </summary>
        #region publics
        //either have a dictionary of these dictionaries or a list so initialising and disposing of them is easy
        public Dictionary<string, Texture2D> SpriteSheets;
        public Dictionary<string, Texture2D> TileSheets;
        public Dictionary<string, Texture2D> Overlays;
        public Dictionary<string, SpriteFont> Fonts;
        public Dictionary<string, SoundEffect> SFX;
        public Dictionary<string, Song> Songs;
        //public Dictionary<string, Map> Maps;

        #endregion

        #region privates
        private List<string> completePaths = new List<string>();

        private SpriteBatch spriteBatch;

        private SpriteFont hudFont;

        private Texture2D successOverlay;
        private Texture2D pauseOverlay;
        private Texture2D failureOverlay;

        private Texture2D playerIdle;
        private Texture2D playerWalk;
        private Texture2D playerAttack;
        private Texture2D playerDie;

        private Texture2D playerSwordSwing;

        private Texture2D enemy1Idle;
        private Texture2D enemy1Walk;
        private Texture2D enemy1Attack;
        private Texture2D enemy1Die;

        private Texture2D tileSet;

        private int numberOfLevels;

        private Song[] songs;
        private SoundEffect[] sfx;
        #endregion

        #region Accessors
        public SpriteBatch SprBatch
        {
            get { return spriteBatch; }
        }

        //public SpriteFont HudFont
        //{
        //    get { return hudFont; }
        //}

        //public Texture2D SuccessOverlay
        //{
        //    get { return successOverlay; }
        //}

        //public Texture2D PauseOverlay
        //{
        //    get { return pauseOverlay; }
        //}

        //public Texture2D FailureOverlay
        //{
        //    get { return failureOverlay; }
        //}

        //public Texture2D PlayerIdle
        //{
        //    get { return playerIdle; }
        //}

        //public Texture2D PlayerWalk
        //{
        //    get { return playerWalk; }
        //}

        //public Texture2D PlayerAttack
        //{
        //    get { return playerAttack; }
        //}

        //public Texture2D PlayerDie
        //{
        //    get { return playerDie; }
        //}

        //public Texture2D PlayerSwordSwing
        //{
        //    get { return playerSwordSwing; }
        //}

        //public Texture2D Enemy1Idle
        //{
        //    get { return enemy1Idle; }
        //}

        //public Texture2D Enemy1Walk
        //{
        //    get { return enemy1Walk; }
        //}

        //public Texture2D Enemy1Attack
        //{
        //    get { return enemy1Attack; }
        //}

        //public Texture2D Enemy1Die
        //{
        //    get { return enemy1Die; }
        //}

        //public Texture2D TileSet
        //{
        //    get { return tileSet; }
        //}

        //public int NumberOfLevels
        //{
        //    get { return numberOfLevels; }
        //}

        ////public Song[] Songs
        ////{
        ////    get { return songs; }
        ////}

        public SoundEffect[] Sfx
        {
            get { return sfx; }
        }
        #endregion

        ///<summary>
        ///Load the content
        ///</summary>
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            //Initialise the dictionaries
            TileSheets = new Dictionary<string, Texture2D>();
            SpriteSheets = new Dictionary<string, Texture2D>();
            SFX = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();
            Fonts = new Dictionary<string, SpriteFont>();
            Overlays = new Dictionary<string, Texture2D>();

            //Get current working directory
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            //List the files
            DirectorySearch(directory);
            
            //Add files to dictionaries
            addAssetsToDictionaries(completePaths, Content);
            
            spriteBatch = new SpriteBatch(graphicsDevice);

            //hudFont = Content.Load<SpriteFont>("Fonts/Hud");

            //failureOverlay = Content.Load<Texture2D>("Overlays/Failure");

            //playerWalk = Content.Load<Texture2D>("Sprites/Player/WalkSpriteSheet");
            //playerSwordSwing = Content.Load<Texture2D>("Sprites/Player/Swordswing");

            //tileSet = Content.Load<Texture2D>("TileSheet/Terrains");
        }

        public void UnloadContent(ContentManager Content)
        {
            //Is it really this easy?
            Content.Unload();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/929276/how-to-recursively-list-all-the-files-in-a-directory-in-c
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private void DirectorySearch(string directory)
        {
            try
            {
                foreach (string dir in Directory.GetDirectories(directory))
                {
                    foreach (string file in Directory.GetFiles(dir))
                    {
                        //Console.WriteLine(file);
                        completePaths.Add(file);
                    }
                    DirectorySearch(dir);
                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        private void addAssetsToDictionaries(List<string> paths, ContentManager Content)
        {
            string cleanedPath;
            string searchStringContent = @"Content\";
            int length = searchStringContent.Length;
            int extensionLength = 4; //length of .xnb

            string tileSheet = "TileSheet";
            string fonts = "Fonts";
            string overlays = "Overlays";
            string sprites = "Sprites";
            string maps = "Maps";
            string sounds = "Sounds";
            string songs = "Songs";

            foreach(string path in paths)
            {
                cleanedPath = path.Substring(path.IndexOf(searchStringContent) + length);
                cleanedPath = cleanedPath.Substring(0, cleanedPath.Length - extensionLength);
                cleanedPath = cleanedPath.Replace(@"\", "/");

                if (path.Contains(tileSheet))
                {                    
                    TileSheets.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                    //Console.WriteLine("Asset loaded to dictionary: " + TileSheets[cleanedPath].ToString());
                }
                else if (path.Contains(sprites))
                {
                    SpriteSheets.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                    Console.WriteLine("sprites found: " + cleanedPath);
                }
                else if (path.Contains(fonts))
                {
                    Fonts.Add(cleanedPath, Content.Load<SpriteFont>(cleanedPath));
                    //Console.WriteLine("fonts found: " + cleanedPath);
                }
                else if (path.Contains(maps))
                {
                    //Console.WriteLine("maps found: " + cleanedPath);
                }
                else if (path.Contains(overlays))
                {
                    Overlays.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                    //Console.WriteLine("overlays found: " + cleanedPath);
                }
                else if (path.Contains(sounds))
                {
                    SFX.Add(cleanedPath, Content.Load<SoundEffect>(cleanedPath));
                    //Console.WriteLine("sounds found: " + cleanedPath);
                }
                else if (path.Contains(songs))
                {
                    Songs.Add(cleanedPath, Content.Load<Song>(cleanedPath));
                    //Console.WriteLine("songs found: " + cleanedPath);
                }
                else
                {
                    //Throw a wobbly
                    Console.WriteLine("Not found it: " + path);
                }              
            }
        }
    }
}
