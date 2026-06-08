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
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnIndicatorManage = new System.Windows.Forms.Button();
            this.btnIndicatorDev = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            //
            // lblConnectionString
            //
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(12, 12);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(113, 13);
            this.lblConnectionString.TabIndex = 0;
            this.lblConnectionString.Text = "Oracle 连接字符串:";
            //
            // txtConnectionString
            //
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(12, 32);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionString.Size = new System.Drawing.Size(760, 55);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Text = "User Id=username;Password=password;Data Source=//host:port/service_name";
            //
            // lblPort
            //
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(12, 100);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(58, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "API 端口:";
            //
            // nudPort
            //
            this.nudPort.Location = new System.Drawing.Point(80, 97);
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
            this.nudPort.Size = new System.Drawing.Size(80, 20);
            this.nudPort.TabIndex = 3;
            this.nudPort.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            //
            // btnStartStop
            //
            this.btnStartStop.Location = new System.Drawing.Point(180, 95);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(100, 23);
            this.btnStartStop.TabIndex = 4;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            //
            // btnIndicatorManage
            //
            this.btnIndicatorManage.Location = new System.Drawing.Point(295, 95);
            this.btnIndicatorManage.Name = "btnIndicatorManage";
            this.btnIndicatorManage.Size = new System.Drawing.Size(100, 23);
            this.btnIndicatorManage.TabIndex = 6;
            this.btnIndicatorManage.Text = "指标管理";
            this.btnIndicatorManage.UseVisualStyleBackColor = true;
            this.btnIndicatorManage.Click += new System.EventHandler(this.btnIndicatorManage_Click);
            //
            // btnIndicatorDev
            //
            this.btnIndicatorDev.Location = new System.Drawing.Point(410, 95);
            this.btnIndicatorDev.Name = "btnIndicatorDev";
            this.btnIndicatorDev.Size = new System.Drawing.Size(100, 23);
            this.btnIndicatorDev.TabIndex = 7;
            this.btnIndicatorDev.Text = "指标开发";
            this.btnIndicatorDev.UseVisualStyleBackColor = true;
            this.btnIndicatorDev.Click += new System.EventHandler(this.btnIndicatorDev_Click);
            //
            // txtLog
            //
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 130);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(760, 320);
            this.txtLog.TabIndex = 5;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnIndicatorDev);
            this.Controls.Add(this.btnIndicatorManage);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.lblConnectionString);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "Form1";
            this.Text = "Data API Service - 指标管理系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnIndicatorManage;
        private System.Windows.Forms.Button btnIndicatorDev;
        private System.Windows.Forms.TextBox txtLog;
    }
}
