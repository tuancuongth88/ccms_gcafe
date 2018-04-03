using CCMS.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.model
{
    class SlideProcess 
    {
        public string callApiSlide()
        {
            String json = "";
            string URI =Constant.serverHost + Constant.methodSlide;
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(URI);
            }
            return json;
        }
    }
}
