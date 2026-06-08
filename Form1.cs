using System;
using System.Windows.Forms;
using DataApiServiceForm.Properties;
using DataApiServiceForm.Services;

namespace DataApiServiceForm
{
    public partial class Form1 : Form
    {
        private ApiHttpServer _server;

        public Form1()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            txtConnectionString.Text = Settings.Default.ConnectionString;
            nudPort.Value = Settings.Default.ApiPort;
        }

        private void SaveSettings()
        {
            Settings.Default.ConnectionString = txtConnectionString.Text;
            Settings.Default.ApiPort = (int)nudPort.Value;
            Settings.Default.Save();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_server != null && _server.IsRunning)
            {
                StopServer();
            }
            else
            {
                StartServer();
            }
        }

        private void StartServer()
        {
            var port = (int)nudPort.Value;
            var connectionString = txtConnectionString.Text;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("Please enter an Oracle connection string.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _server = new ApiHttpServer(
                    port,
                    () => txtConnectionString.Text,
                    msg => this.BeginInvoke(new Action(() => AppendLog(msg)))
                );

                _server.Start();

                SaveSettings();

                // Update UI to "running" state
                txtConnectionString.Enabled = false;
                nudPort.Enabled = false;
                btnStartStop.Text = "停止服务";
            }
            catch (Exception ex)
            {
                AppendLog(string.Format("Startup failed: {0}", ex.Message));
                MessageBox.Show(ex.Message, "Failed to start server",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopServer()
        {
            try
            {
                if (_server != null) { _server.Stop(); }
            }
            finally
            {
                // Update UI to "stopped" state
                txtConnectionString.Enabled = true;
                nudPort.Enabled = true;
                btnStartStop.Text = "启动服务";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            if (_server != null && _server.IsRunning)
            {
                _server.Stop();
            }
        }

        private void btnIndicatorManage_Click(object sender, EventArgs e)
        {
            var form = new IndicatorManageForm(() => txtConnectionString.Text);
            form.Show(this);
        }

        private void btnIndicatorDev_Click(object sender, EventArgs e)
        {
            var form = new IndicatorDevForm(() => txtConnectionString.Text);
            form.Show(this);
        }

        private void AppendLog(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtLog.AppendText(string.Format("[{0}] {1}{2}", timestamp, message, Environment.NewLine));
        }
    }
}
