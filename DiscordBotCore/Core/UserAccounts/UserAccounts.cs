﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBotCore.Core.UserAccounts  
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;

        static UserAccounts()
        {

        }

        public static UserAccount GetAccount(SocketUser user) //translation between socket & ID
        {
            return GetOrCreateAccount(user.Id); 
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.ID == id
                         select a;

            var account = result.FirstOrDefault();

            if (account == null) account = CreateUserAccount(id);
            
            return account;         
        
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Points = 10,
                XP = 0
            };

            accounts.Add(newAccount);
            return newAccount;
        }
    }
}