using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

/// <summary>
/// Got this from Lab02
/// </summary>
namespace GameArchitectureEngine
{
    public class FileLoader
    {
        const char DELIMITER = ',';
        private Stream mFileStream = null;

        public FileLoader()
        {
        }

        public FileLoader(Stream stream)
        {
            mFileStream = stream;
        }

        public String readTextFileComplete()
        {
            StringBuilder result = new StringBuilder();

            try
            {
                using (StreamReader reader = new StreamReader(mFileStream))
                {
                    result.Append(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File could not be read");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            return result.ToString();
        }

        public List<string> ReadLinesFromTextFile()
        {
            string line = "";

            List<string> lines = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(mFileStream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File could not be read");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            return lines;
        }

        public void ReadXML(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    GameInfo.Instance = (GameInfo)new XmlSerializer(typeof(GameInfo)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: XML file could not be deserialised");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }

        public Map ReadMap(string path)
        {
            int lineNo = 0;
            List<string[]> strings = new List<string[]>();

            try
            {
                using (StreamReader fileStream = new StreamReader(path))
                {
                    string line;

                    while (!string.IsNullOrEmpty(line = fileStream.ReadLine()))
                    {
                        string[] lineElements = line.Split(DELIMITER);

                        strings.Add(lineElements);

                        lineNo++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: File could not be read");
                Console.WriteLine("Exception Message: " + e.Message);
            }

            return new Map(strings);
        }
    }
}
