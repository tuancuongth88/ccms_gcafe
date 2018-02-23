using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using CliendGcafe.Config;
using Newtonsoft.Json.Linq;
using CliendGcafe.model;
using System.Media;
using CliendGcafe.Properties;
using System.Drawing;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;
using CliendGcafe.view;
using System.Runtime.InteropServices;
using System.Threading;

namespace CliendGcafe.lib
{
    class Helper
    {
        /*code needed to disable start menu*/
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);
        public Helper()
        {

        }

        static ASCIIEncoding encoding = new ASCIIEncoding();
        public static void callAPI()
        {
            
        }

        public static void showMessageError(string message)
        {
            MessageBox.Show(message);
        }

        public static void killProcess()
        {
            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    var proce = process;
                    process.Kill();
                }
            }catch(Exception ex)
            {
                showMessageError(ex.Message);
            }
        }
        public static void SetTaskManager(bool enable)
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                objRegistryKey.DeleteValue("DisableTaskMgr");
            else
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            objRegistryKey.Close();
        }

       

        public static void refreshMoney()
        {
            try
            {
                if(GlobalSystem.user != null)
                {
                    string myParameters = "token=" + GlobalSystem.user.token;

                    string URI = Constant.serverHost + Constant.methodRefresh;
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        wc.Headers.Add("token", GlobalSystem.user.token);
                        string HtmlResult = wc.UploadString(URI, myParameters);

                        dynamic data = JObject.Parse(HtmlResult);
                        if (data.status == 200)
                        {
                            User objUser = new User();
                            objUser.id = data.data.id;
                            objUser.username = data.data.username;
                            objUser.name = data.data.name;
                            objUser.email = data.data.email;
                            objUser.type = data.data.type;
                            objUser.total_monney = data.data.total_monney;
                            objUser.total_discount = data.data.total_discount;
                            objUser.token = data.data.token;
                            GlobalSystem.userUpdate = objUser;
                        }
                    }
                }
            }catch(Exception e)
            {
                MessageBox.Show("Tạm thời mất kết nối đến Server, nhấn Ok để tiếp tục!", "Mất kết nối server", MessageBoxButtons.OK);
            }
        }
        // lay thong tin profile
        public static void getProfile()
        {
            if (GlobalSystem.user != null)
            {
                //string myParameters = "token=" + GlobalSystem.user.token;

                string URI = Constant.serverHost + Constant.methodProfile;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers.Add("token", GlobalSystem.user.token);
                    string HtmlResult = wc.DownloadString(URI);

                    dynamic data = JObject.Parse(HtmlResult);
                    if (data.status == 200)
                    {
                        User objUser = new User();
                        objUser.total_monney = data.data.total_monney;
                        objUser.total_discount = data.data.total_discount;
                        objUser.token = GlobalSystem.user.token;
                        var totalMonney = Int32.Parse(objUser.total_monney.ToString()) + Int32.Parse(objUser.total_discount.ToString());
                        objUser.total_money_login_session = totalMonney;
                        GlobalSystem.user.total_monney = data.data.total_monney;
                        GlobalSystem.user.total_discount = data.data.total_discount;
                        GlobalSystem.user.total_money_login_session = totalMonney;
                        GlobalSystem.userUpdate.total_monney = data.data.total_monney;
                        GlobalSystem.userUpdate.total_discount = data.data.total_discount;
                        GlobalSystem.userUpdate.total_money_login_session = totalMonney;
                    }
                }
            }
        }

        public void testStartMp3()
        {
            SoundPlayer player = new SoundPlayer(Resources._1);
            player.Play();
        }  

        public static void processLogin(string json)
        {

        }

        public static void notification(string title, String content)
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = content;

            notifyIcon.ShowBalloonTip(30000);

        }
        public static void roleWindown(bool is_admin)
        {
            if (!is_admin)
            {
                SetTaskManager(false);
                ShowStartMenu(1);
            }
        }
        // enable key
        /// <summary>
        /// 
        /// </summary>
        /// <param name="show">0: an thank task menu, 1: hien thi</param>
        public static void ShowStartMenu(int show)
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, show);
        }

    }
}
