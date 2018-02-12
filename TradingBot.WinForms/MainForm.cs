namespace TradingBot.WinForms
{
	using System;
	using System.Windows.Forms;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainMenu_ManageAccounts_Click(object sender, EventArgs e)
		{
			var rand = new Random().Next(1, 100);
			var newAcc = new ToolStripMenuItem("Some account " + rand);
			newAcc.Click += Account_Click;
			MainMenu_Accounts.DropDownItems.Add(newAcc);
		}

		private void Account_Click(object sender, EventArgs e)
		{
			var item = sender as ToolStripMenuItem;
			MessageBox.Show(item.Text);
		}
	}
}