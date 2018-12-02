namespace dejitaruyubi
{
    partial class startup
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
            this.components = new System.ComponentModel.Container();
            this.lbllabel = new System.Windows.Forms.Label();
            this.lbltech = new System.Windows.Forms.Label();
            this.lblcopyright = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblloadstatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbllabel
            // 
            this.lbllabel.AutoSize = true;
            this.lbllabel.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbllabel.Location = new System.Drawing.Point(56, 137);
            this.lbllabel.Name = "lbllabel";
            this.lbllabel.Size = new System.Drawing.Size(363, 28);
            this.lbllabel.TabIndex = 0;
            this.lbllabel.Text = "Digital FingerPrint Scanner";
            // 
            // lbltech
            // 
            this.lbltech.AutoSize = true;
            this.lbltech.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbltech.Location = new System.Drawing.Point(353, 325);
            this.lbltech.Name = "lbltech";
            this.lbltech.Size = new System.Drawing.Size(117, 13);
            this.lbltech.TabIndex = 1;
            this.lbltech.Text = "Fingers-On Technology";
            this.lbltech.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbltech.Click += new System.EventHandler(this.lbltech_Click);
            // 
            // lblcopyright
            // 
            this.lblcopyright.AutoSize = true;
            this.lblcopyright.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcopyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblcopyright.Location = new System.Drawing.Point(12, 326);
            this.lblcopyright.Name = "lblcopyright";
            this.lblcopyright.Size = new System.Drawing.Size(97, 13);
            this.lblcopyright.TabIndex = 2;
            this.lblcopyright.Text = "Copyright @2018";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblloadstatus
            // 
            this.lblloadstatus.AutoSize = true;
            this.lblloadstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblloadstatus.ForeColor = System.Drawing.Color.White;
            this.lblloadstatus.Location = new System.Drawing.Point(182, 176);
            this.lblloadstatus.Name = "lblloadstatus";
            this.lblloadstatus.Size = new System.Drawing.Size(0, 12);
            this.lblloadstatus.TabIndex = 3;
            this.lblloadstatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(482, 348);
            this.Controls.Add(this.lblloadstatus);
            this.Controls.Add(this.lblcopyright);
            this.Controls.Add(this.lbltech);
            this.Controls.Add(this.lbllabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "startup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.startup_FormClosing);
            this.Load += new System.EventHandler(this.startup_Load);
            this.Shown += new System.EventHandler(this.startup_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbllabel;
        private System.Windows.Forms.Label lbltech;
        private System.Windows.Forms.Label lblcopyright;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblloadstatus;
    }
}