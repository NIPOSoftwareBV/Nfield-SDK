using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class MainForm : Form
    {
        private Nfield _nfield = new Nfield(ApplicationConfiguration.Current.DomainName);
        public MainForm()
        {
            InitializeComponent();
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            var result = await _nfield.AuthenticateAsync(s => _commonStatusLabel.Text = s);
            
            _userStatusLabel.Text = result.Account.Username;
            _commonStatusLabel.Text = string.Empty;

            var surveys = await _nfield.ListSurveysAsync();

            var source = new BindingSource();
            source.DataSource = surveys.ToList();
            _dataGridView.AutoGenerateColumns = true;
            _dataGridView.DataSource = source;
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            await _nfield.LogoutAsync();

            _userStatusLabel.Text = string.Empty;
        }
    }
}