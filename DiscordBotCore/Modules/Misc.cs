using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotCore.Core.UserAccounts;

using System;
using CoreHtmlToImage;

namespace DiscordBotCore.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
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

    }
}
