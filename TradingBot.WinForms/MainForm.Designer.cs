namespace TradingBot
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.MainMenu_Accounts = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu_ManageAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_Accounts});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1096, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // MainMenu_Accounts
            // 
            this.MainMenu_Accounts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainMenu_ManageAccounts});
            this.MainMenu_Accounts.Name = "MainMenu_Accounts";
            this.MainMenu_Accounts.Size = new System.Drawing.Size(69, 20);
            this.MainMenu_Accounts.Text = "Accounts";
            // 
            // MainMenu_ManageAccounts
            // 
            this.MainMenu_ManageAccounts.Name = "MainMenu_ManageAccounts";
            this.MainMenu_ManageAccounts.Size = new System.Drawing.Size(170, 22);
            this.MainMenu_ManageAccounts.Text = "Manage Accounts";
            this.MainMenu_ManageAccounts.Click += new System.EventHandler(this.MainMenu_ManageAccounts_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 485);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trading Bot";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_Accounts;
        private System.Windows.Forms.ToolStripMenuItem MainMenu_ManageAccounts;
    }
}

