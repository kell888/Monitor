using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;
using KellControls;

namespace Monitor.Report
{
    public partial class chart : MonitorForm
    {
        DataTable dt;
        string pointType, device, train, where;
        int lastRecordCount = 30;
        public chart(Index owner)
            : base(owner)
        {
            InitializeComponent();
            //winFormPager1.PageSize = 20;
            owner.ShowInfo("监测数据分析");
        }

        private void chart_Load(object sender, EventArgs e)
        {
            int last = 50;
            string lastDays = ConfigurationManager.AppSettings["lastDays"];
            int RET;
            if (!string.IsNullOrEmpty(lastDays))
            {
                if (int.TryParse(lastDays, out RET))
                {
                    last = RET;
                }
            }
            DateTime dt = DateTime.Now;
            dateTimePicker1.Value = dt.AddDays(-last);//默认最近50天内的数据
            dateTimePicker2.Value = DateTime.Now;
            if (Common.LoadTrains(comboBox1, true))
            {
                int train_id = Convert.ToInt32(comboBox1.GetCurrentItemValue());
                Common.LoadDevices(comboBox2, train_id, true);
            }
            Common.LoadPointTypes(comboBox3, 1, true);
            comboBox4.SelectedIndex = 0;
            string lastRecords = ConfigurationManager.AppSettings["lastRecords"];
            if (!string.IsNullOrEmpty(lastRecords))
            {
                if (int.TryParse(lastRecords, out RET))
                {
                    lastRecordCount = RET;
                }
            }
            int interval = getArticle(FanG.Chartlet.ChartTypes.Bar, lastRecordCount);
            SearchOrders(string.Empty, lastRecordCount, interval);
        }

        void SearchOrders(string sWhere, int maxCount, int interval)
        {
            int allCount = 0;
            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    dt = sqlHelper.ExecuteQueryDataTable("select top " + maxCount + " * from (select ROW_NUMBER() OVER(order by flash_time asc) as row, * from v_data_log where flash_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' " + sWhere + ") a where (a.row % " + interval + ") = 0");
                    //MergeQuery.GetDataRange("v_data_log", "top " + maxCount + " * from (select ROW_NUMBER() OVER(order by flash_time asc) as row, *", "flash_time", dateTimePicker1.Value, dateTimePicker2.Value, sWhere + ") a where (a.row % " + interval + ") = 0", null, null);
                    if (dt != null)
                    {
                        allCount = dt.Rows.Count;
                        Chartlet1.Tips.Show = false;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("请确认选定的时间段内是否有数据，且时间前后顺序的正确性：" + e.Message);
            }
            winFormPager1.RecordCount = allCount;
        }

