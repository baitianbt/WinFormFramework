using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class SystemSettingForm : BaseForm
    {
        private readonly ISystemSettingService _settingService;
        private Dictionary<string, List<SystemSettingDTO>> _settingGroups = new();

        public SystemSettingForm(ISystemSettingService settingService, ILogService logger)
            : base(logger)
        {
            InitializeComponent();
            _settingService = settingService;
            InitializeSystemSettingForm();
        }

        private void InitializeSystemSettingForm()
        {
            this.Text = "系统设置";
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 初始化控件
            InitializeControls();

            // 加载数据
            LoadSettings();
        }

        private void InitializeControls()
        {
            // 创建TabControl
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(12, 4)
            };

            // 创建按钮
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

            // 创建布局
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            btnCancel.Location = new Point(panel.Width - btnCancel.Width - 20, 10);
            btnSave.Location = new Point(btnCancel.Left - btnSave.Width - 10, 10);

            panel.Controls.AddRange(new Control[] { btnSave, btnCancel });

            this.Controls.AddRange(new Control[] { tabControl, panel });

            // 绑定事件
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private async void LoadSettings()
        {
            try
            {
                var settings = await _settingService.GetAllAsync();
                _settingGroups = settings.GroupBy(s => s.Group)
                                       .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var group in _settingGroups)
                {
                    AddSettingTab(group.Key, group.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading settings", ex);
                MessageBox.Show("加载设置失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSettingTab(string groupName, List<SystemSettingDTO> settings)
        {
            var tabPage = new TabPage(groupName);
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 2,
                RowCount = settings.Count,
                Padding = new Padding(10)
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            int row = 0;
            foreach (var setting in settings)
            {
                var label = new Label
                {
                    Text = setting.Description ?? setting.Key,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Margin = new Padding(5)
                };

                var textBox = new TextBox
                {
                    Text = setting.Value,
                    Dock = DockStyle.Fill,
                    Tag = setting,
                    Enabled = !setting.IsSystem
                };

                panel.Controls.Add(label, 0, row);
                panel.Controls.Add(textBox, 1, row);
                row++;
            }

            tabPage.Controls.Add(panel);
            tabControl.TabPages.Add(tabPage);
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    var panel = (TableLayoutPanel)tabPage.Controls[0];
                    foreach (Control control in panel.Controls)
                    {
                        if (control is TextBox textBox && textBox.Tag is SystemSettingDTO setting)
                        {
                            if (setting.Value != textBox.Text)
                            {
                                setting.Value = textBox.Text;
                                await _settingService.UpdateAsync(setting);
                            }
                        }
                    }
                }

                Logger.Information("Settings saved successfully");
                MessageBox.Show("设置已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving settings", ex);
                MessageBox.Show("保存设置失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private TabControl tabControl = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
} 