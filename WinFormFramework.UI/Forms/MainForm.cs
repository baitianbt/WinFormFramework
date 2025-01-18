using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class MainForm : BaseForm
    {
        private readonly IUserService _userService;
        private Form? _activeForm;

        public MainForm(IUserService userService, ILogService logger) 
            : base(logger)
        {
            InitializeComponent();
            _userService = userService;
            InitializeMainForm();
        }

        private void InitializeMainForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;

            // 初始化菜单栏
            InitializeMenuStrip();

            // 初始化状态栏
            InitializeStatusStrip();
        }

        private void InitializeMenuStrip()
        {
            MenuStrip menuStrip = new MenuStrip();
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // 系统管理菜单
            var systemMenu = new ToolStripMenuItem("系统管理(&S)");
            var userManageItem = new ToolStripMenuItem("用户管理(&U)", null, UserManage_Click);
            var roleManageItem = new ToolStripMenuItem("角色管理(&R)", null, RoleManage_Click);
            var exitItem = new ToolStripMenuItem("退出(&X)", null, Exit_Click);
            systemMenu.DropDownItems.AddRange(new ToolStripItem[] { userManageItem, roleManageItem, new ToolStripSeparator(), exitItem });

            menuStrip.Items.Add(systemMenu);
        }

        private void InitializeStatusStrip()
        {
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "就绪";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void OpenChildForm(Form childForm)
        {
            _activeForm?.Close();
            _activeForm = childForm;
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void UserManage_Click(object? sender, EventArgs e)
        {
            try
            {
                var userManageForm = Program.ServiceProvider.GetRequiredService<UserManageForm>();
                OpenChildForm(userManageForm);
            }
            catch (Exception ex)
            {
                Logger.Error("Error opening user management form", ex);
                MessageBox.Show("无法打开用户管理窗口：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoleManage_Click(object? sender, EventArgs e)
        {
            try
            {
                var roleManageForm = Program.ServiceProvider.GetRequiredService<RoleManageForm>();
                OpenChildForm(roleManageForm);
            }
            catch (Exception ex)
            {
                Logger.Error("Error opening role management form", ex);
                MessageBox.Show("无法打开角色管理窗口：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Exit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出系统吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }
    }
} 