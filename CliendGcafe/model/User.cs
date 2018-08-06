using CCMS.Config;
using CCMS.lib;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace CCMS.model
{
    class User
    {
        public int id { get; set; }
        public String name { get; set; }
        public String email { get; set; }
        public int user_group_id { get; set; }
        public int type { get; set; }
        public int total_monney { get; set; }
        public int total_discount { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String Level { get; set; }
        public int promotion_percent { get; set; }
        //group computer
        public int price { get; set; }
        // so tien dang nhap theo phien
        public int total_money_login_session { get; set; }
        public string token { get; set; }
        // mat khau khoa may
        public String pass_lock_computer { get; set; }

        // thời gian gửi lên tính tiền
        public String last_time_request { get; set; }

        //thời gian server trả về xử lý công tiếp 3 phut để gửi lên
        public String last_request_time { get; set; }

        //check xem có phải máy thi đấu hay không
        public bool in_competitive { get; set; } = false;

        //id map voi userid 
        public int conversation_id { get; set; }

        public static void updateStatusLoginAdmin(int status)
        {
            String HtmlResult = "";
            try
            {
                string myParameters = "status=" + status;
                string URI = Constant.serverHost + Constant.methodAdminAction;
                Logger.LogDebugFile("------------- Call Update status Admin Login------------------");
                Logger.LogDebugFile("URL: " + URI);
                Logger.LogDebugFile("PARAM: " + myParameters);
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers.Add("token", Constant.token_admin);
                    HtmlResult = wc.UploadString(URI, myParameters);
                    Logger.LogDebugFile("update status admin " + HtmlResult);
                   
                }
                Logger.LogDebugFile("------------- End call Update status Admin Login------------------");

            }
            catch (Exception ex)
            {
                Logger.LogThisLine("updateStatusLoginAdmin :" + ex.ToString());
                return;
            }
        }
    } 

}
