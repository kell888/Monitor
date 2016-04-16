using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellCalendarEx;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class DayReportView : MonitorForm
    {
        public DayReportView(Index owner)
        : base(owner)
        {
            InitializeComponent();
            this.owner = owner;
            owner.ShowInfo("每日过车信息");
        }

        Index owner;

        public void AddLinks(YearMonth currentmonth)
        {
            loadingCircle1.Show();
            try
            {
                Dictionary<Date, Dictionary<int, List<TrainInfo>>> days = GetErrInfos(currentmonth);
                foreach (Date date in days.Keys)
                {
                    List<LinkObject> links = new List<LinkObject>();
                    int year = date.Year;
                    int month = date.Month;
                    int day = date.Day;
                    train_report train = new train_report(owner, date.Start, date.End);
                    List<object> args = new List<object>();
                    args.Add("过车信息报表");
                    args.Add(train);
                    Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                    object target = owner;
                    Dictionary<int, List<TrainInfo>> trainInfos = days[date];
                    foreach (int status in trainInfos.Keys)
                    {
                        string text = TrainInfo.GetStatusName(status);
                        Color statusColor = GetColorByStatus(status);
                        //foreach (TrainInfo ei in trainInfos[status])
                        //{
                        //    args.Add(ei);
                        //}
                        LinkObject link = new LinkObject(text, statusColor, true, null, dlgt, target, args.ToArray());
                        links.Add(link);
                    }
                    kellCalendarEx1.AddLinkMsg(year, month, day, links);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错：" + ex.Message);
            }
            finally
            {
                loadingCircle1.Hide();
            }
        }

        private Color GetColorByStatusName(string status)
        {
            if (status == "正常")
                return Color.Blue;
            if (status == "突变" || status == "报警")
                return Color.Red;
            else
                return Color.Blue;
        }

        private Color GetColorByStatus(int status)
        {
            if (status == 0)
                return Color.Blue;
            if (status == 1 || status == 2)
                return Color.Red;
            else if (status > 2)
                return Color.Orange;
            else
                return Color.Blue;
        }

        private Dictionary<Date, Dictionary<int, List<TrainInfo>>> GetErrInfos(YearMonth month)
        {
            Dictionary<Date, Dictionary<int, List<TrainInfo>>> dict = new Dictionary<Date, Dictionary<int, List<TrainInfo>>>();
            DateTime start = DateTime.Parse(month.Year + "-" + month.Month + "-1");
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            DateTime end = DateTime.Parse(month.Year + "-" + month.Month + "-" + daysInMonth + " 23:59:59");
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select id,come_time,alarm_status from d_train_log where come_time between '" + start + "' and '" + end + "'");//MergeQuery.GetDataRange("d_train_log", "id,come_time,alarm_status", "come_time", start, end, null);
                if (dt.Rows.Count > 0)
                {
                    Dictionary<int, List<TrainInfo>> statuses = null;
                    List<TrainInfo> eis = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TrainInfo ei = new TrainInfo();
                        ei.Id = Convert.ToInt32(dt.Rows[i]["id"]);
                        ei.Alarm_status = Convert.ToInt32(dt.Rows[i]["alarm_status"]);
                        DateTime time = Convert.ToDateTime(dt.Rows[i]["start_time"]);
                        Date cur = new Date(time);
                        if (dict.ContainsKey(cur))
                        {
                            if (!dict[cur].ContainsKey(ei.Alarm_status))
                            {
                                eis = new List<TrainInfo>();
                                eis.Add(ei);
                                dict[cur].Add(ei.Alarm_status, eis);
                            }
                            else
                            {
                                dict[cur][ei.Alarm_status].Add(ei);
                            }
                        }
                        else
                        {
                            statuses = new Dictionary<int, List<TrainInfo>>();
                            eis = new List<TrainInfo>();
                            eis.Add(ei);
                            statuses.Add(ei.Alarm_status, eis);
                            dict.Add(cur, statuses);
                        }
                    }
                }
            }
            return dict;
        }

        private void DayReportView_Load(object sender, EventArgs e)
        {
            kellCalendarEx1.GotoToMonth();
        }

        private void kellCalendarEx1_MonthChanged(object sender, YearMonth e)
        {
            AddLinks(e);
        }
    }

    public struct Date
    {
        public int Year;
        public int Month;
        public int Day;

        public Date(DateTime time)
        {
            this.Year = time.Year;
            this.Month = time.Month;
            this.Day = time.Day;
        }

        public DateTime Start
        {
            get
            {
                return DateTime.Parse(this.ToString() + " 00:00:00");
            }
        }

        public DateTime End
        {
            get
            {
                return DateTime.Parse(this.ToString() + " 23:59:59.999");
            }
        }

        public static bool operator==(Date a, Date b)
        {
            return a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;
        }
        public static bool operator !=(Date a, Date b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Date)
            {
                Date other = (Date)obj;
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Year + "-" + Month + "-" + Day;
        }
    } 
}
