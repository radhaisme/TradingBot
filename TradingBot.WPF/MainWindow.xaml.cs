
namespace TradingBot.WPF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Core.Entities;
	using Models;
	using Yobit.Api;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Load();
		}

		private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var pair = (Pair)((DataGridRow)sender).Item;
			Price.Text = pair.Fee.ToString();
			Title = String.Format("{0}:{1}", pair.Name, pair.Fee);
		}

		private async void Load()
		{
			var client = new YobitClient("https://yobit.net/api/3/", "https://yobit.net/tapi/", new YobitSettings
			{
				Secret = "5ceeeb6072789d30e79a961335e63d50",
				ApiKey = "B03E731C650825B49CB2840E8449D98D",
				CreatedAt = new DateTime(2018, 1, 1)
			});
			var pairs = await client.GetPairsAsync();
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

		private void BuyClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void SellClick(object sender, RoutedEventArgs e)
		{

		}
	}
}