        public int getArticle(FanG.Chartlet.ChartTypes type, int maxCount)
        {
            int interval = 1;
            double tb = 0;
            double xx = 0;
            double sx = 0;
            string name = "";
            string unit = "";
            train = comboBox1.GetCurrentItemText();
            device = comboBox2.GetCurrentItemText();
            pointType = comboBox3.GetCurrentItemText();
            string direction = null;
            if (comboBox4.SelectedIndex > 0)
                direction = comboBox4.SelectedValue.ToString();
            string ss = "";
            if (!string.IsNullOrEmpty(device))
            {
                if (direction != null)
                    ss = "and device_name = '" + device + "' and direction = '" + direction + "' and point_type_name='" + pointType + "'";
                else
                    ss = "and device_name = '" + device + "' and point_type_name='" + pointType + "'";
            }
            else
            {
                if (direction != null)
                    ss = "and direction = '" + direction + "' and point_type_name='" + pointType + "'";
                else
                    ss = "and point_type_name='" + pointType + "'";
            }
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_data_log where flash_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' " + ss);//MergeQuery.GetDataRange("v_data_log", "*", "flash_time", dateTimePicker1.Value, dateTimePicker2.Value, ss, null, null);
                if (dt.Rows.Count > 0)
                {
                    tb = Convert.ToDouble(dt.Rows[0][0]);
                    xx = Convert.ToDouble(dt.Rows[0][1]);
                    sx = Convert.ToDouble(dt.Rows[0][2]);
                    name = dt.Rows[0][3].ToString();
                    unit = dt.Rows[0][4].ToString();
                }

                DataTable d = sqlHelper.ExecuteQueryDataTable("select count(*) from v_data_log where (1=1) " + ss);//MergeQuery.GetData("v_data_log", "count(*)", "flash_time", ss, null, null);
                if (d.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(d.Rows[0][0]);
                    if (count >= maxCount)
                        interval = count / maxCount;
                }

                //string sql = MergeSQLQuery.GetQuerySQLRange("v_data_log", "top " + maxCount + " * from (select flash_time as 监测时间,ROW_NUMBER() OVER(order by flash_time asc) as row,data_value as 检测值", "flash_time", dateTimePicker1.Value, dateTimePicker2.Value, ss + ") a where (a.row % " + interval + ") = 0", null);
                //if (!string.IsNullOrEmpty(sql))
                //{
                dt = sqlHelper.ExecuteQueryDataTable("select top " + maxCount + " * from (select flash_time as 监测时间,ROW_NUMBER() OVER(order by flash_time asc) as row,data_value as 检测值 from v_data_log where flash_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "') a where (a.row % " + interval + ") = 0");//sql
                switch (type)
                {
                    //case FanG.Chartlet.ChartTypes.Line:
                    //    Chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles
                    //    break;
                    case FanG.Chartlet.ChartTypes.Pie:
                        Chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_3D_Aurora_NoCrystal_NoGlow_NoBorder;
                        break;
                    case FanG.Chartlet.ChartTypes.Bar:
                    default:
                        Chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Line_2D_Aurora_ThinRound_NoGlow_NoBorder;
                        break;
                }
                double max = 0;
                double min;
                max = GetMax(dt, 2, out min);
                Chartlet1.MaxValueY = max > sx ? max : sx;
                Chartlet1.MinValueY = min < xx ? min : xx;
                Chartlet1.Shadow.Radius = 15;
                Chartlet1.XLabels.SampleSize = 2;
                Chartlet1.BaseLineX = sx;
                Chartlet1.ChartTitle.Text = pointType + "趋势分析图表";
                Chartlet1.ChartTitle.OffsetY = -20;
                if (!string.IsNullOrEmpty(unit))
                    Chartlet1.YLabels.UnitText = pointType + "(" + unit + ")";
                else
                    Chartlet1.YLabels.UnitText = pointType;
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        System.Collections.Generic.List<int> hideColumnIndeies = new System.Collections.Generic.List<int>();
                        hideColumnIndeies.Add(1);
                        Chartlet1.BindChartData(dt, hideColumnIndeies);
                    }
                    catch (Exception e)
                    {
                        Logs.Error(e.Message);
                    }
                }
                where = ss;
                this.Text = "受电弓历史趋势分析时间:" + dateTimePicker1.Value + "～" + dateTimePicker2.Value + ",列车号: " + train + ",车厢号: " + device + ",分析类型: " + pointType + ",行车方向: " + direction;
                //}
            }
            return interval;
        }

        private double GetMax(DataTable dt, int columnIndex, out double min)
        {
            double max = 0;
            min = 0;
            if (dt.Rows.Count > 0)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double tmp = Convert.ToDouble(dt.Rows[i][columnIndex]);
                        if (tmp > max)
                            max = tmp;
                        if (tmp < min)
                            min = tmp;
                    }
                }
            }
            return max;
        }

        private double GetMax(DataSet ds, int columnIndex)
        {
            double max = 0;
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double tmp = Convert.ToDouble(dt.Rows[i][columnIndex]);
                        if (tmp > max)
                            max = tmp;
                    }
                }
            }
            return max;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int interval = getArticle(FanG.Chartlet.ChartTypes.Bar, lastRecordCount);
            winFormPager1.PageIndex = 1;
            SearchOrders(where, lastRecordCount, interval);
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
            Chartlet1.ChartType = FanG.Chartlet.ChartTypes.Bar;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int interval = getArticle(FanG.Chartlet.ChartTypes.Pie, lastRecordCount);
            winFormPager1.PageIndex = 1;
            SearchOrders(where, lastRecordCount, interval);
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
            Chartlet1.ChartType = FanG.Chartlet.ChartTypes.Pie;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Common.LoadDevices(comboBox2, Convert.ToInt32(comboBox1.GetCurrentItemValue()));
        }

        private void winFormPager1_PageIndexChanged(object sender, EventArgs e)
        {
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
        }
    }
}
