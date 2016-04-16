using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Monitor
{
    public partial class MediaPlayer : UserControl
    {
        private VlcPlayer vlc_player;
        private bool is_playinig;
        private string file;
        private string url;
        private bool is_paused;
        private bool isFullscreen;
        private float speed = 1.0F;

        public MediaPlayer()
        {
            InitializeComponent();

            string pluginPath = System.Environment.CurrentDirectory + "\\plugins\\";
            vlc_player = new VlcPlayer(pluginPath);
            vlc_player.SetRenderWindow(this.panel1);

            tbVideoTime.Text = "00:00:00/00:00:00";

            is_playinig = false;
        }

        ~MediaPlayer()
        {
            vlc_player.Dispose();
        }

        [Description("播放的媒体文件路径")]
        public string File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
            }
        }
        [Description("播放的网络地址")]
        public string URL
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        [Description("媒体的播放长度(单位：秒)")]
        public int Duration
        {
            get
            {
                return (int)vlc_player.Duration();
            }
        }
        [Browsable(false)]
        public bool IsPlaying
        {
            get
            {
                return is_playinig;
            }
        }
        [Browsable(false)]
        public bool IsPaused
        {
            get
            {
                return is_paused;
            }
        }
        [Browsable(false)]
        public bool IsFullscreen
        {
            get
            {
                return isFullscreen;
            }
        }
        [Description("播放的音量")]
        public int Volumn
        {
            get
            {
                return vlc_player.GetVolume();
            }
            set
            {
                if (value > -1 && value < 256)
                    vlc_player.SetVolume(value);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (is_playinig)
            {
                if (trackBar1.Value == trackBar1.Maximum)
                {
                    vlc_player.Stop();
                    timer1.Stop();
                }
                else
                {
                    trackBar1.Value = trackBar1.Value + 1;
                    tbVideoTime.Text = string.Format("{0}/{1}", GetTimeString(trackBar1.Value), GetTimeString(trackBar1.Maximum));
                }
            }
        }

        private string GetTimeString(int val)
        {
            int hour = val / 3600;
            val %= 3600;
            int minute = val / 60;
            int second = val % 60;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (is_playinig)
            {
                vlc_player.SetPlayTime(trackBar1.Value);
                trackBar1.Value = (int)vlc_player.GetPlayTime();
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            vlc_player.SetVolume(trackBar2.Value);
            tbValue.Text = trackBar2.Value.ToString();
        }

        private void MediaPlayer_Load(object sender, EventArgs e)
        {
            trackBar2.Value = vlc_player.GetVolume();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(file))
            {
                if (is_playinig)
                {
                    try
                    {
                        vlc_player.Pause();
                        timer1.Stop();
                        is_paused = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("暂停播放出错：" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        vlc_player.PlayFile(file);
                        trackBar1.SetRange(0, (int)vlc_player.Duration());
                        timer1.Start();
                        is_playinig = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("开始播放出错：" + ex.Message);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(url))
            {
                if (is_playinig)
                {
                    try
                    {
                        vlc_player.Pause();
                        timer1.Stop();
                        is_paused = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("暂停播放出错：" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        vlc_player.PlayURL(url);
                        trackBar1.SetRange(0, (int)vlc_player.Duration());
                        timer1.Start();
                        is_playinig = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("开始播放出错：" + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (is_playinig || is_paused)
            {
                try
                {
                    vlc_player.Stop();
                    trackBar1.Value = 0;
                    timer1.Stop();
                    is_playinig = false;
                    is_paused = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("停止播放出错：" + ex.Message);
                }
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            isFullscreen=!isFullscreen;
            vlc_player.SetFullScreen(isFullscreen);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "保存截图";
            saveFileDialog1.Filter = "BMP图片(*.bmp)|*.bmp|所有文件(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool b = vlc_player.Snap(saveFileDialog1.FileName);
                if (!b)
                {
                    MessageBox.Show("保存截图失败！");
                }
                else if (MessageBox.Show("是否要立即打开查看截图？", "保存成功") == DialogResult.OK)
                {
                    try
                    {
                        Process.Start(saveFileDialog1.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开图片时出错：" + ex.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//Slow
            speed -= 0.25F;
            vlc_player.SetPlaySpeed(speed);
        }

        private void button4_Click(object sender, EventArgs e)
        {//Fast
            speed += 0.25F;
            vlc_player.SetPlaySpeed(speed);
        }
    }
}
