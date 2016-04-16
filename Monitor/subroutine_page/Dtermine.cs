using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;
using MergeQueryUtil;
using System.Data.SqlClient;

namespace Monitor.subroutine_page
{
    public partial class Dtermine : MonitorForm
    {
        public Dtermine(Index owner, DateTime time, string device_name)
            : base(owner, time, device_name)
        {
            InitializeComponent();
            this.owner = owner;
            this.time = time;
            this.device_name = device_name;
        }

        Index owner;
        DateTime time;
        string device_name;

        private void Dtermine_Load(object sender, EventArgs e)
        {
            DataTable dt = MergeQuery.GetDataAt("v_alarm_log", "status_type_name,start_time,address,train_no,device_name,alarm_value,point_type_name", "Start_time", time, "device_name='" + device_name + "'", null, null);
            if (dt.Rows.Count > 0)//如果这里没有数据，说明没有报警表中没有响应的报警记录！！！
            {
                StringBuilder sb = new StringBuilder();
                string N_time = dt.Rows[0][1].ToString();
                string N_address = Common.GetAddress(Convert.ToInt32(dt.Rows[0][2]));
                string N_train = dt.Rows[0][3].ToString();
                string N_place = dt.Rows[0][4].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "球铰高度")
                    //a_1 = dt.Rows[i][5].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "前羊角左X值报警")
                    //    a_2 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "前羊角左Y值报警")
                    //    a_3 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "前羊角右X值报警")
                    //    a_4 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "前羊角右Y值报警")
                    //    a_5 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "后羊角左X值报警")
                    //    a_6 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "后羊角左Y值报警")
                    //    a_7 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "后羊角右X值报警")
                    //    a_8 = dt.Rows[i][0].ToString();
                    //if (ds.Tables["News"].Rows[i][5].ToString() == "后羊角右Y值报警")
                    //    a_9 = dt.Rows[i][0].ToString();
                    //sb.Append(dt.Rows[i][5].ToString());
                    //sb.Append(":");
                    sb.Append(dt.Rows[i][6].ToString());
                    sb.Append(":");
                    sb.Append(dt.Rows[i][5].ToString());
                    sb.Append("[" + dt.Rows[0][0].ToString() + "],");
                }
                string s = sb.ToString();
                if (s.EndsWith(","))
                    s = s.Substring(0, s.Length - 1);
                label1.Text = "行车时间：" + N_time + ",故障地点：" + N_address + ",列车号：" + N_train + ",车箱号：" + N_place + ",故障类型及故障值：" + s;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = MergeQuery.GetDataAt("v_alarm_log", "id", "Start_time", time, "device_name='" + device_name + "'", null, null);
            int id = 0;
            if (dt.Rows.Count > 0)
            {
                id = Convert.ToInt32(dt.Rows[0][0]);
                MergeUpdate.UpdateData("d_alarmAction_log", "confirmer_id=" + Index.User.ID + ", affirmance=1, confirm_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'", "id", id.ToString(), MergeCommon.GetYYYYMM(time));
                SqlHelper sqlHelper = new SqlHelper();
                string train_log_id = sqlHelper.ExecuteScalar("select train_log_id from d_alarm_log" + MergeCommon.GetYYYYMM(time) + " where id=" + id).ToString();
                if (MergeUpdate.UpdateData("d_train_log", "alarm_status=30", "come_time", Convert.ToDateTime(time), "id=" + train_log_id, null))
                {
                    MessageBox.Show("报警确认成功！");
                    owner.ShowInfo("报警确认成功！");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
