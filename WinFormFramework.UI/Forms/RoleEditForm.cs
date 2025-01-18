using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class RoleEditForm : BaseForm
    {
        private readonly IRoleService _roleService;
        private RoleDTO? _currentRole;
        private bool _isEdit;

        public RoleEditForm(IRoleService roleService, ILogService logger)
            : base(logger)
        {
            InitializeComponent();
            _roleService = roleService;
            InitializeRoleEditForm();
        }

        private void InitializeRoleEditForm()
        {
            this.Text = "添加角色";
            this.Size = new Size(400, 300);
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
            // 角色名称
            Label lblRoleName = new Label
            {
                Text = "角色名称：",
                Location = new Point(30, 30),
                AutoSize = true
            };
            txtRoleName = new TextBox
            {
                Location = new Point(120, 27),
                Width = 200
            };

            // 描述
            Label lblDescription = new Label
            {
                Text = "描述：",
                Location = new Point(30, 70),
                AutoSize = true
            };
            txtDescription = new TextBox
            {
                Location = new Point(120, 67),
                Width = 200,
                Multiline = true,
                Height = 80
            };

            // 是否系统角色
            chkIsSystem = new CheckBox
            {
                Text = "系统角色",
                Location = new Point(120, 160),
                Enabled = false
            };

            // 按钮
            btnSave = new Button
            {
                Text = "保存",
                Location = new Point(120, 200),
                Width = 80
            };

            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(240, 200),
                Width = 80
            };

            // 添加控件
            this.Controls.AddRange(new Control[]
            {
                lblRoleName, txtRoleName,
                lblDescription, txtDescription,
                chkIsSystem,
                btnSave, btnCancel
            });
        }

        public void SetRole(RoleDTO role)
        {
            _currentRole = role;
            _isEdit = true;
            this.Text = "编辑角色";

            txtRoleName.Text = role.RoleName;
            txtDescription.Text = role.Description;
            chkIsSystem.Checked = role.IsSystem;

            if (role.IsSystem)
            {
                txtRoleName.Enabled = false;
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (_isEdit)
                    {
                        await UpdateRole();
                    }
                    else
                    {
                        await CreateRole();
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving role", ex);
                MessageBox.Show("保存角色失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show("请输入角色名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private async Task CreateRole()
        {
            var roleDto = new RoleDTO
            {
                RoleName = txtRoleName.Text,
                Description = txtDescription.Text,
                IsSystem = false
            };

            await _roleService.CreateAsync(roleDto);
            Logger.Information($"Created role: {roleDto.RoleName}");
        }

        private async Task UpdateRole()
        {
            if (_currentRole != null)
            {
                _currentRole.RoleName = txtRoleName.Text;
                _currentRole.Description = txtDescription.Text;

                await _roleService.UpdateAsync(_currentRole);
                Logger.Information($"Updated role: {_currentRole.RoleName}");
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private TextBox txtRoleName = null!;
        private TextBox txtDescription = null!;
        private CheckBox chkIsSystem = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
} 