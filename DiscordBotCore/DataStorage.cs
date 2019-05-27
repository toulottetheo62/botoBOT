using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
namespace DiscordBotCore
{
    class DataStorage
    {
        public static Dictionary<string, string> pairs = new Dictionary<string, string>() ;

        static DataStorage()
        {

            if (!ValidateStorageFile("DataStorage.json")) return;
                 

            //json in a string
            string json = File.ReadAllText("DataStorage.json");
           
            //convert from json to data structure
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
           
               
                
        }

        public static void SaveData()
        {
            //save data
            string json = JsonConvert.SerializeObject(pairs);
            File.WriteAllText("DataStorage.json", json);
        }

        private static bool ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }

    }

   
}
