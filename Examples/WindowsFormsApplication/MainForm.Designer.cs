namespace WindowsFormsApplication
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._loginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._userStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._commonStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            this._aboutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loginMenuItem,
            this._logoutMenuItem});
            this._fileMenuItem.Name = "_fileMenuItem";
            this._fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this._fileMenuItem.Text = "&File";
            // 
            // _loginMenuItem
            // 
            this._loginMenuItem.Name = "_loginMenuItem";
            this._loginMenuItem.Size = new System.Drawing.Size(112, 22);
            this._loginMenuItem.Text = "&Login";
            this._loginMenuItem.Click += new System.EventHandler(this.LoginClicked);
            // 
            // _logoutMenuItem
            // 
            this._logoutMenuItem.Name = "_logoutMenuItem";
            this._logoutMenuItem.Size = new System.Drawing.Size(112, 22);
            this._logoutMenuItem.Text = "Log&out";
            this._logoutMenuItem.Click += new System.EventHandler(this.LogoutClicked);
            // 
            // _aboutMenuItem
            // 
            this._aboutMenuItem.Name = "_aboutMenuItem";
            this._aboutMenuItem.Size = new System.Drawing.Size(52, 20);
            this._aboutMenuItem.Text = "&About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._userStatusLabel,
            this._commonStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 426);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _userStatusLabel
            // 
            this._userStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._userStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._userStatusLabel.Name = "_userStatusLabel";
            this._userStatusLabel.Size = new System.Drawing.Size(26, 19);
            this._userStatusLabel.Text = "---";
            this._userStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _commonStatusLabel
            // 
            this._commonStatusLabel.Name = "_commonStatusLabel";
            this._commonStatusLabel.Size = new System.Drawing.Size(759, 19);
            this._commonStatusLabel.Spring = true;
            this._commonStatusLabel.Text = "toolStripStatusLabel2";
            this._commonStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dataGridView.Location = new System.Drawing.Point(0, 24);
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.ReadOnly = true;
            this._dataGridView.RowTemplate.Height = 25;
            this._dataGridView.Size = new System.Drawing.Size(800, 402);
            this._dataGridView.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._dataGridView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _loginMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _aboutMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _userStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel _commonStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem _logoutMenuItem;
        private System.Windows.Forms.DataGridView _dataGridView;
    }
}