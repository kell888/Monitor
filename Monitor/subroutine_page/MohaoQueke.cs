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
    public partial class MohaoQueke : Form
    {
        public MohaoQueke(Index owner, DateTime time, string device_name, string station_name, bool direction, string train_no, int point_type_id, bool frontOrBack, string type)
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
            string a_1 = "", day1 = "", day2 = "", time1 = "", time2 = "";
            int train_log_id = TrainInfo.GetLogId(train_no, time);
            string N_Image_Path = TrainInfo.GetTrainPicPath(train_log_id);
            SqlHelper sqlHelper = new SqlHelper();

            string fb = "forward";
            string gong = "前弓";
            string forb = device_name;// Common.GetFrontOrBack(device_id);
            if (forb != "前弓")
            {
                fb = "behind";
                gong = "后弓";
            }
            label6.Text = "上次过车" + gong + "前滑板远景图片";
            label7.Text = "本次过车" + gong + "前滑板远景图片";
            label8.Text = "上次过车" + gong + "后滑板远景图片";
            label9.Text = "本次过车" + gong + "后滑板远景图片";
            label3.Text = device_name;
            //string pt = Common.GetPointPictureId(Convert.ToInt32(object_id));

            string s = frontOrBack ? "前" : "后";
            string pt = "B_";
            if (frontOrBack)
                pt = "A_";

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
                    DataTable dt = MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,filepath,flash_time,status_type_name", "flash_time", Convert.ToDateTime(time), "and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and point_type_id=" + point_type_id + " and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        if (type == "磨耗")
                        {
                            Double d16 = Double.Parse(dt.Rows[0]["data_value"].ToString());
                            Double d13 = Double.Parse(dt.Rows[1]["data_value"].ToString());
                            label16.Text = d16.ToString("0.00");
                            label13.Text = d13.ToString("0.00");
                            Double X_1 = Math.Abs(d16 - d13);
                            label19.Text = X_1.ToString("0.00");
                        }
                        day1 = dt.Rows[0][1].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][1].ToString();         //上一趟图片文件夹
                        //time1 = dt.Rows[0][2].ToString();         //本趟时间
                        time2 = dt.Rows[1][2].ToString();         //上趟时间
                    }
                    label4.Text = "上次行车时间：" + time2;
                    label5.Text = "本次行车时间：" + time;
                    label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 磨耗缺口影像对比";
                    pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/" + pt + "2_Result.jpg");
                    pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/" + pt + "2_Result.jpg");
                    pictureBox3.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/" + pt + "1_Result.jpg");
                    pictureBox4.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/" + pt + "1_Result.jpg");

                    string Thickness = "";
                    string Gap = "";
                    DataTable ddt = sqlHelper.ExecuteQueryDataTable("Select s_point_type.point_type_name,s_argument.max_value from s_argument,s_point_type where s_argument.point_type_id = s_point_type.point_type_id and s_argument.isenable = 1");
                    if (ddt.Rows.Count > 0)
                    {
                        if (ddt.Rows[0][0].ToString() == s + "滑板磨耗")
                            Thickness = ddt.Rows[0][3].ToString();         //磨耗报警值
                        if (ddt.Rows[0][0].ToString() == s + "滑板缺口")
                            Gap = ddt.Rows[0][3].ToString();              //缺口报警值
                                                                          
                        if (type == "缺口")
                        {
                            string fileName = Environment.CurrentDirectory + "/Image/" + day1 + "/forward/gaplist_1.txt";
                            if (pt == "B_")
                                fileName = Environment.CurrentDirectory + "/Image/" + day1 + "/forward/gaplist_4.txt";
                            label2.Text = gong + s + "碳滑板缺口位置及面积：" + File.ReadAllText(fileName, Encoding.Default);
                            label10.Text = "缺口报警值：" + Gap + "mm2";
                        }
                        else
                        {
                            label16.Text = dt.Rows[0]["data_value"].ToString();
                            a_1 = dt.Rows[0]["status_type_name"].ToString();
                            label12.Text = a_1;
                            if (a_1 == "警报" || a_1 == "报警" || a_1 == "突变" || a_1 == "已确认" || a_1 == "已处理")
                            {
                                label12.ForeColor = Color.Red;
                            }
                            if (dt.Rows.Count > 1)
                            {
                                label13.Text = dt.Rows[1]["data_value"].ToString();
                                a_1 = dt.Rows[1]["status_type_name"].ToString();
                                label12.Text = a_1;
                                if (a_1 == "警报" || a_1 == "报警" || a_1 == "突变" || a_1 == "已确认" || a_1 == "已处理")
                                {
                                    label12.ForeColor = Color.Red;
                                }
                            }
                            label10.Text = "磨耗报警值：" + Thickness + "mm";
                        }
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
                    DataTable dt = MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", Convert.ToDateTime(time), "and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and point_type_id=" + point_type_id + " and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "order by flash_time DESC", null);
                    if (dt.Rows.Count > 0)
                    {
                        day1 = dt.Rows[0][0].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][0].ToString();         //上一趟图片文件夹
                        time1 = dt.Rows[0][1].ToString();         //本趟时间
                        time2 = dt.Rows[1][1].ToString();         //上趟时间
                    }
                    label4.Text = "上次行车时间：" + time2;
                    label5.Text = "本次行车时间：" + time1;
                    label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 磨耗缺口影像对比";
                    pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/" + pt + "2.jpg");
                    pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/" + pt + "2.jpg");
                    pictureBox3.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/" + pt + "1.jpg");
                    pictureBox4.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/" + pt + "1.jpg");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("图像无法识别,请查看本次与上次过车图片是否正常！" + ex.Message);
                }
            }
        }
    }
}
