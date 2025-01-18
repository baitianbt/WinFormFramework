using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class RoleManageForm : BaseForm
    {
        private readonly IRoleService _roleService;
        private List<RoleDTO> _roles = new();

        public RoleManageForm(IRoleService roleService, ILogService logger)
            : base(logger)
        {
            InitializeComponent();
            _roleService = roleService;
            InitializeRoleManageForm();
        }

        private void InitializeRoleManageForm()
        {
            this.Text = "角色管理";
            this.Size = new Size(800, 600);

            // 初始化DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            dataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "RoleName", HeaderText = "角色名称", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "描述", Width = 250 },
                new DataGridViewCheckBoxColumn { DataPropertyName = "IsSystem", HeaderText = "系统角色", Width = 80 }
            });

            // 工具栏
            var toolStrip = new ToolStrip();
            btnAdd = new ToolStripButton("添加", null, BtnAdd_Click) { Text = "添加" };
            btnEdit = new ToolStripButton("编辑", null, BtnEdit_Click) { Text = "编辑" };
            btnDelete = new ToolStripButton("删除", null, BtnDelete_Click) { Text = "删除" };
            btnRefresh = new ToolStripButton("刷新", null, BtnRefresh_Click) { Text = "刷新" };

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                btnAdd, new ToolStripSeparator(),
                btnEdit, new ToolStripSeparator(),
                btnDelete, new ToolStripSeparator(),
                btnRefresh
            });

            // 布局
            var container = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };

            container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            container.Controls.Add(toolStrip, 0, 0);
            container.Controls.Add(dataGridView, 0, 1);

            this.Controls.Add(container);

            // 加载数据
            LoadRoles();
        }

        private async void LoadRoles()
        {
            try
            {
                _roles = (await _roleService.GetAllAsync()).ToList();
                dataGridView.DataSource = null;
                dataGridView.DataSource = _roles;
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading roles", ex);
                MessageBox.Show("加载角色数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var addRoleForm = Program.ServiceProvider.GetRequiredService<RoleEditForm>();
            if (addRoleForm.ShowDialog() == DialogResult.OK)
            {
                LoadRoles();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is RoleDTO selectedRole)
            {
                var editRoleForm = Program.ServiceProvider.GetRequiredService<RoleEditForm>();
                editRoleForm.SetRole(selectedRole);
                if (editRoleForm.ShowDialog() == DialogResult.OK)
                {
                    LoadRoles();
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dataGridView.CurrentRow?.DataBoundItem is RoleDTO selectedRole)
            {
                if (selectedRole.IsSystem)
                {
                    MessageBox.Show("系统角色不能删除！", "警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"确定要删除角色 {selectedRole.RoleName} 吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        await _roleService.DeleteAsync(selectedRole.Id);
                        LoadRoles();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error deleting role: {selectedRole.RoleName}", ex);
                        MessageBox.Show("删除角色失败：" + ex.Message, "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadRoles();
        }

        private DataGridView dataGridView = null!;
        private ToolStripButton btnAdd = null!;
        private ToolStripButton btnEdit = null!;
        private ToolStripButton btnDelete = null!;
        private ToolStripButton btnRefresh = null!;
    }
} 