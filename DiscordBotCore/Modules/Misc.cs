using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBotCore.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("to :" + Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("",false,embed.Build());
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
