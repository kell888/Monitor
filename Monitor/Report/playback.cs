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
    public partial class playback : MonitorForm
    {
        public playback(Index owner, int train_log_id)
            : base(owner, train_log_id)
        {
            InitializeComponent();
            this.train_log_id = train_log_id;
            owner.ShowInfo("视频回放");
        }

        int train_log_id;

        private void playback_Load(object sender, System.EventArgs e)
        {
            string file = TrainInfo.GetTrainVideo(train_log_id);
            axWindowsMediaPlayer1.openPlayer(file);
        }

        private void playback_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (!axWindowsMediaPlayer1.IsDisposed)
                axWindowsMediaPlayer1.Dispose();
        }
    }
}
