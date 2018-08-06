using CCMS.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CCMS.lib
{
    public class FormView: Form 
    {
        public string host_connent = null;
        public string methodLogin = null;
        public bool isConnectHost = false;
        public FormView()
        {
            //this.host_connent = Constant.serverHost;
            //this.methodLogin = Constant.methodLogin;
            //this.isConnectHost = checkConnectServer();
        }
        public bool checkConnectServer()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead(Constant.serverHost))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }       
    }
}
