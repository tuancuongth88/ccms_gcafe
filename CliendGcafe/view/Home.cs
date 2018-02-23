using CliendGcafe.Config;
using CliendGcafe.lib;
using CliendGcafe.model;
using CliendGcafe.Properties;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe.view
{
    public partial class Home : MetroFramework.Forms.MetroForm
    {
        TimeGameUser objTimeGame = new TimeGameUser();
        double minute = 60;
        private Socket socket;
        Thread t = null;
        public Home()
        {
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
            //end an cac form    
            GlobalSystem.userUpdate = GlobalSystem.user;            
            calGameHouse();

            //start soket lang nghe cac su kien tra ve
            startServer(); 

            t = new Thread(processTimer);
            t.Start();
        }
        public void calGameHouse()
        {
            try
            {
                // tổng tiền còn lại
                float totalMoney = GlobalSystem.userUpdate.total_monney + GlobalSystem.userUpdate.total_discount;
                // check co giam gia hay khong
                float gameOnHouse = GlobalSystem.user.price;
                if (GlobalSystem.user.promotion_percent > 0)
                {
                    gameOnHouse = gameOnHouse - ((GlobalSystem.user.price * GlobalSystem.user.promotion_percent) / 100);
                }

                // tong thoi gian
                float seconds = ((GlobalSystem.user.total_money_login_session / gameOnHouse) * 60) * 60;
                TimeSpan totalTime = TimeSpan.FromSeconds(seconds);
                objTimeGame.TotalTime = totalTime.ToString(@"hh\:mm\:ss");
                
                txtTotalTime.Invoke(new Action(() =>
                {
                    this.txtTotalTime.Text = objTimeGame.TotalTime.ToString();
                }));
                // tong thoi gian su dung
                TimeSpan timeUser = DateTime.Now - GlobalSystem.timeStart;
                txtTotaTimelUse.Invoke(new Action(() =>
                {
                    txtTotaTimelUse.Text = timeUser.ToString(@"hh\:mm\:ss");
                }));
                // thoi gian con lai
                var TimeRemaining = seconds - timeUser.TotalSeconds;
                TimeSpan timeRemaining = TimeSpan.FromSeconds(TimeRemaining);
                objTimeGame.TimeRemaining = timeRemaining.ToString(@"hh\:mm\:ss");
                txtTimeUser.Invoke(new Action(() =>
                {
                    txtTimeUser.Text = objTimeGame.TimeRemaining;
                }));
                // chi phi gio choi
                var tong_tien_game = timeUser.TotalHours * gameOnHouse;
                
                txtPrice.Invoke(new Action(() =>
                {
                    txtPrice.Text = tong_tien_game.ToString("C");
                }));
                txtTotalPrice.Invoke(new Action(() =>
                {
                    txtTotalPrice.Text = totalMoney.ToString("C");
                }));
                if (TimeRemaining < (minute * 10))
                {
                    //Helper.notification("Sắp hết thời gian", "Tài khoản của bạn sắp hết vui lòng nạp thêm tiền để sử dụng");
                    if (InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate {
                            alert.Show("Tài khoản của bạn sắp hết vui lòng nạp thêm tiền để sử dụng", alert.AlertType.warnig);
                        }));
                        return;
                    }
                }
                if (totalMoney < 1)
                {
                    logout();
                }
            }catch(Exception ex)
            {
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
            OrderService frmSr = new OrderService();
            frmSr.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.socket.Close();
            logout();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //LockComputer frmlock = new LockComputer();
            //frmlock.ShowDialog();

            GlobalSystem.islogin = 1;
            Slide2 frm = new Slide2();
            this.Hide();
            frm.ShowDialog();
            this.Close();
        }

        private void processTimer()
        {

            while (true)
            {                
                Thread.Sleep((5 * 60000));
                if (GlobalSystem.isLogout == 1)
                {
                    this.t.Abort();
                    return;
                }
                Helper.refreshMoney();
                calGameHouse();
            }
        }       

        public void logout()
        {
           
            string myParameters = "token=" + GlobalSystem.user.token;

            string URI = Constant.serverHost + Constant.methodLogout;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("token", GlobalSystem.user.token);
                string HtmlResult = wc.UploadString(URI, myParameters);
                dynamic data = JObject.Parse(HtmlResult);
                if (data.status == 200)
                {
                    GlobalSystem.isLogout = 1;
                    GlobalSystem.islogin = 0;
                    GlobalSystem.user = null;
                    Slide2 frm = new Slide2();
                    this.Hide();
                    frm.ShowDialog();
                    this.Close();
                }
                {
                    alert.Show("Lỗi gọi server", alert.AlertType.error);
                }
            }
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
                    JObject jout = JObject.FromObject(new { rooms = "clients", userid = GlobalSystem.user.id });
                    socket.Emit("add user", jout);
                });

                // lăng nghe thông báo từ server trả về
                socket.On("new message", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    if(result.userid != null)
                    {
                        int userid = result.userid;
                        if (GlobalSystem.user.id == userid)
                        {
                            string taget = result.target;
                            if (taget == "4")
                            {
                                string message = result.message.content;
                                //Helper.notification("Thông báo", message);
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate {
                                        alert.Show(message, alert.AlertType.info);
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
                                //Helper.notification("Thông báo", message);
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate {
                                        alert.Show(message, alert.AlertType.success);
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
                                //Helper.notification("Thông báo", message);
                                if (InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate {
                                        alert.Show(message, alert.AlertType.success);
                                    }));
                                    return;
                                }
                            }
                        }
                    }
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
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
                    string ip = result.ip;
                    int type = result.type;
                    if (ip == GlobalSystem.ipv4 || ip =="127.0.0.1")
                    {
                        if(type == 1)
                        {
                            
                            if (InvokeRequired)
                            {
                                // after we've done all the processing, 
                                this.Invoke(new MethodInvoker(delegate {
                                    // send lại server thong tin logout thanh cong
                                    JObject jout = JObject.FromObject(new { ip = ip, userid = GlobalSystem.user.id, message = "Success" });
                                    socket.Emit("other helper", jout);
                                    this.socket.Close();
                                    logout();
                                }));
                                return;
                            }
                        }
                        if(type == 2)
                        {
                            if (InvokeRequired)
                            {
                                // after we've done all the processing, 
                                this.Invoke(new MethodInvoker(delegate {
                                    this.socket.Close();
                                    logout();
                                }));
                                // restart lại máy tính
                                Process.Start("shutdown","/r /t 0");
                                return;
                            }
                        }
                        
                    }
                });
                socket.On("minus money", (data) =>
                {
                    string json = data.ToString();
                    dynamic result = JObject.Parse(json);
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
    }
}
