using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliendGcafe.Config
{
    static class Constant
    {
        public static String serverHost         = System.Configuration.ConfigurationSettings.AppSettings["ServerConnect"];
        public static String methodLogin    = System.Configuration.ConfigurationSettings.AppSettings["methodLogin"];
        public static String methodService = System.Configuration.ConfigurationSettings.AppSettings["methodService"];
        public static String methodRefresh = System.Configuration.ConfigurationSettings.AppSettings["methodRefresh"];
        public static String methodLogout = System.Configuration.ConfigurationSettings.AppSettings["methodLogout"];
        public static String methodChangePass = System.Configuration.ConfigurationSettings.AppSettings["methodChangePass"]; 
        public static String methodProfile = System.Configuration.ConfigurationSettings.AppSettings["methodProfile"];
        public static String methodSlide = System.Configuration.ConfigurationSettings.AppSettings["methodSlide"];

        public static string time_auto_call_serve = System.Configuration.ConfigurationSettings.AppSettings["time_auto_call_serve"];
        public static string serverSoket = System.Configuration.ConfigurationSettings.AppSettings["ServerSoket"]; 
    }
}
