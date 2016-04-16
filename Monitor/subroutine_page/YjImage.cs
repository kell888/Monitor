using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.subroutine_page
{
    public partial class YjImage : Form
    {
        public YjImage(Index owner, DateTime time, string device_name, string station_name, bool direction, string train_no, int point_type_id, bool frontOrBack, string type)
        {
            InitializeComponent();
            this.time = time;
            //this.device_id = device_id;
            this.device_name = device_name;
            this.station_name = station_name;
            this.direction = direction == false ? "正向" : "反向";
            this.train_no = train_no;
            this.point_type_id = point_type_id;
            this.frontOrBack = frontOrBack;
            this.type = type;
        }

        DateTime time;
        //int device_id;
        int point_type_id;
        string device_name;
        string station_name;
        string direction;
        string train_no;
        bool frontOrBack;
        string type;

        private void FB_Angle_Load(object sender, EventArgs e)
        {
      int N_SDG_PLACE_1=0, N_SDG_PLACE_2=0, N_SDG_PLACE_3=0, N_SDG_PLACE_4=0;
        Double N_JiaoLian_Height_1=0, N_JiaoLian_Height_2=0, N_JiaoLian_Height_3=0, N_JiaoLian_Height_4=0;
            string date_1 = "", date_2 = "", date_3 = "", date_4 = "", Time_1 = "", Time_2 = "";
            int train_log_id = TrainInfo.GetLogId(train_no, time);
            string N_Image_Path = TrainInfo.GetTrainPicPath(train_log_id);
            SqlHelper sqlHelper = new SqlHelper();

            //string fb = "forward";
            string gong = "前弓";
            string forb = device_name;// Common.GetFrontOrBack(device_id);
            if (forb != "前弓")
            {
                //fb = "behind";
                gong = "后弓";
            }
            label6.Text = "上次过车" + gong + "前滑板远景图片";
            label7.Text = "本次过车" + gong + "前滑板远景图片";
            label3.Text = device_name;
            //string pt = Common.GetPointPictureId(Convert.ToInt32(object_id));

            string day = "08:00:00";
            string d = ConfigurationManager.AppSettings["day"];
            if (!string.IsNullOrEmpty(d))
            {
                day = d;
            }
            string night1 = "19:00:00";
            string night2 = "04:00:00";
            string n = ConfigurationManager.AppSettings["night"];
            if (!string.IsNullOrEmpty(n))
            {
                string[] ns = n.Split('|');
                if (ns.Length == 2)
                {
                    night1 = ns[0];
                    night2 = ns[1];
                }
                else if (ns.Length == 1)
                {
                    night1 = ns[0];
                    night2 = day;
                }
            }
            DataTable dt = null;
            //if (N_Image_Path.Length < 12) N_Image_Path = N_Image_Path.PadRight(12, '0');
            //DateTime current = Convert.ToDateTime(N_Image_Path.Substring(8, 2) + ":" + N_Image_Path.Substring(10, 2));
            //if (N_Image_Path.Length >= 14)
            //    current = Convert.ToDateTime(N_Image_Path.Substring(8, 2) + ":" + N_Image_Path.Substring(10, 2) + ":" + N_Image_Path.Substring(12, 2));
            DateTime current = Convert.ToDateTime(N_Image_Path.Substring(8, 2) + ":" + N_Image_Path.Substring(10, 2) + ":" + N_Image_Path.Substring(12, 2));
            DateTime morning = Convert.ToDateTime(day);
            DateTime nightfall = Convert.ToDateTime(night1);
            if (current < morning || current >= nightfall)//判断是白天还是晚上（晚上）
            {
                try
                {
                    dt = MergeQuery.GetDataBefore("v_data_log", "top 4 device_name,data_value", "flash_time", time, "train_no = '" + train_no + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_id=" + point_type_id + " and not(status_type_name='警示' or status_type_name='疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        N_SDG_PLACE_1 = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());
                        N_SDG_PLACE_2 = Convert.ToInt32(dt.Rows[1][0].ToString().Trim());
                        N_SDG_PLACE_3 = Convert.ToInt32(dt.Rows[2][0].ToString().Trim());
                        N_SDG_PLACE_4 = Convert.ToInt32(dt.Rows[3][0].ToString().Trim());
                        N_JiaoLian_Height_1 = Double.Parse(dt.Rows[0][2].ToString().Trim());
                        N_JiaoLian_Height_2 = Double.Parse(dt.Rows[1][2].ToString().Trim());
                        N_JiaoLian_Height_3 = Double.Parse(dt.Rows[2][2].ToString().Trim());
                        N_JiaoLian_Height_4 = Double.Parse(dt.Rows[3][2].ToString().Trim());
                    }
                    DataTable ddt = sqlHelper.ExecuteQueryDataTable("Select s_argument.standard_value,s_argument.min_value,s_argument.max_value from s_argument,s_point_type where s_argument.point_type_id = s_point_type.point_type_id and s_argument.isenable = 1 and s_point_type.point_type_name='球铰高度'");
                    if (ddt.Rows.Count > 0)
                    {
                        string qj1 = ddt.Rows[0][2].ToString();              //球铰上限
                        string qj2 = ddt.Rows[0][1].ToString();             //球铰下限
                        string qj_B = ddt.Rows[0][0].ToString();             //球铰突变
                        Double d13 = N_JiaoLian_Height_4;
                        Double d16 = N_JiaoLian_Height_2;
                        label13.Text = d13.ToString("0.00");
                        label16.Text = d16.ToString("0.00");
                        Double Cz = Math.Abs(N_JiaoLian_Height_4 - N_JiaoLian_Height_2);
                        label19.Text = Cz.ToString("0.00");
                        label2.Text = "报警上下限：(" + qj2 + "," + qj1 + ")；突变值：" + qj_B;
                    }
                    dt = MergeQuery.GetDataBefore("v_data_log", "filepath,flash_time", "flash_time", time, "train_no = '" + train_no + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_id=" + point_type_id + " and not(status_type_name='警示' or status_type_name='疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        date_2 = dt.Rows[1][0].ToString();
                        date_4 = dt.Rows[3][0].ToString();
                        Time_1 = dt.Rows[1][1].ToString();
                        Time_2 = dt.Rows[3][1].ToString();
                    }
                    label4.Text = "上次行车时间：" + Time_2;
                    label5.Text = "本次行车时间：" + Time_1;
                    label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 球铰影像对比";
                    label6.Text = "上次过车" + gong + "球铰高度图片";
                    label7.Text = "此次过车" + gong + "球铰高度图片";
                    if (N_SDG_PLACE_1 > N_SDG_PLACE_2 && N_SDG_PLACE_1 == N_SDG_PLACE_3)                    //都正
                    {
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_4 + "/forward/1_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_2 + "/forward/1_Result.jpg");
                    }
                    else if (N_SDG_PLACE_1 > N_SDG_PLACE_2 && N_SDG_PLACE_1 != N_SDG_PLACE_3)                         //前正后反
                    {
                        label3.Text = "注意：近二次行车车辆号相反，因此对应的图片也相反。";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/forward/1_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/behind/1_Result.jpg");
                    }
                    else if (N_SDG_PLACE_1 < N_SDG_PLACE_2 && N_SDG_PLACE_1 == N_SDG_PLACE_3)                  //都反
                    {
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/behind/1_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/behind/1_Result.jpg");
                    }
                    else if (N_SDG_PLACE_1 < N_SDG_PLACE_2 && N_SDG_PLACE_1 != N_SDG_PLACE_3)                        //前反后正
                    {
                        label3.Text = "注意：近二次行车车辆号相反，因此对应的图片也相反。";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/behind/1_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/forward/1_Result.jpg");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("图像无法识别,请查看本次与上次过车图片是否正常！" + ex.Message);
                }
            }
            else//白天
            {
                try
                {
                    dt = MergeQuery.GetDataBefore("v_data_log", "top 4 device_name,data_value", "flash_time", time, "train_no = '" + train_no + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_id=" + point_type_id + " and not(status_type_name='警示' or status_type_name='疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        N_SDG_PLACE_1 = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());
                        N_SDG_PLACE_2 = Convert.ToInt32(dt.Rows[1][0].ToString().Trim());
                        N_SDG_PLACE_3 = Convert.ToInt32(dt.Rows[2][0].ToString().Trim());
                        N_SDG_PLACE_4 = Convert.ToInt32(dt.Rows[3][0].ToString().Trim());
                    }
                    dt = MergeQuery.GetDataBefore("v_data_log", "filepath,flash_time", "flash_time", time, "train_no = '" + train_no + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_id=" + point_type_id + " and not(status_type_name='警示' or status_type_name='疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        date_2 = dt.Rows[1][0].ToString();
                        date_4 = dt.Rows[3][0].ToString();
                        Time_1 = dt.Rows[1][1].ToString();
                        Time_2 = dt.Rows[3][1].ToString();
                    }
                    label4.Text = "上次行车时间：" + Time_2;
                    label5.Text = "本次行车时间：" + Time_1;
                    label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 球铰影像对比";
                    label6.Text = "上次过车" + gong + "球铰高度图片";
                    label7.Text = "此次过车" + gong + "球铰高度图片";
                    if (N_SDG_PLACE_1 > N_SDG_PLACE_2 && N_SDG_PLACE_1 == N_SDG_PLACE_3)                              //都正向
                    {
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_4 + "/forward/1.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_2 + "/forward/1.jpg");
                    }
                    else if (N_SDG_PLACE_1 > N_SDG_PLACE_2 && N_SDG_PLACE_1 != N_SDG_PLACE_3)                       //前正，后反
                    {
                        label3.Text = "注意：近二次行车车辆号相反，因此对应的图片也相反。";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/forward/1.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/behind/1.jpg");
                    }
                    else if (N_SDG_PLACE_1 < N_SDG_PLACE_2 && N_SDG_PLACE_1 == N_SDG_PLACE_3)                              //都反
                    {
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/behind/1.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/behind/1.jpg");
                    }
                    else if (N_SDG_PLACE_1 < N_SDG_PLACE_2 && N_SDG_PLACE_1 != N_SDG_PLACE_3)                                     //前反后正
                    {
                        label3.Text = "注意：近二次行车车辆号相反，因此对应的图片也相反。";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_3 + "/behind/1.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + date_1 + "/forward/1.jpg");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("图像无法识别,请查看本次与上次过车图片是否正常！" + ex.Message);
                }
            }
        }
    }
}
