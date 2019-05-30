using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace DiscordBotCore.Service
{

    class PicturesUtilities
    {
       
        private static Dictionary<string, string> pictures;

        static PicturesUtilities()
        {
            

            string json = File.ReadAllText("SystemLang/pictures.json");

            //convert from json to data structure
           var data = JsonConvert.DeserializeObject<dynamic>(json);
            pictures = data.ToObject<
                Dictionary<string, string>
                >();
        }
        public static string GetPicture(string key)
        {
            if (pictures.ContainsKey(key)) return pictures[key];

            return "";
        }

        internal static object GetAlert(string v)
        {
            throw new NotImplementedException();
        }
    }
}
