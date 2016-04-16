using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class TrainReport : MonitorForm
    {
        public TrainReport(Index owner, int id)
            : base(owner, id)
        {
            InitializeComponent();
            this.owner = owner;
            this.id = id;
            owner.ShowInfo("过车信息浏览");
        }

        Index owner;
        int id;

        private void TrainReport_Load(object sender, EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_train_log where id=" + id + " order by point_type_name");//MergeQuery.GetData("v_train_log", "*", "come_time", "id=" + id, "point_type_name");
                GridUtil.BindData(outlookGrid1, dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lbl_Time.Text = dt.Rows[0]["come_time"].ToString();
                    lbl_TrainNo.Text = dt.Rows[0]["train_no"].ToString();
                    lbl_Station.Text = dt.Rows[0]["station_name"].ToString();
                    lbl_Direction.Text = dt.Rows[0]["direction"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time = TrainInfo.GetTimeByTrainLogId(id);
            string train_no = TrainInfo.GetTrainNoByTrainLogId(id);
            data_report data = new data_report(owner, time, train_no);
            owner.AddTabPage("监测数据报表", data);
        }
    }
}
