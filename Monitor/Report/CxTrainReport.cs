using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class CxTrainReport : MonitorForm
    {
        DataTable dt;
        Index owner;
        string startTime, endTime;
        public CxTrainReport(Index owner, string startTime, string endTime)
            : base(owner, startTime, endTime)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            this.owner = owner;
            this.startTime = startTime;
            this.endTime = endTime;
            owner.ShowInfo("近期行车记录");
        }

        private void CxTrainReport_Load(object sender, EventArgs e)
        {
            winFormPager1.PageIndex = 1;
            SearchOrders();
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
        }

        void SearchOrders()
        {
            DataTable dtt = null;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int allCount = 0;
                if (startTime == null || endTime == null)
                {
                    int maxCount = 300;
                    string defaultCount = ConfigurationManager.AppSettings["defaultCount"];
                    if (!string.IsNullOrEmpty(defaultCount))
                    {
                        int RET;
                        if (int.TryParse(defaultCount, out RET))
                            maxCount = RET;
                    }
                    dtt = sqlHelper.ExecuteQueryDataTable("select top " + maxCount + " * from v_train_log order by come_time desc");//MergeQuery.GetData("v_train_log", "top " + maxCount + " *", "come_time", null, "come_time desc");
                    if (dtt != null)
                        allCount = dtt.Rows.Count;
                }
                else
                {
                    DateTime start = Convert.ToDateTime(startTime);
                    DateTime end = Convert.ToDateTime(endTime);
                    dtt = sqlHelper.ExecuteQueryDataTable("select * from v_train_log where come_time between '" + start + "' and '" + end + "' order by come_time desc");//MergeQuery.GetDataRange("v_train_log", "*", "come_time", start, end, null, "come_time desc");
                    if (dtt != null)
                        allCount = dtt.Rows.Count;
                }
                Dictionary<string, string> cols = new Dictionary<string, string>();
                cols.Add("id", "ID");
                cols.Add("train_no", "列车号");
                cols.Add("station_name", "监测站");
                cols.Add("come_time", "来车时间");
                cols.Add("go_time", "去车时间");
                cols.Add("alarm_count", "报警数");
                cols.Add("direction", "行车方向");
                cols.Add("status_type_name", "检测状态");
                cols.Add("line_no", "线路");
                dt = GridUtil.ViewData(dtt, cols);
                winFormPager1.RecordCount = allCount;
            }
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
