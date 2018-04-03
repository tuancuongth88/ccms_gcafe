using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.model
{
    class TimeGameUser
    {
        // tien con lai
        public string total_monney { get; set; }
        // tong thoi gian
        public string TotalTime { get; set; }
        // thoi gian su dung
        public string TimeUse { get; set; }
        //thoi gian con lai
        public string TimeRemaining { get; set; }
        // chi phi su dung
        public string CostOfUse { get; set; }

        public int iscombo { get; set; }

    }
}
