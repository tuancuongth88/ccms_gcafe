using CliendGcafe.Config;
using CliendGcafe.lib;
using CliendGcafe.Properties;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CliendGcafe.model;

namespace CliendGcafe.view
{
    public partial class Slide2 : FormView
    {
        Thread t = null;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;
        public Slide2()
        {
            InitializeComponent();
            // set ung dung chay cung win khi khoi dong
            rkApp.SetValue("MyAppCliendGcafe", Application.ExecutablePath.ToString());

            // set full nam hinh
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
        }
        public void showImage(WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    pictureBox1.Image = Bitmap.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                slideImage();
            }
        }

        public void slideImage()
        {
            try
            {
                string URI = host_connent + Constant.methodSlide;
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(URI);
                    dynamic data = JObject.Parse(json);
                    var listImg = (JArray)data.data;
                    var count = listImg.Count();
                    while (true)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, count);
                        string img_string = listImg[randomNumber].ToString();
                        var request = WebRequest.Create(img_string);
                        showImage(request);
                        Thread.Sleep(GlobalSystem.sleep);
                    }
                }
            }catch(Exception ex)
            {
                Thread.Sleep(GlobalSystem.sleep);
                slideImage();
            }
        }
        private void Slide2_Load(object sender, EventArgs e)
        {
            string myHost = System.Net.Dns.GetHostName();
            string myIP = null;

            for (int i = 0; i <= System.Net.Dns.GetHostEntry(myHost).AddressList.Length - 1; i++)
            {
                if (System.Net.Dns.GetHostEntry(myHost).AddressList[i].IsIPv6LinkLocal == false)
                {
                    myIP = System.Net.Dns.GetHostEntry(myHost).AddressList[i].ToString();
                    GlobalSystem.ipv4 = myIP;
                }
            }
            if (this.isConnectHost == true)
            {
                t = new Thread(new ThreadStart(slideImage));
                t.Start();
                startServer();
            }
            else
            {
                pictureBox1.Image = Resources.background;
            }

        }

        
        private void Slide2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Login frmlogin = new Login(t);
            frmlogin.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login frmlogin = new Login(t);
            frmlogin.ShowDialog(this);
        }

        public void startServer()
        {
            try
            {
                var socket = IO.Socket(Constant.serverSoket);
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                });           
                socket.On("login helper", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    if (ip == GlobalSystem.ipv4)
                    {
                        processLoginHelper(result);                        
                    }
                });
            }
            catch (Exception e)
            {
                Logger.LogThisLine(e.Message);
                Thread.Sleep(GlobalSystem.sleep);
                startServer();
            }
        }

        public void processLoginHelper(dynamic data)
        {
            try
            {
                if (GlobalSystem.islogin == 0)
                {
                    GlobalSystem.timeStart = DateTime.Now;
                }
                if (data.status == 200)
                {
                    Helper.roleWindown(false);
                    this.t.Abort();
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
                    if (InvokeRequired)
                    {                        
                        // after we've done all the processing, 
                        this.Invoke(new MethodInvoker(delegate {
                            Home frmHome = new Home();
                            this.Hide();

                            frmHome.ShowDialog(this);
                            this.Close();
                        }));
                        return;
                    }
                   

                }
                else
                {
                    String message = data.message;
                    Helper.showMessageError(message);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThisLine(ex.Message);
                MessageBox.Show("Xảy ra lỗi đăng nhập từ xa!");
                return;
            }
        }

        private void Slide2_KeyDown(object sender, KeyEventArgs e)
        {
            Login frmlogin = new Login(t);
            frmlogin.ShowDialog(this);
        }
      

        private void Slide2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

    }
}
