using System;
using System.Linq;
using System.Threading.Tasks;
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

            UpdateMenuState(true);

            await LoadSurveyAsync();
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            await _nfield.LogoutAsync();

            _userStatusLabel.Text = "---";
            _dataGridView.DataSource = null;

            UpdateMenuState(false);
        }

        private async void FormLoad(object sender, EventArgs e)
        {
            var result = await _nfield.TryAuthenticateSilentAsync();
            var authenticated = result != null;
            UpdateMenuState(authenticated);
            if (!authenticated)
                return;

            _userStatusLabel.Text = result.Account.Username;
            _commonStatusLabel.Text = string.Empty;
            await LoadSurveyAsync();
        }

        private void UpdateMenuState(bool authenticated)
        {
            _loginMenuItem.Enabled = !authenticated;
            _logoutMenuItem.Enabled = authenticated;
        }

        private async Task LoadSurveyAsync()
        {
            var surveys = await _nfield.ListSurveysAsync();

            var source = new BindingSource();
            source.DataSource = surveys.ToList();
            _dataGridView.AutoGenerateColumns = true;
            _dataGridView.DataSource = source;
        }
    }
}