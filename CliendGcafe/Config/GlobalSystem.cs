using CCMS.model;
using System;
using Quobject.SocketIoClientDotNet.Client;

namespace CCMS.Config
{
    static class GlobalSystem
    {
        public static User user = null;

        public static TimeGameUser timeGameUser = null;

        public static User userUpdate = null;

        public static DateTime timeStart = new DateTime();

        public static int islogin = 0;
        public static int isLogout = 0;

        public static string language = "vi-VN";

        public static string ipv4 = "";

        public static int sleep = 5000;

        public static int time_shutdown = 0;

        //1: admin 0:user
        public static int is_admin = 0;

        //count mat ket noi 3 lan thi cho out luon
        public static int count_disconnect = 0;

        public static Socket socket;

        //soket để ghi log
        public static Socket socketLog;



    }
}
