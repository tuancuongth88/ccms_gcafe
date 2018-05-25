using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Forms;

namespace CCMS.lib
{
  
    class DisableKey
        {
            [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);
            [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int UnhookWindowsHookEx(int hHook);
            public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
            [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
            public const int WH_KEYBOARD_LL = 13;

            /*code needed to disable start menu*/
            [DllImport("user32.dll")]
            private static extern int FindWindow(string className, string windowText);
            [DllImport("user32.dll")]
            private static extern int ShowWindow(int hwnd, int command);

            private const int SW_HIDE = 0;
            private const int SW_SHOW = 1;

            public static int intLLKey;

            public void KillCtrlAltDelete()
            {
                RegistryKey regkey;
                string keyValueInt = "1";
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

                try
                {
                    regkey = Registry.CurrentUser.CreateSubKey(subKey);
                    regkey.SetValue("DisableTaskMgr", keyValueInt);
                    regkey.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            public struct KBDLLHOOKSTRUCT
            {
                public int vkCode;
                public int scanCode;
                public int flags;
                public int time;
                public int dwExtraInfo;
            }

            public int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
            {
                bool blnEat = false;

                switch (wParam)
                {
                    case 256:
                    case 257:
                    case 260:
                    case 261:
                        //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key,
                        blnEat = ((lParam.vkCode == 9) && (lParam.flags == 32)) | ((lParam.vkCode == 27) && (lParam.flags == 32)) | ((lParam.vkCode == 27) && (lParam.flags == 0)) | ((lParam.vkCode == 91) && (lParam.flags == 1)) | ((lParam.vkCode == 92) && (lParam.flags == 1)) | ((lParam.vkCode == 73) && (lParam.flags == 0));
                        break;
                }

                if (blnEat == true)
                {
                    return 1;
                }
                else
                {
                    return CallNextHookEx(0, nCode, wParam, ref lParam);
                }
            }

            public void KillStartMenu()
            {
                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_HIDE);
            }

            public void setDisable()
            {
            try
            {
                intLLKey = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardProc, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
            }
            catch (Exception ex)
            {
                Logger.LogThisLine(ex.ToString());
            }
            }

            public void enable()
            {
                UnhookWindowsHookEx(intLLKey);
                ShowStartMenu();
                EnableCTRLALTDEL();
            }

            public static void ShowStartMenu()
            {
                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_SHOW);
            }

            public static void EnableCTRLALTDEL()
            {
                try
                {
                    string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                    RegistryKey rk = Registry.CurrentUser;
                    RegistryKey sk1 = rk.OpenSubKey(subKey);
                    if (sk1 != null)
                        rk.DeleteSubKeyTree(subKey);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }    
}
