using CCMS.Config;
using CCMS.lib;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace CCMS.view
{
    public partial class ChangePassword : MetroFramework.Forms.MetroForm
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnchangepass_Click(object sender, EventArgs e)
        {
            changePass();
        }
        public void changePass()
        {
            try
            {
                if (GlobalSystem.user != null)
                {
                    string pwdOld = txtOldPass.Text.Trim();
                    string pwdNew = txtNewpass.Text.Trim();
                    string pwdComfirm = txtComfirmPass.Text.Trim();
                    if (string.IsNullOrEmpty(pwdOld))
                    {
                        Helper.showMessageError("Phải nhập mật khẩu cũ!");
                        return;
                    }
                    if (String.IsNullOrEmpty(pwdNew))
                    {
                        Helper.showMessageError("Phải nhập mật khẩu mới!");
                        return;
                    }
                    if (string.IsNullOrEmpty(pwdComfirm))
                    {
                        Helper.showMessageError("Phải nhập lại mật khẩu mới!");
                        return;
                    }
                    if (pwdNew == pwdComfirm)
                    {
                        string myParameters = "old_password=" + pwdOld + "&password=" + pwdNew + "&password_confirmation=" + pwdComfirm;
                        string URI = Constant.serverHost + Constant.methodChangePass;
                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            wc.Headers.Add("token", GlobalSystem.user.token);
                            string HtmlResult = wc.UploadString(URI, myParameters);

                            dynamic data = JObject.Parse(HtmlResult);
                            if (data.status == 200)
                            {
                                //GlobalSystem.user.password = txtNewpass.Text.ToString();
                                Helper.showMessageError("Đổi mật khẩu thành công");
                                this.Close();
                                return;
                            }
                            else
                            {
                                String message = data.message;
                                Helper.showMessageError(message);
                            }
                        }
                    }
                    else
                    {
                        Helper.showMessageError("Mật khẩu nhập lại phải trùng với mật khẩu mới");
                        return;
                    }
                }
            }
            catch(Exception e)
            {
                Helper.showMessageError("Xảy ra lỗi khi đổi mật khẩu!");
                this.Close();
            }
        }
    }
}
