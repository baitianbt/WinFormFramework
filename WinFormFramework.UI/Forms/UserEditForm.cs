using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class UserEditForm : BaseForm
    {
        private readonly IUserService _userService;
        private UserDTO? _currentUser;
        private bool _isEdit;

        public UserEditForm(IUserService userService, ILogService logger)
            : base(logger)
        {
            InitializeComponent();
            _userService = userService;
            InitializeUserEditForm();
        }

        private void InitializeUserEditForm()
        {
            this.Text = "添加用户";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 初始化控件
            InitializeControls();

            // 绑定事件
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void InitializeControls()
        {
            // 用户名
            Label lblUsername = new Label
            {
                Text = "用户名：",
                Location = new Point(30, 30),
                AutoSize = true
            };
            txtUsername = new TextBox
            {
                Location = new Point(120, 27),
                Width = 200
            };

            // 真实姓名
            Label lblRealName = new Label
            {
                Text = "真实姓名：",
                Location = new Point(30, 70),
                AutoSize = true
            };
            txtRealName = new TextBox
            {
                Location = new Point(120, 67),
                Width = 200
            };

            // 密码
            Label lblPassword = new Label
            {
                Text = "密码：",
                Location = new Point(30, 110),
                AutoSize = true
            };
            txtPassword = new TextBox
            {
                Location = new Point(120, 107),
                Width = 200,
                UseSystemPasswordChar = true
            };

            // 邮箱
            Label lblEmail = new Label
            {
                Text = "邮箱：",
                Location = new Point(30, 150),
                AutoSize = true
            };
            txtEmail = new TextBox
            {
                Location = new Point(120, 147),
                Width = 200
            };

            // 电话
            Label lblPhone = new Label
            {
                Text = "电话：",
                Location = new Point(30, 190),
                AutoSize = true
            };
            txtPhone = new TextBox
            {
                Location = new Point(120, 187),
                Width = 200
            };

            // 是否启用
            chkEnabled = new CheckBox
            {
                Text = "启用",
                Location = new Point(120, 227),
                Checked = true
            };

            // 按钮
            btnSave = new Button
            {
                Text = "保存",
                Location = new Point(120, 380),
                Width = 80
            };

            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(240, 380),
                Width = 80
            };

            // 添加控件
            this.Controls.AddRange(new Control[]
            {
                lblUsername, txtUsername,
                lblRealName, txtRealName,
                lblPassword, txtPassword,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                chkEnabled,
                btnSave, btnCancel
            });
        }

        public void SetUser(UserDTO user)
        {
            _currentUser = user;
            _isEdit = true;
            this.Text = "编辑用户";

            txtUsername.Text = user.UserName;
            txtRealName.Text = user.RealName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            chkEnabled.Checked = user.IsEnabled;

            txtUsername.Enabled = false;
            txtPassword.Text = "********";
            txtPassword.Enabled = false;
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (_isEdit)
                    {
                        await UpdateUser();
                    }
                    else
                    {
                        await CreateUser();
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving user", ex);
                MessageBox.Show("保存用户失败：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!_isEdit && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async Task CreateUser()
        {
            var userDto = new UserDTO
            {
                UserName = txtUsername.Text,
                RealName = txtRealName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                IsEnabled = chkEnabled.Checked
            };

            await _userService.CreateAsync(userDto, txtPassword.Text);
            Logger.Information($"Created user: {userDto.UserName}");
        }

        private async Task UpdateUser()
        {
            if (_currentUser != null)
            {
                _currentUser.RealName = txtRealName.Text;
                _currentUser.Email = txtEmail.Text;
                _currentUser.Phone = txtPhone.Text;
                _currentUser.IsEnabled = chkEnabled.Checked;

                await _userService.UpdateAsync(_currentUser);
                Logger.Information($"Updated user: {_currentUser.UserName}");
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private TextBox txtUsername = null!;
        private TextBox txtRealName = null!;
        private TextBox txtPassword = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private CheckBox chkEnabled = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
} 