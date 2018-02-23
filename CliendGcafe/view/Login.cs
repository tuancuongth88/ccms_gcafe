using CliendGcafe.Config;
using CliendGcafe.lib;
using CliendGcafe.model;
using CliendGcafe.Properties;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe.view
{
    public partial class Login : FormView
    {
        public Thread t = null;
        CultureInfo culture;
        ResourceManager rm;
        
        public Login(Thread t)
        {
            this.t = t;
            InitializeComponent();
            setLanguage();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if(this.isConnectHost == true)
            {
                processLogin();
            }
            else
            {
                MessageBox.Show(this.rm.GetString("checkconecthost", culture));
            }
        }

        public void processLogin()
        {
            try
            {
                if(GlobalSystem.islogin == 0)
                {
                    GlobalSystem.timeStart = DateTime.Now;
                }
                string username = txtUser.Text.Trim();
                string password = txtPassword.Text.Trim();
                string myParameters = "username=" + username + "&password=" + password;

                string URI = host_connent + methodLogin;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
                    
                    dynamic data = JObject.Parse(HtmlResult);
                    if (data.status == 200)
                    {
                        Helper.roleWindown(false);

                        t.Abort();
                        GlobalSystem.isLogout = 0;
                        User objUser = new User();
                        objUser.id = data.data.id;
                        objUser.username = data.data.username;
                        objUser.name = data.data.name;
                        objUser.email = data.data.email;
                        objUser.type = data.data.type;
                        objUser.total_monney = data.data.total_monney;
                        objUser.total_discount = data.data.total_discount;
                        objUser.token = data.data.token;
                        
                        if (data.data.group_user_info != null)
                        {
                            objUser.promotion_percent = data.data.group_user_info.promotion_percent;
                        }
                        else
                        {
                            Helper.showMessageError("Lỗi đăng nhập. User không thuộc nhóm user nào");
                        }
                        if (data.data.group_computer_info != null)
                        {
                            objUser.price = data.data.group_computer_info.price;
                        }
                        else
                        {
                            Helper.showMessageError("Lỗi đăng nhập. máy tính hiện tại chưa được cấu hình vào nhóm máy");
                        }
                        // check tiền có hay không
                        var totalMonney = Int32.Parse(objUser.total_monney.ToString()) + Int32.Parse(objUser.total_discount.ToString());
                        objUser.total_money_login_session = totalMonney;
                        if (totalMonney <= 0)
                        {
                            Helper.showMessageError("Tài khoản bạn không đủ tiền!");
                            return;
                        }
                        GlobalSystem.user = objUser;
                        Home frmHome = new Home();
                        this.Hide();
                        frmHome.ShowDialog(this);
                        this.Close();

                        Slide2 frm1 = new Slide2();
                        frm1.Close();
                    }
                    else
                    {
                        String message = data.message;
                        Helper.showMessageError(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.showMessageError(ex.Message);
            }
        }
        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setLanguage()
        {
            this.culture = CultureInfo.CreateSpecificCulture(GlobalSystem.language);
            this.rm = new ResourceManager("CliendGcafe.Lang.MyResource", typeof(Login).Assembly);
            label1.Text = rm.GetString("username", culture);
            label2.Text = rm.GetString("password", culture);
            btnlogin.Text = rm.GetString("btnlogin", culture);
            btnhuy.Text = rm.GetString("btnhuy", culture);
            groupBox1.Text = rm.GetString("pannel_login", culture);
        }

       
    }
}
