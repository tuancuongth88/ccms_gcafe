using CCMS.Config;
using Newtonsoft.Json.Linq;
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

        private static void writeLogServer(string type, String log)
        {
            try
            {
                if (GlobalSystem.islogin == 0)
                {
                    GlobalSystem.timeStart = DateTime.Now;
                }

                string myParameters = "type_log=" + type + "&log_content=" + log;

                string URI = Constant.serverHost + Constant.methodLogin;
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, myParameters);
                    Logger.LogDebugFile("Login json tra ve: " + HtmlResult);
                    dynamic data = JObject.Parse(HtmlResult);
                }
                    
            }catch (Exception ex){
                Logger.LogThisLine("writeLogServer: " + ex.Message);
            }
        }
    }
}
