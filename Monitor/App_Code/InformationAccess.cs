using System;
using System.Data;
using System.Data.Common;

namespace Monitor.App_Code
{
    /// <summary>
    /// InformationAccess 的摘要说明
    /// </summary>
    public static class InformationAccess
    {
        //public InformationAccess()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //}
        public static DataTable GetDepartments()
        {
            DbCommand comm = GenericDataAccess.CreateCommand();
            comm.CommandText = "table1";
            return GenericDataAccess.ExecuteSelectCommand(comm);
        }
        public static DataTable PictureList()
        {
            DbCommand comm = GenericDataAccess.CreateCommand();
            comm.CommandText = "PictureList";
            return GenericDataAccess.ExecuteSelectCommand(comm);
        }
        public static DataTable GetTrainPantographOnImage(string pageNumber, out int howManyPages)
        {
            DbCommand comm = GenericDataAccess.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "GetTrainPantographOnImage";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@PageNumber";
            param.Value = pageNumber;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            param = comm.CreateParameter();
            param.ParameterName = "@ProductsPerPage";
            param.Value = 6;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            param = comm.CreateParameter();
            param.ParameterName = "@HowManyProducts";
            param.Direction = ParameterDirection.Output;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);

            DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);

            int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
            howManyPages = (int)Math.Ceiling((double)howManyProducts / 6);

            return table;

        }
    }
}