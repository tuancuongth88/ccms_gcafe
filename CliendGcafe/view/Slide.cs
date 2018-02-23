using CliendGcafe.view;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CliendGcafe
{
    public partial class Slide : Form
    {
        Thread t = null;
        public Slide()
        {
            InitializeComponent();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
        public void showImage(WebRequest request)
        {
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                pictureBox1.Image = Bitmap.FromStream(stream);
            }
        }

        public void slideImage()
        {
            while (true)
            {
               
                List<String> img = new List<string>();
                img.Add("http://hddesktopwallpapers.in/wp-content/uploads/2015/11/gaming-desktop-background.jpg");
                img.Add("https://cdn.push-entertainment.com/HD/Space-Journey-3D-Moving-Desktop-Background.jpg");
                img.Add("https://3.bp.blogspot.com/-ws-l8ieVM-c/WZ17B02BnqI/AAAAAAAAqEY/mwjJD6lDpjECpzyvrconFdgxghj_iUVjQCLcBGAs/s1600/3D-Computer-Photos-desktop-wallpapers-cool-colourful-background-photos-download-free-windows-apple-1920x1080.jpg");
                string imgStr = img.OrderBy(s => Guid.NewGuid()).First();
                var request = WebRequest.Create(imgStr);
                showImage(request);
                Thread.Sleep(3000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //List<String> img = new List<string>();
            //img.Add("http://hddesktopwallpapers.in/wp-content/uploads/2015/11/gaming-desktop-background.jpg");
            //img.Add("https://cdn.push-entertainment.com/HD/Space-Journey-3D-Moving-Desktop-Background.jpg");
            //img.Add("https://3.bp.blogspot.com/-ws-l8ieVM-c/WZ17B02BnqI/AAAAAAAAqEY/mwjJD6lDpjECpzyvrconFdgxghj_iUVjQCLcBGAs/s1600/3D-Computer-Photos-desktop-wallpapers-cool-colourful-background-photos-download-free-windows-apple-1920x1080.jpg");
            //string imgStr = img.OrderBy(s => Guid.NewGuid()).First();
            //var request = WebRequest.Create(imgStr);
            //showImage(request);
        }

        private void Slide_Load(object sender, EventArgs e)
        {
            t = new Thread(new ThreadStart(slideImage));
            t.Start();
        }

        private void Slide_KeyPress(object sender, KeyPressEventArgs e)
        {
            t.Abort();
            //Login frmlogin = new Login();
            //frmlogin.ShowDialog(this);
        }
    }
}
