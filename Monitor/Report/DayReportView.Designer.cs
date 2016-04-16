namespace Monitor.Report
{
    partial class DayReportView
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
            this.kellCalendarEx1 = new KellCalendarEx.KellCalendarEx();
            this.loadingCircle1 = new KellControls.LoadingCircle();
            this.SuspendLayout();
            // 
            // kellCalendarEx1
            // 
            this.kellCalendarEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kellCalendarEx1.Location = new System.Drawing.Point(0, 0);
            this.kellCalendarEx1.MinimumSize = new System.Drawing.Size(500, 400);
            this.kellCalendarEx1.Name = "kellCalendarEx1";
            this.kellCalendarEx1.Size = new System.Drawing.Size(738, 477);
            this.kellCalendarEx1.TabIndex = 0;
            this.kellCalendarEx1.TodayColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.kellCalendarEx1.MonthChanged += new System.EventHandler<KellCalendarEx.YearMonth>(this.kellCalendarEx1_MonthChanged);
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Active = true;
            this.loadingCircle1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingCircle1.Color = System.Drawing.Color.DarkOrange;
            this.loadingCircle1.InnerCircleRadius = 29;
            this.loadingCircle1.Location = new System.Drawing.Point(319, 188);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.NumberSpoke = 12;
            this.loadingCircle1.OuterCircleRadius = 43;
            this.loadingCircle1.RotationSpeed = 100;
            this.loadingCircle1.Size = new System.Drawing.Size(100, 100);
            this.loadingCircle1.SpokeThickness = 14;
            this.loadingCircle1.TabIndex = 1;
            this.loadingCircle1.Text = "loadingCircle1";
            this.loadingCircle1.Visible = false;
            // 
            // DayReportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 477);
            this.Controls.Add(this.loadingCircle1);
            this.Controls.Add(this.kellCalendarEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DayReportView";
            this.ShowInTaskbar = false;
            this.Text = "每日过车信息";
            this.Load += new System.EventHandler(this.DayReportView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private KellCalendarEx.KellCalendarEx kellCalendarEx1;
        private KellControls.LoadingCircle loadingCircle1;
    }
}