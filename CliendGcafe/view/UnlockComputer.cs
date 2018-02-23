using CliendGcafe.Config;
using CliendGcafe.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe.view
{
    public partial class UnlockComputer : Form
    {
        public Thread t = null;
        public UnlockComputer(Thread t)
        {
            this.t = t;
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            String pass = txtPassword.Text;
            if(pass == GlobalSystem.user.pass_lock_computer)
            {
                t.Abort();
                Home frmHome = new Home();
                this.Hide();
                frmHome.ShowDialog(this);
                this.Close();
            }
        }
        

        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
