using System;
using System.Drawing;
using System.Windows.Forms;

namespace CCMS.view
{
    public partial class alert : Form
    {
        public alert(string _message, AlertType type)
        {
            InitializeComponent();
            
            TopMost = false;
            message.Text = _message;
            switch (type)
            {
                case AlertType.success:
                    this.BackColor = Color.SeaGreen;
                    icon.Image = imageList1.Images[0];
                    break;
                case AlertType.info:
                    this.BackColor = Color.Gray;
                    icon.Image = imageList1.Images[1];
                    break;
                case AlertType.warnig:
                    this.BackColor = Color.Crimson;
                    icon.Image = imageList1.Images[2];
                    break;
                case AlertType.error:
                    this.BackColor = Color.FromArgb(255, 128, 0);
                    icon.Image = imageList1.Images[3];
                    break;

            }
        }

        public enum AlertType
        {
            success, info, warnig, error
        }

        public static void Show(string message, AlertType type, bool off_soud = true, int typeSourd = 0)
        {
            if(off_soud == true)
            {
                System.IO.Stream str = null;
                switch (typeSourd)
                {
                    case 0:
                        str = CCMS.Properties.Resources.notification;
                        break;
                    case 1:
                        str = CCMS.Properties.Resources._1;
                        break;
                    case 2:
                        str = CCMS.Properties.Resources._2;
                        break;
                    case 3:
                        str = CCMS.Properties.Resources._3;
                        break;
                    case 4:
                        str = CCMS.Properties.Resources._4;
                        break;
                    case 5:
                        str = CCMS.Properties.Resources._5;
                        break;
                    case 6:
                        str = CCMS.Properties.Resources._6;
                        break;
                    case 7:
                        str = CCMS.Properties.Resources._7;
                        break;
                    case 8:
                        str = CCMS.Properties.Resources._8;
                        break;
                    case 9:
                        str = CCMS.Properties.Resources._9;
                        break;
                    case 10:
                        str = CCMS.Properties.Resources._10;
                        break;
                }                                     
                System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                snd.Play();
            }
            new CCMS.view.alert(message, type).Show();
            
        }
        private void alert_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
            show.Start();
        }

        private void timeout_Tick(object sender, EventArgs e)
        {
            closealert.Start();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            closealert.Start();
        }

        int interval = 0;

        //show transition
        private void show_Tick(object sender, EventArgs e)
        {
            if (this.Top < 60)
            {
                this.Top += interval; // drop the alert
                interval += 2; // double the speed
            }
            else
            {
                show.Stop();
            }
        }

        private void close_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.2; //reduce opacity to zero
            }
            else
            {
                this.Close(); //then close
            }
        }
    }
}
