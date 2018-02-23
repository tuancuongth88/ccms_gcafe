using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliendGcafe.model
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

        public String pass_lock_computer { get; set; }
    }
}
