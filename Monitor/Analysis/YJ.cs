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
    /// 问题：图片是以“后滑板右羊角Y值”的监测类型来找出来的！！！羊角图片的图片名为A_3,B-3
    /// </summary>
    public partial class YJ : Form
    {
        public YJ(Index owner, int id)
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
            string a_1, a_2, a_3, a_4, a_5, a_6, a_7, a_8, day1, day2, time1, time2;
            int train_log_id = TrainInfo.GetTrainLogId(train_no, time);
            string N_Image_Path = TrainInfo.GetTrainPicPath(train_log_id);
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string fb = "forward";
                string gong = "前弓";
                string forb = device_name;// Common.GetFrontOrBack(device_id);
                string s = "前";
                //bool frontOrBack = true;
                if (forb != "前弓")
                {
                    s = "后";
                    //frontOrBack = false;
                    fb = "behind";
                    gong = "后弓";
                }
                label6.Text = "上次过车" + gong + "前滑板远景图片";
                label7.Text = "本次过车" + gong + "前滑板远景图片";
                label8.Text = "上次过车" + gong + "后滑板远景图片";
                label9.Text = "本次过车" + gong + "后滑板远景图片";

                //string s = frontOrBack ? "前" : "后";
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
                        DataTable dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板左羊角X值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板左羊角X值'", "flash_time DESC", null);
                        Double d16 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d13 = Double.Parse(dt.Rows[1][0].ToString());
                        label16.Text = d16.ToString("0.00");
                        label13.Text = d13.ToString("0.00");
                        Double X_1 = Math.Abs(d16 - d13);
                        label19.Text = X_1.ToString("0.00");
                        a_1 = dt.Rows[0][3].ToString();
                        label27.Text = a_1;
                        if (a_1 == "警报" || a_1 == "报警" || a_1 == "突变" || a_1 == "已确认" || a_1 == "已处理")
                        {
                            label27.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板左羊角Y值'");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板左羊角Y值'", "order by flash_time DESC", null);
                        Double d17 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d14 = Double.Parse(dt.Rows[1][0].ToString());
                        label17.Text = d17.ToString("0.00");
                        label14.Text = d14.ToString("0.00");
                        Double X_2 = Math.Abs(d17 - d14);
                        label20.Text = X_2.ToString("0.00");
                        a_2 = dt.Rows[0][3].ToString();
                        label28.Text = a_2;
                        if (a_2 == "警报" || a_2 == "报警" || a_2 == "突变" || a_2 == "已确认" || a_2 == "已处理")
                        {
                            label28.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板右羊角X值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板右羊角X值'", "flash_time DESC", null);
                        Double d18 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d15 = Double.Parse(dt.Rows[1][0].ToString());
                        label18.Text = d18.ToString("0.00");
                        label15.Text = d15.ToString("0.00");
                        Double X_3 = Math.Abs(d18 - d15);
                        label21.Text = X_3.ToString("0.00");
                        a_3 = dt.Rows[0][3].ToString();
                        label29.Text = a_3;
                        if (a_3 == "警报" || a_3 == "报警" || a_3 == "突变" || a_3 == "已确认" || a_3 == "已处理")
                        {
                            label29.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板右羊角Y值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='前滑板右羊角Y值'", "flash_time DESC", null);
                        Double d45 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d40 = Double.Parse(dt.Rows[1][0].ToString());
                        label45.Text = d45.ToString("0.00");
                        label40.Text = d40.ToString("0.00");
                        Double X_4 = Math.Abs(d45 - d40);
                        label50.Text = X_4.ToString("0.00");
                        a_4 = dt.Rows[0][3].ToString();
                        label35.Text = a_4;
                        if (a_4 == "警报" || a_4 == "报警" || a_4 == "突变" || a_4 == "已确认" || a_4 == "已处理")
                        {
                            label35.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板左羊角X值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板左羊角X值'", "flash_time DESC", null);
                        Double d46 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d41 = Double.Parse(dt.Rows[1][0].ToString());
                        label46.Text = d46.ToString("0.00");
                        label41.Text = d41.ToString("0.00");
                        Double Y_1 = Math.Abs(d46 - d41);
                        label51.Text = Y_1.ToString("0.00");
                        a_5 = dt.Rows[0][3].ToString();
                        label36.Text = a_5;
                        if (a_5 == "警报" || a_5 == "报警" || a_5 == "突变" || a_5 == "已确认" || a_5 == "已处理")
                        {
                            label36.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板左羊角Y值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板左羊角Y值'", "flash_time DESC", null);
                        Double d47 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d42 = Double.Parse(dt.Rows[1][0].ToString());
                        label47.Text = d47.ToString("0.00");
                        label42.Text = d42.ToString("0.00");
                        Double Y_2 = Math.Abs(d47 - d42);
                        label52.Text = Y_2.ToString("0.00");
                        a_6 = dt.Rows[0][3].ToString();
                        label37.Text = a_6;
                        if (a_6 == "警报" || a_6 == "报警" || a_6 == "突变" || a_6 == "已确认" || a_6 == "已处理")
                        {
                            label37.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板右羊角X值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板右羊角X值'", "flash_time DESC", null);
                        Double d48 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d43 = Double.Parse(dt.Rows[1][0].ToString());
                        label48.Text = d48.ToString("0.00");
                        label43.Text = d43.ToString("0.00");
                        Double Y_3 = Math.Abs(d48 - d43);
                        label53.Text = Y_3.ToString("0.00");
                        a_7 = dt.Rows[0][3].ToString();
                        label38.Text = a_7;
                        if (a_7 == "警报" || a_7 == "报警" || a_7 == "突变" || a_7 == "已确认" || a_7 == "已处理")
                        {
                            label38.ForeColor = Color.Red;
                        }
                        dt = sqlHelper.ExecuteQueryDataTable("select top 2 data_value,picturepath,flash_time,status_type_name from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板右羊角Y值' order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 data_value,picturepath,flash_time,status_type_name", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + night1 + "'and'24:00:00' or convert(varchar(8),flash_time,114) between '00:00:00'and'" + night2 + "') and point_type_name='" + point_type_name + "'" + " and (status_type_name<>'警示' and status_type_name<>'疑似报警') and point_type_name='后滑板右羊角Y值'", "flash_time DESC", null);
                        Double d49 = Double.Parse(dt.Rows[0][0].ToString());
                        Double d44 = Double.Parse(dt.Rows[1][0].ToString());
                        label49.Text = d49.ToString("0.00");
                        label44.Text = d44.ToString("0.00");
                        Double Y_4 = Math.Abs(d49 - d44);
                        label54.Text = Y_4.ToString("0.00");
                        a_8 = dt.Rows[0][3].ToString();
                        label39.Text = a_8;
                        if (a_8 == "警报" || a_8 == "报警" || a_8 == "突变" || a_8 == "已确认" || a_8 == "已处理")
                        {
                            label39.ForeColor = Color.Red;
                        }
                        day1 = dt.Rows[0][1].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][1].ToString();         //上一趟图片文件夹
                        //time1 = dt.Rows[0][2].ToString();         //本趟时间
                        time2 = dt.Rows[1][2].ToString();         //上趟时间
                        label4.Text = "上次行车时间：" + time2;
                        label5.Text = "本次行车时间：" + time;
                        label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 羊角影像对比";
                        pictureBox1.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/A_3_Result.jpg");
                        pictureBox2.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/A_3_Result.jpg");
                        pictureBox3.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day2 + "/" + fb + "/B_3_Result.jpg");
                        pictureBox4.ImageLocation = PictureUtil.GetPicture(Environment.CurrentDirectory + "/Image/" + day1 + "/" + fb + "/B_3_Result.jpg");

                        string X1 = "0";
                        string X2 = "0";
                        string X_B = "0";
                        string y1 = "0";
                        string y2 = "0";
                        string y_b = "0";
                        DataTable ddt = sqlHelper.ExecuteQueryDataTable("Select s_point_type.point_type_name,s_argument.standard_value,s_argument.min_value,s_argument.max_value from s_argument,s_point_type where s_argument.point_type_id = s_point_type.point_type_id and s_argument.isenable = 1");
                        if (ddt.Rows.Count > 0)
                        {
                            for (int i = 0; i < ddt.Rows.Count; i++)
                            {
                                if (ddt.Rows[i][0].ToString().Length > 6 && ddt.Rows[i][0].ToString().Substring(0, 6) == s + "滑板左羊角")
                                {
                                    if (ddt.Rows[i][0].ToString() == s + "滑板左羊角X值")
                                    {
                                        X1 = ddt.Rows[i][3].ToString();              //羊角X上限
                                        X2 = ddt.Rows[i][2].ToString();             //羊角X下限
                                        X_B = ddt.Rows[i][1].ToString();            //羊角X突变
                                    }
                                    if (ddt.Rows[i][0].ToString() == s + "滑板左羊角Y值")
                                    {
                                        y1 = ddt.Rows[i][3].ToString();             //羊角Y上限
                                        y2 = ddt.Rows[i][2].ToString();             //羊角Y下限
                                        y_b = ddt.Rows[i][1].ToString();            //羊角Y突变
                                    }
                                }
                                if (ddt.Rows[i][0].ToString().Length > 6 && ddt.Rows[i][0].ToString().Substring(0, 6) == s + "滑板右羊角")
                                {
                                    if (ddt.Rows[i][0].ToString() == s + "滑板右羊角X值")
                                    {
                                        X1 = ddt.Rows[i][3].ToString();              //羊角X上限
                                        X2 = ddt.Rows[i][2].ToString();             //羊角X下限
                                        X_B = ddt.Rows[i][1].ToString();            //羊角X突变
                                    }
                                    if (ddt.Rows[i][0].ToString() == s + "滑板右羊角Y值")
                                    {
                                        y1 = ddt.Rows[i][3].ToString();             //羊角Y上限
                                        y2 = ddt.Rows[i][2].ToString();             //羊角Y下限
                                        y_b = ddt.Rows[i][1].ToString();            //羊角Y突变
                                    }
                                }
                            }
                            label2.Text = "报警上下限：X(" + X2 + "," + X1 + ") Y(" + y2 + "," + y1 + ")；突变值：X坐标(±" + X_B + ") Y坐标(±" + y_b + ")";
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
                        DataTable dt = sqlHelper.ExecuteQueryDataTable("select top 2 filepath,flash_time from v_data_log where flash_time <= '" + time + "' and device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and (status_type_name<>'警示' and status_type_name<>'疑似报警') order by flash_time DESC");//MergeQuery.GetDataBefore("v_data_log", "top 2 filepath,flash_time", "flash_time", time, "device_name ='" + device_name + "' and train_no ='" + train_no + "'  and direction ='" + direction + "' and (convert(varchar(8),flash_time,114) between '" + day + "'and'" + night1 + "') and (status_type_name<>'警示' and status_type_name<>'疑似报警')", "flash_time DESC", null);
                        day1 = dt.Rows[0][0].ToString();         //本趟图片文件夹
                        day2 = dt.Rows[1][0].ToString();         //上一趟图片文件夹
                        //time1 = dt.Rows[0][1].ToString();         //本趟时间
                        time2 = dt.Rows[1][1].ToString();         //上趟时间
                        label4.Text = "上次行车时间：" + time2;
                        label5.Text = "本次行车时间：" + time;
                        label1.Text = "列车号：" + train_no + "车厢号：" + device_name + " 羊角影像对比";
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
