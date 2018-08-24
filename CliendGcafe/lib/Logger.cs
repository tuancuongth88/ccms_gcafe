using CCMS.Config;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.lib
{
    
    public static class Logger
    {
        private static StreamWriter swLog;
        private static StreamWriter swLogDebug;
        private const string sLOG_FILE_PATH = "log.txt";

        private const string sDEBUG_FILE_PATH = "debug_app.txt";

        static Logger()
        {
            Logger.OpenLogger();
        }

        public static void OpenLogger()
        {
            try
            {
                Logger.swLog = new StreamWriter(sLOG_FILE_PATH, false);
                Logger.swLog.AutoFlush = true;
                Logger.swLogDebug = new StreamWriter(sDEBUG_FILE_PATH, false);
                Logger.swLogDebug.AutoFlush = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void LogDebugFile(String log)
        {
            try
            {
                Logger.swLogDebug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + log);
                Logger.swLogDebug.Flush();
                //writeLogServer("info", log);
                writeLogServer(1, log);
            }
            catch(Exception ex)
            {
                CloseLogger();
                return;
            }
            
        }
        public static void LogThisLine(string sLogLine)
        {
            try
            {
                Logger.swLog.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + sLogLine);
                Logger.swLog.Flush();
                //writeLogServer("errors", sLogLine);
                writeLogServer(2, sLogLine);
            }
            catch(Exception ex)
            {
                CloseLogger();
                return;
            }
            
        }

        public static void CloseLogger()
        {
            Logger.swLog.Flush();
            Logger.swLog.Close();

            Logger.swLogDebug.Flush();
            Logger.swLogDebug.Close();
        }

        private static void writeLogServer(int type, String log)
        {
            try
            {
                if (GlobalSystem.socketLog == null)
                {
                    reconnectSoket();
                }
                String fomartLog = "";
                //serverSoketLog
                if (type == 1)
                {
                    if (GlobalSystem.user != null)
                    {
                        fomartLog = "logfile_" + GlobalSystem.user.username + "_" + GlobalSystem.ipv4 + "_" + ClientPartner.partner_id;
                    }
                    else
                    {
                        fomartLog = "logfile_" + GlobalSystem.ipv4;
                    }
                }
                if (type == 2)
                {
                    //excation_user_ip_chinhanh
                    if (GlobalSystem.user != null)
                    {
                        fomartLog = "exception_" + GlobalSystem.user.username + "_" + GlobalSystem.ipv4 + "_" + ClientPartner.partner_id;
                    }
                    else
                    {
                        fomartLog = "exception" + GlobalSystem.ipv4;
                    }
                }
                //logfile_user_ip_chinhanh
                JObject logObject = JObject.FromObject(new { log = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + fomartLog + log });
                GlobalSystem.socketLog.Emit("logs", logObject);
            }
            catch (Exception ex){
                Logger.LogThisLine("writeLogServer: " + ex.Message);
            }
            finally
            {
                
            }
        }

        private static void reconnectSoket()
        {
            //Start Soket Log file
            var OPT = new Quobject.SocketIoClientDotNet.Client.IO.Options();
            OPT.ForceNew = true;
            OPT.Timeout = 3000;
            GlobalSystem.socketLog = IO.Socket(Constant.serverSoketLog, OPT);
            GlobalSystem.socketLog.On(Socket.EVENT_CONNECT, () =>
            {
            });
        }
    }
}
