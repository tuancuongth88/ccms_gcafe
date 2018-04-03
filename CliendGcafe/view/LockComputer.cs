using CCMS.Config;
using CCMS.lib;
using System;
using System.Windows.Forms;

namespace CCMS.view
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
                string pass = txtpass.Text;
                string passComfirm = txtComfirmPass.Text;
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
                GlobalSystem.islogin = 1;
                // an cac form truoc
                for (int i = Application.OpenForms.Count - 1; i >= 0; i += -1)
                {
                    if (!object.ReferenceEquals(Application.OpenForms[i], this))
                    {
                        Application.OpenForms[i].Hide();
                    }
                }
                this.Hide();
                Slide2 frm = new Slide2();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống!");
                Logger.LogThisLine("Lỗi hệ thống!: " + ex.Message);
                this.Close();
            }
        }
    }
}
