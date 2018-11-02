using System;
using System.IO;

using BrnMall.Core;

namespace BrnMall.LogStrategy.Txt
{
    /// <summary>
    /// 基于txt文件的日志策略
    /// </summary>
    public partial class LogStrategy : ILogStrategy
    {
        private static object _locker = new object();//锁对象

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Write(string message)
        {
            lock (_locker)
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    string fileName = IOHelper.GetMapPath("/App_Data/txtlogs/") + DateTime.Now.ToString("yyyyMMdd") + ".log";

                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    if (!fileInfo.Exists)
                    {
                        fileInfo.Create().Close();
                    }
                    else if (fileInfo.Length > 2048 * 1000)
                    {
                        fileInfo.Delete();
                    }

                    fs = fileInfo.OpenWrite();
                    sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.Write("Log Entry : ");
                    sw.Write("{0}", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
                    sw.Write(Environment.NewLine);
                    sw.Write(message);
                    sw.Write(Environment.NewLine);
                    sw.Write("------------------------------------");
                    sw.Write(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }

        }
    }
}
