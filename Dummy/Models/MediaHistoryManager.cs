using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Playlist.Models
{
    static class MediaHistoryManager
    {
        private static StreamWriter sw;        
        private static string logFilePath;

        static MediaHistoryManager()
        {
            logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\xplayer-log.txt";
        }
        
        public static void SaveMediaPath(string path)
        {
            sw = new StreamWriter(logFilePath);
            sw.WriteLine(path);
            sw.Close();
        }

        public static string GetMediaPath()
        {
            if(File.Exists(logFilePath))
            {
                string path = File.ReadAllText(logFilePath);
                return path;
            }
            return "";
        }
    }
}
