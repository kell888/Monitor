namespace Monitor.Report
{
    partial class SdgReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_Direction = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_Station = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_TrainNo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.outlookGrid1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbl_Direction);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lbl_Station);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lbl_TrainNo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbl_Time);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 35);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::Monitor.Properties.Resources.print;
            this.button1.Location = new System.Drawing.Point(739, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 29);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_Direction
            // 
            this.lbl_Direction.AutoSize = true;
            this.lbl_Direction.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Direction.Location = new System.Drawing.Point(647, 11);
            this.lbl_Direction.Name = "lbl_Direction";
            this.lbl_Direction.Size = new System.Drawing.Size(41, 12);
            this.lbl_Direction.TabIndex = 7;
            this.lbl_Direction.Text = "[方向]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(601, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "方向：";
            // 
            // lbl_Station
            // 
            this.lbl_Station.AutoSize = true;
            this.lbl_Station.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Station.Location = new System.Drawing.Point(402, 11);
            this.lbl_Station.Name = "lbl_Station";
            this.lbl_Station.Size = new System.Drawing.Size(53, 12);
            this.lbl_Station.TabIndex = 5;
            this.lbl_Station.Text = "[监测站]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "监测站：";
            // 
            // lbl_TrainNo
            // 
            this.lbl_TrainNo.AutoSize = true;
            this.lbl_TrainNo.ForeColor = System.Drawing.Color.Blue;
            this.lbl_TrainNo.Location = new System.Drawing.Point(238, 11);
            this.lbl_TrainNo.Name = "lbl_TrainNo";
            this.lbl_TrainNo.Size = new System.Drawing.Size(41, 12);
            this.lbl_TrainNo.TabIndex = 3;
            this.lbl_TrainNo.Text = "[车号]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "车号：";
            // 
            // lbl_Time
            // 
            this.lbl_Time.AutoSize = true;
            this.lbl_Time.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Time.Location = new System.Drawing.Point(73, 11);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(41, 12);
            this.lbl_Time.TabIndex = 1;
            this.lbl_Time.Text = "[时间]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "过车时间：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.outlookGrid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(774, 427);
            this.panel2.TabIndex = 1;
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
            this.outlookGrid1.Size = new System.Drawing.Size(774, 427);
            //this.outlookGrid1.SumColumn = -1;
            this.outlookGrid1.TabIndex = 0;
            // 
            // SdgReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 462);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SdgReport";
            this.ShowInTaskbar = false;
            this.Text = "监测数据浏览";
            this.Load += new System.EventHandler(this.SdgReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outlookGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView outlookGrid1;
        private System.Windows.Forms.Label lbl_Direction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_Station;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_TrainNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}