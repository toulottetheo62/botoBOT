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

namespace DiscordBotCore.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        /*
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
            var res = Utilities.GetAlert("CLASS&" + fName);
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://api-beta.open5e.com/classes/?name=" + fName);
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            var embed = new EmbedBuilder();

            //embed.WithThumbnailUrl(Utilities.GetPicture("PIC&" + fName));

            embed.WithTitle(message.ToUpper());
            embed.WithImageUrl(Utilities.GetPicture("PIC&" + fName));

            //embed.AddField("", new EmbedBuilder().WithImageUrl(Utilities.GetPicture("PIC&" + fName)), true);
            
            embed.WithDescription(res);
            //embed.AddField(new EmbedFieldBuilder());
            embed.AddField("Hit Dice", dataObject.results[0].hit_dice);

            embed.AddField("Armor", dataObject.results[0].prof_armor,true);
            embed.AddField("Weapon", dataObject.results[0].prof_weapons,true);
            embed.AddField("Tool", dataObject.results[0].prof_tools);
            embed.AddField("Saving Throw", dataObject.results[0].prof_saving_throws);
            embed.AddField("Available skills", dataObject.results[0].prof_skills);
            //embed.AddField("Table", dataObject.results[0].table);
            //embed .WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync(embed : embed.Build());
            //await Context.Channel.SendMessageAsync(res);
        }

        private static string GetTitleString(string message)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(message.ToLower());
        }

        [Command("Hello")]
        public async Task Hello()
        {
            string css = "<style>\n    h1{\n        background-color: " + "red" + ";\n    }\n</style>\n";
            string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);

            var converter = new HtmlConverter();

            var jpgBytes = converter.FromHtmlString(html+css,250);

            // = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);

            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");

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


                await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET"));
            }
        }
        [Command("mystats")]
        public async Task MyStats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("MYSTATS_&XP_&PTS", account.XP, account.Points));
        }

        [Command("addXp")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddXp(uint xp)
        {
            var account = UserAccounts.GetAccount(Context.User);
            account.XP += xp;
            UserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("GIVEXP_&NAME_&XP", Context.User.Username, xp));



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
