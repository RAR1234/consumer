using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TRAWebApplication.ConfigSetting
{
    public class Exceptions
    {

        public static void exceptionHandlers(string msg)
        {

            try
            {
                string path = "~/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
                {

                    File.Create(HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(path)))
                {
                    sw.WriteLine("\r\nLog Entry :");
                    sw.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in : " + HttpContext.Current.Request.Url.ToString() + ". \n\nError message:" + msg;
                    sw.WriteLine(err);
                    sw.WriteLine("=============================================================================================");
                    sw.Flush();
                    sw.Close();


                }
            }
            catch
            {

                throw;
            }

        }

        public static void exceptionHandler(string message)
        {
            System.IO.FileStream fileStream = null;
            System.IO.StreamWriter streamWriter = null;

            try
            {
                string logFilePath = System.Configuration.ConfigurationManager.AppSettings["exceptionPath"].ToString();

                logFilePath = logFilePath + "Exception" + "-" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";


                if (logFilePath.Equals("")) return;
                #region Create the Log file directory if it does not exists
                System.IO.DirectoryInfo logDirInfo = null;
                System.IO.FileInfo logFileInfo = new System.IO.FileInfo(logFilePath);
                logDirInfo = new System.IO.DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                #endregion Create the Log file directory if it does not exists


                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();

                }
                else
                {
                    fileStream = new System.IO.FileStream(logFilePath, System.IO.FileMode.Append);
                }
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(DateTime.Now.ToString());

                sb.AppendLine(message);
                sb.AppendLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                streamWriter = new System.IO.StreamWriter(fileStream);
                streamWriter.Write(sb.ToString());

                streamWriter.Flush();
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }

       
    }
}