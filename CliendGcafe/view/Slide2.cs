using CCMS.Config;
using CCMS.lib;
//using CliendGcafe.Properties;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CCMS.model;
using CCMS.Properties;

namespace CCMS.view
{
    public partial class Slide2 : FormView
    {
        Thread t, t1 = null;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;
        private String json;
        public Slide2()
        {
            InitializeComponent();
            // set ung dung chay cung win khi khoi dong
            rkApp.SetValue("MyAppCCMS", Application.ExecutablePath.ToString());
            // set full nam hinh
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

        }
        private void Slide2_Load(object sender, EventArgs e)
        {
            try
            {
                // set ip glocal
                Helper.setIp4Global();

                string URI = this.host_connent + Constant.methodSlide;
                using (WebClient wc = new WebClient())
                {
                    this.json = wc.DownloadString(URI);
                }
                if (GlobalSystem.islogin > 0)
                {
                    pictureBox1.Image = Resources._lock;
                }
                else
                {
                    if (this.isConnectHost == true)
                    {
                        t = new Thread(slideImage);
                        t.Start();
                        startServer();
                    }
                    else
                    {
                        pictureBox1.Image = Resources.background;
                    }
                }

                //check shutdown
                if (GlobalSystem.time_shutdown > 0)
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            Logger.LogThisLine("start thread shuwdown Invoke");
                            t1 = new Thread(shutdown);
                            t1.Start();
                        }));
                    }
                    else
                    {
                        Logger.LogThisLine("start thread shuwdown");
                        t1 = new Thread(shutdown);
                        t1.Start();
                    }

                }
            }
            catch(Exception ex)
            {
                Logger.LogThisLine("Slide2_Load: " + ex.Message);
            }
            

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
                if(GlobalSystem.islogin == 1)
                {
                    t.Abort();
                }
                if (!string.IsNullOrEmpty(this.json))
                {
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

        private void shutdown()
        {
            try
            {
                int sleep_time = GlobalSystem.time_shutdown * 1000;
                Logger.LogThisLine("Time start shutdown: " + DateTime.Now);
                DateTime endTime = DateTime.Now.AddSeconds(GlobalSystem.time_shutdown);
                
                if (t1.IsAlive == false)
                {
                    t1.Start();
                }
                Thread.Sleep(sleep_time);
                if(GlobalSystem.islogin == 1)
                {
                    t1.Abort();
                    return;
                }
                Logger.LogThisLine("Time end shutdown: " + DateTime.Now);
                Logger.LogDebugFile("Shutdown may tinh: " + GlobalSystem.time_shutdown * 1000);
                if(DateTime.Now >= endTime)
                {
                    Logger.LogThisLine("Shut down may tinh");
                    System.Diagnostics.Process.Start("Shutdown", "/r /t 0");
                    Environment.Exit(0);
                }
                else
                {
                    shutdown();
                }
            }catch(Exception ex)
            {
                Logger.LogThisLine("shutdown :" + ex.Message);
            }
            finally
            {
                //shutdown();
            }
        }

        private void Slide2_KeyPress(object sender, KeyPressEventArgs e)
        {
            showFormLogin();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            showFormLogin();
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
                    Logger.LogThisLine("Debug: "+ip +"---------------"+ GlobalSystem.ipv4);
                    
                    if (ip == GlobalSystem.ipv4)
                    {
                        bool checklogin =  processLoginHelper(json);
                        Logger.LogThisLine("check login soket: " + checklogin);
                        Logger.LogThisLine("chuỗi json trả về: " + json);
                        if (checklogin)
                        {
                            JObject jout = JObject.FromObject(new { status = 1 });
                            socket.Emit("refresh view", jout);
                            if (InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate {
                                    Home frmHome = new Home(json);
                                    this.Hide();
                                    frmHome.ShowDialog(this);
                                    this.Close();
                                }));
                                return;
                            }
                        }
                        else
                        {
                            JObject jout = JObject.FromObject(new { status = 0});
                            socket.Emit("refresh view", jout);
                        }
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

        public bool processLoginHelper(String json)
        {
            try
            {
                Logger.LogDebugFile("-----------Start Login tu xa-------------");
                dynamic data = JObject.Parse(json);
                if (GlobalSystem.islogin == 0)
                {
                    Logger.LogDebugFile("Thoi gian dang nhap: "+ DateTime.Now);
                    GlobalSystem.timeStart = DateTime.Now;
                }
                if (data.status == 200)
                {
                    Helper.roleWindown(false);                    

                    if (this.t.IsAlive == true)
                        this.t.Abort();
                    if(this.t1.IsAlive == true)
                        this.t1.Abort();
                    GlobalSystem.isLogout = 0;
                    GlobalSystem.islogin = 1;
                    User objUser = new User();
                    objUser.id = data.data.id;
                    objUser.username = data.data.username;
                    objUser.password = data.password;
                    objUser.name = data.data.name;
                    objUser.email = data.data.email;
                    objUser.type = data.data.type;
                    objUser.total_monney = data.data.total_monney;
                    objUser.total_discount = data.data.total_discount;
                    objUser.token = data.data.token;
                    Logger.LogThisLine("Login last_time_request: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " |Tong tien" + data.data.total_monney);
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
                        return false;
                    }
                    GlobalSystem.user = objUser;
                    Helper.updateGlobalTimeGame(json);
                    return true;                                      

                }
                else
                {
                    String message = data.message;
                    Helper.showMessageError(message);
                    Logger.LogThisLine(message);
                }
                Logger.LogDebugFile("-----------END Login tu xa-------------");
            }
            catch (Exception ex)
            {
                Logger.LogThisLine(ex.Message);
                return false;
            }
            return false;
        }

        private void Slide2_KeyDown(object sender, KeyEventArgs e)
        {
            showFormLogin();
        }

        private void showFormLogin()
        {
            if (GlobalSystem.islogin == 0)
            {
                Login frmlogin = new Login(t, t1);
                frmlogin.ShowDialog(this);
            }
            else
            {
                UnlockComputer frmUnlockLock = new UnlockComputer(this.t);
                frmUnlockLock.ShowDialog(this);
            }
        }
      

        private void Slide2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

    }
}
