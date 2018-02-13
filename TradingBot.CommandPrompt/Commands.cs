﻿
namespace TradingBot.CommandPrompt
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TradingBot.Common;
    using TradingBot.Core;
    using TradingBot.Core.Enums;
    using TradingBot.Data.Entities;
    using TradingBot.Services;
    using Yobit.Api;

    public enum CommandEnum
    {
        None,
        Exit,
        Help,
        RegisterUser,
        Login,
        Logout,
        AddAccount,
        RemoveAccount,
        GetAccounts,
        GetPairs,
        GetPairInfo,
        GetActiveOrders,
    }

    public class Command
    {
        public CommandEnum Type { get; set; }

        public List<string> Aliases { get; set; }

        public string Description { get; set; }

        public bool AllowAnonymous { get; set; }

        public string Info
        {
            get
            {
                return string.Format("{3}- Command '{0}', aliases: {1}, Info: {2}\n\r", Type.ToString(),
                    string.Join(", ", Aliases.Select(m => string.Format("{0}", m))), Description, AllowAnonymous ? "" : "[ONLY WHEN AUTHORIZED] ");
            }
        }

        public Command()
        {
            AllowAnonymous = true;
            Aliases = new List<string>();
        }

        public Command(CommandEnum type, List<string> aliases, string description = "", bool allowAnonym = true)
        {
            Type = type;
            Aliases = aliases;
            Description = description;
            AllowAnonymous = allowAnonym;
        }

        public Command(CommandEnum type, List<string> aliases, bool allowAnonym, string description = "")
        {
            Type = type;
            Aliases = aliases;
            Description = description;
            AllowAnonymous = allowAnonym;
        }
    }

    public class CommandsHelper
    {

        public static List<Command> List;

        public static Command None = new Command { Type = CommandEnum.None };

        static CommandsHelper()
        {
            List = new List<Command>();

            List.Add(None);

            List.Add(
                new Command(CommandEnum.Exit, new List<string> { "exit", "e", "finish", "esc" }, "Finish the program, amazing?")
            );

            List.Add(
                new Command(CommandEnum.Help, new List<string> { "help", "h", "?" }, "Show all valid commands")
            );

            List.Add(
                new Command(CommandEnum.RegisterUser, new List<string> { "register", "signup", "create-user" }, "Register user to allow to add accounts and get more private functionality. Parameters (* - required): username*, password*")
            );

            List.Add(
                new Command(CommandEnum.Login, new List<string> { "login", "signin", "let-me-enter", "authorization" }, "Sign in to add accounts and get more private functionality. Parameters (* - required): username*, password*")
            );

            List.Add(
                new Command(CommandEnum.Logout, new List<string> { "logout", "signoff", "signout", "lock" }, false, "Allow you to sign out, you will not have access to private functionality")
            );

            List.Add(
                new Command(CommandEnum.AddAccount, new List<string> { "addaccount", "add-account" }, false, "Allow you to add private Api key")
            );

            List.Add(
               new Command(CommandEnum.GetAccounts, new List<string> { "my-accounts", "get-accounts" }, false, "Allow you to view all your accounts (for example to know which accountId you should use when run private commands)")
           );

            //Commands.Add(
            //    new Command(CommandEnum.RemoveAccount, new List<string> { "removeaccount", "remove-account", "rem-account", "del-account", "delete-account" }, false, "Allow you to remove private Api key")
            //);

            List.Add(
                new Command(CommandEnum.GetActiveOrders, new List<string> { "get-orders", "orders" }, false, "Allow you to get your active orders")
            );

            List.Add(
              new Command(CommandEnum.GetPairs, new List<string> { "getpairs", "all", "info" },
              string.Format("Get all list of tickers with basic statistics. Parameters (* - required): exchangeType* ({0})",
              ExchangeInfo.GetAccountTypes))
            );

            List.Add(
              new Command(CommandEnum.GetPairInfo, new List<string> { "getpairinfo", "tickerinfo", "get-ticker-info", "get-pair-info", "pair-info", "ticker-info" },
              string.Format("Get basic ticker info. It uses info stored from last 'getpairs'. Parameters (* - required): exchangeType* ({0}), tickerCode* ", ExchangeInfo.GetAccountTypes))
            );
        }

        public User CurrentUser { get; private set; }

        public CommandsHelper()
        {
        }

        public void ExecuteCommand(CommandEnum command, params string[] list)
        {
            var com = command.ToString();
            var method = typeof(CommandsHelper).GetMethod(com);
            if (method == null)
            {
                Console.WriteLine("Such method is not supported");
                return;
            }
            method.Invoke(this, new[] { list });
        }

        public void Help(params string[] list)
        {
            Console.WriteLine("\n\rAvailable commands:\n\r\n\r" +
                string.Join("\n\r", List.Where(m => m.Type != CommandEnum.None)
                .Select(m => m.Info)));
        }

        public void RegisterUser(params string[] list)
        {
            if (list.Length != 2)
            {
                Console.WriteLine("You must enter your username and password");
                return;
            }

            using (var usrService = new UserService())
            {
                var usr = usrService.RegisterUser(list[0].Trim(), list[1].Trim());
                if (usr == null)
                {
                    Console.WriteLine("Such username already registered");
                    return;
                }

                Console.WriteLine("Registration complete");

                CurrentUser = usr;
            }
        }

        public void Login(params string[] list)
        {
            if (list.Length != 2)
            {
                Console.WriteLine("You must enter your username and password");
                return;
            }

            using (var usrService = new UserService())
            {
                var usr = usrService.AuthenticateUser(list[0].Trim(), list[1].Trim());
                if (usr == null)
                {
                    Console.WriteLine("Incorrect username or password");
                    return;
                }

                Console.WriteLine("You are logged in");

                CurrentUser = usr;
            }
        }

        public void Logout(params string[] list)
        {
            if (CurrentUser == null)
                Console.WriteLine("You are not authorized");

            CurrentUser = null;
        }

        public void GetAccounts(params string[] list)
        {
            if (CurrentUser == null)
                Console.WriteLine("You are not authorized");

            using (var accService = new AccountService())
            {
                var result = accService.GetAccounts(CurrentUser.Id);

                Console.WriteLine(string.Format("Your Account(s):\n\r {0}",
                    string.Join("\n\r * ", result.Select(m => JsonHelper.ToJson(m)))));
            }
            return;
        }

        public void AddAccount(params string[] list)
        {
            if (CurrentUser == null)
                Console.WriteLine("You are not authorized");

            if (list.Length < 3)
            {
                Console.WriteLine("You must enter: any name, exchange type, api key and settings according your exchange type");
                return;
            }

            int type;
            string settings;

            if (!int.TryParse(list[1], out type))
            {
                Console.WriteLine(string.Format("You must enter valid Exchange type: {0}", ExchangeInfo.GetAccountTypes));
                return;
            }
            var typeEnum = (AccountType)type;

            switch (typeEnum)
            {
                case AccountType.Yobit:
                    if (list.Length != 4)
                    {
                        Console.WriteLine("For Yobit you must to enter Secret as 4th parameter");
                        return;
                    }
                    settings = JsonHelper.ToJson(new YobitSettings
                    {
                        Secret = list[3]
                    });
                    break;
                default:
                    settings = "";
                    break;
            }

            using (var accService = new AccountService())
            {
                var result = accService.CreateOrUpdate(CurrentUser.Id, list[0], typeEnum, list[2], settings);

                Console.WriteLine(string.Format("Account successfully added: {0}", JsonHelper.ToJson(result)));
            }
            return;
        }

        public void GetActiveOrders(params string[] list)
        {
            if (CurrentUser == null)
                Console.WriteLine("You are not authorized");

            if (list.Length != 2 || string.IsNullOrWhiteSpace(list[1]))
            {
                Console.WriteLine("You must enter your account Id/Name and pair code");
                return;
            }

            var pair = list[1].ToLowerInvariant();
            int accId = 0;
            string accName = "";

            if (!int.TryParse(list[0], out accId))
            {
                accId = 0;
                accName = list[0].ToLowerInvariant();
            }

            using (var accService = new AccountService())
            {
                var account = accId > 0 ? accService.GetById(accId) : accService.GetByName(CurrentUser.Id, accName);
                if (account == null)
                {
                    Console.WriteLine("Can't find such account");
                    return;
                }
                //todo requires refactoring
                var exchange = ExchangeInfo.Exchanges[account.Type];
                switch (account.Type)
                {
                    case AccountType.Yobit:
                        {
                            using (var api = exchange.Api)
                            {
                                var yobitApi = api as YobitApi;
                                var result = yobitApi.GetActiveOrdersOfUser(pair, account);

                                if (result == null || result.success != 1)
                                    Console.WriteLine(string.Format("Something wrong: {0}", result.error));
                                else
                                {
                                    var yobitSettings = account.YobitSettings;
                                    if (yobitSettings.Counter == 0)
                                        yobitSettings.Counter = 1;
                                    else
                                        yobitSettings.Counter++;

                                    accService.UpdateSettings(account.Id, JsonHelper.ToJson(yobitSettings));

                                    Console.WriteLine("Done: " + JsonHelper.ToJson(result));
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Is not supported yet.");
                        break;
                }
            }

            return;
        }

        public void GetPairInfo(params string[] list)
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

        public void GetPairs(params string[] list)
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