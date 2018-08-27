using System;

namespace CCMS.Config
{
    static class Constant
    {
        public static String serverHost             = System.Configuration.ConfigurationSettings.AppSettings["ServerConnect"];
        public static String methodLogin            = System.Configuration.ConfigurationSettings.AppSettings["methodLogin"];
        public static String methodService          = System.Configuration.ConfigurationSettings.AppSettings["methodService"];
        public static String methodRefresh          = System.Configuration.ConfigurationSettings.AppSettings["methodRefresh"];
        public static String methodLogout           = System.Configuration.ConfigurationSettings.AppSettings["methodLogout"];
        public static String methodChangePass       = System.Configuration.ConfigurationSettings.AppSettings["methodChangePass"];
        public static String methodProfile          = System.Configuration.ConfigurationSettings.AppSettings["methodProfile"];
        public static String methodSlide            = System.Configuration.ConfigurationSettings.AppSettings["methodSlide"];
        public static String methodBlacklist        = System.Configuration.ConfigurationSettings.AppSettings["methodBlacklist"];
        public static String methodAdminAction      = System.Configuration.ConfigurationSettings.AppSettings["methodAdminAction"];
        public static String methodGetClientInfo    = System.Configuration.ConfigurationSettings.AppSettings["methodGetClientInfo"];
        public static String methodWriteLog         = System.Configuration.ConfigurationSettings.AppSettings["methodWriteLoger"];

        public static string time_auto_call_serve   = System.Configuration.ConfigurationSettings.AppSettings["time_auto_call_serve"];
        public static string serverSoket            = System.Configuration.ConfigurationSettings.AppSettings["ServerSoket"];
        public static string serverSoketOnline      = System.Configuration.ConfigurationSettings.AppSettings["ServerSoketOnline"];
        public static string serverSoketLog         = System.Configuration.ConfigurationSettings.AppSettings["ServerSoketLog"];        

        //account admin
        public static string username               = "admin";
        public static string password               = "khatvongchanthanhsangtao";
        public static string token_admin            = System.Configuration.ConfigurationSettings.AppSettings["token_admin"];
    }
}
