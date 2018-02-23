using CliendGcafe.Config;
using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp.WinForms;
using CliendGcafe.lib;

namespace CliendGcafe.view
{
    public partial class OrderService : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        public OrderService()
        {
            InitializeComponent();
            InitializeChromium();
        }
        public void InitializeChromium()
        {
            try
            {
                chromeBrowser = new ChromiumWebBrowser(Constant.serverHost + Constant.methodService);
                panel2.Controls.Add(chromeBrowser);
                chromeBrowser.Dock = DockStyle.Fill;
            }
            catch(Exception ex)
            {
                Logger.LogThisLine(ex.Message);
                MessageBox.Show("Dịch vụ lỗi không hoạt động được");
                return;
            }
            
        }

        private void OrderService_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cef.Shutdown();
        }

        private void OrderService_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.BackColor = Color.Red;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
