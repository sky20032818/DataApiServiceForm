using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataApiServiceForm.Models;
using DataApiServiceForm.Services;

namespace DataApiServiceForm
{
    public partial class IndicatorDevForm : Form
    {
        private readonly Func<string> _connectionStringProvider;
        private readonly OracleQueryExecutor _executor;
        private readonly IndicatorService _indicatorService;

        public IndicatorDevForm(Func<string> connectionStringProvider)
        {
            InitializeComponent();
            _connectionStringProvider = connectionStringProvider;
            _executor = new OracleQueryExecutor();
            _indicatorService = new IndicatorService(connectionStringProvider);
        }

        private void btnLoadSql_Click(object sender, EventArgs e)
        {
            try
            {
                var indicators = _indicatorService.GetAllIndicators();
                if (indicators.Count == 0)
                {
                    MessageBox.Show("没有可用的指标定义", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new IndicatorSelectForm(indicators))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK && dlg.SelectedIndicator != null)
                    {
                        txtSql.Text = dlg.SelectedIndicator.QuerySql ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载指标失败: {0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var sql = txtSql.Text.Trim();
            if (string.IsNullOrEmpty(sql))
            {
                MessageBox.Show("请输入 SQL", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get connection string
            var connectionString = _connectionStringProvider();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("请在主界面配置Oracle连接字符串", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Collect parameters
            var parameters = new Dictionary<string, object>();
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;
                var key = row.Cells[0].Value?.ToString()?.Trim();
                var value = row.Cells[1].Value?.ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    parameters[key] = value ?? "";
                }
            }

            // Replace param placeholders
            var finalSql = sql;
            foreach (var kvp in parameters)
            {
                var placeholder = "#{" + kvp.Key + "}";
                finalSql = finalSql.Replace(placeholder, kvp.Value != null ? kvp.Value.ToString() : "");
            }

            // Execute
            try
            {
                List<string> columns;
                var data = _executor.ExecuteQuery(connectionString, finalSql, out columns);

                // Show results
                dgvResult.Columns.Clear();
                if (columns != null && columns.Count > 0)
                {
                    foreach (var col in columns)
                    {
                        dgvResult.Columns.Add(col, col);
                    }
                    foreach (var row in data)
                    {
                        var rowIndex = dgvResult.Rows.Add();
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var key = columns[i];
                            var val = row.ContainsKey(key) ? row[key] : null;
                            dgvResult.Rows[rowIndex].Cells[i].Value = val;
                        }
                    }
                    dgvResult.AutoResizeColumns();
                }
                grpResult.Text = string.Format("执行结果 (共 {0} 行)", data.Count);
            }
            catch (Exception ex)
            {
                dgvResult.Columns.Clear();
                dgvResult.Rows.Clear();
                grpResult.Text = "执行结果 (失败)";
                MessageBox.Show(string.Format("执行失败: {0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSql.Clear();
            dgvParams.Rows.Clear();
            dgvResult.Columns.Clear();
            dgvResult.Rows.Clear();
            grpResult.Text = "执行结果";
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            ParseSqlParams();
        }

        private void ParseSqlParams()
        {
            var sql = txtSql.Text;
            if (string.IsNullOrEmpty(sql)) return;

            // Find all #{paramName} placeholders
            var matches = Regex.Matches(sql, @"#\{(\w+)\}");
            var paramNames = new HashSet<string>();
            foreach (Match match in matches)
            {
                paramNames.Add(match.Groups[1].Value);
            }

            // Keep existing values
            var existingValues = new Dictionary<string, string>();
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;
                var key = row.Cells[0].Value?.ToString()?.Trim();
                var value = row.Cells[1].Value?.ToString();
                if (!string.IsNullOrEmpty(key))
                {
                    existingValues[key] = value ?? "";
                }
            }

            // Rebuild param grid
            dgvParams.Rows.Clear();
            foreach (var name in paramNames.OrderBy(n => n))
            {
                var val = existingValues.ContainsKey(name) ? existingValues[name] : "";
                dgvParams.Rows.Add(name, val);
            }
        }
    }

    /// <summary>
    /// 指标选择弹窗
    /// </summary>
    public class IndicatorSelectForm : Form
    {
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

        public IndicatorDef SelectedIndicator { get; private set; }

        public IndicatorSelectForm(List<IndicatorDef> indicators)
        {
            InitializeComponent();
            PopulateList(indicators);
        }

        private void PopulateList(List<IndicatorDef> indicators)
        {
            foreach (var ind in indicators)
            {
                var rowIndex = dgv.Rows.Add(ind.IndicatorCode, ind.IndicatorName, ind.CategoryName ?? "");
                dgv.Rows[rowIndex].Tag = ind;
            }
        }

        private void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();

            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colCode, this.colName, this.colCategory});
            this.dgv.Location = new System.Drawing.Point(12, 12);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(460, 300);
            this.dgv.TabIndex = 0;

            this.colCode.HeaderText = "编码";
            this.colCode.FillWeight = 30;

            this.colName.HeaderText = "名称";
            this.colName.FillWeight = 40;

            this.colCategory.HeaderText = "分类";
            this.colCategory.FillWeight = 30;

            this.btnOk.Location = new System.Drawing.Point(280, 325);
            this.btnOk.Size = new System.Drawing.Size(90, 28);
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    SelectedIndicator = dgv.SelectedRows[0].Tag as IndicatorDef;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("请选择一条指标", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            this.btnCancel.Location = new System.Drawing.Point(382, 325);
            this.btnCancel.Size = new System.Drawing.Size(90, 28);
            this.btnCancel.Text = "取消";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(484, 365);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择指标";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
