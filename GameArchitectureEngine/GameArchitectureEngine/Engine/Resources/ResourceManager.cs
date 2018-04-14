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
        public Dictionary<string, Texture2D> SpriteSheets;
        public Dictionary<string, Texture2D> TileSheets;
        public Dictionary<string, Texture2D> Overlays;
        public Dictionary<string, SpriteFont> Fonts;
        public Dictionary<string, SoundEffect> SFX;
        public Dictionary<string, Song> Songs;
        public Dictionary<string, Map> Maps;
        public Dictionary<string, Texture2D> Screens;

        #endregion

        #region privates
        private List<Object> dictionaries;

        private List<string> completePaths = new List<string>();

        private GraphicsDevice graphicsDevice;

        private FileLoader fileLoader;

        int extensionLength = 4;
        #endregion

        #region Accessors

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }
        #endregion

        ///<summary>
        ///Load the content
        ///</summary>
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            dictionaries = new List<Object>();

            //Initialise the dictionaries
            TileSheets = new Dictionary<string, Texture2D>();
            dictionaries.Add(TileSheets);

            SpriteSheets = new Dictionary<string, Texture2D>();
            dictionaries.Add(SpriteSheets);

            SFX = new Dictionary<string, SoundEffect>();
            dictionaries.Add(SFX);

            Songs = new Dictionary<string, Song>();
            dictionaries.Add(Songs);
            
            Fonts = new Dictionary<string, SpriteFont>();
            dictionaries.Add(Fonts);

            Overlays = new Dictionary<string, Texture2D>();
            dictionaries.Add(Overlays);

            Maps = new Dictionary<string, Map>();
            dictionaries.Add(Maps);

            Screens = new Dictionary<string, Texture2D>();
            dictionaries.Add(Screens);

            //Get current working directory
            string directory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            //List the files
            DirectorySearch(directory);

            //Add files to dictionaries
            addAssetsToDictionaries(completePaths, Content);

            this.graphicsDevice = graphicsDevice;    
        }

        public void UnloadContent(ContentManager Content)
        {
            //Clear all the dictionaries and unload the content.
            foreach(Object obj in dictionaries)
            {
                Dictionary<string, Texture2D> texture2Ddictionary = obj as Dictionary<string, Texture2D>;
                Dictionary<string, SoundEffect> sfxDictionary = obj as Dictionary<string, SoundEffect>;
                Dictionary<string, Song> songDictionary = obj as Dictionary<string, Song>;
                Dictionary<string, SpriteFont> fontDictionary = obj as Dictionary<string, SpriteFont>;
                Dictionary<string, Map> mapDictionary = obj as Dictionary<string, Map>;                

                if (texture2Ddictionary != null)
                    texture2Ddictionary.Clear();

                if (sfxDictionary != null)
                    sfxDictionary.Clear();

                if (songDictionary != null)
                    songDictionary.Clear();

                if (fontDictionary != null)
                    fontDictionary.Clear();

                if (mapDictionary != null)
                    mapDictionary.Clear();
            }                

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
                        completePaths.Add(file);
                    }
                    DirectorySearch(dir);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        private void addAssetsToDictionaries(List<string> paths, ContentManager Content)
        {
            string cleanedPath;
            string searchStringContent = @"Content\";
            int length = searchStringContent.Length;
             //length of .xnb

            string tileSheet = "TileSheet";
            string fonts = "Fonts";
            string overlays = "Overlays";
            string sprites = "Sprites";
            string maps = "Maps";
            string songs = "Songs";
            string sfx = "SFX";
            string screens = "Screens";

            //TODO make this less reliant on the game programmer remembering to include all these types of asshats!!! maybe just some error checking            
            foreach (string path in paths)
            {
                cleanedPath = CleanPath(path.Substring(path.IndexOf(searchStringContent) + length));

                if (path.Contains(tileSheet))
                {
                    if (!TileSheets.ContainsKey(cleanedPath))
                        TileSheets.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                }
                else if (path.Contains(sprites))
                {
                    if (!SpriteSheets.ContainsKey(cleanedPath))
                        SpriteSheets.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                }
                else if (path.Contains(fonts))
                {
                    if (!Fonts.ContainsKey(cleanedPath))
                        Fonts.Add(cleanedPath, Content.Load<SpriteFont>(cleanedPath));
                }
                else if (path.Contains(maps))
                {
                    if (!Maps.ContainsKey(cleanedPath))
                    {
                        fileLoader = new FileLoader();

                        Map map = fileLoader.ReadMap(path);

                        Maps.Add(cleanedPath, map);
                    }
                }
                else if (path.Contains(overlays))
                {
                    if (!Overlays.ContainsKey(cleanedPath))
                        Overlays.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                }
                else if (path.Contains(sfx))
                {
                    if (!SFX.ContainsKey(cleanedPath))
                        SFX.Add(cleanedPath, Content.Load<SoundEffect>(cleanedPath));
                }
                else if (path.Contains(songs))
                {
                    if (!Songs.ContainsKey(cleanedPath))
                        Songs.Add(cleanedPath, Content.Load<Song>(cleanedPath));
                }
                else if (path.Contains(screens))
                {
                    if (!Screens.ContainsKey(cleanedPath))
                        Screens.Add(cleanedPath, Content.Load<Texture2D>(cleanedPath));
                }
                else
                {
                    //Throw a wobbly
                    //Console.WriteLine("Not found it: " + path);
                }
            }
        }

        public void LoadMap(string filename)
        {            
            filename = filename.Substring(filename.IndexOf("Content")); 
        }

        public string CleanPath(string path)
        {
            string cleanedPath = path;
            cleanedPath = cleanedPath.Substring(0, cleanedPath.Length - extensionLength);
            cleanedPath = cleanedPath.Replace(@"\", "/");

            return cleanedPath;
        }
    }
}
