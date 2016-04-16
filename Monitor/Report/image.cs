using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class image : MonitorForm
    {
        public image(Index owner, int train_log_id)
            : base(owner, train_log_id)
        {
            InitializeComponent();
            this.train_log_id = train_log_id;
            owner.ShowInfo("查看图片");
        }

        int train_log_id;

        private void image_Load(object sender, EventArgs e)
        {
            List<string> files = TrainInfo.GetTrainPicFiles(train_log_id);
            imageViewerEx1.Files = files;
        }
    }
}
