using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class UserRoleForm : BaseForm
    {
        private readonly IRoleService _roleService;
        private readonly UserDTO _user;
        private List<RoleDTO> _allRoles = new();
        private List<RoleDTO> _userRoles = new();

        public UserRoleForm(IRoleService roleService, ILogService logger, UserDTO user)
            : base(logger)
        {
            InitializeComponent();
            _roleService = roleService;
            _user = user;
            InitializeUserRoleForm();
        }

        private void InitializeUserRoleForm()
        {
            this.Text = $"角色分配 - {_user.UserName}";
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeControls();
            LoadRoles();
        }

        private void InitializeControls()
        {
            // 标签
            Label lblAvailable = new Label
            {
                Text = "可用角色：",
                Location = new Point(30, 20),
                AutoSize = true
            };

            Label lblAssigned = new Label
            {
                Text = "已分配角色：",
                Location = new Point(320, 20),
                AutoSize = true
            };

            // 列表框
            lstAvailable = new ListBox
            {
                Location = new Point(30, 50),
                Size = new Size(200, 250),
                SelectionMode = SelectionMode.MultiExtended
            };

            lstAssigned = new ListBox
            {
                Location = new Point(320, 50),
                Size = new Size(200, 250),
                SelectionMode = SelectionMode.MultiExtended
            };

            // 按钮
            btnAssign = new Button
            {
                Text = ">>",
                Location = new Point(250, 120),
                Width = 50
            };

            btnRemove = new Button
            {
                Text = "<<",
                Location = new Point(250, 170),
                Width = 50
            };

            btnSave = new Button
            {
                Text = "保存",
                Location = new Point(320, 320),
                Width = 80
            };

            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(440, 320),
                Width = 80
            };

            // 绑定事件
            btnAssign.Click += BtnAssign_Click;
            btnRemove.Click += BtnRemove_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // 添加控件
            this.Controls.AddRange(new Control[]
            {
                lblAvailable, lblAssigned,
                lstAvailable, lstAssigned,
                btnAssign, btnRemove,
                btnSave, btnCancel
            });
        }

        private async void LoadRoles()
        {
            try
            {
                // 加载所有角色
                _allRoles = (await _roleService.GetAllAsync()).ToList();
                
                // 加载用户的角色
                _userRoles = (await _roleService.GetUserRolesAsync(_user.Id)).ToList();

                RefreshLists();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading roles", ex);
                MessageBox.Show("加载角色数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshLists()
        {
            // 清空列表
            lstAvailable.Items.Clear();
            lstAssigned.Items.Clear();

            // 填充可用角色列表
            var availableRoles = _allRoles.Where(r => !_userRoles.Any(ur => ur.Id == r.Id));
            foreach (var role in availableRoles)
            {
                lstAvailable.Items.Add(role);
            }

            // 填充已分配角色列表
            foreach (var role in _userRoles)
            {
                lstAssigned.Items.Add(role);
            }
        }

        private void BtnAssign_Click(object? sender, EventArgs e)
        {
            var selectedRoles = lstAvailable.SelectedItems.Cast<RoleDTO>().ToList();
            foreach (var role in selectedRoles)
            {
                _userRoles.Add(role);
            }
            RefreshLists();
        }

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            var selectedRoles = lstAssigned.SelectedItems.Cast<RoleDTO>().ToList();
            foreach (var role in selectedRoles)
            {
                _userRoles.Remove(role);
            }
            RefreshLists();
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                await _roleService.AssignRolesToUserAsync(_user.Id, _userRoles.Select(r => r.Id));
                Logger.Information($"Updated roles for user: {_user.UserName}");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving user roles", ex);
                MessageBox.Show("保存角色分配失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private ListBox lstAvailable = null!;
        private ListBox lstAssigned = null!;
        private Button btnAssign = null!;
        private Button btnRemove = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
} 