using WinFormFramework.Common.Attributes;
using WinFormFramework.Infrastructure.Logging;
using WinFormFramework.Common.Security;

namespace WinFormFramework.UI.Forms
{
    public partial class BaseForm : Form
    {
        protected readonly ILogService Logger;
        protected readonly IPermissionValidator? PermissionValidator;

        public BaseForm(ILogService logger, IPermissionValidator? permissionValidator = null)
        {
            Logger = logger;
            PermissionValidator = permissionValidator;
            InitializeBaseForm();
        }

        private void InitializeBaseForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Microsoft YaHei UI", 9F);

            this.Load += BaseForm_Load;
            this.FormClosing += BaseForm_FormClosing;
        }

        private void BaseForm_Load(object? sender, EventArgs e)
        {
            Logger.Debug($"Form loaded: {this.Name}");

            // 检查窗体权限
            if (PermissionValidator != null && !CheckPermissions())
            {
                MessageBox.Show("您没有访问此功能的权限", "权限不足",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
        }

        private bool CheckPermissions()
        {
            if (PermissionValidator == null)
                return true;

            return PermissionChecker.HasRequiredPermissions(this.GetType(), PermissionValidator);
        }

        protected bool CheckMethodPermissions([System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            if (PermissionValidator == null)
                return true;

            var method = this.GetType().GetMethod(methodName);
            if (method == null)
                return true;

            return PermissionChecker.HasRequiredPermissions(method, PermissionValidator);
        }

        private void BaseForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Logger.Debug($"Form closing: {this.Name}");
        }

        protected override void OnError(Exception ex)
        {
            Logger.Error("An error occurred", ex);
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            base.OnError(ex);
        }
    }
} 