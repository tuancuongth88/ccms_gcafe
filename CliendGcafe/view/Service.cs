using CliendGcafe.Config;
using CliendGcafe.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe.view
{
    public partial class Service : Form
    {
        string link = Constant.serverHost + Constant.methodService;
        public Service()
        {
            InitializeComponent();
            webBrowser1.Url = new Uri(Constant.serverHost + Constant.methodService);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
       
      
        

}
}
