using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.Analysis
{
    /// <summary>
    /// 问题：图片是以“前后倾斜量”的监测类型来找出来的！！！碳滑板的图片名为A_3,B_3
    /// </summary>
    public partial class THB : Form
    {
        public THB(Index owner, int id)
        {
            InitializeComponent();
            this.id = id;
        }
        int id;
        DateTime time;
        //int device_id;
        string point_type_name;
        string device_name;
        //string station_name;
        string direction;
        string train_no;

        private void FB_Angle_Load(object sender, EventArgs e)
        {
            TrainInfo.GetParas(id, out time, out point_type_name, out device_name, out train_no, out direction);
            string a_1 = "", a_2 = "", a_3 = "", day1, day2, time1, time2;
            int train_log_id = TrainInfo.GetTrainLogId(train_no, time);
            string N_Image_Path = TrainInfo.GetTrainPicPath(train_log_id);
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string fb = "forward";
                string gong = "前弓";
                string forb = device_name;// Common.GetFrontOrBack(device_id);
                //bool frontOrBack = true;
                if (forb != "前弓")
                {
                    //frontOrBack = false;
                    fb = "behind";
                    gong = "后弓";
                }
                label6.Text = "上次过车" + gong + "前滑板远景图片";
                label7.Text = "本次过车" + gong + "前滑板远景图片";
                label8.Text = "上次过车" + gong + "后滑板远景图片";
                label9.Text = "本次过车" + gong + "后滑板远景图片";
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
                        DataTable dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,filepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "' and '24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00' and '" + night2 + "') and point_type_name='中心偏移平均值' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,filepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "' and '24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00' and '" + night2 + "') and point_type_name='中心偏移平均值' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "flash_time DESC", null);
                        if (dt.Rows.Count > 0)
                        {
                            Double d16 = Double.Parse(dt.Rows[0][0].ToString());
                            label16.Text = d16.ToString("0.00");
                            Double d13 = Double.Parse(dt.Rows[1][0].ToString());
                            label13.Text = d13.ToString("0.00");
                            Double X_1 = Math.Abs(d16 - d13);
                            label19.Text = X_1.ToString("0.00");
                            a_1 = dt.Rows[0][3].ToString();
                            label27.Text = a_1;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,filepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "' and '24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00' and '" + night2 + "') and point_type_name='上下倾斜平均值' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,filepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "' and '24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00' and '" + night2 + "') and point_type_name='上下倾斜平均值' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "flash_time DESC", null);
                        if (dt.Rows.Count > 0)
                        {
                            Double d17 = Double.Parse(dt.Rows[0][0].ToString());
                            label17.Text = d17.ToString("0.00");
                            Double d14 = Double.Parse(dt.Rows[1][0].ToString());
                            label14.Text = d14.ToString("0.00");
                            Double X_2 = Math.Abs(d17 - d14);
                            label20.Text = X_2.ToString("0.00");
                            a_2 = dt.Rows[0][3].ToString();
                            label28.Text = a_2;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,filepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='前后倾斜量' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,filepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='前后倾斜量' and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "flash_time DESC", null);
                        if (dt.Rows.Count > 0)
                        {
                            Double d18 = Double.Parse(dt.Rows[0][0].ToString());
                            label18.Text = d18.ToString("0.00");
                            Double d15 = Double.Parse(dt.Rows[1][0].ToString());
                            label15.Text = d15.ToString("0.00");
                            Double X_3 = Math.Abs(d18 - d15);
                            label21.Text = X_3.ToString("0.00");
                            a_3 = dt.Rows[0][3].ToString();
                            label29.Text = a_3;
                        }
                        if (a_1 == "警报" || a_1 == "报警" || a_1 == "突变" || a_1 == "已确认" || a_1 == "已处理")
                        {
                            label27.ForeColor = Color.Red;
                        }
                        if (a_2 == "警报" || a_2 == "报警" || a_2 == "突变" || a_2 == "已确认" || a_2 == "已处理")
                        {
                            label28.ForeColor = Color.Red;
                        }
                        if (a_3 == "警报" || a_3 == "报警" || a_3 == "突变" || a_3 == "已确认" || a_3 == "已处理")
                        {
                            label29.ForeColor = Color.Red;
                        }
                        day1 = dt.Rows[0][1].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][1].ToString();         //上一趟图片文件夹
                        //time1 = dt.Rows[0][2].ToString();         //本趟时间
                        time2 = dt.Rows[1][2].ToString();         //上趟时间
                        label4.Text = "上次行车时间：" + time2;
                        label5.Text = "本次行车时间：" + time;
                        label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 碳滑板影像对比";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/A_3_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/A_3_Result.jpg");
                        pictureBox3.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/B_3_Result.jpg");
                        pictureBox4.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/B_3_Result.jpg");

                        string Offset = "0";
                        string UP = "0";
                        string FB = "0";

                        DataTable ddt = sqlHelper.ExecuteQueryDataTable("Select s_point_type.point_type_name,s_argument.max_value from s_argument,s_point_type where s_argument.point_type_id = s_point_type.point_type_id and s_argument.isenable = 1");
                        if (ddt.Rows.Count > 0)
                        {
                            for (int i = 0; i < ddt.Rows.Count; i++)
                            {
                                if (ddt.Rows[i][0].ToString() == "中心偏移平均值")
                                    Offset = ddt.Rows[i][3].ToString();         //中心偏移
                                if (ddt.Rows[i][0].ToString() == "上下倾斜平均值")
                                    UP = ddt.Rows[i][3].ToString();              //上下倾斜
                                if (ddt.Rows[i][0].ToString() == "前后倾斜量")
                                    FB = ddt.Rows[i][3].ToString();             //前后倾斜
                            }
                        }
                        label2.Text = "中心偏移报警值：" + Offset + "；上下倾斜报警值：" + UP + "；前后倾斜报警值：" + FB;
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
                        DataTable dt = sqlHelper.ExecuteQueryDataTable("select top 2 filepath,flash_time from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and (status_type_name<>'警示' and status_type_name<>'疑似报警') order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 filepath,flash_time", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "flash_time DESC", null);
                        day1 = dt.Rows[0][0].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][0].ToString();         //上一趟图片文件夹
                        //time1 = dt.Rows[0][1].ToString();         //本趟时间
                        time2 = dt.Rows[1][1].ToString();         //上趟时间
                        label4.Text = "上次行车时间：" + time2;
                        label5.Text = "本次行车时间：" + time;
                        label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 碳滑板影像对比";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/A_3.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/A_3.jpg");
                        pictureBox3.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/B_3.jpg");
                        pictureBox4.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/B_3.jpg");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("图像无法识别,请查看本次与上次过车图片是否正常！" + ex.Message);
                    }
                }
            }
        }
    }
}
