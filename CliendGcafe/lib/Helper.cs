﻿using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;
using CCMS.Config;
using Newtonsoft.Json.Linq;
using CCMS.model;
using System.Media;
//using CliendGcafe.Properties;
using System.Drawing;
using System.Runtime.InteropServices;
using CCMS.Properties;
using System.Configuration;
using System.IO;
using Quobject.SocketIoClientDotNet.Client;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CCMS.lib
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

        public static void killProcess(int status = 0)
        {
            try
            {
                Process me = Process.GetCurrentProcess();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.Id != me.Id
                    && !p.ProcessName.ToLower().StartsWith("winlogon")
                    && !p.ProcessName.ToLower().StartsWith("system idle process")
                    && !p.ProcessName.ToLower().StartsWith("taskmgr") && !p.ProcessName.ToLower().StartsWith("spoolsv") && !p.ProcessName.ToLower().StartsWith("csrss") && !p.ProcessName.ToLower().StartsWith("smss") && !p.ProcessName.ToLower().StartsWith("svchost") && !p.ProcessName.ToLower().StartsWith("services") && !p.ProcessName.ToLower().StartsWith("lsass")
                    )
                    {
                        if (p.MainWindowHandle != IntPtr.Zero)
                        {
                            p.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showMessageError(ex.Message);
            }
        }

        public static void refreshMoney()
        {
            String HtmlResult = "";
            try
            {
                if (GlobalSystem.user != null)
                {
                    string myParameters = "last_time_request=" + GlobalSystem.user.last_time_request;
                    string URI = Constant.serverHost + Constant.methodRefresh;
                    Logger.LogDebugFile("------------- call refreshMoney------------------");
                    Logger.LogDebugFile("URL: "+ URI);
                    Logger.LogDebugFile("PARAM: " + myParameters);
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        wc.Headers.Add("token", GlobalSystem.user.token);
                        HtmlResult = wc.UploadString(URI, myParameters);

                        Logger.LogDebugFile("refreshMoney "+ HtmlResult);
                        dynamic data = JObject.Parse(HtmlResult);
                        if (data.status == 200)
                        {
                            GlobalSystem.user.last_time_request = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

                            //update glocal time game
                            updateGlobalTimeGame(HtmlResult);
                            Logger.LogDebugFile("last_time_request: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | Tong tien: " + GlobalSystem.timeGameUser.total_monney);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogThisLine("refreshMoney: " + GlobalSystem.user.token);
                Logger.LogThisLine("Token: " + GlobalSystem.user.token);
                Logger.LogThisLine("Thoi gian gui len server: " + GlobalSystem.user.last_time_request);
                Logger.LogThisLine("Lỗi Json: "+ HtmlResult);
                Logger.LogThisLine("refreshMoney "+ e.Message);
                MessageBox.Show("Tạm thời mất kết nối đến Server, nhấn Ok để tiếp tục!", "Mất kết nối server", MessageBoxButtons.OK);

            }
        }

        public static void updateGlobalTimeGame(string json)
        {
            try
            {
                dynamic data = JObject.Parse(json);
                TimeGameUser objTimeGame = new TimeGameUser();
                String night_combo = data.data.night_combo_id;
                int id_night_combo = 0;
                if (night_combo != null)
                {
                    id_night_combo = data.data.night_combo_id;
                }
                if (id_night_combo > 0)
                {
                    String str_end = data.data.night_combo_end_at;
                    DateTime end_combo = DateTime.Parse(str_end);
                    TimeSpan timeRemaining = end_combo.Subtract(DateTime.Now);
                    if (timeRemaining.Seconds > 0)
                    {
                        var money = data.data.total_monney + data.data.total_discount;
                        objTimeGame.total_monney = money.ToString("#,##0");
                        objTimeGame.TotalTime = "00:00:00";
                        objTimeGame.TimeUse = "00:00:00";
                        objTimeGame.TimeRemaining = timeRemaining.ToString(@"hh\:mm\:ss");// string.Format("{0}:{1}:{2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
                        objTimeGame.CostOfUse = "0";
                        objTimeGame.iscombo = 1;
                        GlobalSystem.timeGameUser = objTimeGame;
                        return;
                    }
                }
                objTimeGame.total_monney = data.data.show_app_info.summary_money.ToString("#,##0");
                objTimeGame.TotalTime = data.data.show_app_info.hours_game;
                objTimeGame.TimeUse = data.data.show_app_info.time_played;
                objTimeGame.TimeRemaining = data.data.show_app_info.time_expired;
                objTimeGame.CostOfUse = data.data.show_app_info.price_per_hour.ToString("#,##0");
                objTimeGame.iscombo = 0;
                GlobalSystem.timeGameUser = objTimeGame;
                return;
            }
            catch (Exception ex)
            {
                Logger.LogThisLine("updateGlobalTimeGame: " + ex.Message);
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
                        //update glocal time game
                        updateGlobalTimeGame(HtmlResult);
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
            disableRoleWindown(is_admin);
            restart_explorer();
        }

        public static void disableRoleWindown(bool is_admin)
        {
            //Software\Microsoft\Windows\CurrentVersion\Policies\Explorer
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
              @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
            //HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System
            RegistryKey objCurrentUserSystem = Registry.CurrentUser.CreateSubKey(
              @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer
            RegistryKey objRegistryKeyMachineExplorer = Registry.LocalMachine.CreateSubKey(
             @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");

            //HKEY_LOCAL_MACHINE\SOFTWARE \Microsoft\Windows\CurrentVersion\Policies\System
            RegistryKey objRegistryKeySystem = Registry.LocalMachine.CreateSubKey(
              @"Software\Microsoft\Windows\CurrentVersion\Policies\System");

            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
            RegistryKey objRegistryKeyMachineWinlogon = Registry.LocalMachine.CreateSubKey(
              @"Software\Microsoft\Windows NT\CurrentVersion\Winlogon");

            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System
            RegistryKey objRegistryKeyMachineSystem = Registry.LocalMachine.CreateSubKey(
              @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            try
            {
                if (is_admin == false)
                {
                    //Add_Shut-Down_Restart_Sleep_Hibernation 
                    objRegistryKey.SetValue("NoClose", "1", RegistryValueKind.DWord);
                    objRegistryKeyMachineExplorer.SetValue("NoClose", "1", RegistryValueKind.DWord);
                    //to disable log off
                    objRegistryKey.SetValue("NoLogoff", "1", RegistryValueKind.DWord);
                    objRegistryKey.SetValue("StartMenuLogOff", "1", RegistryValueKind.DWord);
                    //to disable switch user 
                    objCurrentUserSystem.SetValue("HideFastUserSwitching", "1", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("HideFastUserSwitching", "1", RegistryValueKind.DWord);
                    //to disable lock
                    objRegistryKeySystem.SetValue("DisableLockWorkstation", "1", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("ShutdownWithoutLogon", "1", RegistryValueKind.DWord);
                    objRegistryKeyMachineWinlogon.SetValue("ShutdownWithoutLogon", "0", RegistryValueKind.DWord);
                    //DisableTaskMgr 
                    objCurrentUserSystem.SetValue("DisableTaskMgr", "1", RegistryValueKind.DWord);

                    objRegistryKeyMachineWinlogon.SetValue("DisableCAD ", "1", RegistryValueKind.DWord);
                    objRegistryKeyMachineWinlogon.SetValue("DisableCad ", "1", RegistryValueKind.DWord);
                    objRegistryKeyMachineSystem.SetValue("DisableCAD ", "1", RegistryValueKind.DWord);

                    //DisableRegistryTools 
                    objCurrentUserSystem.SetValue("DisableRegistryTools", "1", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("DisableRegistryTools", "1", RegistryValueKind.DWord);
                }
                else
                {
                    //Remove_Shut - Down_Restart_Sleep_Hibernation
                    objCurrentUserSystem.SetValue("DisableRegistryTools", "0", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("DisableRegistryTools", "0", RegistryValueKind.DWord);
                    objRegistryKey.SetValue("NoClose", "0", RegistryValueKind.DWord);
                    objRegistryKey.SetValue("NoLogoff", "0", RegistryValueKind.DWord);
                    objRegistryKeyMachineExplorer.SetValue("NoClose", "0", RegistryValueKind.DWord);
                    objRegistryKey.SetValue("StartMenuLogOff", "0", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("HideFastUserSwitching", "0", RegistryValueKind.DWord);
                    objCurrentUserSystem.SetValue("HideFastUserSwitching", "0", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("DisableLockWorkstation", "0", RegistryValueKind.DWord);
                    objRegistryKeySystem.SetValue("ShutdownWithoutLogon", "0", RegistryValueKind.DWord);
                    objRegistryKeyMachineWinlogon.SetValue("ShutdownWithoutLogon", "1", RegistryValueKind.DWord);
                    objCurrentUserSystem.SetValue("DisableTaskMgr", "0", RegistryValueKind.DWord);
                    objRegistryKeyMachineWinlogon.SetValue("DisableCAD ", "0", RegistryValueKind.DWord);
                    objRegistryKeyMachineWinlogon.SetValue("DisableCad ", "0", RegistryValueKind.DWord);
                    objRegistryKeyMachineSystem.SetValue("DisableCAD ", "0", RegistryValueKind.DWord);
                }

                // update webBrowser use IE 11
                var pricipal = new System.Security.Principal.WindowsPrincipal(
                System.Security.Principal.WindowsIdentity.GetCurrent());
                if (pricipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    RegistryKey registrybrowser = Registry.LocalMachine.OpenSubKey
                    (@"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                    var currentValue = registrybrowser.GetValue("*");
                    if (currentValue == null || (int)currentValue != 0x00002af9)
                        registrybrowser.SetValue("*", 0x00002af9, RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThisLine(ex.Message);
            }
            finally
            {
                objRegistryKey.Close();
                objRegistryKeyMachineExplorer.Close();
                objRegistryKeySystem.Close();
                objRegistryKeyMachineWinlogon.Close();

            }
        }

        public static void restart_explorer()
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    try
                    {
                        Process[] exp = Process.GetProcessesByName("explorer");
                        foreach (Process explorer in exp)
                        {
                            explorer.Kill();
                        }
                    }
                    catch { }
                }

                Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
            }
            catch { }
        }

        public static void logoutCliendWithAccount(string ip, string type, string user_id)
        {
            try
            {
                var socket = IO.Socket(Constant.serverSoket);
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    JObject jout = JObject.FromObject(new { ip = ip, type = type, user_id = user_id, message = "Tài khoản này đăng nhập trên máy khác. Máy bạn sẽ bị thoát ra trong vài giây" });
                    socket.Emit("other helper", jout);

                });
                //socket.Close();

            }
            catch (Exception e)
            {
                Logger.LogThisLine(e.Message);
            }
        }

        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
    }
}