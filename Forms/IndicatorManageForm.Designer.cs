using System.Windows.Forms;

namespace DataApiServiceForm
{
    partial class IndicatorManageForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddCategory = new System.Windows.Forms.ToolStripButton();
            this.btnAddIndicator = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewCategories = new System.Windows.Forms.TreeView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.dgvIndicators = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicators)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // toolStrip1
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnAddCategory,
                this.btnAddIndicator,
                this.toolStripSeparator1,
                this.btnRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1040, 28);
            this.toolStrip1.TabIndex = 0;

            // btnAddCategory
            this.btnAddCategory.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddCategory.Text = " 新增分类";
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);

            // btnAddIndicator
            this.btnAddIndicator.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddIndicator.Text = " 新增指标";
            this.btnAddIndicator.Click += new System.EventHandler(this.btnAddIndicator_Click);

            // toolStripSeparator1
            this.toolStripSeparator1.Name = "toolStripSeparator1";

            // btnRefresh
            this.btnRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefresh.Text = " 刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(1040, 568);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 1;

            // treeViewCategories
            this.splitContainer1.Panel1.Controls.Add(this.treeViewCategories);
            this.treeViewCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCategories.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F);
            this.treeViewCategories.HideSelection = false;
            this.treeViewCategories.Indent = 19;
            this.treeViewCategories.ItemHeight = 24;
            this.treeViewCategories.Location = new System.Drawing.Point(0, 0);
            this.treeViewCategories.Name = "treeViewCategories";
            this.treeViewCategories.Size = new System.Drawing.Size(220, 568);
            this.treeViewCategories.TabIndex = 0;
            this.treeViewCategories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCategories_AfterSelect);

            // panelRight
            this.splitContainer1.Panel2.Controls.Add(this.panelRight);
            this.panelRight.Controls.Add(this.dgvIndicators);
            this.panelRight.Controls.Add(this.panelFilter);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(816, 568);
            this.panelRight.TabIndex = 0;

            // panelFilter
            this.panelFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelFilter.Controls.Add(this.lblSearch);
            this.panelFilter.Controls.Add(this.txtSearch);
            this.panelFilter.Controls.Add(this.btnSearch);
            this.panelFilter.Controls.Add(this.lblStatus);
            this.panelFilter.Controls.Add(this.cmbStatusFilter);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(816, 42);
            this.panelFilter.TabIndex = 0;

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 14);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(35, 13);
            this.lblSearch.Text = "搜索:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(52, 11);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(278, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(360, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.Text = "状态:";

            // cmbStatusFilter
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.FormattingEnabled = true;
            this.cmbStatusFilter.Items.AddRange(new object[] { "全部", "启用", "禁用" });
            this.cmbStatusFilter.Location = new System.Drawing.Point(398, 11);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(80, 21);
            this.cmbStatusFilter.TabIndex = 3;
            this.cmbStatusFilter.SelectedIndex = 0;
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cmbStatusFilter_SelectedIndexChanged);

            // dgvIndicators
            this.dgvIndicators.AllowUserToAddRows = false;
            this.dgvIndicators.AllowUserToDeleteRows = false;
            this.dgvIndicators.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIndicators.BackgroundColor = System.Drawing.Color.White;
            this.dgvIndicators.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvIndicators.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvIndicators.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvIndicators.ColumnHeadersHeight = 30;
            this.dgvIndicators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvIndicators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colCode, this.colName, this.colCategory, this.colDataType, this.colStatus, this.colEdit, this.colDelete});
            this.dgvIndicators.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIndicators.EnableHeadersVisualStyles = false;
            this.dgvIndicators.Name = "dgvIndicators";
            this.dgvIndicators.ReadOnly = true;
            this.dgvIndicators.RowHeadersVisible = false;
            this.dgvIndicators.RowTemplate.Height = 28;
            this.dgvIndicators.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIndicators.TabIndex = 1;
            this.dgvIndicators.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIndicators_CellContentClick);

            // colCode
            this.colCode.HeaderText = "编码";
            this.colCode.Name = "colCode";
            this.colCode.FillWeight = 95;

            // colName
            this.colName.HeaderText = "名称";
            this.colName.Name = "colName";
            this.colName.FillWeight = 130;

            // colCategory
            this.colCategory.HeaderText = "分类";
            this.colCategory.Name = "colCategory";
            this.colCategory.FillWeight = 80;

            // colDataType
            this.colDataType.HeaderText = "类型";
            this.colDataType.Name = "colDataType";
            this.colDataType.FillWeight = 50;

            // colStatus
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.FillWeight = 50;

            // colEdit
            this.colEdit.HeaderText = "";
            this.colEdit.Name = "colEdit";
            this.colEdit.Text = "编辑";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.FillWeight = 45;

            // colDelete
            this.colDelete.HeaderText = "";
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "删除";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.FillWeight = 45;

            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 596);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1040, 22);
            this.statusStrip1.TabIndex = 2;

            // lblCount
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(59, 17);
            this.lblCount.Text = "共 0 条指标";

            // IndicatorManageForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 618);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(860, 500);
            this.Name = "IndicatorManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "指标管理";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicators)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddCategory;
        private System.Windows.Forms.ToolStripButton btnAddIndicator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewCategories;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.DataGridView dgvIndicators;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
    }
}
