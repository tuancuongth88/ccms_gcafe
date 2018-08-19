namespace CCMS.view
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.btnlogin = new System.Windows.Forms.Button();
            this.btnhuy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnloading = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.loading1 = new CircularProgressBar.CircularProgressBar();
            this.pnlogin = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.pnloading.SuspendLayout();
            this.pnlogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnlogin
            // 
            resources.ApplyResources(this.btnlogin, "btnlogin");
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.UseVisualStyleBackColor = true;
            this.btnlogin.Click += new System.EventHandler(this.btnlogin_Click);
            // 
            // btnhuy
            // 
            this.btnhuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnhuy, "btnhuy");
            this.btnhuy.Name = "btnhuy";
            this.btnhuy.UseVisualStyleBackColor = true;
            this.btnhuy.Click += new System.EventHandler(this.btnhuy_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnhuy);
            this.groupBox1.Controls.Add(this.btnlogin);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            resources.ApplyResources(this.txtUser, "txtUser");
            this.txtUser.Name = "txtUser";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // pnloading
            // 
            this.pnloading.Controls.Add(this.label3);
            this.pnloading.Controls.Add(this.loading1);
            resources.ApplyResources(this.pnloading, "pnloading");
            this.pnloading.Name = "pnloading";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Name = "label3";
            // 
            // loading1
            // 
            this.loading1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.loading1.AnimationSpeed = 500;
            this.loading1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.loading1, "loading1");
            this.loading1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.loading1.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.loading1.InnerMargin = 2;
            this.loading1.InnerWidth = -1;
            this.loading1.MarqueeAnimationSpeed = 2000;
            this.loading1.Name = "loading1";
            this.loading1.OuterColor = System.Drawing.Color.Gray;
            this.loading1.OuterMargin = -25;
            this.loading1.OuterWidth = 10;
            this.loading1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.loading1.ProgressWidth = 25;
            this.loading1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.loading1.StartAngle = 270;
            this.loading1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.loading1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.loading1.SubscriptText = "";
            this.loading1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.loading1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.loading1.SuperscriptText = "";
            this.loading1.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            // 
            // pnlogin
            // 
            this.pnlogin.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.pnlogin, "pnlogin");
            this.pnlogin.Name = "pnlogin";
            // 
            // Login
            // 
            this.AcceptButton = this.btnlogin;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.CancelButton = this.btnhuy;
            this.Controls.Add(this.pnlogin);
            this.Controls.Add(this.pnloading);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnloading.ResumeLayout(false);
            this.pnloading.PerformLayout();
            this.pnlogin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnhuy;
        private System.Windows.Forms.Button btnlogin;
        private System.Windows.Forms.Panel pnloading;
        private CircularProgressBar.CircularProgressBar loading1;
        private System.Windows.Forms.Panel pnlogin;
        private System.Windows.Forms.Label label3;
    }
}