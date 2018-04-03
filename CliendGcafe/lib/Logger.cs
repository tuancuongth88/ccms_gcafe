using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Logger.swLogDebug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + log);
            Logger.swLogDebug.Flush();
        }
        public static void LogThisLine(string sLogLine)
        {
            Logger.swLog.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "\t:" + "\t" + sLogLine);
            Logger.swLog.Flush();
        }

        public static void CloseLogger()
        {
            Logger.swLog.Flush();
            Logger.swLog.Close();

            Logger.swLogDebug.Flush();
            Logger.swLogDebug.Close();
        }
    }
}
