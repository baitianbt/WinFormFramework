using WinFormFramework.Infrastructure.Logging;

namespace WinFormFramework.UI.Controls
{
    public class BaseUserControl : UserControl
    {
        protected readonly ILogService Logger;

        public BaseUserControl(ILogService logger)
        {
            Logger = logger;
            InitializeBaseControl();
        }

        private void InitializeBaseControl()
        {
            this.Font = new Font("Microsoft YaHei UI", 9F);
            this.Load += BaseUserControl_Load;
        }

        private void BaseUserControl_Load(object? sender, EventArgs e)
        {
            Logger.Debug($"Control loaded: {this.Name}");
        }
    }
} 