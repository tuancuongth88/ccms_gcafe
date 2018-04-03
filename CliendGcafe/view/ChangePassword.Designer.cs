namespace CCMS.view
{
    partial class ChangePassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnchangepass = new MetroFramework.Controls.MetroButton();
            this.txtComfirmPass = new MetroFramework.Controls.MetroTextBox();
            this.txtNewpass = new MetroFramework.Controls.MetroTextBox();
            this.txtOldPass = new MetroFramework.Controls.MetroTextBox();
            this.lbltotaltimeuse = new MetroFramework.Controls.MetroLabel();
            this.lbltimeuser = new MetroFramework.Controls.MetroLabel();
            this.lbltotaltime = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnchangepass);
            this.groupBox1.Controls.Add(this.txtComfirmPass);
            this.groupBox1.Controls.Add(this.txtNewpass);
            this.groupBox1.Controls.Add(this.txtOldPass);
            this.groupBox1.Controls.Add(this.lbltotaltimeuse);
            this.groupBox1.Controls.Add(this.lbltimeuser);
            this.groupBox1.Controls.Add(this.lbltotaltime);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(6, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 156);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnchangepass
            // 
            this.btnchangepass.Location = new System.Drawing.Point(56, 123);
            this.btnchangepass.Name = "btnchangepass";
            this.btnchangepass.Size = new System.Drawing.Size(112, 23);
            this.btnchangepass.TabIndex = 4;
            this.btnchangepass.Text = "Đổi mật khẩu";
            this.btnchangepass.Click += new System.EventHandler(this.btnchangepass_Click);
            // 
            // txtComfirmPass
            // 
            this.txtComfirmPass.Location = new System.Drawing.Point(125, 85);
            this.txtComfirmPass.Name = "txtComfirmPass";
            this.txtComfirmPass.PasswordChar = '●';
            this.txtComfirmPass.Size = new System.Drawing.Size(75, 23);
            this.txtComfirmPass.TabIndex = 3;
            this.txtComfirmPass.UseSystemPasswordChar = true;
            // 
            // txtNewpass
            // 
            this.txtNewpass.Location = new System.Drawing.Point(125, 53);
            this.txtNewpass.Name = "txtNewpass";
            this.txtNewpass.PasswordChar = '●';
            this.txtNewpass.Size = new System.Drawing.Size(75, 23);
            this.txtNewpass.TabIndex = 2;
            this.txtNewpass.UseSystemPasswordChar = true;
            // 
            // txtOldPass
            // 
            this.txtOldPass.Location = new System.Drawing.Point(125, 19);
            this.txtOldPass.Name = "txtOldPass";
            this.txtOldPass.PasswordChar = '●';
            this.txtOldPass.Size = new System.Drawing.Size(75, 23);
            this.txtOldPass.TabIndex = 1;
            this.txtOldPass.UseSystemPasswordChar = true;
            // 
            // lbltotaltimeuse
            // 
            this.lbltotaltimeuse.AutoSize = true;
            this.lbltotaltimeuse.Location = new System.Drawing.Point(2, 53);
            this.lbltotaltimeuse.Name = "lbltotaltimeuse";
            this.lbltotaltimeuse.Size = new System.Drawing.Size(93, 19);
            this.lbltotaltimeuse.TabIndex = 1;
            this.lbltotaltimeuse.Text = "Mật khẩu mới:";
            // 
            // lbltimeuser
            // 
            this.lbltimeuser.AutoSize = true;
            this.lbltimeuser.Location = new System.Drawing.Point(2, 85);
            this.lbltimeuser.Name = "lbltimeuser";
            this.lbltimeuser.Size = new System.Drawing.Size(65, 19);
            this.lbltimeuser.TabIndex = 1;
            this.lbltimeuser.Text = "Nhập lại :";
            // 
            // lbltotaltime
            // 
            this.lbltotaltime.AutoSize = true;
            this.lbltotaltime.Location = new System.Drawing.Point(2, 17);
            this.lbltotaltime.Name = "lbltotaltime";
            this.lbltotaltime.Size = new System.Drawing.Size(83, 19);
            this.lbltotaltime.TabIndex = 1;
            this.lbltotaltime.Text = "Mặt khẩu cũ:";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(62, 21);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(98, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Đổi mật khẩu";
            // 
            // ChangePassword
            // 
            this.AcceptButton = this.btnchangepass;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(222, 214);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ChangePassword";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroTextBox txtComfirmPass;
        private MetroFramework.Controls.MetroTextBox txtNewpass;
        private MetroFramework.Controls.MetroTextBox txtOldPass;
        private MetroFramework.Controls.MetroLabel lbltotaltimeuse;
        private MetroFramework.Controls.MetroLabel lbltimeuser;
        private MetroFramework.Controls.MetroLabel lbltotaltime;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton btnchangepass;
    }
}