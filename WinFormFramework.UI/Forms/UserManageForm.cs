using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class UserManageForm : BaseForm
    {
        private readonly IUserService _userService;
        private List<UserDTO> _users = new();
        private readonly IRoleService _roleService;

        public UserManageForm(IUserService userService, ILogService logger, IRoleService roleService) 
            : base(logger)
        {
            InitializeComponent();
            _userService = userService;
            _roleService = roleService;
            InitializeUserManageForm();
        }

        private void InitializeUserManageForm()
        {
            // 初始化DataGridView
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "UserName", HeaderText = "用户名", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "RealName", HeaderText = "真实姓名", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "邮箱", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "电话", Width = 120 },
                new DataGridViewCheckBoxColumn { DataPropertyName = "IsEnabled", HeaderText = "启用", Width = 60 },
                new DataGridViewTextBoxColumn { DataPropertyName = "LastLoginTime", HeaderText = "最后登录时间", Width = 150 }
            });

            // 加载数据
            LoadUsers();

            // 绑定事件
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // 在工具栏中添加角色分配按钮
            btnAssignRoles = new ToolStripButton("分配角色", null, BtnAssignRoles_Click) { Text = "分配角色" };

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                btnAdd, new ToolStripSeparator(),
                btnEdit, new ToolStripSeparator(),
                btnDelete, new ToolStripSeparator(),
                btnAssignRoles, new ToolStripSeparator(),
                btnRefresh
            });
        }

        private async void LoadUsers()
        {
            try
            {
                _users = (await _userService.GetAllAsync()).ToList();
                dataGridView.DataSource = null;
                dataGridView.DataSource = _users;
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading users", ex);
                MessageBox.Show("加载用户数据失败：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var addUserForm = Program.ServiceProvider.GetRequiredService<UserEditForm>();
            if (addUserForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is UserDTO selectedUser)
            {
                var editUserForm = Program.ServiceProvider.GetRequiredService<UserEditForm>();
                editUserForm.SetUser(selectedUser);
                if (editUserForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is UserDTO selectedUser)
            {
                if (MessageBox.Show($"确定要删除用户 {selectedUser.UserName} 吗？", "确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        await _userService.DeleteAsync(selectedUser.Id);
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error deleting user: {selectedUser.UserName}", ex);
                        MessageBox.Show("删除用户失败：" + ex.Message, "错误", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadUsers();
        }

        private void BtnAssignRoles_Click(object? sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is UserDTO selectedUser)
            {
                var userRoleForm = new UserRoleForm(_roleService, Logger, selectedUser);
                if (userRoleForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private ToolStripButton btnAssignRoles = null!;
    }
} 