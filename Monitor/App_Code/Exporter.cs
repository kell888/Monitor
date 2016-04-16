using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;

namespace Monitor.App_Code
{
    /// <summary>
    ///Exporter 的摘要说明
    /// </summary>
    public static class Exporter
    {
        public static bool ExportToExcel(DataTable dt, String exportName)
        {
            FileInfo fi = null;
            if (dt != null)
            {
                dt.TableName = exportName;
                fi = CreateExcel(dt);//在服务器上生成Excel文件
                if (fi != null && fi.Exists)
                {
                    try
                    {
                        Process.Start(fi.FullName);
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("创建Excel文件成功，但是打开出错：" + e.Message);
                    }
                }
                else
                {

                }
            }
            return false;
        }

        public static FileInfo CreateExcel(DataTable dt)
        {
            if (dt.Columns.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    string FileName = "Report";
                    if (!string.IsNullOrEmpty(dt.TableName))
                        FileName = dt.TableName;
                    string reportDir = Directory.GetCurrentDirectory() + "\\Reports";
                    if (!Directory.Exists(reportDir))
                        Directory.CreateDirectory(reportDir);
                    string file = reportDir + "\\" + FileName + ".xls";
                    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=Excel 8.0";
                    OleDbConnection myConn = new OleDbConnection(strCon);
                    try
                    {
                        string cols = "";
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (cols == "")
                                cols = "[" + dt.Columns[i].ColumnName + "] Text";
                            else
                                cols += ",[" + dt.Columns[i].ColumnName + "] Text";
                        }
                        OleDbCommand com = new OleDbCommand();
                        com.Connection = myConn;
                        myConn.Open();
                        com.CommandText = "CREATE TABLE [Report](" + cols + ")";
                        com.ExecuteNonQuery();
                        string strCom = "SELECT * FROM [Report$]";
                        OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                        System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                        //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。   
                        //获取insert语句中保留字符（起始位置）  
                        builder.QuotePrefix = "[";
                        //获取insert语句中保留字符（结束位置）   
                        builder.QuoteSuffix = "]";
                        //获得表结构
                        DataTable ndt = dt.Clone();
                        //清空数据
                        //ndt.Rows.Clear();
                        ndt.TableName = dt.TableName;
                        //myCommand.Fill(newds, TableName);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //在这里不能使用ImportRow方法将一行导入到news中，
                            //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                            //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added     
                            DataRow row = ndt.NewRow();
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                row[j] = dt.Rows[i][j];
                            }
                            ndt.Rows.Add(row);
                        }
                        //插入数据
                        myCommand.Update(ndt);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("创建Excel文件失败：" + e.Message);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                    if (File.Exists(file))
                        return new FileInfo(file);
                }
                else
                {
                    MessageBox.Show("数据源无任何数据行！");
                }
            }
            else
            {
                MessageBox.Show("数据源无架构信息！");
            }
            return null;
        }

        //用法：ToExcel(datagrid1);

        ///// <summary>
        ///// dv为要输出到Excel的数据,str为标题名称 
        ///// </summary>
        ///// <param name="dv"></param>
        ///// <param name="str"></param>
        //public static void OutputExcel(DataView dv,string str)
        //{
        //   GC.Collect();
        //   Application excel;// = new Application();
        //   int rowIndex=4;
        //   int colIndex=1;
        //   _Workbook xBk;
        //   _Worksheet xSt;
        //   excel= new ApplicationClass();
        //   xBk = excel.Workbooks.Add(true);
        //   xSt = (_Worksheet)xBk.ActiveSheet;
        //   //
        //   //取得标题
        //   //
        //   foreach(DataColumn col in dv.Table.Columns)
        //    {
        //    colIndex++;
        //    excel.Cells[4,colIndex] = col.ColumnName;
        //    xSt.get_Range(excel.Cells[4,colIndex],excel.Cells[4,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置标题格式为居中对齐
        //   }
        //   //
        //   //取得表格中的数据
        //   //
        //   foreach(DataRowView row in dv)
        //    {
        //    rowIndex ++;
        //    colIndex = 1;
        //    foreach(DataColumn col in dv.Table.Columns)
        //     {
        //     colIndex ++;
        //     if(col.DataType == System.Type.GetType("System.DateTime"))
        //      {
        //      excel.Cells[rowIndex,colIndex] = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
        //      xSt.get_Range(excel.Cells[rowIndex,colIndex],excel.Cells[rowIndex,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置日期型的字段格式为居中对齐 
        //     }
        //     else
        //      if(col.DataType == System.Type.GetType("System.String"))
        //      {
        //      excel.Cells[rowIndex,colIndex] = "'"+row[col.ColumnName].ToString();
        //      xSt.get_Range(excel.Cells[rowIndex,colIndex],excel.Cells[rowIndex,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置字符型的字段格式为居中对齐 
        //     }
        //     else 
        //      {
        //      excel.Cells[rowIndex,colIndex] = row[col.ColumnName].ToString();
        //     }
        //    }
        //   }
        //   //
        //   //加载一个合计行 
        //   //
        //   int rowSum = rowIndex + 1;
        //   int colSum = 2;
        //   excel.Cells[rowSum,2] = "合计";
        //   xSt.get_Range(excel.Cells[rowSum,2],excel.Cells[rowSum,2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
        //   //
        //   //设置选中的部分的颜色 
        //   //
        //   xSt.get_Range(excel.Cells[rowSum,colSum],excel.Cells[rowSum,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[rowSum,colSum],excel.Cells[rowSum,colIndex]).Interior.ColorIndex = 19;//设置为浅黄色,共计有56种 
        //   //
        //   //取得整个报表的标题 
        //   //
        //   excel.Cells[2,2] = str;
        //   //
        //   //设置整个报表的标题格式 
        //   //
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,2]).Font.Bold = true;
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,2]).Font.Size = 22;
        //   //
        //   //设置报表表格为最适应宽度 
        //   //
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Columns.AutoFit();
        //   //
        //   //设置整个报表的标题为跨列居中 
        //   //
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,colIndex]).HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;
        //   //
        //   //绘制边框 
        //   //
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Borders.LineStyle = 1;
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,2]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;//设置左边线加粗 
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[4,colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;//设置上边线加粗 
        //   xSt.get_Range(excel.Cells[4,colIndex],excel.Cells[rowSum,colIndex]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;//设置右边线加粗 
        //   xSt.get_Range(excel.Cells[rowSum,2],excel.Cells[rowSum,colIndex]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;//设置下边线加粗 
        //   //
        //   //显示效果 
        //   //
        //   excel.Visible=true;
        //   //xSt.Export(Server.MapPath(".")+""""+this.xlfile.Text+".xls",SheetExportActionEnum.ssExportActionNone,Microsoft.Office.Interop.OWC.SheetExportFormat.ssExportHTML);
        //   xBk.SaveCopyAs(HttpContext.Current.Server.MapPath(".")+""+str+".xls");
        //    ds = null;
        //            xBk.Close(false, null,null);
        //            excel.Quit();
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
        //            xBk = null;
        //            excel = null;
        //   xSt = null;
        //            GC.Collect();
    }
}