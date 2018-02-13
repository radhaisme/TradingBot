
namespace TradingBot.CommandPrompt
{
	using System;
	using System.Linq;
	using Common;
	using Core;
	using Core.Enums;
	using Data.Entities;
	using Services;
    using Yobit.Api;

    class Program
	{
		static Program()
		{

		}

		static User CurrentUser { get; set; }


		static void Main(string[] args)
		{
			Console.WriteLine("Use help to get list of commands");
			var input = "";
			var command = Command.None;
			do
			{
				Console.Write(string.Format("{0}> ", CurrentUser == null ? "Anonymous" : CurrentUser.Username));
				input = Console.ReadLine().Trim();
				if (input.Length < 1)
					continue;
				var parts = input.Split(' ');

				command = Command.Commands.FirstOrDefault(m => m.Aliases.Contains(parts[0].Trim()));
				if (command == null)
					command = Command.None;

				var parameters = parts.Skip(1).ToArray();
				if (!command.AllowAnonymous && CurrentUser == null)
					Console.WriteLine("You are not authorized");
				else
					switch (command.Type)
					{
						case CommandEnum.Help:
                            Help(parameters);
                            break;
						case CommandEnum.RegisterUser:
							CurrentUser = RegisterUser(parameters);
							break;
						case CommandEnum.Login:
							CurrentUser = Login(parameters);
							break;
						case CommandEnum.Logout:
							CurrentUser = Logout(parameters);
							break;
                        case CommandEnum.AddAccount:
                            AddAccount(parameters);
                            break;
                        case CommandEnum.GetAccounts:
                            GetAccounts(parameters);
                            break;
                        case CommandEnum.GetPairs:
							GetPairs(parameters);
							break;
						case CommandEnum.GetPairInfo:
							GetPairInfo(parameters);
							break;
                        case CommandEnum.GetActiveOrders:
                            GetActiveOrders(parameters);
                            break; 

                    }
			}
			while (command.Type != CommandEnum.Exit);

			Console.Write("You are finished. Thanks");
			Console.ReadLine();
		}

        private static void Help(params string[] list)
        {
            Console.WriteLine("\n\rAvailable commands:\n\r\n\r" +
                string.Join("\n\r", Command.Commands.Where(m => m.Type != CommandEnum.None)
                .Select(m => m.Info)));
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

		private static User Login(params string[] list)
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

		private static User Logout(params string[] list)
		{
			if (CurrentUser == null)
				Console.WriteLine("You are not authorized");

            return null;
        }

        private static void GetAccounts(params string[] list)
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

        private static void AddAccount(params string[] list)
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

            if(!int.TryParse(list[1], out type))
            {
                Console.WriteLine(string.Format("You must enter valid Exchange type: {0}", ExchangeInfo.GetAccountTypes));
                return;
            }
            var typeEnum = (AccountType)type;

            switch(typeEnum)
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

        private static void GetActiveOrders(params string[] list)
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