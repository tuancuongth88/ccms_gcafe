namespace CliendGcafe.view
{
    partial class LockComputer
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
            this.btnchangepass = new MetroFramework.Controls.MetroButton();
            this.txtComfirmPass = new MetroFramework.Controls.MetroTextBox();
            this.txtpass = new MetroFramework.Controls.MetroTextBox();
            this.lbltotaltimeuse = new MetroFramework.Controls.MetroLabel();
            this.lbltimeuser = new MetroFramework.Controls.MetroLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnchangepass
            // 
            this.btnchangepass.Location = new System.Drawing.Point(28, 87);
            this.btnchangepass.Name = "btnchangepass";
            this.btnchangepass.Size = new System.Drawing.Size(73, 23);
            this.btnchangepass.TabIndex = 4;
            this.btnchangepass.Text = "Khóa";
            this.btnchangepass.Click += new System.EventHandler(this.btnchangepass_Click);
            // 
            // txtComfirmPass
            // 
            this.txtComfirmPass.Location = new System.Drawing.Point(106, 47);
            this.txtComfirmPass.Name = "txtComfirmPass";
            this.txtComfirmPass.PasswordChar = '●';
            this.txtComfirmPass.Size = new System.Drawing.Size(94, 23);
            this.txtComfirmPass.TabIndex = 3;
            this.txtComfirmPass.UseSystemPasswordChar = true;
            // 
            // txtpass
            // 
            this.txtpass.Location = new System.Drawing.Point(106, 15);
            this.txtpass.Name = "txtpass";
            this.txtpass.PasswordChar = '●';
            this.txtpass.Size = new System.Drawing.Size(94, 23);
            this.txtpass.TabIndex = 2;
            this.txtpass.UseSystemPasswordChar = true;
            // 
            // lbltotaltimeuse
            // 
            this.lbltotaltimeuse.AutoSize = true;
            this.lbltotaltimeuse.Location = new System.Drawing.Point(2, 15);
            this.lbltotaltimeuse.Name = "lbltotaltimeuse";
            this.lbltotaltimeuse.Size = new System.Drawing.Size(98, 19);
            this.lbltotaltimeuse.TabIndex = 1;
            this.lbltotaltimeuse.Text = "Mật khẩu khóa:";
            // 
            // lbltimeuser
            // 
            this.lbltimeuser.AutoSize = true;
            this.lbltimeuser.Location = new System.Drawing.Point(2, 47);
            this.lbltimeuser.Name = "lbltimeuser";
            this.lbltimeuser.Size = new System.Drawing.Size(65, 19);
            this.lbltimeuser.TabIndex = 1;
            this.lbltimeuser.Text = "Nhập lại :";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.metroButton1);
            this.groupBox1.Controls.Add(this.btnchangepass);
            this.groupBox1.Controls.Add(this.txtComfirmPass);
            this.groupBox1.Controls.Add(this.txtpass);
            this.groupBox1.Controls.Add(this.lbltotaltimeuse);
            this.groupBox1.Controls.Add(this.lbltimeuser);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(10, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 119);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(116, 87);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(73, 23);
            this.metroButton1.TabIndex = 4;
            this.metroButton1.Text = "Hủy bỏ";
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(74, 10);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(76, 19);
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "Khóa máy";
            // 
            // LockComputer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(230, 157);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LockComputer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnchangepass;
        private MetroFramework.Controls.MetroTextBox txtComfirmPass;
        private MetroFramework.Controls.MetroTextBox txtpass;
        private MetroFramework.Controls.MetroLabel lbltotaltimeuse;
        private MetroFramework.Controls.MetroLabel lbltimeuser;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroLabel metroLabel1;
    }
}