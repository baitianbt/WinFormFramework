using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class LoginForm : BaseForm
    {
        private readonly IUserService _userService;

        public LoginForm(IUserService userService, ILogService logger) 
            : base(logger)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter username and password", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isValid = await _userService.ValidateUserAsync(txtUsername.Text, txtPassword.Text);
                if (isValid)
                {
                    Logger.Information($"User logged in: {txtUsername.Text}");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Login failed", ex);
                MessageBox.Show("Login failed: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 