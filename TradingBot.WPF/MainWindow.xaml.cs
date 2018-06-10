
namespace TradingBot.WPF
{
	using Core.Entities;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Yobit.Api;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly YobitClient _client;
		private Pair _pair;

		public MainWindow()
		{
			_client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			{
				Secret = "5ceeeb6072789d30e79a961335e63d50",
				ApiKey = "B03E731C650825B49CB2840E8449D98D",
				CreatedAt = new DateTime(2018, 1, 1)
			});
			InitializeComponent();
			Load();
		}

		private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			_pair = (Pair)((DataGridRow)sender).Item;
			Amount.Increment = Price.Increment = (double)new decimal(1, 0, 0, false, _pair.DecimalPlaces);
			Price.Value = (double)_pair.MaxPrice;
			//Title = String.Format("{0}:{1}", pair.Name, pair.Fee);
		}

		private async void Load()
		{
			var pairs = await _client.GetPairsAsync();
			IDictionary<string, List<Pair>> result = pairs.Pairs
				.Where(x => !x.Value.IsHidden)
				.Select(x =>
				{
					string[] keys = x.Key.Split('_');
					x.Value.Name = keys[0].ToUpper();

					return new { Key = keys[1].ToUpper(), x.Value };
				})
				.GroupBy(x => x.Key, y => y.Value)
				.TakeWhile(x => x.Count() > 1)
				.ToDictionary(x => x.Key, y => y.ToList());
			var model = new MainViewModel { Pairs = result };
			DataContext = model;
		}

		private async void BuyClick(object sender, RoutedEventArgs e)
		{
			if (_pair == null)
			{
				return;
			}

			CreateOrder order = await _client.CreateOrderAsync("", OrderType.Buy, 0, 0);
		}

		private async void SellClick(object sender, RoutedEventArgs e)
		{
			if (_pair == null)
			{
				return;
			}

			CreateOrder order = await _client.CreateOrderAsync("", OrderType.Sell, 0, 0);
		}

		private bool ValidateOrderParams()
		{
			if (_pair == null)
			{
				return false;
			}

			if (Price.Value == 0)
			{
				return false;
			}



			return true;
		}
	}
}