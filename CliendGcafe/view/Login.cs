using CCMS.Config;
using CCMS.lib;
using CCMS.model;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class Login : FormView
    {
        public Thread t1 = null;
        CultureInfo culture;
        ResourceManager rm;
        Socket socketOnline;
        public Login()
        {
            
            InitializeComponent();

            setLanguage();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // showQRCodeLogin();
            //startServer();
            pnlogin.Visible = true;
            pnloading.Visible = false;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            // check login admin
            if (txtUser.Text == Constant.username && txtPassword.Text == Constant.password)
            {
                User.updateStatusLoginAdmin(1);
                closeThread();
                GlobalSystem.is_admin = 1;
                GlobalSystem.islogin = 1;
                Helper.roleWindown(true);
                Home frmHome = new Home("");
                this.Hide();
                frmHome.ShowDialog(this);
                this.Close();

                Slide2 frm1 = new Slide2();
                frm1.Close();
                return;
            }
            if(this.checkConnectServer() == true)
            {
                //login on server
                string username = txtUser.Text.Trim();
                string password = txtPassword.Text.Trim();
                processLogin(username, password);
            }
            else
            {
                MessageBox.Show(this.rm.GetString("checkconecthost", culture));
            }
        }

        private void showQRCodeLogin()
        {
            try
            {
                string urlQAcode = ClientPartner.QALink;
                var request = WebRequest.Create(urlQAcode);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    //ptbQacode.Image = Bitmap.FromStream(stream);
                    //ptbQacode.SizeMode = PictureBoxSizeMode.StretchImage;
                    //ptbQacode.Size = new Size(200, 200);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThisLine("showQRCodeLogin: "+ex.ToString());
                return;
            }
        }


        public void processLogin(string username, String password, String token = "")
        {
            try
            {
                if (GlobalSystem.islogin == 0)
                {
                    GlobalSystem.timeStart = DateTime.Now;
                }

                string myParameters = "username=" + username + "&password=" + password + "&token=" + token;

                string URI = Constant.serverHost + Constant.methodLogin;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
                    Logger.LogDebugFile("Login json tra ve: " + HtmlResult);
                    dynamic data = JObject.Parse(HtmlResult);
                    Logger.LogDebugFile("check status login: " + data.status);
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
                        objUser.type = data.data.type;
                        objUser.total_monney = data.data.total_monney;
                        objUser.total_discount = data.data.total_discount;
                        objUser.token = data.data.token;
                        //thời gian này dung để gửi lên cho server tính tiền
                        Logger.LogDebugFile("Login last_time_request: "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ " |Tong tien" + data.data.total_monney);
                        objUser.last_time_request = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        //thời gian này dùng để công thêm 3 phút rồi gọi lên server
                        objUser.last_request_time = data.data.last_request_time;

                        //them truong check có phải may thi đấu hay không
                        objUser.in_competitive = data.data.in_competitive;

                        objUser.conversation_id = data.data.conversation_id;
                        Logger.LogDebugFile("Start check group_user_info");
                        if (data.data.group_user_info != null)
                        {
                            Logger.LogDebugFile("Data group_user_info: " + data.data.group_user_info.promotion_percent);
                            objUser.promotion_percent = data.data.group_user_info.promotion_percent;
                        }                        
                        else
                        {
                            if (InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    Helper.showMessageError("Lỗi đăng nhập. User không thuộc nhóm user nào");
                                }));
                            }
                        }
                        Logger.LogDebugFile("End check group_user_info");
                        Logger.LogDebugFile("Start check group_computer_info");
                        if (data.data.group_computer_info != null)
                        {
                            Logger.LogDebugFile("Data group_computer_info: " + data.data.group_computer_info.price);
                            objUser.price = data.data.group_computer_info.price;
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    Helper.showMessageError("Lỗi đăng nhập. máy tính hiện tại chưa được cấu hình vào nhóm máy");
                                }));
                            }
                        }
                        Logger.LogDebugFile("End check group_computer_info");
                        // check tiền có hay không
                        var totalMonney = Int32.Parse(objUser.total_monney.ToString()) + Int32.Parse(objUser.total_discount.ToString());
                        Logger.LogDebugFile("Total Monney: "+ totalMonney);
                        objUser.total_money_login_session = totalMonney;
                       
                        Logger.LogDebugFile("Set updateGlobalTimeGame");
                        Helper.updateGlobalTimeGame(HtmlResult);
                        Logger.LogDebugFile("End set updateGlobalTimeGame");
                        GlobalSystem.user = objUser;
                        //go to home
                        goToHome(HtmlResult);
                    }
                    else if(data.status == 401)
                    {
                        //check xem co may nao da login tren tai khoan nay chua
                        String ip = data.data[0].ip.ToString();
                        String _username = data.data[0].username.ToString();
                        Helper.logoutCliendWithAccount(ip, "1", _username);
                        //ShowProcessLoad frmAlert = new ShowProcessLoad("Tài khoản bạn đang đăng nhập nơi khác, vui lòng đăng nhập lại sau vài giây nữa!");
                        //frmAlert.ShowDialog(this);
                        pnlogin.Visible = false;
                        pnloading.Visible = true;
                        loadingProgressBar();

                        return;
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                String message = data.message;
                                Helper.showMessageError(message);
                            }));
                        }
                        else
                        {
                            String message = data.message;
                            Helper.showMessageError(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Logger.LogThisLine("processLogin: " + ex.Message);
                        Helper.showMessageError("processLogin: " + ex.Message);
                    }));
                }
                else
                {
                    Logger.LogThisLine("processLogin: " + ex.Message);
                    Helper.showMessageError("processLogin: " + ex.Message);
                }
            }
        }

        private async void loadingProgressBar()
        {
            try
            {
                loading1.Value = 0;
                for (int i = 1; i <= 100; i++)
                {
                    loading1.Value = i;
                    loading1.Text = i.ToString();
                    loading1.Update();
                    await Task.Delay(100);
                }
                pnloading.Visible = false;
                pnlogin.Visible = true;
            }
            catch(Exception ex)
            {
                Logger.LogThisLine("loadingProgressBar" + ex.ToString());
            }
        }

        public void goToHome(String json)
        {
            if(GlobalSystem.socket != null)
            {
                GlobalSystem.socket.Disconnect();
                GlobalSystem.socket.Close();
            }
            Home frmHome = new Home(json);
            this.Hide();
            frmHome.ShowDialog(this);
            this.Close();
            Slide2 frm1 = new Slide2();
            frm1.Close();
        }

        public void closeThread()
        {
            try
            {
                if (this.t1 != null)
                    t1.Abort();
            }catch(Exception ex)
            {
                Logger.LogThisLine("closeThread: " + ex.ToString());
            }
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setLanguage()
        {
            try
            {

            }catch(Exception ex)
            {

            }
            this.culture = CultureInfo.CreateSpecificCulture(GlobalSystem.language);
            this.rm = new ResourceManager("CCMS.Lang.MyResource", typeof(Login).Assembly);
            label1.Text = rm.GetString("username", culture);
            label2.Text = rm.GetString("password", culture);
            btnlogin.Text = rm.GetString("btnlogin", culture);
            btnhuy.Text = rm.GetString("btnhuy", culture);
            groupBox1.Text = rm.GetString("pannel_login", culture);            
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void startServer()
        {
            try
            {
                socketOnline = IO.Socket(Constant.serverSoketOnline);
                socketOnline.On(Socket.EVENT_CONNECT, () =>
                {
                });
                socketOnline.On("request app remote login", (data) =>
                {
                    Logger.LogDebugFile("---------------START LOGIN QR CODE-------------------- ");
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    string partner_id = result.partner_id;
                    string username = result.username;
                    string token = result.token;
                    Logger.LogDebugFile("request app remote login Debug: " + ip + "---------------" + GlobalSystem.ipv4);
                    Logger.LogDebugFile("request app remote login Debug: " + partner_id + "---------------" + ClientPartner.partner_id);
                    if (ip == GlobalSystem.ipv4 && partner_id == ClientPartner.partner_id)
                    {
                        Logger.LogDebugFile("xử lý login ");
                        processLogin(username, null, token);
                    }
                    Logger.LogDebugFile("---------------START LOGIN QR CODE-------------------- ");
                });
            }
            catch (Exception e)
            {
                Logger.LogThisLine("startServer: " + e.Message);
                Thread.Sleep(GlobalSystem.sleep);
                
                startServer();
            }
        }

       
       
    }
}
