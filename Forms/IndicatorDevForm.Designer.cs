using System.Windows.Forms;

namespace DataApiServiceForm
{
    partial class IndicatorDevForm
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
            this.btnLoadSql = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.txtSql = new System.Windows.Forms.TextBox();
            this.splitContainerBottom = new System.Windows.Forms.SplitContainer();
            this.grpParams = new System.Windows.Forms.GroupBox();
            this.dgvParams = new System.Windows.Forms.DataGridView();
            this.colParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParamValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).BeginInit();
            this.splitContainerBottom.Panel1.SuspendLayout();
            this.splitContainerBottom.Panel2.SuspendLayout();
            this.splitContainerBottom.SuspendLayout();
            this.grpParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).BeginInit();
            this.grpResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();

            // toolStrip1
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnLoadSql,
                this.toolStripSeparator1,
                this.btnExecute,
                this.toolStripSeparator2,
                this.btnClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 0;

            // btnLoadSql
            this.btnLoadSql.Text = "从指标加载SQL";
            this.btnLoadSql.Click += new System.EventHandler(this.btnLoadSql_Click);

            // toolStripSeparator1
            this.toolStripSeparator1.Name = "toolStripSeparator1";

            // btnExecute
            this.btnExecute.Text = "执行测试";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);

            // toolStripSeparator2
            this.toolStripSeparator2.Name = "toolStripSeparator2";

            // btnClear
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // splitContainerMain
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainerMain.Size = new System.Drawing.Size(984, 536);
            this.splitContainerMain.SplitterDistance = 320;
            this.splitContainerMain.TabIndex = 1;

            // Panel1 - SQL editor
            this.splitContainerMain.Panel1.Controls.Add(this.panelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Controls.Add(this.txtSql);
            this.panelTop.Padding = new System.Windows.Forms.Padding(4);

            this.txtSql.AcceptsReturn = true;
            this.txtSql.AcceptsTab = true;
            this.txtSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSql.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtSql.Location = new System.Drawing.Point(4, 4);
            this.txtSql.Multiline = true;
            this.txtSql.Name = "txtSql";
            this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSql.Size = new System.Drawing.Size(976, 312);
            this.txtSql.TabIndex = 0;
            this.txtSql.WordWrap = false;
            this.txtSql.TextChanged += new System.EventHandler(this.txtSql_TextChanged);

            // Panel2 - Params + Results
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerBottom);

            // splitContainerBottom
            this.splitContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottom.Name = "splitContainerBottom";
            this.splitContainerBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainerBottom.Size = new System.Drawing.Size(984, 212);
            this.splitContainerBottom.SplitterDistance = 90;
            this.splitContainerBottom.TabIndex = 0;

            // Panel1 - Params
            this.splitContainerBottom.Panel1.Controls.Add(this.grpParams);
            this.grpParams.Controls.Add(this.dgvParams);
            this.grpParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpParams.Location = new System.Drawing.Point(0, 0);
            this.grpParams.Name = "grpParams";
            this.grpParams.Size = new System.Drawing.Size(984, 90);
            this.grpParams.TabIndex = 0;
            this.grpParams.Text = "参数";

            this.dgvParams.AllowUserToAddRows = true;
            this.dgvParams.AllowUserToDeleteRows = true;
            this.dgvParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colParamName, this.colParamValue});
            this.dgvParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParams.Location = new System.Drawing.Point(3, 16);
            this.dgvParams.Name = "dgvParams";
            this.dgvParams.RowHeadersVisible = false;
            this.dgvParams.Size = new System.Drawing.Size(978, 71);
            this.dgvParams.TabIndex = 0;

            // colParamName
            this.colParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colParamName.FillWeight = 40;
            this.colParamName.HeaderText = "参数名";
            this.colParamName.Name = "colParamName";

            // colParamValue
            this.colParamValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colParamValue.FillWeight = 60;
            this.colParamValue.HeaderText = "参数值";
            this.colParamValue.Name = "colParamValue";

            // Panel2 - Result
            this.splitContainerBottom.Panel2.Controls.Add(this.grpResult);
            this.grpResult.Controls.Add(this.dgvResult);
            this.grpResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResult.Location = new System.Drawing.Point(0, 0);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(984, 118);
            this.grpResult.TabIndex = 0;
            this.grpResult.Text = "执行结果";

            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(3, 16);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.Size = new System.Drawing.Size(978, 99);
            this.dgvResult.TabIndex = 0;

            // IndicatorDevForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "IndicatorDevForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "指标开发";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitContainerBottom.Panel1.ResumeLayout(false);
            this.splitContainerBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).EndInit();
            this.splitContainerBottom.ResumeLayout(false);
            this.grpParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParams)).EndInit();
            this.grpResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLoadSql;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtSql;
        private System.Windows.Forms.SplitContainer splitContainerBottom;
        private System.Windows.Forms.GroupBox grpParams;
        private System.Windows.Forms.DataGridView dgvParams;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParamValue;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.DataGridView dgvResult;
    }
}
