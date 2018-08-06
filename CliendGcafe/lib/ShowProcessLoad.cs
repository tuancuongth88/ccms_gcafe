using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.lib
{
    public partial class ShowProcessLoad : Form
    {
        string message;
        public ShowProcessLoad(String message)
        {
            InitializeComponent();
            this.message = message;
            
        }

        private void ShowProcessLoad_Load(object sender, EventArgs e)
        {
            show(this.message);
        }

        public void show(String message)
        {
            lblalert.Text = message;
            lblalert.AutoSize = false;
            lblalert.Size = new Size(365, 50);
            lblalert.TextAlign = ContentAlignment.MiddleCenter;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 3000;
            progressBar1.Maximum = 10;
            timer1.Tick += new EventHandler(timer1_Tick);
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value != 10)
            {
                progressBar1.Value++;
            }
            else
            {
                timer1.Stop();
                this.Close();
            }
        }

    }
}
