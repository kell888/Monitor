namespace Monitor.SystemManager
{
    partial class LoginList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginList));
            this.winFormPager1 = new KellControls.WinFormPager();
            this.outlookGrid1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // winFormPager1
            // 
            this.winFormPager1.BackColor = System.Drawing.Color.Transparent;
            this.winFormPager1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("winFormPager1.BackgroundImage")));
            this.winFormPager1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.winFormPager1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.winFormPager1.Location = new System.Drawing.Point(0, 420);
            this.winFormPager1.Name = "winFormPager1";
            this.winFormPager1.RecordCount = 0;
            this.winFormPager1.Size = new System.Drawing.Size(679, 23);
            this.winFormPager1.TabIndex = 4;
            this.winFormPager1.PageIndexChanged += new System.EventHandler(this.winFormPager1_PageIndexChanged);
            // 
            // outlookGrid1
            // 
            //this.outlookGrid1.CollapseIcon = null;
            this.outlookGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.outlookGrid1.ExpandIcon = null;
            //this.outlookGrid1.GroupType = KellOutlookGrid.DateGroupType.Day;
            this.outlookGrid1.Location = new System.Drawing.Point(0, 0);
            this.outlookGrid1.Name = "outlookGrid1";
            this.outlookGrid1.Size = new System.Drawing.Size(679, 443);
            //this.outlookGrid1.SumColumn = -1;
            this.outlookGrid1.TabIndex = 3;
            // 
            // LoginList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 443);
            this.Controls.Add(this.winFormPager1);
            this.Controls.Add(this.outlookGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginList";
            this.ShowInTaskbar = false;
            this.Text = "用户登录日志";
            this.Load += new System.EventHandler(this.LoginList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private KellControls.WinFormPager winFormPager1;
        private System.Windows.Forms.DataGridView outlookGrid1;
    }
}