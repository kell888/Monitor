using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Monitor.App_Code
{
    /// <summary>
    ///PictureUtil 的摘要说明
    /// </summary>
    public class PictureUtil
    {
        public static string GetPicture(string src)
        {
            if (!File.Exists(src))
            {
                return "Images/wutu.gif";
            }
            return src;
        }
    }
}