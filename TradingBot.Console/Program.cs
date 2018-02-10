using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBot.Domain;

namespace TradingBot.Cmd
{
    class Program
    {
        static Program()
        {
          
        }

        static User CurrentUser { get; set; }

        static void Main(string[] args)
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
            if(CurrentUser == null)
                Console.WriteLine("You are not authorized");
            
            return null;
        }
    }
}

