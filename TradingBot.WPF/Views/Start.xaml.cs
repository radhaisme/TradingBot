namespace TradingBot.WPF.Views
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using Core.Entities;
	using Models;
	using Yobit.Api;

	/// <summary>
	/// Interaction logic for Start.xaml
	/// </summary>
	public partial class Start : UserControl
	{
		private YobitClient _client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
		{
			Secret = "5ceeeb6072789d30e79a961335e63d50",
			ApiKey = "B03E731C650825B49CB2840E8449D98D",
			CreatedAt = new DateTime(2018, 1, 1)
		});

		public Start()
		{
			InitializeComponent();
			Initialize();
			//var model = new StartViewModel();
			//var r = _client.GetPairs();

			//var c = new HttpClient();
			//HttpResponseMessage response = c.GetAsync(new Uri("https://yobit.net/api/3/info?ignore_invalid=1")).Result;

			//model.Pairs = _client.GetPairs().Pairs.Keys;
			//DataContext = model;
		}

		private async void Initialize()
		{
			var model = new StartViewModel();
			PairsInfo pairs = await _client.GetPairsAsync();
			model.Pairs = pairs.Pairs.Keys.Select(x => x.ToUpper().Replace("_", "/"));
			DataContext = model;
		}
	}
}