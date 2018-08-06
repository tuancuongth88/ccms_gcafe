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
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace CCMS.view
{
    public partial class Slide2 : FormView
    {
        private int countShowformHome = 0;
        Thread t, t1 = null;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private Socket socket;
        //private DisableKey lockey = new DisableKey();
        public Slide2()
        {
            InitializeComponent();
            //Helper.roleWindown(false);
            Helper.ToggleTaskManager(true);
            // set ung dung chay cung win khi khoi dong
            if (rkApp.GetValue("Kingdom_NextGen") == null)
            {
                rkApp.SetValue("Kingdom_NextGen", Application.ExecutablePath.ToString());
            }
            
            Helper.killProcessDupplicate();
            // set full nam hinh
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

        }
        private void Slide2_Load(object sender, EventArgs e)
        {
            try
            {
                this.countShowformHome = 0;
                // set ip glocal
                Helper.setIp4Global();
                //get QA code and partner_id
                Helper.setPartnerAndQR();

                if (GlobalSystem.islogin > 0)
                {
                    pictureBox1.Image = Resources._lock;
                }
                else
                {
                    pictureBox1.Image = Resources.background1;
                    if (this.checkConnectServer() == true)
                    {
                        //ket noi den soket server
                        startServer();
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

                showFormLogin();
            }
            catch(Exception ex)
            {
                Logger.LogThisLine("Slide2_Load: " + ex.Message);
            }
            

        }

        #region Tam thoi fix cung 1 hinh
        //public void showImage(WebRequest request)
        //{
        //    try
        //    {
        //        using (var response = request.GetResponse())
        //        using (var stream   = response.GetResponseStream())
        //        {
        //            pictureBox1.Image = Bitmap.FromStream(stream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {                
        //        slideImage();
        //    }
        //}

        //Tam thoi fix cung 1 hinh
        //public void slideImage()
        //{
        //    try
        //    {
        //        if(GlobalSystem.islogin == 1)
        //        {
        //            t.Abort();
        //        }
        //        if (!string.IsNullOrEmpty(this.json))
        //        {
        //            dynamic data = JObject.Parse(json);
        //            var listImg = (JArray)data.data;
        //            var count = listImg.Count();
        //            while (true)
        //            {
        //                Random random = new Random();
        //                int randomNumber = random.Next(0, count);
        //                string img_string = listImg[randomNumber].ToString();
        //                var request = WebRequest.Create(img_string);
        //                showImage(request);
        //                Thread.Sleep(GlobalSystem.sleep);
        //            }
        //        }
        //    }catch(Exception ex)
        //    {
        //        Thread.Sleep(GlobalSystem.sleep);
        //        Logger.LogThisLine("slideImage " + ex.ToString());
        //    }
        //}
        #endregion

        public async void shutdown()
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
                await Task.Delay(sleep_time);
                if (GlobalSystem.islogin == 1)
                {
                    t1.Abort();
                    return;
                }
                Logger.LogThisLine("Time end shutdown: " + DateTime.Now);
                Logger.LogDebugFile("Shutdown may tinh: " + GlobalSystem.time_shutdown * 1000);
                if (DateTime.Now >= endTime)
                {
                    Logger.LogThisLine("Shut down may tinh");
                    System.Diagnostics.Process.Start("Shutdown", "/s /t 0");
                    Environment.Exit(0);
                }
                else
                {
                    shutdown();
                }
            }
            catch (Exception ex)
            {
                Logger.LogThisLine("shutdown :" + ex.Message);
            }
        }

        private void Slide2_KeyPress(object sender, KeyPressEventArgs e)
        {
            showFormLogin();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                showFormLogin();
            }catch(Exception ex)
            {
                Logger.LogThisLine("pictureBox1_Click: " + ex.ToString());
            }
        }

        public void startServer()
        {
            try
            {
                var OPT = new Quobject.SocketIoClientDotNet.Client.IO.Options();
                OPT.ForceNew = true;
                OPT.Timeout = 3000;
                socket = IO.Socket(Constant.serverSoket, OPT);
                //socket.On(Socket.EVENT_CONNECT, () =>
                //{
                //});           
                socket.On("login helper", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    Logger.LogDebugFile("Debug: "+ip +"---------------"+ GlobalSystem.ipv4);
                    
                    if (ip == GlobalSystem.ipv4)
                    {
                        countShowformHome++;
                        Logger.LogDebugFile("-------------------login helper-----------------");
                        Logger.LogDebugFile("chuỗi json trả về: " + json);
                        bool checklogin =  processLoginHelper(json);
                        Logger.LogDebugFile("check login soket: " + checklogin);                        
                        if (checklogin)
                        {
                           
                            if (InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate {
                                    
                                    if(countShowformHome == 1)
                                    {
                                        Logger.LogDebugFile("Đếm số lần soket bán lên: " + countShowformHome);
                                        JObject jout = JObject.FromObject(new { status = 1 });
                                        socket.Emit("refresh view", jout);
                                        goToHome(json);
                                    }
                                    
                                }));
                                return;
                            }
                        }
                        else
                        {
                            JObject jout = JObject.FromObject(new { status = 0, });
                            socket.Emit("refresh view", jout);
                        }
                        Logger.LogDebugFile("-------------------End login helper-----------------");
                    }
                });
                //socket online
                var socketOnline = IO.Socket(Constant.serverSoketOnline);
                socketOnline.On(Socket.EVENT_CONNECT, () =>
                {
                });
                socketOnline.On("request app remote login", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    string partner_id = result.partner_id;
                    string user_name = result.username;
                    string token =  result.token;
                    Logger.LogDebugFile("request app remote login Debug: " + ip + "---------------" + GlobalSystem.ipv4);
                    if (ip == GlobalSystem.ipv4 && partner_id == ClientPartner.partner_id)
                    {

                    }
                });
            }
            catch (Exception e)
            {
                Logger.LogThisLine("startServer: "+ e.Message);
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
                    GlobalSystem.isLogout = 0;
                    GlobalSystem.islogin = 1;
                    User objUser = new User();
                    objUser.id = data.data.id;
                    objUser.username = data.data.username;
                    objUser.password = data.password;
                    objUser.name = data.data.name;
                    objUser.type = data.data.type;
                    objUser.total_monney = data.data.total_monney;
                    objUser.total_discount = data.data.total_discount;
                    objUser.token = data.data.token;
                    Logger.LogDebugFile("Login last_time_request: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " |Tong tien" + data.data.total_monney);
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
                    Logger.LogDebugFile("Check totalMonney: "+ totalMonney);
                    objUser.total_money_login_session = totalMonney;
                    
                    GlobalSystem.user = objUser;
                    Helper.updateGlobalTimeGame(json);
                    return true;                                      

                }
                else
                {
                    String message = data.message;
                    Logger.LogDebugFile(message);
                    Helper.showMessageError(message);
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
        #region su kien click phim bat ki
        private void Slide2_KeyDown(object sender, KeyEventArgs e)
        {
            showFormLogin();
        }
        #endregion

        #region Hiện thị form login
        private void showFormLogin()
        {
            if (GlobalSystem.islogin == 0)
            {
                Login frmlogin = new Login();
                frmlogin.ShowDialog(this);
            }
            else
            {
                UnlockComputer frmUnlockLock = new UnlockComputer(this.t);
                frmUnlockLock.ShowDialog(this);
            }
        }
        #endregion

        #region xử lý close form
        private void Slide2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }
        #endregion

        public void goToHome(String json)
        {
            this.socket.Disconnect();
            socket.Close();
            Home frmHome = new Home(json);
            this.Hide();
            frmHome.ShowDialog(this);
            this.Close();
        }

    }
}
