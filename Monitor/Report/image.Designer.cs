namespace Monitor.Report
{
    partial class image
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(image));
            this.imageViewerEx1 = new KellImageViewer.ImageViewerEx();
            this.SuspendLayout();
            // 
            // imageViewerEx1
            // 
            this.imageViewerEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewerEx1.Files = ((System.Collections.Generic.List<string>)(resources.GetObject("imageViewerEx1.Files")));
            this.imageViewerEx1.Location = new System.Drawing.Point(0, 0);
            this.imageViewerEx1.MinimumSize = new System.Drawing.Size(340, 260);
            this.imageViewerEx1.Name = "imageViewerEx1";
            this.imageViewerEx1.Size = new System.Drawing.Size(622, 474);
            this.imageViewerEx1.TabIndex = 0;
            // 
            // Pantograph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 474);
            this.Controls.Add(this.imageViewerEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "image";
            this.ShowInTaskbar = false;
            this.Text = "图片浏览";
            this.Load += new System.EventHandler(this.image_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private KellImageViewer.ImageViewerEx imageViewerEx1;

    }
}