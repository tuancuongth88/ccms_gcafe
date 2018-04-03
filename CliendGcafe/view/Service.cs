using CCMS.Config;
using CCMS.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class Service : Form
    {
        public Service()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
            webBrowser1.Url = new Uri(Constant.serverHost + Constant.methodService);
            webBrowser1.ScriptErrorsSuppressed = true;
        }
       
      
        

}
}
