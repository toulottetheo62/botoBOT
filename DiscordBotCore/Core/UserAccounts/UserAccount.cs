using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotCore.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }
        public uint Points { get; set; }
        public uint XP { get; set; }

        public DateTime LastDailyReward { get; set; }

        public uint Level
        {
            get
            {
                return (uint)Math.Sqrt(XP / 10);
            }
        }

        public Heroe MainHeroe { get; set; }

        //private Heroe[] heroes; 

    }
}
