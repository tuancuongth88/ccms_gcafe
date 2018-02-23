using CliendGcafe.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliendGcafe.Config
{
    static class GlobalSystem
    {
        public static User user = null;

        public static User userUpdate = null;

        public static DateTime timeStart = new DateTime();

        public static int islogin = 0;
        public static int isLogout = 0;

        public static string language = "vi-VN";

        public static string ipv4 = "";

        public static int sleep = 5000;

    }
}
