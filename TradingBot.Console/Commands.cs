using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Core;

namespace TradingBot.Cmd
{
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
        GetPairs,
        GetPairInfo,
    }

    public class Command
    {
        public static List<Command> Commands;

        public static Command None = new Command { Type = CommandEnum.None };

        public CommandEnum Type { get; set; }

        public List<string> Aliases { get; set; }

        public string Description { get; set; }

        public bool AllowAnonymous { get; set; }

        public string Info
        {
            get
            {
                return string.Format("{3}- Command '{0}', aliases: {1}, Info: {2}\n\r", Type.ToString(),
                    string.Join(", ", Aliases.Select(m => string.Format("/{0}", m))), Description, AllowAnonymous ? "" : "[ONLY WHEN AUTHORIZED] ");
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


        static Command()
        {
            Commands = new List<Command>();

            Commands.Add(None);

            Commands.Add(
                new Command(CommandEnum.Exit, new List<string> { "exit", "e", "finish", "esc" }, "Finish the program, amazing?")
            );

            Commands.Add(
                new Command(CommandEnum.Help, new List<string> { "help", "h", "?" }, "Show all valid commands")
            );

            Commands.Add(
                new Command(CommandEnum.RegisterUser, new List<string> { "register", "signup", "create-user" }, "Register user to allow to add accounts and get more private functionality. Parameters (* - required): username*, password*")
            );

            Commands.Add(
                new Command(CommandEnum.Login, new List<string> { "login", "signin", "let-me-enter", "authorization" }, "Sign in to add accounts and get more private functionality. Parameters (* - required): username*, password*")
            );

            Commands.Add(
                new Command(CommandEnum.Logout, new List<string> { "logout", "signoff", "signout", "lock" }, false, "Allow you to sign out, you will not have access to private functionality")
            );

            //Commands.Add(
            //    new Command(CommandEnum.AddAccount, new List<string> { "addaccount", "add-account" }, false, "Allow you to add private Api key")
            //);

            //Commands.Add(
            //    new Command(CommandEnum.RemoveAccount, new List<string> { "removeaccount", "remove-account" , "rem-account", "del-account", "delete-account" }, false, "Allow you to remove private Api key")
            //);

            Commands.Add(
              new Command(CommandEnum.GetPairs, new List<string> { "getpairs", "all", "info" },
              string.Format("Get all list of tickers with basic statistics. Parameters (* - required): exchangeType* ({0})",
              ExchangeInfo.GetAccountTypes))
            );

            Commands.Add(
              new Command(CommandEnum.GetPairInfo, new List<string> { "getpairinfo", "tickerinfo", "get-ticker-info", "get-pair-info", "pair-info", "ticker-info" },
              string.Format("Get basic ticker info. It uses info stored from last 'getpairs'. Parameters (* - required): exchangeType* ({0}), tickerCode* ", ExchangeInfo.GetAccountTypes))
            );

        }
    }
}

