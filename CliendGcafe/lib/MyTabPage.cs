using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMS.lib
{
    class MyTabPage : TabPage
    {
        private Form frm;
        public MyTabPage(MyFormPage frm_contenido)
        {
            this.frm = frm_contenido;
            this.Controls.Add(frm_contenido.pnl);
            this.Text = frm_contenido.Text;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                frm.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class MyFormPage : Form
    {
        public Panel pnl;
    }
}
