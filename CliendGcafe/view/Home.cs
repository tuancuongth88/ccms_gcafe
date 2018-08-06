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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class Home : Form
    {

        private Socket socket;
        Thread t = null;
        public String jsonLogin;
        List<Process> lstprocess;
        private int countLogout;
        public Home(string json)
        {
            this.jsonLogin = json;
            this.countLogout = 0;
            InitializeComponent();
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
            load();
            Helper.blockWebsite();
        }

        #region Hàm load form khi mở lên lần đầu
        public void load()
        {
            try
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

                    if (GlobalSystem.userUpdate.in_competitive == true)
                    {
                        //set đếm ngược dung trong trường hợp máy thi đấu
                        waitLockOutComboGroup(GlobalSystem.timeGameUser.time_expired_millisecond);
                    }

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
            catch(Exception ex)
            {
                Logger.LogThisLine("Load: "+ ex.ToString());
            }            
        }
        #endregion
        // private async void waitLockOutComboGroup(int millisecond)
        private async void waitLockOutComboGroup(String millisecond)
        {
            try
            {
                Logger.LogDebugFile("waitLockOutComboGroup: " + millisecond);
                await Task.Delay(Int32.Parse(millisecond));
                Logger.LogDebugFile("-----------Start Get Profile --------------------");
                Helper.getProfile();
                Logger.LogDebugFile("-----------END Get Profile --------------------");
                if (GlobalSystem.timeGameUser.time_expired_millisecond == "0")
                {
                    Logger.LogDebugFile("waitLockOutComboGroup: Logout");
                    logout();
                }
                else
                {
                    Logger.LogDebugFile("waitLockOutComboGroup: Call Lại");
                    waitLockOutComboGroup(GlobalSystem.timeGameUser.time_expired_millisecond);
                }
            }
            catch(Exception ex)
            {
                Logger.LogThisLine("waitLockOutComboGroup: " + ex.ToString());
                logout();
            }
            
            
        }

        #region cập nhật thời gian chơi game
        public void calGameHouse()
        {
            Logger.LogDebugFile("----------------------Start calGameHouse------------------------");
            // tổng tiền còn lại
            float totalMoney = GlobalSystem.userUpdate.total_monney + GlobalSystem.userUpdate.total_discount;
            Logger.LogDebugFile("totalMoney : " + totalMoney);
            TimeGameUser objTimeGame = new TimeGameUser();
            objTimeGame = GlobalSystem.timeGameUser;
            try
            {

                // tong tien con lai
                txtTotalPrice.Invoke(new Action(() =>
                {
                    this.txtTotalPrice.Text = objTimeGame.total_monney;
                }));

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
                    // neu khong phai may thi dau thi day ra khi het tien
                    if(GlobalSystem.userUpdate.in_competitive == false)
                    {
                        if (house == 0 && minutes == 0)
                        {
                            logout(1);
                        }                      
                        return;
                    }
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

                Logger.LogDebugFile("----------------------END calGameHouse------------------------");
            }
            catch (Exception ex)
            {
                Logger.LogThisLine("calGameHouse: " + objTimeGame.TimeRemaining);
                Logger.LogThisLine(ex.Message);
            }

        }
        #endregion

        #region sự kiện click vào nút close form
        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region xử lý set form home trên góc màn hình
        private void Home_Move(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.FromPoint(this.Location).WorkingArea.Right - this.Width, 0);
        }
        #endregion

        #region sự kiên click vào nút service
        private void pictureBox2_Click(object sender, EventArgs e)
        {            
            System.Diagnostics.Process.Start(Constant.serverHost+ Constant.methodService);

        }
        #endregion

        #region sự kiện click vào logout
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
                    GlobalSystem.time_shutdown = 0;
                    GlobalSystem.is_admin = 0;
                    User.updateStatusLoginAdmin(0);
                    if (this.socket != null) {
                        this.socket.Disconnect();
                        this.socket.Close();
                    }
                       
                    if (this.t.IsAlive == true)
                        t.Abort();                  
                    closingFormHome();
                }
                catch (Exception ex)
                {
                    Logger.LogThisLine("pictureBox3_Click: "+ ex.Message);
                }
                finally
                {
                    closingFormHome();
                }
            }
        }
        #endregion

        #region sự kiên click vào khóa form
        private void pictureBox5_Click(object sender, EventArgs e)
        {            
            LockComputer frmlock = new LockComputer();
            frmlock.ShowDialog(this);
        }
        #endregion

        #region hàm cập nhật money sau vài phút 1 lần
        private async void processTimer()
        {
            try
            {
                while (true)
                {
                    await Task.Delay(3 * 60000);
                    if (GlobalSystem.isLogout == 1)
                    {
                        if (this.t.IsAlive == true)
                        {
                            this.t.Abort();
                        }
                        return;
                    }
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        bool checkRefresh = Helper.refreshMoney();
                        if (checkRefresh == true)
                        {
                            calGameHouse();
                        }
                        else
                        {
                            logout();
                        }
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            alert.Show(Helper.StripTagsRegex("Cảnh báo. Kiểm tra lại kết nối mạng"), alert.AlertType.error);
                        }));
                        await Task.Delay(3000);
                        logout();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogThisLine("processTimer: " + ex.ToString());
            }
        }
        #endregion

        #region Hàm xử lý logout
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
                if(status == 1)
                {
                    myParameters += "&out_of_money=true";
                }
                else
                {
                    myParameters += "&out_of_money=false";
                }
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
                        this.socket.Disconnect();
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
                Logger.LogThisLine("logout "+ex.Message);
            }
            finally
            {
                closingFormHome();
            }
        }
        #endregion

        #region xử lý close toàn bộ form home và trở về form slide
        public void closingFormHome()
        {

            this.Invoke(new MethodInvoker(delegate
            {
                Logger.LogDebugFile("Set null and logout delegate");
                GlobalSystem.isLogout = 1;
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
        #endregion

        #region sự kiện click vào thay đổi password
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ChangePassword frm = new ChangePassword();
            frm.ShowDialog();

        }
        #endregion

        #region  khởi động socket lắng nghe các sự kiên trên server trả về
        public void startServer()
        {
            try
            {
                var OPT = new Quobject.SocketIoClientDotNet.Client.IO.Options();
                OPT.ForceNew = true;
                OPT.Timeout = 3000;
                socket = IO.Socket(Constant.serverSoket, OPT);
                socket.On(Socket.EVENT_CONNECT, ()      =>
                {
                    Logger.LogDebugFile("-------------EVEN SOKET EMIT ADD USER---------------");
                    Logger.LogDebugFile("add user rooms = clients username =" + GlobalSystem.user.username);
                    JObject jout = JObject.FromObject(new { rooms = "clients", username = GlobalSystem.user.username });
                    socket.Emit("add user", jout);
                    Logger.LogDebugFile("-------------END SOKET EMIT ADD USER---------------");
                    if (!string.IsNullOrEmpty(jsonLogin))
                    {
                        dynamic data = JObject.Parse(jsonLogin);
                        if (data.data["listClientInLastLogin"] != null)
                        {
                            String ip = data.data.listClientInLastLogin[0].ip.ToString();
                            String username = data.data.listClientInLastLogin[0].username.ToString();
                            JObject jout1 = JObject.FromObject(new { ip = ip, type = 1, username = username, message = "Tài khoản này đăng nhập trên máy khác. Máy bạn sẽ bị thoát ra trong vài giây" });
                            socket.Emit("other helper", jout1);
                        }
                    }
                });

                // lăng nghe thông báo từ server trả về
                socket.On("new message", (data)         =>
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
                                        alert.Show(Helper.StripTagsRegex(message), alert.AlertType.success, false);
                                    }));
                                    return;
                                }
                            }
                            //order service => huy don hang
                            if (taget == "7")
                            {
                                Helper.getProfile();
                                calGameHouse();
                                string message = result.message.content;
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate
                                    {
                                        alert.Show(Helper.StripTagsRegex(message), alert.AlertType.error);
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
                            //order card success
                            if (taget == "8")
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
                socket.On("order card success", (data)  =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });

                socket.On("order success", (data)       =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });
                socket.On("other helper", (data)        =>
                {
                    Logger.LogDebugFile("-------------START ORTHER HELPER---------------");
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    int type = result.type;
                    String username_log = result.username.ToString();
                    Logger.LogDebugFile("Debug log off: " + ip + "|---------|" + GlobalSystem.ipv4 + "|||  -  ------ username: " + username_log);
                    Logger.LogDebugFile("Debug log off json: " + json);
                    if (ip == GlobalSystem.ipv4 && username_log == GlobalSystem.user.username)                    
                    {
                        countLogout++;
                        if(countLogout == 1)
                        {
                            Logger.LogDebugFile("Số làn xử lý other helper: " + countLogout);
                            // logout 
                            Logger.LogDebugFile("Debug log off Type: " + type);
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
                                this.socket.Disconnect();
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
                                        Process.Start("shutdown", "/r /t 10");
                                        Logger.LogThisLine("Tat may tinh trong truong hop restart");
                                        logout();
                                        Environment.Exit(0);
                                    }));
                                    // restart lại máy tính
                                    return;
                                }
                            }
                        }
                        
                    }
                    Logger.LogDebugFile("-------------END ORTHER HELPER---------------");
                });
                socket.On("minus money", (data)         =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                });
                socket.On("end task", (data)            =>
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
                socket.On("load client task", (data)    =>
                {
                    string ip = data.ToString();

                    if (GlobalSystem.ipv4.Equals(ip))
                    {
                        getProcess();
                    }
                });

                socket.On("client performance", data =>
                {
                    string ip = data.ToString();

                    if (GlobalSystem.ipv4.Equals(ip))
                    {
                        JObject jout = JObject.FromObject(new { cpu = "18%", ram = "25%" });
                        socket.Emit("client performance", jout);
                    }
                });

                socket.On("load progress task admin", data =>
                {
                    string ip = data.ToString();

                    if (GlobalSystem.ipv4.Equals(ip))
                    {
                        JObject jout = JObject.FromObject(new { cpu = "10%", ram = "25%" });
                        socket.Emit("client performance", jout);
                    }
                });

            }
            catch (Exception e)
            {
                Logger.LogThisLine("startServer" + e.Message);
                Task.Delay(5000);
                startServer();
            }
        }
        # endregion
             

        #region show các ứng dụng đang chạy trên máy tính trả về cho socket
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
        #endregion
     
         
        private void lblclose_MouseHover(object sender, EventArgs e)
        {
            lblclose.BackColor = Color.FromArgb(255, 232, 232);
        }

        private void lblclose_MouseLeave(object sender, EventArgs e)
        {
            lblclose.BackColor = Color.Gray;
        }

        private void lblclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    class ListProcess
    {
        public int id { get; set; }
        public String name{ get; set; }
    }
}
