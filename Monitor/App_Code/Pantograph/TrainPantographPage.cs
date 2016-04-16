using System;
using System.Data;
using System.Collections.Generic;

namespace Monitor.App_Code
{
    public class TrainPantographPage
    {
        private DataTable table;        //第几页的数据
        private int currentPage;        //当前是哪页
        private int countPage;           //总数页  
        private string currentUrlPath;  //当前网页路径


        #region 外部访问内部属性
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get { return this.currentPage; }
            set { this.currentPage = value; }
        }
        /// <summary>
        /// 当前总页数
        /// </summary>
        public int CountPage
        {
            get { return this.countPage; }
        }
        /// <summary>
        /// 当前网页路径
        /// </summary>
        public string CurrentUrlPath
        {
            get { return this.currentUrlPath; }
            set { this.currentUrlPath = value; }
        }

        #endregion

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public TrainPantographPage() 
        {
            currentPage = 1;
            countPage = 1;

            table = InformationAccess.GetTrainPantographOnImage(currentPage.ToString(), out countPage);
        }


        /// <summary>
        /// 获取最后一个图片，是排序过的。
        /// </summary>
        /// <returns></returns>
        public string GetMaxImagePath()
        {
            return table.Rows[0]["ImageFileName"].ToString();
        }

        /// <summary>
        /// 返回页数的信息
        /// </summary>
        /// <returns></returns>
        public string GetShowStringFromToCountPage()
        {
            return "第" + this.currentPage.ToString() + "页共" + this.countPage.ToString() + "页";
        }

        public string FirstUrlLink()
        {
            return this.currentUrlPath + (1).ToString();
        }

        public string PreviousLink()
        {
            return this.currentUrlPath + (this.currentPage - 1).ToString();
        }

        public string NextLink()
        {
            return this.currentUrlPath + (this.currentPage + 1).ToString();
        }

        public string LastLink()
        {
            return this.currentUrlPath + (this.countPage).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TrainPantographImage> GetTxtFileAllInfo()
        {
            DataTable data = GetDataTable(countPage.ToString(), out countPage);
            List<TrainPantographImage> imageList = null;

            if (data.Rows.Count >0)
            {
                imageList = new List<TrainPantographImage>();

                TrainPantographImage trainimage = new TrainPantographImage();

                for (int i = 0; i < data.Rows.Count;i++ )
                {
                    trainimage.PantographID = Convert.ToInt32(data.Rows[i]["PantographID"]);
                    trainimage.ImageFilePath = data.Rows[i]["ImageFilePath"].ToString();
                    trainimage.ImageFileName = data.Rows[i]["ImageFileName"].ToString();
                    trainimage.ImageDateTime = Convert.ToDateTime(data.Rows[i]["ImageDatetime"].ToString());
                }

                imageList.Add(trainimage);
            }

            return imageList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            table = InformationAccess.GetTrainPantographOnImage(currentPage.ToString(), out countPage);
            return table;
        }

        #region 内部函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="countPage"></param>
        /// <returns></returns>
        private DataTable GetDataTable(string currentPage, out int countPage)
        {
            return InformationAccess.GetTrainPantographOnImage(currentPage.ToString(), out countPage);
        }
        #endregion

    }
}
