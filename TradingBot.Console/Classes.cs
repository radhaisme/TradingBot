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
        public static Command None = new Command { Type = CommandEnum.None };

        public CommandEnum Type { get; set; }

        public List<string> Aliases { get; set; }

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
    }
}

