﻿
namespace TradingBot.CommandPrompt
{
	using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TradingBot.Common;
    using TradingBot.Core;
    using TradingBot.Core.Enums;
    using TradingBot.Data.Entities;
    using TradingBot.Services;
    using Yobit.Api;

    public class Program
    {
        static User CurrentUser { get; set; }

        private static async Task Main(string[] args)
		{
            Console.WriteLine("Use /help to get list of commands");
            var input = "";
            var command = Command.None;
            do
            {
                Console.Write(string.Format("{0}> ", CurrentUser == null ? "Anonymous" : CurrentUser.Username));
                input = Console.ReadLine().ToLowerInvariant().Trim();
                if (input.Length < 1 || input[0] != '/')
                    continue;
                input = input.Substring(1);
                var parts = input.Split(' ');

                command = Command.Commands.FirstOrDefault(m => m.Aliases.Contains(parts[0]));
                if (command == null)
                    command = Command.None;

                var parameters = parts.Skip(1).ToArray();
                if (!command.AllowAnonymous && CurrentUser == null)
                    Console.WriteLine("You are not authorized");
                else
                    switch (command.Type)
                    {
                        case CommandEnum.Help:
                            Console.WriteLine("\n\rAvailable commands:\n\r\n\r" +
                                string.Join("\n\r", Command.Commands.Where(m => m.Type != CommandEnum.None)
                                .Select(m => m.Info)));
                            break;
                        case CommandEnum.RegisterUser:
                            CurrentUser = RegisterUser(parameters);
                            break;
                        case CommandEnum.Login:
                            CurrentUser = LoginUser(parameters);
                            break;
                        case CommandEnum.Logout:
                            CurrentUser = LogoutUser();
                            break;
                        case CommandEnum.GetPairs:
                            GetPairs(parameters);
                            break;
                        case CommandEnum.GetPairInfo:
                            GetPairInfo(parameters);
                            break;
                    }
            }
            while (command.Type != CommandEnum.Exit);

            Console.Write("You are finished. Thanks");
            Console.ReadLine();
            
		}


        private static User RegisterUser(params string[] list)
        {
            if (list.Length != 2)
            {
                Console.WriteLine("You must enter your username and password");
                return null;
            }

            using (var usrService = new UserService())
            {
                var usr = usrService.RegisterUser(list[0].Trim(), list[1].Trim());
                if (usr == null)
                {
                    Console.WriteLine("Such username already registered");
                    return usr;
                }

                Console.WriteLine("Registration complete");

                return usr;
            }
        }

        private static User LoginUser(params string[] list)
        {
            if (list.Length != 2)
            {
                Console.WriteLine("You must enter your username and password");
                return null;
            }

            using (var usrService = new UserService())
            {
                var usr = usrService.AuthenticateUser(list[0].Trim(), list[1].Trim());
                if (usr == null)
                {
                    Console.WriteLine("Incorrect username or password");
                    return usr;
                }

                Console.WriteLine("You are logged in");

                return usr;
            }
        }

        private static User LogoutUser()
        {
            if (CurrentUser == null)
                Console.WriteLine("You are not authorized");

            return null;
        }

        private static void GetPairInfo(params string[] list)
        {
            if (list.Length != 2)
            {
                Console.WriteLine("You must enter Exchange type and Ticker code");
                return;
            }
            var tickerCode = list[1];
            var param = list[0];
            int type;
            if (!int.TryParse(param, out type))
            {
                Console.WriteLine(string.Format("You must enter valid Exchange type: {0}", ExchangeInfo.GetAccountTypes));
                return;
            }

            using (var pairService = new PairService())
            {
                var info = pairService.GetPair((AccountType)type, tickerCode);
                if (info == null)
                    Console.WriteLine("Nothing found");
                else
                    Console.WriteLine(string.Format("Ticker info: {0}", JsonHelper.ToJson(info)));
            }
        }

        private static void GetPairs(params string[] list)
        {
            if (list.Length != 1)
            {
                Console.WriteLine("You must enter only Exchange type");
                return;
            }

            var param = list.FirstOrDefault();
            int type = 0;
            if (!int.TryParse(param, out type))
            {
                Console.WriteLine(string.Format("You must enter valid Exchange type: {0}", ExchangeInfo.GetAccountTypes));
                return;
            }

            var eType = (AccountType)type;
            if (!ExchangeInfo.Exchanges.ContainsKey(eType))
            {
                Console.WriteLine("It's not implemented Exchange type");
                return;
            }

            var exchange = ExchangeInfo.Exchanges[eType];
            using (ExchangeApi api = exchange.Api)
            {
                try
                {
                    dynamic result = null;
                    using (var pairService = new PairService())
                    {
                        result = pairService.PullPairs(api);
                    }
                    if (result != null && result.IsSuccess)
                        Console.WriteLine("Pairs info received, you can use /tickerInfo [tickerCode] to get appropriate info");
                    else
                        Console.WriteLine(string.Format("Error when pull pairs: {0}", result));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected error");
                }
            }
        }
    }
}