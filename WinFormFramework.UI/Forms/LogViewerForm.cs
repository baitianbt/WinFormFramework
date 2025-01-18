using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;

namespace WinFormFramework.UI.Forms
{
    public partial class LogViewerForm : BaseForm
    {
        private readonly ISystemLogService _logService;
        private DateTime _startTime = DateTime.Today;
        private DateTime _endTime = DateTime.Now;
        private string? _selectedLevel;
        private string? _keyword;
        private int _currentPage = 1;
        private const int PageSize = 50;

        public LogViewerForm(ISystemLogService logService, ILogService logger)
            : base(logger)
        {
            InitializeComponent();
            _logService = logService;
            InitializeLogViewerForm();
        }

        private void InitializeLogViewerForm()
        {
            this.Text = "日志查看器";
            this.Size = new Size(1200, 800);
            this.WindowState = FormWindowState.Maximized;

            InitializeControls();
            LoadLogs();
        }

        private void InitializeControls()
        {
            // 工具栏
            var toolStrip = new ToolStrip();
            
            // 日期选择
            var lblStartDate = new ToolStripLabel("开始日期：");
            dtpStartDate = new ToolStripDateTimePicker
            {
                Value = _startTime,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm:ss"
            };

            var lblEndDate = new ToolStripLabel("结束日期：");
            dtpEndDate = new ToolStripDateTimePicker
            {
                Value = _endTime,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm:ss"
            };

            // 日志级别下拉框
            cboLevel = new ToolStripComboBox();
            cboLevel.Items.AddRange(new[] { "全部", "Debug", "Information", "Warning", "Error" });
            cboLevel.SelectedIndex = 0;

            // 搜索框
            txtSearch = new ToolStripTextBox { Width = 150 };
            var btnSearch = new ToolStripButton("搜索", null, BtnSearch_Click);
            var btnExport = new ToolStripButton("导出", null, BtnExport_Click);
            var btnClear = new ToolStripButton("清理", null, BtnClear_Click);

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                lblStartDate, dtpStartDate,
                new ToolStripSeparator(),
                lblEndDate, dtpEndDate,
                new ToolStripSeparator(),
                new ToolStripLabel("级别："), cboLevel,
                new ToolStripSeparator(),
                new ToolStripLabel("关键字："), txtSearch,
                btnSearch,
                new ToolStripSeparator(),
                btnExport,
                btnClear
            });

            // 日志列表
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
                new DataGridViewTextBoxColumn { DataPropertyName = "LogTime", HeaderText = "时间", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Level", HeaderText = "级别", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Message", HeaderText = "消息", Width = 400 },
                new DataGridViewTextBoxColumn { DataPropertyName = "UserName", HeaderText = "用户", Width = 100 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Source", HeaderText = "来源", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "IpAddress", HeaderText = "IP地址", Width = 120 }
            });

            // 分页控件
            var pagePanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            btnPrevPage = new Button { Text = "上一页", Width = 80 };
            btnNextPage = new Button { Text = "下一页", Width = 80 };
            lblPageInfo = new Label { AutoSize = true };

            btnPrevPage.Click += BtnPrevPage_Click;
            btnNextPage.Click += BtnNextPage_Click;

            pagePanel.Controls.AddRange(new Control[]
            {
                btnPrevPage,
                lblPageInfo,
                btnNextPage
            });

            // 布局
            this.Controls.AddRange(new Control[]
            {
                toolStrip,
                dataGridView,
                pagePanel
            });
        }

        private async void LoadLogs()
        {
            try
            {
                _startTime = dtpStartDate.Value;
                _endTime = dtpEndDate.Value;
                _selectedLevel = cboLevel.SelectedIndex == 0 ? null : cboLevel.Text;
                _keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text;

                var logs = await _logService.GetLogsAsync(
                    _startTime, _endTime, _selectedLevel, _keyword, _currentPage, PageSize);
                
                var totalCount = await _logService.GetLogsCountAsync(
                    _startTime, _endTime, _selectedLevel, _keyword);

                dataGridView.DataSource = logs;
                UpdatePagingInfo(totalCount);
            }
            catch (Exception ex)
            {
                Logger.Error("Error loading logs", ex);
                MessageBox.Show("加载日志失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePagingInfo(int totalCount)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            lblPageInfo.Text = $"第 {_currentPage} 页，共 {totalPages} 页";
            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            _currentPage = 1;
            LoadLogs();
        }

        private async void BtnExport_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dialog = new SaveFileDialog
                {
                    Filter = "CSV文件|*.csv",
                    FileName = $"系统日志_{DateTime.Now:yyyyMMddHHmmss}.csv"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    await _logService.ExportLogsAsync(dialog.FileName, _startTime, _endTime);
                    MessageBox.Show("导出完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error exporting logs", ex);
                MessageBox.Show("导出日志失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnClear_Click(object? sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要清理30天前的日志吗？", "确认",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await _logService.ClearLogsAsync(DateTime.Now.AddDays(-30));
                    LoadLogs();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error clearing logs", ex);
                MessageBox.Show("清理日志失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrevPage_Click(object? sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadLogs();
            }
        }

        private void BtnNextPage_Click(object? sender, EventArgs e)
        {
            _currentPage++;
            LoadLogs();
        }

        private DataGridView dataGridView = null!;
        private ToolStripDateTimePicker dtpStartDate = null!;
        private ToolStripDateTimePicker dtpEndDate = null!;
        private ToolStripComboBox cboLevel = null!;
        private ToolStripTextBox txtSearch = null!;
        private Button btnPrevPage = null!;
        private Button btnNextPage = null!;
        private Label lblPageInfo = null!;
    }
} 