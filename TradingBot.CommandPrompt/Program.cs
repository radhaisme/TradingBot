
namespace TradingBot.CommandPrompt
{
	using System;
	using Yobit.Api;

	public class Program
	{
		private static void Main(string[] args)
		{
			var settings = new YobitSettings();
			settings.BaseAddress = "https://yobit.net";
			settings.ApiPrefix = "";
			var client = new YobitClient(settings);
			var r = client.GetPairOrders("ltc_btc");


			//Console.WriteLine("Use help to get list of commands");
			//var input = String.Empty;

   //         var commandsHelper = new CommandsHelper();

   //         var command = CommandsHelper.None;

			//do
   //         {
   //             Console.Write(string.Format("{0}> ", commandsHelper.CurrentUser == null ? "Anonymous" : commandsHelper.CurrentUser.Username));
   //             input = Console.ReadLine().Trim();
   //             if (input.Length < 1)
   //                 continue;
   //             var parts = input.Split(' ');

   //             command = CommandsHelper.List.FirstOrDefault(m => m.Aliases.Contains(parts[0].Trim()));
   //             if (command == null)
   //                 command = CommandsHelper.None;

   //             var parameters = parts.Skip(1).ToArray();
   //             if (!command.AllowAnonymous && commandsHelper.CurrentUser == null)
   //                 Console.WriteLine("You are not authorized");
   //             else
   //             {
   //                 commandsHelper.ExecuteCommand(command.Type, parameters);
   //             }
   //         }
   //         while (command.Type != CommandEnum.Exit);

			//Console.Write("You are finished. Thanks");
			Console.ReadLine();
		}
	}
}