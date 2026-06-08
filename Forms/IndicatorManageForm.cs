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
        private int _selectedCategoryId = -1; // -1 means all

        public IndicatorManageForm(Func<string> connectionStringProvider)
        {
            InitializeComponent();
            _service = new IndicatorService(connectionStringProvider);
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

            var allNode = treeViewCategories.Nodes.Add("全部指标");
            allNode.Tag = -1;

            foreach (var cat in _categories)
            {
                var node = treeViewCategories.Nodes.Add(cat.CategoryName);
                node.Tag = cat.CategoryId;
            }

            treeViewCategories.ExpandAll();
            // Select "全部" by default
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

            // Filter by category
            if (_selectedCategoryId > 0)
            {
                filtered = filtered.Where(i => i.CategoryId == _selectedCategoryId);
            }

            // Filter by status
            var statusFilter = cmbStatusFilter.SelectedIndex;
            if (statusFilter == 1) // 启用
            {
                filtered = filtered.Where(i => i.Status == "1");
            }
            else if (statusFilter == 2) // 禁用
            {
                filtered = filtered.Where(i => i.Status == "0");
            }

            // Filter by search text
            var searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(i =>
                    (i.IndicatorCode != null && i.IndicatorCode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (i.IndicatorName != null && i.IndicatorName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            BindIndicators(filtered.ToList());
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
            if (e.Node?.Tag is int catId)
            {
                _selectedCategoryId = catId;
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
            {
                OpenIndicatorEdit(indicator);
            }
            else if (e.ColumnIndex == colDelete.Index)
            {
                DeleteIndicator(indicator);
            }
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
                    {
                        entity.CreateUser = Environment.UserName;
                    }

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
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSortOrder;
        private System.Windows.Forms.NumericUpDown nudSortOrder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

        public string CategoryCode { get { return txtCode.Text.Trim(); } }
        public string CategoryName { get { return txtName.Text.Trim(); } }
        public int SortOrder { get { return (int)nudSortOrder.Value; } }

        public CategoryEditForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSortOrder = new System.Windows.Forms.Label();
            this.nudSortOrder = new System.Windows.Forms.NumericUpDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSortOrder)).BeginInit();
            this.SuspendLayout();

            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(12, 15);
            this.lblCode.Text = "分类编码:";

            this.txtCode.Location = new System.Drawing.Point(85, 12);
            this.txtCode.Size = new System.Drawing.Size(250, 20);
            this.txtCode.TabIndex = 0;

            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 45);
            this.lblName.Text = "分类名称:";

            this.txtName.Location = new System.Drawing.Point(85, 42);
            this.txtName.Size = new System.Drawing.Size(250, 20);
            this.txtName.TabIndex = 1;

            this.lblSortOrder.AutoSize = true;
            this.lblSortOrder.Location = new System.Drawing.Point(12, 75);
            this.lblSortOrder.Text = "排序号:";

            this.nudSortOrder.Location = new System.Drawing.Point(85, 72);
            this.nudSortOrder.Maximum = 9999;
            this.nudSortOrder.Size = new System.Drawing.Size(80, 20);
            this.nudSortOrder.TabIndex = 2;

            this.btnOk.Location = new System.Drawing.Point(160, 110);
            this.btnOk.Size = new System.Drawing.Size(80, 28);
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("分类编码和名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            };

            this.btnCancel.Location = new System.Drawing.Point(255, 110);
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.Text = "取消";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(350, 150);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.nudSortOrder);
            this.Controls.Add(this.lblSortOrder);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增分类";
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            ((System.ComponentModel.ISupportInitialize)(this.nudSortOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    /// <summary>
    /// 指标编辑弹窗
    /// </summary>
    public class IndicatorEditForm : Form
    {
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblQuerySql;
        private System.Windows.Forms.TextBox txtQuerySql;
        private System.Windows.Forms.Label lblParamDef;
        private System.Windows.Forms.TextBox txtParamDef;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

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
        public string DataType { get { return cmbDataType.SelectedItem?.ToString() ?? "LIST"; } }
        public string Unit { get { return txtUnit.Text.Trim(); } }
        public bool Status { get { return chkStatus.Checked; } }

        public IndicatorEditForm(List<IndicatorCategory> categories, IndicatorDef existing = null)
        {
            _categories = categories;
            InitializeComponent();
            PopulateCategories(existing?.CategoryId);

            if (existing != null)
            {
                this.Text = "编辑指标";
                txtCode.Text = existing.IndicatorCode;
                txtCode.ReadOnly = true;
                txtName.Text = existing.IndicatorName;
                txtQuerySql.Text = existing.QuerySql ?? "";
                txtParamDef.Text = existing.ParamDef ?? "";
                txtDescription.Text = existing.Description ?? "";
                txtUnit.Text = existing.Unit ?? "";
                chkStatus.Checked = existing.Status == "1";
                cmbDataType.SelectedItem = existing.DataType ?? "LIST";
            }
        }

        private void PopulateCategories(int? selectedCategoryId)
        {
            foreach (var cat in _categories)
            {
                cmbCategory.Items.Add(cat);
                if (selectedCategoryId.HasValue && cat.CategoryId == selectedCategoryId.Value)
                {
                    cmbCategory.SelectedItem = cat;
                }
            }
            if (cmbCategory.SelectedIndex < 0 && cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblQuerySql = new System.Windows.Forms.Label();
            this.txtQuerySql = new System.Windows.Forms.TextBox();
            this.lblParamDef = new System.Windows.Forms.Label();
            this.txtParamDef = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int y = 10;
            int labelX = 12;
            int inputX = 80;

            // Code
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblCode.Text = "指标编码:";
            this.txtCode.Location = new System.Drawing.Point(inputX, y);
            this.txtCode.Size = new System.Drawing.Size(150, 20);
            this.txtCode.TabIndex = 0;

            // DataType
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(250, y + 3);
            this.lblDataType.Text = "类型:";
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataType.Items.AddRange(new object[] { "SINGLE", "LIST" });
            this.cmbDataType.Location = new System.Drawing.Point(280, y);
            this.cmbDataType.Size = new System.Drawing.Size(80, 21);
            this.cmbDataType.SelectedIndex = 1;

            y += 30;

            // Name
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblName.Text = "指标名称:";
            this.txtName.Location = new System.Drawing.Point(inputX, y);
            this.txtName.Size = new System.Drawing.Size(420, 20);

            // Unit
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(250, y + 3);
            this.lblUnit.Text = "单位:";
            this.txtUnit.Location = new System.Drawing.Point(280, y);
            this.txtUnit.Size = new System.Drawing.Size(80, 20);

            y += 30;

            // Category + Status
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblCategory.Text = "所属分类:";
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(inputX, y);
            this.cmbCategory.Size = new System.Drawing.Size(200, 21);

            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(300, y + 2);
            this.chkStatus.Text = "启用";
            this.chkStatus.Checked = true;

            y += 30;

            // Description
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblDescription.Text = "指标说明:";
            this.txtDescription.Location = new System.Drawing.Point(inputX, y);
            this.txtDescription.Size = new System.Drawing.Size(420, 20);

            y += 30;

            // QuerySQL
            this.lblQuerySql.AutoSize = true;
            this.lblQuerySql.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblQuerySql.Text = "查询SQL:";
            this.txtQuerySql.Location = new System.Drawing.Point(inputX, y);
            this.txtQuerySql.Multiline = true;
            this.txtQuerySql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuerySql.Size = new System.Drawing.Size(420, 150);
            this.txtQuerySql.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtQuerySql.WordWrap = false;

            y += 160;

            // ParamDef
            this.lblParamDef.AutoSize = true;
            this.lblParamDef.Location = new System.Drawing.Point(labelX, y + 3);
            this.lblParamDef.Text = "参数定义:";
            this.txtParamDef.Location = new System.Drawing.Point(inputX, y);
            this.txtParamDef.Size = new System.Drawing.Size(420, 40);
            this.txtParamDef.Multiline = true;

            y += 55;

            // Buttons
            this.btnOk.Location = new System.Drawing.Point(280, y);
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("指标编码和名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
            };

            this.btnCancel.Location = new System.Drawing.Point(390, y);
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.Text = "取消";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            y += 40;

            this.ClientSize = new System.Drawing.Size(520, y + 10);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbDataType);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtParamDef);
            this.Controls.Add(this.lblParamDef);
            this.Controls.Add(this.txtQuerySql);
            this.Controls.Add(this.lblQuerySql);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增指标";
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
