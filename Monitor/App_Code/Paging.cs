using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Monitor.App_Code
{
    public static class Paging
    {
        public static int GetPageCount(int record_count, int page_size)
        {
            int pagecount = 0;
            if (record_count % page_size == 0)
                pagecount = record_count / page_size;
            else
                pagecount = (record_count / page_size) + 1;

            return pagecount;
        }

        public static DataView GetPagerForView(DataTable dt, int page_size, int page_index, out int page_count, out string msg)
        {
            page_index = page_index - 1;
            DataView dv = new DataView();
            page_count = 0;
            if (dt != null)
            {
                int recordCount = dt.Rows.Count; //总记录数
                int page_sum = GetPageCount(recordCount, page_size);
                if (page_size < dt.Rows.Count)//kl2 :SQL查询函数返回的DATASET   
                {
                    if (page_size == 0)//text_intpase :判断用户设置的分页是否合法
                        page_size = 10;
                    //recordCount = kl2.Tables[0].Rows.Count;//假设每页只显示1条数据,则共可以显示的页数:pagemark页
                    if (page_size < 1)
                    {
                        msg = "请将page_size设置在[1-" + dt.Rows.Count.ToString() + "]之间";
                    }
                    msg = "共" + page_sum.ToString() + "页," + dt.Rows.Count.ToString() + "条";//page_num :lable
                    DataTable page_table = new DataTable();//记录当前正在操作的是哪个表,全局变量,值由查询函数获取
                    page_table.TableName = dt.TableName;
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        page_table.Columns.Add(dt.Columns[k].ColumnName);
                    }
                    if (dt.Rows.Count != 0 && page_size < dt.Rows.Count)
                    {
                        page_table.Clear();
                        try    //普通页面显示
                        {
                            page_table.Clear();
                            for (int i = 0; i < page_size; i++)
                            {
                                page_table.Rows.Add(dt.Rows[i + (page_index * page_size)].ItemArray);
                            }
                        }
                        catch //最后不足一个页面的显示
                        {
                            page_table.Clear();
                            try
                            {
                                for (int s = 0; s < recordCount - (page_index * page_size); s++)
                                {
                                    page_table.Rows.Add(dt.Rows[s + (page_index * page_size)].ItemArray);
                                }
                            }
                            catch { }
                        }
                        msg += "　当前第" + (page_index + 1).ToString() + "页";
                    }
                    dv = page_table.DefaultView;
                }
                else
                {
                    dv = dt.DefaultView;
                    msg = "共1页," + dt.Rows.Count.ToString() + "条";
                    msg += "　当前第" + (page_index + 1).ToString() + "页";
                }
                page_count = page_sum;
                return dv;
            }
            else
            {
                msg = "没有数据！";
                return null;
            }
        }
    }
}