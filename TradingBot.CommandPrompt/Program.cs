
namespace TradingBot.CommandPrompt
{
	using System;
	using System.Linq;
	using Core;
	using Core.Enums;
	using Yobit.Api;

	public class Program
	{
		private static void Main(string[] args)
		{
			var client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			{
				Secret = "5ceeeb6072789d30e79a961335e63d50",
				ApiKey = "B03E731C650825B49CB2840E8449D98D",
				CreatedOn = new DateTime(2018, 1, 1)
			});
			var r = client.GetInfo();

            //Console.WriteLine("Use help to get list of commands");
            //var input = String.Empty;

            //var commandsHelper = new CommandsHelper();

            //var command = CommandsHelper.None;

            //do
            //{
            //    Console.Write(string.Format("{0}> ", commandsHelper.CurrentUser == null ? "Anonymous" : commandsHelper.CurrentUser.Username));
            //    input = Console.ReadLine().Trim();
            //    if (input.Length < 1)
            //        continue;
            //    var parts = input.Split(' ');

            //    command = CommandsHelper.List.FirstOrDefault(m => m.Aliases.Contains(parts[0].Trim()));
            //    if (command == null)
            //        command = CommandsHelper.None;

            //    var parameters = parts.Skip(1).ToArray();
            //    if (!command.AllowAnonymous && commandsHelper.CurrentUser == null)
            //        Console.WriteLine("You are not authorized");
            //    else
            //    {
            //        commandsHelper.ExecuteCommand(command.Type, parameters);
            //    }
            //}
            //while (command.Type != CommandEnum.Exit);

            //Console.Write("You are finished. Thanks");
            Console.ReadLine();
		}
	}
}