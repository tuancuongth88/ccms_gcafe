using CCMS.Config;
using CCMS.lib;
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

namespace CCMS.view
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
            String pass = txtpasswordLog.Text;
            if(pass == GlobalSystem.user.pass_lock_computer)
            {
                unLock();
            }
            else
            {
                Helper.showMessageError("Sai mật khẩu!");
            }
        }
        

        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void unLock()
        {
            if(this.t != null)
                this.t.Abort();
            Home frmHome = new Home("");
            this.Hide();
            frmHome.ShowDialog(this);
            this.Close();

            Slide2 frm1 = new Slide2();
            frm1.Close();
        }

        
    }
}
