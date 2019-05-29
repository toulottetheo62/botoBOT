using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace DiscordBotCore
{

    class Utilities
    {
        private static Dictionary<string, string> alerts;
        private static Dictionary<string, string> pictures;

        static Utilities()
        {
            //json in a string
            string json = File.ReadAllText("SystemLang/alerts.json");

            //convert from json to data structure
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<
                Dictionary<string, string>
                >();

            json = File.ReadAllText("SystemLang/pictures.json");

            //convert from json to data structure
            data = JsonConvert.DeserializeObject<dynamic>(json);
            pictures = data.ToObject<
                Dictionary<string, string>
                >();
        }
        public static string GetPicture(string key)
        {
            if (pictures.ContainsKey(key)) return pictures[key];

            return "";
        }

        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];

            return "";
        }
    
        public static string GetFormattedAlert(string key,params object[] parameter)
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key],parameter);
            }
            return "";
        }

        public static string GetFormattedAlert(string key, object parameter)
        {
            return GetFormattedAlert(key, new object[] { parameter }); 
        }

        




    }
}
