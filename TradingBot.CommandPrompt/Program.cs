
namespace TradingBot.CommandPrompt
{
	using System;
	using System.Linq;

	public class Program
	{
		private static void Main(string[] args)
		{
			//var factory = new ClientFactory();
			//var client = factory.Create(AccountType.Yobit);

			//var settings = new YobitSettings();
			//settings.BaseAddress = "https://yobit.net";
			//settings.PublicKey = "1A3EB44FEDA024D8B65C5BEE374D628C";
			//settings.Secret = "e21731f4c25156fe871962a2571fb40c";
			//settings.Counter = 1;
			//var client = new YobitClient(settings);
			//var r = client.GetActiveOrdersOfUser("ltc_btc");


			Console.WriteLine("Use help to get list of commands");
			var input = String.Empty;

			var commandsHelper = new CommandsHelper();

			var command = CommandsHelper.None;

			do
			{
				Console.Write(string.Format("{0}> ", commandsHelper.CurrentUser == null ? "Anonymous" : commandsHelper.CurrentUser.Username));
				input = Console.ReadLine().Trim();
				if (input.Length < 1)
					continue;
				var parts = input.Split(' ');

				command = CommandsHelper.List.FirstOrDefault(m => m.Aliases.Contains(parts[0].Trim()));
				if (command == null)
					command = CommandsHelper.None;

				var parameters = parts.Skip(1).ToArray();
				if (!command.AllowAnonymous && commandsHelper.CurrentUser == null)
					Console.WriteLine("You are not authorized");
				else
				{
					commandsHelper.ExecuteCommand(command.Type, parameters);
				}
			}
			while (command.Type != CommandEnum.Exit);

			Console.Write("You are finished. Thanks");
			Console.ReadLine();
		}
	}
}