using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotCore.Core.UserAccounts;
using System;
using CoreHtmlToImage;
using System.Net;
using Newtonsoft.Json;
using DiscordBotCore.Service;
using System.Globalization;
using System.Text;

namespace DiscordBotCore.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        /* recevoir la fiche de creation de classe par message privée
        [Command("classSheetInPM")]
        public async Task getClassInfo(string message)
        {
            string fName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower());

            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://api-beta.open5e.com/classes/?name=" + fName);
                
            }
            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string desc = dataObject.results[0].desc.ToString();
            //string split lol mdr

            string[] descList = desc.Split( "###" , StringSplitOptions.RemoveEmptyEntries);

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();


            foreach (string item in descList )
            {
                await dmChannel.SendMessageAsync($"{item}");
            }
            
        }
        */

        [Command("class")]
        public async Task GetClassFlavor(string message)
        {
            string fName = GetTitleString(message);
            var res = AlertsUtilities.GetAlert("CLASS&" + fName);
            string json = "";


            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://api-beta.open5e.com/classes/?name=" + fName);   //API DONJON&DRAGON5
                
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            var embed = new EmbedBuilder();
            
            embed.WithTitle(message.ToUpper());
            embed.WithThumbnailUrl(PicturesUtilities.GetPicture("PIC&" + fName));

           
            embed.WithDescription(res);
            embed.AddField("Hit Dice", dataObject.results[0].hit_dice);

            embed.AddField("Armor", dataObject.results[0].prof_armor,true);
            embed.AddField("Weapon", dataObject.results[0].prof_weapons,true);
            embed.AddField("Tool", dataObject.results[0].prof_tools);
            embed.AddField("Saving Throw", dataObject.results[0].prof_saving_throws);
            embed.AddField("Available skills", dataObject.results[0].prof_skills);
            //  embed.AddField("Table", dataObject.results[0].table);  //penser a ajouter la table au data du joueur plutot que de l'affiher ici ( tro long )
            embed .WithColor(new Color(0, 255, 0));
            await Context.Channel.SendMessageAsync(embed : embed.Build());
            
        }

        [Command("race")]
        public async Task GetRaceFlavor(string message)
        {
            string fName = GetTitleString(message);
            Console.WriteLine(fName);
            
            //var res = AlertsUtilities.GetAlert("CLASS&" + fName);
            string json = "";
            //await Context.Channel.SendMessageAsync(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower()));

            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://api-beta.open5e.com/races/?name="+fName);   //API DONJON&DRAGON5
               
            }   
            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            var embed = new EmbedBuilder();

            embed.WithTitle(message.ToUpper());
            //embed.WithThumbnailUrl(PicturesUtilities.GetPicture("PIC&" + fName));
            //embed.WithDescription(dataObject.results[0].desc);

     

            embed.AddField(fName,
                dataObject.results[0].desc +"\n"+
                dataObject.results[0].age + "\n" +
                dataObject.results[0].size + "\n" +
                dataObject.results[0].speed_desc + "\n" +
                dataObject.results[0].languages);
            
            //embed.AddField("Age", dataObject.results[0].age);
            //embed.AddField("alignment", dataObject.results[0].alignment);
            //embed.AddField("size", dataObject.results[0].size);
            //embed.AddField("speed", dataObject.results[0].speed_desc);
            //embed.AddField("languages", dataObject.results[0].languages);
            embed.AddField("vision", dataObject.results[0].vision);
            embed.AddField("traits",dataObject.results[0].traits);
            
            embed.WithColor(new Color(255, 255, 0));
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("spell")]
        public async Task GetSpellInfo([Remainder]string message)
        {
            message = GetTitleString(message);
            message.Replace(' ', '+');
            
            //var res = AlertsUtilities.GetAlert("CLASS&" + fName);
            Console.WriteLine(message);
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://www.dnd5eapi.co/api/spells/?name=" + message);   //API DONJON&DRAGON5 
            }
            string url = JsonConvert.DeserializeObject<dynamic>(json).results[0].url;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(url);   //API DONJON&DRAGON5 
            }
            
            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            var embed = new EmbedBuilder();

            embed.WithTitle(message.ToUpper());

            embed.WithDescription(String.Join('\n',dataObject.desc));
            if(dataObject.higher_level is Newtonsoft.Json.Linq.JArray)
            {
                embed.AddField("higher_level", dataObject.higher_level[0]);
            }
        

            embed.AddField("range", dataObject.range);
            embed.AddField("components", string.Join(',',dataObject.components));
            embed.AddField("ritual", dataObject.ritual);
            embed.AddField("duration", dataObject.duration);
            embed.AddField("concentration", dataObject.concentration);
            embed.AddField("casting_time", dataObject.casting_time);
            embed.AddField("level", dataObject.level);
            

            await Context.Channel.SendMessageAsync(embed: embed.Build());


        }

        private static string GetTitleString(string message)
        {
            //return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower());
            string title = message;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            title = textInfo.ToTitleCase(title.ToLower());
            return(title); 




        }

        [Command("roll")]
        public async Task RollaDice(string message)
        {

            Random rng = new Random();

            if (String.IsNullOrWhiteSpace(message))
            {
                await Context.Channel.SendMessageAsync("You rolled a dice. Result: \n``` Rolled a " + rng.Next(1, 7) + "```");
                
            }

            int numDice, numSides;
            var diceCountSides = message.Split('d');

            if (int.TryParse(message, out numDice) && numDice >= 1 && numDice <= 15)
            {
                var output = new System.Text.StringBuilder();
                output.AppendLine($"You rolled {numDice} dice. Results: \n```");
                for (int i = 1; i <= numDice; i++)
                {
                    output.AppendLine($"dice number {i}: Rolled a {rng.Next(1, 7)}");
                }
                await Context.Channel.SendMessageAsync(output.ToString() + "```");
                
            }

            if (diceCountSides.Length == 2)
            {
                //parse the input (format xdy: x dice with y sides each)
                if (int.TryParse(diceCountSides[0], out numDice) && numDice >= 1 && numDice <= 15 && int.TryParse(diceCountSides[1], out numSides) && numSides >= 1)
                {
                    var output = new StringBuilder();
                    output.AppendLine($"You rolled {numDice} dice with {numSides} sides each. Results: \n```");
                    for (int i = 1; i <= numDice; i++)
                    {
                        output.AppendLine($"Dice number {i}: Rolled a {rng.Next(1, numSides + 1)}");
                    }
                    await Context.Channel.SendMessageAsync(output.ToString() + "```");
                    
                }
                
            }
            
        }
        
        [Command("HelloHtmlPng")]
        public async Task Hello()
        {
            string css = "<style>\n    h1{\n        background-color: " + "red" + ";\n    }\n</style>\n"; //pas tres beau mais pourrait permmetre de contruire des fiches de personnage
            string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);

            var converter = new HtmlConverter();

            var jpgBytes = converter.FromHtmlString(html+css,250);

            // = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);

            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");

        }


        [Command("dailyreward")]
        public async Task DailyReward()
        {
            var account = UserAccounts.GetAccount(Context.User);
            TimeSpan span = DateTime.Now.Subtract(account.LastDailyReward);
            if (span.Hours >= 24 || account.LastDailyReward == DateTime.MinValue)
            {
                account.XP += 5;
                account.LastDailyReward = DateTime.Now;
                UserAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync("Daily reward you earn 5 xp pts");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"You already take your daily reward at {account.LastDailyReward.ToString()}");
            }
            

           

        }

        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("to :" + Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("secret")]
        public async Task Secret([Remainder]string arg = "")
        {
            if (!UserIsSecretOwner((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You dont have permission <3");
            }
            else {

                var dmChannel = await Context.User.GetOrCreateDMChannelAsync();


                await dmChannel.SendMessageAsync(AlertsUtilities.GetAlert("SECRET"));
            }
        }

        [Command("stats")]
        public async Task MyStats(SocketGuildUser user)
        {
            var account = UserAccounts.GetAccount(user);
            await Context.Channel.SendMessageAsync($"{user.Username} is level : {account.Level}");
        }

        [Command("addXp")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddXp(uint xp,SocketGuildUser user)
        {


            var account = UserAccounts.GetAccount(user);
            uint oldLevel = account.Level;
            
            account.XP += xp;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync(AlertsUtilities.GetFormattedAlert("GIVEXP_&NAME_&XP", user.Username, xp));

            if(account.Level != oldLevel)
                await Context.Channel.SendMessageAsync($"also, {user.Username} leveled UP");

        }

        [Command("resetStat")]
        public async Task DelAccount(SocketGuildUser user)
        {

            var account = UserAccounts.GetAccount(user);
            UserAccounts.DeleteUserAccount(account);

            await Context.Channel.SendMessageAsync($"{user.Username} a été effacé.");
        }





        private bool UserIsSecretOwner(SocketGuildUser user)
        {
            String targetRoleName = "SecretOwner";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong roleId = result.FirstOrDefault();
            if (roleId == 0) return false;

            var targetRole = user.Guild.GetRole(roleId);
            return user.Roles.Contains(targetRole);
        }

        private bool IsPrivateMessage(SocketMessage msg)
        {
            return (msg.Channel.GetType() == typeof(SocketDMChannel));
        }

    }
}
