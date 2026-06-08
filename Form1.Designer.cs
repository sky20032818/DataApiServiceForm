namespace DataApiServiceForm
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_server != null) { _server.Dispose(); _server = null; }
                if (components != null) { components.Dispose(); }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.btnIndicatorManage = new System.Windows.Forms.Button();
            this.btnIndicatorDev = new System.Windows.Forms.Button();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.grpTools.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            //
            // grpServer
            //
            this.grpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServer.Controls.Add(this.lblConnectionString);
            this.grpServer.Controls.Add(this.txtConnectionString);
            this.grpServer.Controls.Add(this.lblPort);
            this.grpServer.Controls.Add(this.nudPort);
            this.grpServer.Controls.Add(this.btnStartStop);
            this.grpServer.Location = new System.Drawing.Point(12, 8);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(780, 100);
            this.grpServer.TabIndex = 0;
            this.grpServer.Text = "服务配置";
            //
            // lblConnectionString
            //
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(10, 22);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(113, 13);
            this.lblConnectionString.TabIndex = 0;
            this.lblConnectionString.Text = "Oracle 连接字符串:";
            //
            // txtConnectionString
            //
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(10, 40);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(760, 20);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Text = string.Empty;
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(10, 72);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(58, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "API 端口:";
            //
            // nudPort
            //
            this.nudPort.Location = new System.Drawing.Point(78, 69);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(65, 20);
            this.nudPort.TabIndex = 3;
            this.nudPort.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            //
            // btnStartStop
            //
            this.btnStartStop.Location = new System.Drawing.Point(155, 67);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 24);
            this.btnStartStop.TabIndex = 4;
            this.btnStartStop.Text = "启动服务";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            //
            // grpTools
            //
            this.grpTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTools.Controls.Add(this.btnIndicatorManage);
            this.grpTools.Controls.Add(this.btnIndicatorDev);
            this.grpTools.Location = new System.Drawing.Point(12, 114);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(780, 54);
            this.grpTools.TabIndex = 1;
            this.grpTools.Text = "指标工具";
            //
            // btnIndicatorManage
            //
            this.btnIndicatorManage.Location = new System.Drawing.Point(10, 19);
            this.btnIndicatorManage.Name = "btnIndicatorManage";
            this.btnIndicatorManage.Size = new System.Drawing.Size(100, 26);
            this.btnIndicatorManage.TabIndex = 0;
            this.btnIndicatorManage.Text = "指标管理";
            this.btnIndicatorManage.UseVisualStyleBackColor = true;
            this.btnIndicatorManage.Click += new System.EventHandler(this.btnIndicatorManage_Click);
            //
            // btnIndicatorDev
            //
            this.btnIndicatorDev.Location = new System.Drawing.Point(120, 19);
            this.btnIndicatorDev.Name = "btnIndicatorDev";
            this.btnIndicatorDev.Size = new System.Drawing.Size(100, 26);
            this.btnIndicatorDev.TabIndex = 1;
            this.btnIndicatorDev.Text = "指标开发";
            this.btnIndicatorDev.UseVisualStyleBackColor = true;
            this.btnIndicatorDev.Click += new System.EventHandler(this.btnIndicatorDev_Click);
            //
            // grpLog
            //
            this.grpLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Location = new System.Drawing.Point(12, 174);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(780, 330);
            this.grpLog.TabIndex = 2;
            this.grpLog.Text = "服务日志";
            //
            // txtLog
            //
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtLog.ForeColor = System.Drawing.Color.LightGray;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(10, 18);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(760, 303);
            this.txtLog.TabIndex = 0;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 516);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpTools);
            this.Controls.Add(this.grpServer);
            this.MinimumSize = new System.Drawing.Size(620, 440);
            this.Name = "Form1";
            this.Text = "Data API Service - 指标管理系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.grpTools.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.GroupBox grpTools;
        private System.Windows.Forms.Button btnIndicatorManage;
        private System.Windows.Forms.Button btnIndicatorDev;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.TextBox txtLog;
    }
}
