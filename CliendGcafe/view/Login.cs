using CCMS.Config;
using CCMS.lib;
using CCMS.model;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Net;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class Login : FormView
    {
        public Thread t, t1 = null;
        CultureInfo culture;
        ResourceManager rm;
        
        public Login(Thread t, Thread t1)
        {
            this.t = t;
            this.t1 = t1;
            InitializeComponent();
            setLanguage();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            // check login admin
            if(txtUser.Text == Constant.username && txtPassword.Text == Constant.password)
            {
                closeThread();
                GlobalSystem.is_admin = 1;
                Helper.roleWindown(true);
                Home frmHome = new Home("");
                this.Hide();
                frmHome.ShowDialog(this);
                this.Close();

                Slide2 frm1 = new Slide2();
                frm1.Close();
                return;
            }
            if(this.isConnectHost == true)
            {
                //login on server
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

                        closeThread();
                        GlobalSystem.islogin = 1;
                        GlobalSystem.isLogout = 0;
                        User objUser = new User();
                        objUser.id = data.data.id;
                        objUser.username = data.data.username;
                        objUser.password = password;
                        objUser.name = data.data.name;
                        objUser.email = data.data.email;
                        objUser.type = data.data.type;
                        objUser.total_monney = data.data.total_monney;
                        objUser.total_discount = data.data.total_discount;
                        objUser.token = data.data.token;
                        Logger.LogThisLine("Login last_time_request: "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ " |Tong tien" + data.data.total_monney);
                        objUser.last_time_request = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        objUser.conversation_id = data.data.conversation_id;

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
                       
                        Helper.updateGlobalTimeGame(HtmlResult);
                        
                        GlobalSystem.user = objUser;
                        Home frmHome = new Home(HtmlResult);
                        this.Hide();
                        frmHome.ShowDialog(this);
                        this.Close();

                        Slide2 frm1 = new Slide2();
                        frm1.Close();
                    }
                    else if(data.status == 401)
                    {
                        //check xem co may nao da login tren tai khoan nay chua
                            String ip = data.data[0].ip.ToString();
                            String user_id = data.data[0].user_id.ToString();
                            Helper.logoutCliendWithAccount(ip, "1", user_id);
                            MessageBox.Show("Tài khoản bạn đang đăng nhập nơi khác, vui lòng đăng nhập lại sau vài giây nữa!");
                            return;
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
                Logger.LogThisLine(ex.Message);
                Helper.showMessageError(ex.Message);
            }
        }

        public void closeThread()
        {
            if (this.t != null)
                t.Abort();
            if (this.t1 != null)
                t1.Abort();
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setLanguage()
        {
            this.culture = CultureInfo.CreateSpecificCulture(GlobalSystem.language);
            this.rm = new ResourceManager("CCMS.Lang.MyResource", typeof(Login).Assembly);
            label1.Text = rm.GetString("username", culture);
            label2.Text = rm.GetString("password", culture);
            btnlogin.Text = rm.GetString("btnlogin", culture);
            btnhuy.Text = rm.GetString("btnhuy", culture);
            groupBox1.Text = rm.GetString("pannel_login", culture);
        }

        
       
    }
}
