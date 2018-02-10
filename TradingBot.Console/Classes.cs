using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBot.Cmd
{


    public enum CommandEnum
    {
        None,
        Exit,
        Help,
        RegisterUser,
        Login
    }

    public class Command
    {
        public static List<Command> Commands;

        public static Command None = new Command { Type = CommandEnum.None };

        public CommandEnum Type { get; set; }

        public List<string> Aliases { get; set; }

        public string Description { get; set; }

        public string Info
        {
            get
            {
                return string.Format("* command '{0}', aliases: {1}", Type.ToString(),
                    string.Join(", ", Aliases.Select(m => string.Format("/{0}", m))));
            }
        }

        public Command()
        {
            Aliases = new List<string>();
        }

        static Command()
        {
            Commands = new List<Command>();

            Commands.Add(Command.None);

            Commands.Add(new Command
            {
                Type = CommandEnum.Exit,
                Aliases = new List<string> { "exit", "e", "finish", "esc" }
            });

            Commands.Add(new Command
            {
                Type = CommandEnum.Help,
                Aliases = new List<string> { "help", "h", "?" }
            });

            Commands.Add(new Command
            {
                Type = CommandEnum.RegisterUser,
                Aliases = new List<string> { "register", "signup", "create-user" }
            });

            Commands.Add(new Command
            {
                Type = CommandEnum.Login,
                Aliases = new List<string> { "login", "signin", "let-me-enter", "authorization" }
            });
        }
    }
}

