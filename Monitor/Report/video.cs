using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class video : MonitorForm
    {
        public video(Index owner, int train_log_id)
            : base(owner, train_log_id)
        {
            InitializeComponent();
            //RegisterDlls();
            this.train_log_id = train_log_id;
            owner.ShowInfo("查看视频");
        }

        int train_log_id;

        private void video_Load(object sender, EventArgs e)
        {
            string file = TrainInfo.GetTrainVideo(train_log_id);
            try
            {
                axVLCPlugin21.BaseURL = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置播放源时出错[BaseURL=" + file + "]：" + ex.Message);
            }
        }

        //private void RegisterDlls()
        //{
        //    try
        //    {
        //        File.Copy(System.Environment.CurrentDirectory + "\\libvlc.dll", System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\libvlc.dll", true);
        //        File.Copy(System.Environment.CurrentDirectory + "\\libvlccore.dll", System.Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\libvlccore.dll", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("复制播放器dll文件到系统目录时出错：" + ex.Message);
        //    }
        //}

        private void Video_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.axVLCPlugin21.Dispose();
        }
    }
}
