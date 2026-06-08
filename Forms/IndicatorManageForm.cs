using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataApiServiceForm.Models;
using DataApiServiceForm.Services;

namespace DataApiServiceForm
{
    public partial class IndicatorManageForm : Form
    {
        private readonly IndicatorService _service;
        private List<IndicatorCategory> _categories;
        private List<IndicatorDef> _allIndicators;
        private int _selectedCategoryId = -1;

        public IndicatorManageForm(Func<string> connectionStringProvider)
        {
            InitializeComponent();
            _service = new IndicatorService(connectionStringProvider);
            SetupDataGridViewStyle();
        }

        private void SetupDataGridViewStyle()
        {
            dgvIndicators.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            dgvIndicators.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
            dgvIndicators.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvIndicators.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvIndicators.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            dgvIndicators.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                LoadCategories();
                LoadIndicators();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载数据失败: {0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            _categories = _service.GetCategories();
            treeViewCategories.Nodes.Clear();

            var allNode = treeViewCategories.Nodes.Add("[ 全部指标 ]");
            allNode.Tag = -1;

            foreach (var cat in _categories)
            {
                var node = treeViewCategories.Nodes.Add(cat.CategoryName);
                node.Tag = cat.CategoryId;
            }

            treeViewCategories.ExpandAll();
            if (treeViewCategories.Nodes.Count > 0)
            {
                treeViewCategories.SelectedNode = treeViewCategories.Nodes[0];
            }
        }

        private void LoadIndicators()
        {
            _allIndicators = _service.GetAllIndicators();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allIndicators == null) return;
            var filtered = _allIndicators.AsEnumerable();

            if (_selectedCategoryId > 0)
            {
                filtered = filtered.Where(i => i.CategoryId == _selectedCategoryId);
            }

            var statusFilter = cmbStatusFilter.SelectedIndex;
            if (statusFilter == 1)
                filtered = filtered.Where(i => i.Status == "1");
            else if (statusFilter == 2)
                filtered = filtered.Where(i => i.Status == "0");

            var searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(i =>
                    (i.IndicatorCode != null && i.IndicatorCode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (i.IndicatorName != null && i.IndicatorName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            var list = filtered.ToList();
            BindIndicators(list);
            lblCount.Text = string.Format("共 {0} 条指标", list.Count);
        }

        private void BindIndicators(List<IndicatorDef> indicators)
        {
            dgvIndicators.Rows.Clear();
            foreach (var ind in indicators)
            {
                var rowIndex = dgvIndicators.Rows.Add(
                    ind.IndicatorCode,
                    ind.IndicatorName,
                    ind.CategoryName ?? "",
                    ind.DataTypeText,
                    ind.StatusText
                );
                dgvIndicators.Rows[rowIndex].Tag = ind;
            }
        }

        private void treeViewCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is int)
            {
                _selectedCategoryId = (int)e.Node.Tag;
                ApplyFilter();
            }
        }

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ApplyFilter();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            using (var dlg = new CategoryEditForm())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var cat = new IndicatorCategory
                    {
                        CategoryCode = dlg.CategoryCode,
                        CategoryName = dlg.CategoryName,
                        SortOrder = dlg.SortOrder,
                        Status = "1"
                    };
                    try
                    {
                        _service.SaveCategory(cat);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("保存分类失败: {0}", ex.Message), "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddIndicator_Click(object sender, EventArgs e)
        {
            OpenIndicatorEdit(null);
        }

        private void dgvIndicators_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var indicator = dgvIndicators.Rows[e.RowIndex].Tag as IndicatorDef;
            if (indicator == null) return;

            if (e.ColumnIndex == colEdit.Index)
                OpenIndicatorEdit(indicator);
            else if (e.ColumnIndex == colDelete.Index)
                DeleteIndicator(indicator);
        }

        private void OpenIndicatorEdit(IndicatorDef indicator)
        {
            using (var dlg = new IndicatorEditForm(_categories, indicator))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var entity = indicator ?? new IndicatorDef();
                    entity.IndicatorCode = dlg.IndicatorCode;
                    entity.IndicatorName = dlg.IndicatorName;
                    entity.CategoryId = dlg.SelectedCategoryId;
                    entity.QuerySql = dlg.QuerySql;
                    entity.ParamDef = dlg.ParamDef;
                    entity.Description = dlg.Description;
                    entity.DataType = dlg.DataType;
                    entity.Unit = dlg.Unit;
                    entity.Status = dlg.Status ? "1" : "0";
                    entity.UpdateUser = Environment.UserName;
                    if (indicator == null)
                        entity.CreateUser = Environment.UserName;

                    try
                    {
                        _service.SaveIndicator(entity);
                        LoadIndicators();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("保存指标失败: {0}", ex.Message), "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteIndicator(IndicatorDef indicator)
        {
            var text = string.Format("确定要删除指标 \"{0}\" ({1}) 吗？", indicator.IndicatorName, indicator.IndicatorCode);
            if (MessageBox.Show(text, "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _service.DeleteIndicator(indicator.IndicatorId);
                    LoadIndicators();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("删除失败: {0}", ex.Message), "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    /// <summary>
    /// 分类编辑弹窗
    /// </summary>
    public class CategoryEditForm : Form
    {
        private Label lblCode;
        private TextBox txtCode;
        private Label lblName;
        private TextBox txtName;
        private Label lblSortOrder;
        private NumericUpDown nudSortOrder;
        private Button btnOk;
        private Button btnCancel;

        public string CategoryCode { get { return txtCode.Text.Trim(); } }
        public string CategoryName { get { return txtName.Text.Trim(); } }
        public int SortOrder { get { return (int)nudSortOrder.Value; } }

        public CategoryEditForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblCode = new Label { AutoSize = true, Location = new Point(14, 18), Text = "分类编码:" };
            this.txtCode = new TextBox { Location = new Point(85, 15), Size = new Size(280, 20) };
            this.lblName = new Label { AutoSize = true, Location = new Point(14, 48), Text = "分类名称:" };
            this.txtName = new TextBox { Location = new Point(85, 45), Size = new Size(280, 20) };
            this.lblSortOrder = new Label { AutoSize = true, Location = new Point(14, 78), Text = "排序号:" };
            this.nudSortOrder = new NumericUpDown
            {
                Location = new Point(85, 75),
                Size = new Size(80, 20),
                Maximum = 9999
            };
            this.btnOk = new Button
            {
                Text = "确定",
                Location = new Point(185, 115),
                Size = new Size(85, 28),
                UseVisualStyleBackColor = true,
                DialogResult = DialogResult.OK
            };
            this.btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("分类编码和名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            };
            this.btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(280, 115),
                Size = new Size(85, 28),
                DialogResult = DialogResult.Cancel
            };

            this.ClientSize = new Size(380, 158);
            this.Controls.AddRange(new Control[] { btnCancel, btnOk, nudSortOrder, lblSortOrder, txtName, lblName, txtCode, lblCode });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "新增分类";
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            ((System.ComponentModel.ISupportInitialize)nudSortOrder).BeginInit();
        }
    }

    /// <summary>
    /// 指标编辑弹窗 - 使用 TabControl 组织字段
    /// </summary>
    public class IndicatorEditForm : Form
    {
        private TabControl tabControl;
        private TabPage tabBasic;
        private TabPage tabSql;

        // Basic info fields
        private Label lblCode, lblName, lblCategory, lblDataType, lblUnit, lblDescription;
        private TextBox txtCode, txtName, txtDescription, txtUnit;
        private ComboBox cmbCategory, cmbDataType;
        private CheckBox chkStatus;

        // SQL fields
        private Label lblQuerySql, lblParamDef;
        private TextBox txtQuerySql, txtParamDef;

        private Button btnOk, btnCancel;

        private readonly List<IndicatorCategory> _categories;

        public string IndicatorCode { get { return txtCode.Text.Trim(); } }
        public string IndicatorName { get { return txtName.Text.Trim(); } }
        public int? SelectedCategoryId
        {
            get
            {
                if (cmbCategory.SelectedItem is IndicatorCategory cat)
                    return cat.CategoryId;
                return null;
            }
        }
        public string QuerySql { get { return txtQuerySql.Text.Trim(); } }
        public string ParamDef { get { return txtParamDef.Text.Trim(); } }
        public string Description { get { return txtDescription.Text.Trim(); } }
        public string DataType { get { return cmbDataType.SelectedItem != null ? cmbDataType.SelectedItem.ToString() : "LIST"; } }
        public string Unit { get { return txtUnit.Text.Trim(); } }
        public bool Status { get { return chkStatus.Checked; } }

        public IndicatorEditForm(List<IndicatorCategory> categories, IndicatorDef existing = null)
        {
            _categories = categories;
            InitializeComponent();
            PopulateCategories(existing != null ? existing.CategoryId : (int?)null);

            if (existing != null)
            {
                this.Text = "编辑指标";
                txtCode.Text = existing.IndicatorCode;
                txtCode.ReadOnly = true;
                txtName.Text = existing.IndicatorName;
                txtDescription.Text = existing.Description ?? "";
                txtUnit.Text = existing.Unit ?? "";
                chkStatus.Checked = existing.Status == "1";
                cmbDataType.SelectedItem = existing.DataType ?? "LIST";
                txtQuerySql.Text = existing.QuerySql ?? "";
                txtParamDef.Text = existing.ParamDef ?? "";
            }
        }

        private void PopulateCategories(int? selectedCategoryId)
        {
            foreach (var cat in _categories)
            {
                cmbCategory.Items.Add(cat);
                if (selectedCategoryId.HasValue && cat.CategoryId == selectedCategoryId.Value)
                    cmbCategory.SelectedItem = cat;
            }
            if (cmbCategory.SelectedIndex < 0 && cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl { Location = new Point(10, 8), Size = new Size(535, 400) };
            this.tabBasic = new TabPage("基本信息");
            this.tabSql = new TabPage("SQL 定义");
            this.tabControl.TabPages.AddRange(new TabPage[] { tabBasic, tabSql });

            // ── Tab: 基本信息 ──
            var fontBold = new Font(this.Font, FontStyle.Bold);

            int baseX = 15, baseY = 10, lineH = 32, lblW = 65, shortW = 170, longW = 480;

            this.lblCode = new Label { Text = "指标编码*", Location = new Point(baseX, baseY + 4), AutoSize = true };
            this.txtCode = new TextBox { Location = new Point(baseX + lblW, baseY), Size = new Size(shortW, 21) };

            this.lblDataType = new Label { Text = "类型", Location = new Point(baseX + lblW + shortW + 25, baseY + 4), AutoSize = true };
            this.cmbDataType = new ComboBox
            {
                Location = new Point(baseX + lblW + shortW + 55, baseY - 1),
                Size = new Size(75, 21),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.cmbDataType.Items.AddRange(new object[] { "SINGLE", "LIST" });
            this.cmbDataType.SelectedIndex = 1;

            baseY += lineH;

            this.lblName = new Label { Text = "指标名称*", Location = new Point(baseX, baseY + 4), AutoSize = true };
            this.txtName = new TextBox { Location = new Point(baseX + lblW, baseY), Size = new Size(shortW, 21) };

            this.lblUnit = new Label { Text = "单位", Location = new Point(baseX + lblW + shortW + 25, baseY + 4), AutoSize = true };
            this.txtUnit = new TextBox { Location = new Point(baseX + lblW + shortW + 55, baseY - 1), Size = new Size(75, 21) };

            baseY += lineH;

            this.lblCategory = new Label { Text = "所属分类", Location = new Point(baseX, baseY + 4), AutoSize = true };
            this.cmbCategory = new ComboBox
            {
                Location = new Point(baseX + lblW, baseY),
                Size = new Size(210, 21),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            this.chkStatus = new CheckBox
            {
                Text = "启用",
                Location = new Point(baseX + lblW + 220, baseY + 2),
                AutoSize = true,
                Checked = true
            };

            baseY += lineH;

            this.lblDescription = new Label { Text = "指标说明", Location = new Point(baseX, baseY + 4), AutoSize = true };
            this.txtDescription = new TextBox { Location = new Point(baseX + lblW, baseY), Size = new Size(longW, 21) };

            tabBasic.Controls.AddRange(new Control[] {
                lblCode, txtCode, lblDataType, cmbDataType,
                lblName, txtName, lblUnit, txtUnit,
                lblCategory, cmbCategory, chkStatus,
                lblDescription, txtDescription
            });

            // ── Tab: SQL 定义 ──
            var sqly = 8;

            this.lblQuerySql = new Label { Text = "查询 SQL:", Location = new Point(10, sqly), AutoSize = true, Font = fontBold };
            sqly += 20;
            this.txtQuerySql = new TextBox
            {
                Location = new Point(10, sqly),
                Size = new Size(505, 240),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 9.5F),
                WordWrap = false
            };

            sqly += 248;

            var lblHint = new Label
            {
                Text = "提示: 使用 #{参数名} 作为占位符，例如 #{startDate}",
                Location = new Point(15, sqly),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(this.Font, FontStyle.Italic)
            };

            sqly += 22;

            this.lblParamDef = new Label { Text = "参数定义 (JSON):", Location = new Point(10, sqly), AutoSize = true };
            sqly += 20;
            this.txtParamDef = new TextBox
            {
                Location = new Point(10, sqly),
                Size = new Size(505, 52),
                Multiline = true,
                Font = new Font("Consolas", 9F)
            };

            tabSql.Controls.AddRange(new Control[] {
                lblQuerySql, txtQuerySql, lblHint, lblParamDef, txtParamDef
            });

            // ── Buttons ──
            var btnY = 418;
            this.btnOk = new Button
            {
                Text = "确定",
                Location = new Point(330, btnY),
                Size = new Size(100, 30),
                UseVisualStyleBackColor = true,
                DialogResult = DialogResult.OK
            };
            this.btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("指标编码和名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.None;
                }
            };

            this.btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(440, btnY),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // ── Form ──
            this.ClientSize = new Size(555, 460);
            this.Controls.AddRange(new Control[] { tabControl, btnOk, btnCancel });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "新增指标";
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }
    }
}
