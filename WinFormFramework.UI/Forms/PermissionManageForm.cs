using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class PermissionManageForm : BaseForm
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly RoleDTO _role;

        public PermissionManageForm(
            IPermissionService permissionService,
            IRoleService roleService,
            ILogService logger,
            RoleDTO role)
            : base(logger)
        {
            InitializeComponent();
            _permissionService = permissionService;
            _roleService = roleService;
            _role = role;
            InitializePermissionManageForm();
        }

        private void InitializePermissionManageForm()
        {
            this.Text = $"权限管理 - {_role.RoleName}";
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeControls();
            LoadPermissions();
        }

        private void InitializeControls()
        {
            // 权限树
            treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                CheckBoxes = true
            };

            // 按钮
            btnSave = new Button
            {
                Text = "保存",
                Width = 80,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            btnCancel = new Button
            {
                Text = "取消",
                Width = 80,
                Height = 30,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            // 布局
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            btnCancel.Location = new Point(panel.Width - btnCancel.Width - 20, 10);
            btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 10, 10);

            panel.Controls.AddRange(new Control[] { btnSave, btnCancel });

            this.Controls.AddRange(new Control[] { treeView, panel });

            // 绑定事件
            treeView.AfterCheck += TreeView_AfterCheck;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private async void LoadPermissions()
        {
            try
            {
                var permissions = await _permissionService.GetRolePermissionsAsync(_role.Id);
                var groups = permissions.GroupBy(p => p.Group);

                treeView.Nodes.Clear();
                foreach (var group in groups)
                {
                    var groupNode = new TreeNode(group.Key);
                    foreach (var permission in group)
                    {
                        var node = new TreeNode(permission.Name)
                        {
                            Tag = permission,
                            Checked = permission.IsGranted
                        };
                        groupNode.Nodes.Add(node);
                    }
                    treeView.Nodes.Add(groupNode);
                }

                treeView.ExpandAll();
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading permissions", ex);
                MessageBox.Show("加载权限失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TreeView_AfterCheck(object? sender, TreeViewEventArgs e)
        {
            // 如果选中/取消选中父节点，则同步更新所有子节点
            if (e.Action != TreeViewAction.Unknown)
            {
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = e.Node.Checked;
                }
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var selectedPermissionIds = new List<int>();
                foreach (TreeNode groupNode in treeView.Nodes)
                {
                    foreach (TreeNode node in groupNode.Nodes)
                    {
                        if (node.Checked && node.Tag is PermissionDTO permission)
                        {
                            selectedPermissionIds.Add(permission.Id);
                        }
                    }
                }

                await _permissionService.AssignPermissionsToRoleAsync(_role.Id, selectedPermissionIds);
                Logger.Information($"Updated permissions for role: {_role.RoleName}");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving permissions", ex);
                MessageBox.Show("保存权限失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private TreeView treeView = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
} 