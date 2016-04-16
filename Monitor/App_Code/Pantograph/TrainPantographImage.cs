using System;

namespace Monitor.App_Code
{
    public class TrainPantographImage
    {
        private int pantographID;
        private string imageFilePath;
        private string imageFileName;
        private DateTime imageDateTime;

        public TrainPantographImage() { }

        /// <summary>
        /// 图片ID号
        /// </summary>
        public int PantographID
        {
            get { return this.pantographID; }
            set { this.pantographID = value; }
        }

        /// <summary>
        /// 图片文件绝对路径
        /// </summary>
        public string ImageFilePath
        {
            get { return this.imageFilePath; }
            set { this.imageFilePath = value; }
        }

        /// <summary>
        /// 图片文件相对路径，显示的图片
        /// </summary>
        public string ImageFileName
        {
            get { return this.imageFileName; }
            set { this.imageFileName = value; }
        }
        /// <summary>
        /// 图片添加的时间
        /// </summary>
        public DateTime ImageDateTime
        {
            get { return this.imageDateTime; }
            set { this.imageDateTime = value; }
        }

	}
}
