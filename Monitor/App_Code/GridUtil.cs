using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KellOutlookGrid;
using System.Configuration;
using System.Drawing;

namespace Monitor.App_Code
{
    public static class GridUtil
    {
        /// <summary>
        /// 转换原始数据，以便于在数据网格中正确显示
        /// </summary>
        /// <param name="dt">原始数据</param>
        /// <param name="columnHeaders">要显示(更改)的列头名称</param>
        /// <param name="removeColumns">要移除的列</param>
        /// <param name="colNewOrder">重新排序原来数据的列顺序(如果有移除的列，则表示移除后的顺序)</param>
        /// <returns></returns>
        public static DataTable ViewData(DataTable dt, Dictionary<string, string> columnHeaders, List<string> removeColumns = null, List<string> colNewOrder = null)
        {
            if (dt == null)
                return null;
            DataTable result = dt.Copy();
            if (columnHeaders != null && columnHeaders.Count > 0)
            {
                foreach (string colName in columnHeaders.Keys)
                {
                    int index = result.Columns.IndexOf(colName);
                    if (index > -1)
                    {
                        result.Columns[index].ColumnName = columnHeaders[colName];
                    }
                }
            }
            if (removeColumns != null && removeColumns.Count > 0)
            {
                foreach (string colName in removeColumns)
                {
                    result.Columns.Remove(colName);
                }
            }
            if (colNewOrder != null && colNewOrder.Count > 0)
            {
                for (int i = 0; i < colNewOrder.Count; i++)
                {
                    result.Columns[colNewOrder[i]].SetOrdinal(i);
                }
            }
            return result;
        }

