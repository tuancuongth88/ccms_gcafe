using CliendGcafe.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe.view
{
    public partial class LockComputer : MetroFramework.Forms.MetroForm
    {
        public LockComputer()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnchangepass_Click(object sender, EventArgs e)
        {
            try
            {
                string pass = txtpass.ToString();
                string passComfirm = txtComfirmPass.ToString();
                if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(passComfirm))
                {
                    MessageBox.Show("Phải nhập mật khẩu khóa máy");
                    return;
                }
                if (!passComfirm.Equals(pass))
                {
                    MessageBox.Show("Mật khẩu nhập lại không trùng với mật khẩu trên");
                    return;
                }
                GlobalSystem.user.pass_lock_computer = pass;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống!");
                this.Close();
            }
        }
    }
}
