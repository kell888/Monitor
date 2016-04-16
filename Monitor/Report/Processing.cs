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
    public partial class Processing : MonitorForm
    {
        public Processing(Index owner, DateTime time, string device_name)
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

        private void Processing_Load(object sender, System.EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select status_type_name,start_time,address,train_no,device_name,alarm_value,point_type_name from v_alarm_log where start_time='" + time + "' and device_name='" + device_name + "'");//MergeQuery.GetDataAt("v_alarm_log", "status_type_name,start_time,address,train_no,device_name,alarm_value,point_type_name", "Start_time", time, "device_name='" + device_name + "'", null, null);
                if (dt.Rows.Count > 0)//如果这里没有数据，说明没有报警表中没有响应的报警记录！！！
                {
                    StringBuilder sb = new StringBuilder();
                    string N_time = dt.Rows[0][1].ToString();   //当前行ID
                    string N_address = dt.Rows[0][2].ToString();
                    string N_train = dt.Rows[0][3].ToString();
                    string N_place = dt.Rows[0][4].ToString();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append(dt.Rows[i][6].ToString());
                        sb.Append(":");
                        sb.Append(dt.Rows[i][5].ToString());
                        sb.Append("[" + dt.Rows[0][0].ToString() + "],");
                    }
                    string s = sb.ToString();
                    if (s.EndsWith(","))
                        s = s.Substring(0, s.Length - 1);
                    textBox1.Text = "行车时间：" + N_time + ",故障地点：" + N_address + ",列车号：" + N_train + ",车厢号：" + N_place + ",故障类型及故障值：" + s;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select id from v_alarm_log where Start_time='" + time + "' and device_name='" + device_name + "'");//DataTable dt = MergeQuery.GetDataAt("v_alarm_log", "id", "Start_time", time, "device_name='" + device_name + "'", null, null);
                int id = 0;
                //if (dt.Rows.Count > 0)
                if (o != null && o != DBNull.Value)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    //{
                    //MergeUpdate.UpdateData("d_alarmAction_log", "affirmance=2, process_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',alarm_msg='" + textBox1.Text + "',processer_id=" + Index.User.ID + ",process_msg='" + textBox2.Text + "'", "id", id.ToString(), MergeCommon.GetYYYYMM(time));
                    id = Convert.ToInt32(o);//Convert.ToInt32(dt.Rows[0][0]);
                    if (sqlHelper.ExecuteNonQuery("update d_alarmAction_log set affirmance=2, process_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',alarm_msg='" + textBox1.Text + "',processer_id=" + Index.User.ID + ",process_msg='" + textBox2.Text + "' where id=" + id) > 0)
                    {
                        MessageBox.Show("报警处理成功！");
                        owner.ShowInfo("报警处理成功！");
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    //}
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
