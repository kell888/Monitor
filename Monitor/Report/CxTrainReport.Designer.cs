namespace Monitor.Report
{
    partial class CxTrainReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxTrainReport));
            this.outlookGrid1 = new System.Windows.Forms.DataGridView();
            this.winFormPager1 = new KellControls.WinFormPager();
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).BeginInit();
            this.SuspendLayout();
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
            this.outlookGrid1.Size = new System.Drawing.Size(661, 435);
            //this.outlookGrid1.SumColumn = -1;
            this.outlookGrid1.TabIndex = 0;
            // 
            // winFormPager1
            // 
            this.winFormPager1.BackColor = System.Drawing.Color.Transparent;
            this.winFormPager1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("winFormPager1.BackgroundImage")));
            this.winFormPager1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.winFormPager1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.winFormPager1.Location = new System.Drawing.Point(0, 412);
            this.winFormPager1.Name = "winFormPager1";
            this.winFormPager1.RecordCount = 0;
            this.winFormPager1.Size = new System.Drawing.Size(661, 23);
            this.winFormPager1.TabIndex = 2;
            this.winFormPager1.PageIndexChanged += new System.EventHandler(this.winFormPager1_PageIndexChanged);
            // 
            // CxTrainReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(661, 435);
            this.Controls.Add(this.winFormPager1);
            this.Controls.Add(this.outlookGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CxTrainReport";
            this.ShowInTaskbar = false;
            this.Text = "过车记录查询";
            this.Load += new System.EventHandler(this.CxTrainReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView outlookGrid1;
        private KellControls.WinFormPager winFormPager1;
    }
}