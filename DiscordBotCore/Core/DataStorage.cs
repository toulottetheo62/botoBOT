using System;
using System.Collections.Generic;
using System.Text;
using DiscordBotCore.Core.UserAccounts;
using Newtonsoft.Json;
using System.IO;
namespace DiscordBotCore.Core
{
    public static class DataStorage
    {
        //savealluseraccounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {

            try
            {
                string json = JsonConvert.SerializeObject(accounts);
                File.WriteAllText(filePath, json);
            }
            catch (FileNotFoundException) { /* -- */ };
            
        }

        //getalluseraccounts        
        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<UserAccount>>(json);

        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