        public static void AddLinkColumn(OutlookGrid outlookGrid1, string text, out string columnKey, CallbackArgs callback = null)
        {
            columnKey = "#Link#";
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Link#" + DateTime.Now.Ticks.ToString().Substring(4);
            link.Name = columnKey;
            //link.Text = text;
            link.HeaderText = text;
            link.UseColumnTextForLinkValue = true;
            link.TrackVisitedState = true;
            outlookGrid1.Columns.Add(link);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddLinkColumn(OutlookGrid outlookGrid1, string text, string columnKey = "#LinkSpecial#", CallbackArgs callback = null)
        {
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Link#" + DateTime.Now.Ticks.ToString().Substring(4);
            link.Name = columnKey;
            //link.Text = text;
            link.HeaderText = text;
            link.UseColumnTextForLinkValue = true;
            link.TrackVisitedState = true;
            outlookGrid1.Columns.Add(link);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddButtonColumn(OutlookGrid outlookGrid1, string text, out string columnKey, CallbackArgs callback = null)
        {
            columnKey = "#Button#";
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Button#" + DateTime.Now.Ticks.ToString().Substring(4);
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Name = columnKey;
            //button.Text = text;
            button.HeaderText = text;
            outlookGrid1.Columns.Add(button);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddButtonColumn(OutlookGrid outlookGrid1, string text, string columnKey = "#ButtonSpecial#", CallbackArgs callback = null)
        {
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Button#" + DateTime.Now.Ticks.ToString().Substring(4);
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Name = columnKey;
            //button.Text = text;
            button.HeaderText = text;
            outlookGrid1.Columns.Add(button);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void ClearData(OutlookGrid outlookGrid1)
        {
            outlookGrid1.BindData(null, null);
        }

        public static void BindData(OutlookGrid outlookGrid1, DataTable dt, bool isGroup = false, int groupColumnIndex = -1, int sumColumn = -1, ListSortDirection direction = ListSortDirection.Ascending, int sortColumnIndex = -1)
        {
            outlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            outlookGrid1.ReadOnly = true;
            outlookGrid1.AllowUserToAddRows = false;
            outlookGrid1.BindData(dt, null);
            if (isGroup)
            {
                outlookGrid1.GroupTemplate = new OutlookGridMoneyGroup();
                outlookGrid1.GroupTemplate.Column = outlookGrid1.Columns[groupColumnIndex];
                outlookGrid1.SumColumn = sumColumn;
                outlookGrid1.CollapseIcon = KellOutlookGrid.Properties.Resources.ExpandBig.ToBitmap();
                outlookGrid1.ExpandIcon = KellOutlookGrid.Properties.Resources.CollapseBig.ToBitmap();
            }
            if (sortColumnIndex > -1)
                outlookGrid1.Sort(sortColumnIndex, direction);
            else if (groupColumnIndex > -1)
                outlookGrid1.Sort(groupColumnIndex, direction);
        }

        public static void ClearGroup(OutlookGrid outlookGrid1)
        {
            outlookGrid1.ClearGroups();
        }

        public static void ExpandAll(OutlookGrid outlookGrid1)
        {
            outlookGrid1.ExpandAll();
        }

        public static void CollapseAll(OutlookGrid outlookGrid1)
        {
            outlookGrid1.CollapseAll();
        }

        public static void RegisterLinkEvent(OutlookGrid outlookGrid1, string columnKey, CallbackArgs callback)
        {
            AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        private static void AddNewEvenHandler(OutlookGrid outlookGrid1, string columnKey, CallbackArgs callback)
        {
            Dictionary<string, CallbackArgs> callbacks = outlookGrid1.Tag as Dictionary<string, CallbackArgs>;
            if (callbacks != null)
            {
                if (!callbacks.ContainsKey(columnKey))
                {
                    callbacks.Add(columnKey, callback);
                    //outlookGrid1.Tag = callbacks;
                }
                else
                {
                    callbacks[columnKey] = callback;
                }
            }
            else
            {
                outlookGrid1.CellContentClick += OutlookGrid1_CellContentClick;
                Dictionary<string, CallbackArgs> callbackss = new Dictionary<string, CallbackArgs>();
                callbackss.Add(columnKey, callback);
                outlookGrid1.Tag = callbackss;
            }
        }

        private static void OutlookGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewLinkColumn || dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)//如果是链接列或者按钮列被点击
            {
                string columnKey = dgv.Columns[e.ColumnIndex].Name;
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                if (dgv.Columns[e.ColumnIndex].HeaderText == "确认报警")
                {
                    if (row.Cells[5].Value != null && row.Cells[5].Value.ToString() == "已确认")
                    {
                        return;
                    }
                }
                if (dgv.Columns[e.ColumnIndex].HeaderText == "处理报警")
                {
                    if (row.Cells[5].Value != null && row.Cells[5].Value.ToString() == "已处理")
                    {
                        return;
                    }
                }
                Dictionary<string, CallbackArgs> callbacks = dgv.Tag as Dictionary<string, CallbackArgs>;
                if (callbacks != null && callbacks.ContainsKey(columnKey))
                {
                    CallbackArgs callback = callbacks[columnKey];
                    if (callback != null)
                    {
                        List<object> args = new List<object>();
                        Dictionary<string, object> sysArgList = new Dictionary<string, object>();//保存本系统可能用到的所有参数类型
                        //...Start...
                        sysArgList.Add("{OWNER}", Index.Instance);
                        sysArgList.Add("{TARGET}", callback.Target);
                        sysArgList.Add("{USERNAME}", Index.User.Login_name);
                        sysArgList.Add("{PWD}", Index.User.Password);
                        sysArgList.Add("{ID}", row.Cells[0].Value);
                        DateTime now = DateTime.Now;
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
                        DateTime start = now.AddDays(-last);
                        sysArgList.Add("{STARTTIME}", start.ToString());
                        sysArgList.Add("{ENDTIME}", now.ToString());
                        sysArgList.Add("{FLASHTIME}", row.Cells[1].Value);
                        sysArgList.Add("{DEVICENAME}", row.Cells[2].Value);
                        sysArgList.Add("{TRAINNO}", row.Cells[3].Value);
                        //...End...

                        object target;
                        Common.GetParameter(ref sysArgList, out target);
                        if (callback.Parameters != null)
                        {
                            foreach (object o in callback.Parameters)
                            {
                                if (o != null)
                                {
                                    string arg = o.ToString();
                                    if (arg.StartsWith("$") && arg.EndsWith("$"))
                                    {
                                        List<object> internalArgs = new List<object>();
                                        string formName = arg.Substring(1, arg.Length - 2);
                                        int ind = arg.IndexOf('|');//$SdgReport|{OWNER},{ID}$
                                        if (ind > 1)
                                        {
                                            formName = arg.Substring(1, ind - 1);
                                        }
                                        string para = arg.Substring(ind + 1, arg.Length - ind - 1);
                                        string[] paras = para.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (paras.Length > 0)
                                        {
                                            foreach (string p in paras)
                                            {
                                                if (sysArgList.ContainsKey(p))
                                                {
                                                    internalArgs.Add(sysArgList[p]);
                                                }
                                            }
                                        }
                                        Form form = Common.GetForm(formName, target, internalArgs.ToArray());
                                        args.Add(form);
                                    }
                                    else
                                    {
                                        args.Add(o);
                                    }
                                }
                            }
                        }
                        callback.CallbackHandler.Method.Invoke(callback.Target, args.ToArray());
                    }
                }
            }
        }

        public static void RegisterLinkEvent(DataGridView outlookGrid1, string columnKey, CallbackArgs callback)
        {
            AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        private static void AddNewEvenHandler(DataGridView outlookGrid1, string columnKey, CallbackArgs callback)
        {
            Dictionary<string, CallbackArgs> callbacks = outlookGrid1.Tag as Dictionary<string, CallbackArgs>;
            if (callbacks != null)
            {
                if (!callbacks.ContainsKey(columnKey))
                {
                    callbacks.Add(columnKey, callback);
                    //outlookGrid1.Tag = callbacks;
                }
                else
                {
                    callbacks[columnKey] = callback;
                }
            }
            else
            {
                outlookGrid1.CellContentClick += OutlookGrid1_CellContentClick;
                Dictionary<string, CallbackArgs> callbackss = new Dictionary<string, CallbackArgs>();
                callbackss.Add(columnKey, callback);
                outlookGrid1.Tag = callbackss;
            }
        }

        public static void ClearData(DataGridView outlookGrid1)
        {
            outlookGrid1.DataSource = null;
        }

        public static void BindData(DataGridView outlookGrid1, DataTable dt, ListSortDirection direction = ListSortDirection.Ascending, int sortColumnIndex = -1)
        {
            outlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            outlookGrid1.ReadOnly = true;
            outlookGrid1.AllowUserToAddRows = false;
            outlookGrid1.DataSource = dt;
            if (sortColumnIndex > -1 && sortColumnIndex < outlookGrid1.Columns.Count)
            {
                DataGridViewColumn dgvc = outlookGrid1.Columns[sortColumnIndex];
                outlookGrid1.Sort(dgvc, direction);
            }
        }

        public static void AddLinkColumn(DataGridView outlookGrid1, string text, out string columnKey, CallbackArgs callback = null)
        {
            columnKey = "#Link#";
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Link#" + DateTime.Now.Ticks.ToString().Substring(4);
            link.Name = columnKey;
            //link.Text = text;
            link.HeaderText = text;
            link.UseColumnTextForLinkValue = true;
            link.TrackVisitedState = true;
            outlookGrid1.Columns.Add(link);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddLinkColumn(DataGridView outlookGrid1, string text, string columnKey = "#LinkSpecial#", CallbackArgs callback = null)
        {
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Link#" + DateTime.Now.Ticks.ToString().Substring(4);
            link.Name = columnKey;
            //link.Text = text;
            link.HeaderText = text;
            link.UseColumnTextForLinkValue = true;
            link.TrackVisitedState = true;
            outlookGrid1.Columns.Add(link);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddButtonColumn(DataGridView outlookGrid1, string text, out string columnKey, CallbackArgs callback = null)
        {
            columnKey = "#Button#";
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Button#" + DateTime.Now.Ticks.ToString().Substring(4);
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Name = columnKey;
            //button.Text = text;
            button.HeaderText = text;
            outlookGrid1.Columns.Add(button);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }

        public static void AddButtonColumn(DataGridView outlookGrid1, string text, string columnKey = "#ButtonSpecial#", CallbackArgs callback = null)
        {
            if (outlookGrid1.Columns.Contains(columnKey))
                columnKey = "#Button#" + DateTime.Now.Ticks.ToString().Substring(4);
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Name = columnKey;
            //button.Text = text;
            button.HeaderText = text;
            outlookGrid1.Columns.Add(button);
            if (callback != null)
                AddNewEvenHandler(outlookGrid1, columnKey, callback);
        }
    }

    public class CallbackArgs
    {
        object target;
        object[] parameters;
        Delegate callback;

        public object Target
        {
            get
            {
                return target;
            }
        }
        /// <summary>
        /// 委托方法的参数列表
        /// </summary>
        public object[] Parameters
        {
            get
            {
                return parameters;
            }
        }

        public Delegate CallbackHandler
        {
            get
            {
                return callback;
            }

            set
            {
                callback = value;
            }
        }

        public CallbackArgs(Delegate callback, object target, object[] parameters)
        {
            this.callback = callback;
            this.target = target;
            this.parameters = parameters;
        }
    }
}
