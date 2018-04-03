using CCMS.Config;
using CCMS.lib;
using CCMS.model;
using CCMS.Properties;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class Home : MetroFramework.Forms.MetroForm
    {

        private Socket socket;
        Thread t = null;
        public String jsonLogin;

        List<Process> lstprocess;
        public Home(string json)
        {
            this.jsonLogin = json;
            InitializeComponent();
            load();
        }

        public void load()
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
            // an cac form truoc
            for (int i = Application.OpenForms.Count - 1; i >= 0; i += -1)
            {
                if (!object.ReferenceEquals(Application.OpenForms[i], this))
                {
                    Application.OpenForms[i].Hide();
                }
            }
            this.Hide();

            //check is admin
            if (GlobalSystem.is_admin == 0)
            {
                //end an cac form    
                GlobalSystem.userUpdate = GlobalSystem.user;

                calGameHouse();
                if (GlobalSystem.timeGameUser.iscombo == 1)
                {
                    alert.Show("Bạn đang sử dụng Combo", alert.AlertType.success);
                }
                //start soket lang nghe cac su kien tra ve
                startServer();
                t = new Thread(processTimer);
                t.Start();
            }
            else
            {
                pictureBox2.Image = Resources.service_lock;
                pictureBox5.Image = Resources.log_computer_lock;
                pictureBox4.Image = Resources.change_pas_lock;

                pictureBox2.Enabled = false;
                pictureBox5.Enabled = false;
                pictureBox4.Enabled = false;
            }

        }
        public void calGameHouse()
        {
            // tổng tiền còn lại
            float totalMoney = GlobalSystem.userUpdate.total_monney + GlobalSystem.userUpdate.total_discount;
            TimeGameUser objTimeGame = new TimeGameUser();
            objTimeGame = GlobalSystem.timeGameUser;
            try
            {

                // tong tien con lai
                txtTotalPrice.Invoke(new Action(() =>
                {
                    this.txtTotalPrice.Text = objTimeGame.total_monney;
                }));
                // tong thoi gian
                //txtTotaTimelUse.Invoke(new Action(() =>
                //{
                //this.txtTotalTime.Text = objTimeGame.TotalTime;
                //}));

                // tong thoi gian su dung
                txtTotaTimelUse.Invoke(new Action(() =>
                {
                    this.txtTotaTimelUse.Text = objTimeGame.TimeUse;
                }));

                // thoi gian con lai
                txtTimeUser.Invoke(new Action(() =>
                {
                    txtTimeUser.Text = objTimeGame.TimeRemaining;
                }));

                // chi phi gio choi
                txtPrice.Invoke(new Action(() =>
                {
                    txtPrice.Text = objTimeGame.CostOfUse.ToString();
                }));

                String[] time_exit = objTimeGame.TimeRemaining.Split(':');
                int house = Int32.Parse(time_exit[0]);
                int minutes = Int32.Parse(time_exit[1]);
                if (totalMoney <= 0)
                {
                    logout();
                    return;
                }
                if (house == 0 && minutes <= 10)
                {

                    if (InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            //hết thơi gian su dung của combo hay của tài khoản thường
                            if (objTimeGame.iscombo == 1)
                                alert.Show("Sắp hết thời gian sử dụng Combo!", alert.AlertType.warnig);
                            else
                                alert.Show("Sắp hết thời gian sử dụng. Vui lòng nạp tiền Để tiếp tục!", alert.AlertType.warnig);
                        }));
                        //return;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogThisLine("calGameHouse: " + objTimeGame.TimeRemaining);
                Logger.LogThisLine(ex.Message);
            }

        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }

        private void Home_Move(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //OrderService frmSr = new OrderService();
            //Service frmSr = new Service();
            //frmSr.ShowDialog();
            System.Diagnostics.Process.Start(Constant.serverHost+ Constant.methodService);

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (GlobalSystem.is_admin == 0)
            {
                logout();
            }
            else
            {
                try
                {
                    GlobalSystem.time_shutdown = 30;
                    GlobalSystem.is_admin = 0;
                    if (this.socket != null)
                        this.socket.Close();
                    if (this.t.IsAlive == true)
                        t.Abort();                  
                    closingFormHome();
                }
                catch (Exception ex)
                {
                    Logger.LogThisLine(ex.Message);
                }
                finally
                {
                    closingFormHome();
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có muốn khóa máy",
                                     "Xác nhận",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
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

            //LockComputer frmlock = new LockComputer();
            //frmlock.ShowDialog(this);
        }

        private void processTimer()
        {

            while (true)
            {
                Thread.Sleep((1 * 60000));
                if (GlobalSystem.isLogout == 1)
                {
                    if (this.t != null)
                    {
                        this.t.Abort();
                    }                  
                    return;
                }
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                    Helper.refreshMoney();
                    calGameHouse();
                }
                else
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        alert.Show(Helper.StripTagsRegex("Cảnh báo. Kiểm tra lại kết nối mạng"), alert.AlertType.error);
                    }));
                }

            }
        }

        public void logout(int status = 0)
        {
            try
            {
                Process[] chromeInstances = Process.GetProcessesByName("chrome");

                foreach (Process p in chromeInstances)
                    p.Kill();
                Logger.LogDebugFile("-------------LOGOUT---------------");
                Logger.LogDebugFile("STATUS: " + status);
                string myParameters = "token=" + GlobalSystem.user.token;

                Logger.LogDebugFile("Myparameters: " + myParameters);
                string URI = Constant.serverHost + Constant.methodLogout;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers.Add("token", GlobalSystem.user.token);
                    string HtmlResult = wc.UploadString(URI, myParameters);
                    dynamic data = JObject.Parse(HtmlResult);
                    Logger.LogDebugFile("json status : " + data.status);
                    if (data.status == 200)
                    {
                        this.socket.Close();
                        if (this.t.IsAlive == true)
                            t.Abort();                      
                        GlobalSystem.time_shutdown = data.data.settings.timeLogOut;
                        closingFormHome();
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                Logger.LogDebugFile("Json failt server return: " + HtmlResult);
                                alert.Show("Lỗi gọi server", alert.AlertType.error);
                            }));
                            return;
                        }
                    }
                }
                Logger.LogDebugFile("-------------END LOGOUT---------------");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogThisLine(ex.Message);
            }
            finally
            {
                closingFormHome();
            }
        }

        public void closingFormHome()
        {

            this.Invoke(new MethodInvoker(delegate
            {
                Logger.LogDebugFile("Set null and logout delegate");
                GlobalSystem.islogin = 0;
                GlobalSystem.user = null;
                GlobalSystem.timeGameUser = null;
                Slide2 frm = new Slide2();
                this.Hide();
                frm.ShowDialog(this);
                this.Close();

            }));
            return;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ChangePassword frm = new ChangePassword();
            frm.ShowDialog();

        }

        public void startServer()
        {
            try
            {
                socket = IO.Socket(Constant.serverSoket);
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    Logger.LogDebugFile("-------------EVEN SOKET EMIT ADD USER---------------");
                    Logger.LogDebugFile("add user rooms = clients userid =" + GlobalSystem.user.id);
                    JObject jout = JObject.FromObject(new { rooms = "clients", userid = GlobalSystem.user.id });
                    socket.Emit("add user", jout);
                    Logger.LogDebugFile("-------------END SOKET EMIT ADD USER---------------");
                    if (!string.IsNullOrEmpty(jsonLogin))
                    {
                        dynamic data = JObject.Parse(jsonLogin);
                        if (data.data["listClientInLastLogin"] != null)
                        {
                            String ip = data.data.listClientInLastLogin[0].ip.ToString();
                            String user_id = data.data.listClientInLastLogin[0].user_id.ToString();
                            JObject jout1 = JObject.FromObject(new { ip = ip, type = 1, user_id = user_id, message = "Tài khoản này đăng nhập trên máy khác. Máy bạn sẽ bị thoát ra trong vài giây" });
                            socket.Emit("other helper", jout1);
                        }
                    }
                });

                // lăng nghe thông báo từ server trả về
                socket.On("new message", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    Logger.LogDebugFile("-------------EVEN SOKET NEW MESSAGE---------------");
                    Logger.LogDebugFile("conversations server: " + result.message.conversations_id);
                    Logger.LogDebugFile("conversations cliend: " + GlobalSystem.user.conversation_id);
                    if (result.message.conversations_id != null)
                    {

                        int conversation_id = result.message.conversations_id;
                        if (GlobalSystem.user.conversation_id == conversation_id)
                        {
                            string taget = result.target;
                            Logger.LogDebugFile("taget: " + taget);
                            Logger.LogDebugFile("message content: " + result.message.content);
                            if (taget == "4")
                            {
                                string message = result.message.content;
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        alert.Show(Helper.StripTagsRegex(message), alert.AlertType.info);
                                    }));
                                    return;
                                }
                            }
                            //order service
                            if (taget == "3")
                            {
                                Helper.getProfile();
                                calGameHouse();
                                string message = result.message.content;
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        alert.Show(Helper.StripTagsRegex(message), alert.AlertType.success);
                                    }));
                                    return;
                                }
                            }
                            //order card success
                            if (taget == "6")
                            {
                                Helper.getProfile();
                                calGameHouse();
                                string message = result.message.content;
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        alert.Show(Helper.StripTagsRegex(message), alert.AlertType.success);
                                    }));
                                    return;
                                }
                            }
                        }
                    }
                    Logger.LogDebugFile("-------------END NEW MESSAGE---------------");
                });
                // lắng nghe nạp tiền
                socket.On("order card success", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });

                socket.On("order success", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });
                socket.On("other helper", (data) =>
                {
                    Logger.LogDebugFile("-------------START ORTHER HELPER---------------");
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    int type = result.type;
                    int userid_log = result.user_id;
                    Logger.LogThisLine("Debug log off: " + ip + "|---------|" + GlobalSystem.ipv4 + "|||  -  ------ userid: " + userid_log);
                    Logger.LogThisLine("Debug log off json: " + json);
                    if (ip == GlobalSystem.ipv4 && userid_log == GlobalSystem.user.id)
                    //if (userid_log == GlobalSystem.user.id)
                    {
                        // logout 
                        Logger.LogThisLine("Debug log off Type: " + type);
                        if (type == 1)
                        {
                            if (GlobalSystem.user == null)
                                return;
                            // after we've done all the processing, 
                            JObject jout = JObject.FromObject(new { status = 1 });
                            socket.Emit("refresh view", jout);

                            if (InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                if (result["message"] != null)
                                {
                                    String message = result.message;
                                    //MessageBox.Show(message);
                                    AutoClosingMessageBox.Show(message, "Thông báo", GlobalSystem.sleep);
                                    }
                                }));
                            }
                            // send lại server thong tin logout thanh cong
                            this.socket.Close();
                            logout();
                            return;
                        }
                        // restart
                        if (type == 2)
                        {
                            if (InvokeRequired)
                            {
                                // after we've done all the processing, 
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    this.socket.Close();
                                    logout(1);
                                    Process.Start("shutdown", "/r /t 0");
                                    Logger.LogThisLine("Tat may tinh trong truong hop restart");
                                    Environment.Exit(0);
                                }));
                                // restart lại máy tính
                                return;
                            }
                        }
                    }
                    Logger.LogDebugFile("-------------END ORTHER HELPER---------------");
                });
                socket.On("minus money", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });
                socket.On("end task", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    String ip = result.ip;
                    int id_process = result.task_id;
                    if (GlobalSystem.ipv4.Equals(ip))
                    {
                        lstprocess[id_process].Kill();
                    }
                });
                socket.On("load client task", (data) =>
                {
                    string ip = data.ToString();

                    if (GlobalSystem.ipv4.Equals(ip))
                    {
                        getProcess();
                    }
                });
            }
            catch (Exception e)
            {
                Logger.LogThisLine(e.Message);
                Thread.Sleep(5000);
                startServer();
            }
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {

        }

        public void getProcess()
        {
            try
            {
                lstprocess = new List<Process>();
                List<ListProcess> listProcess = new List<ListProcess>();
                int i = 0;
                var curent_process = Process.GetProcesses();
                foreach (Process item in curent_process)
                {
                    if (!String.IsNullOrEmpty(item.MainWindowTitle))
                    {
                        ListProcess objProcess = new ListProcess();
                        objProcess.id = i;
                        objProcess.name = item.ProcessName.ToString();
                        listProcess.Add(objProcess);
                        lstprocess.Add(item);
                        i++;
                    }
                }
                //add json
                JObject jout = JObject.FromObject(new
                {
                    ip = GlobalSystem.ipv4,
                    userid = GlobalSystem.user.id,
                    conversation_id = GlobalSystem.user.conversation_id,
                    data = listProcess
                });
                string jsonStr = jout.ToString();
                socket.Emit("load client task", jsonStr);
            }
            catch(Exception e)
            {
                Logger.LogThisLine(e.Message);
            }
            
        }
    }

    class ListProcess
    {
        public int id { get; set; }
        public String name{ get; set; }
    }
}
