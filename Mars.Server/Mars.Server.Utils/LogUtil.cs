namespace Mars.Server.Utils
{
    using Log4Simple;
    using Log4Simple.Core;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    /// <summary>
    /// 日志记录类
    /// created by benny
    /// </summary>
    public class LogUtil
    {
        public static string EXStr = "::OP\r\nEX:{0}\r\nSource:{1}\r\n{2}\r\n{3}\r\n::ED";
        private static object[] Locker = new object[0];

        static ILog logobj = LogManager.GetLogger("Log4Smart");

        public static void WriteLog(Exception ex)
        {
            WriteLog(string.Format(EXStr, new object[] { ex.Message, ex.Source, ex.StackTrace, ex.TargetSite }), true);
            logobj.Fatal(ex);
        }

        public static void WriteLog(string Message)
        {
            WriteLog(Message, true);
            logobj.Info(Message);
        }

        public static void WriteLog(string Message, bool IsLogToDisk)
        {
            WriteLog(Message, IsLogToDisk, false);
        }

        public static void WriteLog(string Message, bool IsLogToDisk, bool IsEveryThreadNeedOne)
        {
            if (IsLogToDisk)
            {
                lock (Locker)
                {
                    StreamWriter writer = null;
                    StreamWriter writer2 = null;
                    try
                    {
                        string logFolder = AppPath.LogFolder;
                        if (!string.IsNullOrEmpty(logFolder))
                        {
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str2 = DateTime.Now.ToString("yyyyMMdd");
                            logFolder = Path.Combine(logFolder, str2);
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str3 = "sp" + str2 + "all.log";
                            writer = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                            writer.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                            writer.Flush();
                            if (IsEveryThreadNeedOne)
                            {
                                str3 = "cm.Thread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
                                writer2 = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                                writer2.WriteLine(string.Format("{0}=>{1}", DateTime.Now.ToString(), Message));
                                writer2.Flush();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Close();
                        }
                        if (writer2 != null)
                        {
                            writer2.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        public static void WriteTaskLog(string Message, string TaskName)
        {
            WriteTaskLog(Message, TaskName, true);
        }
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        /// <param name="IsLogToDisk"></param>
        public static void WriteTaskLog(string Message, string TaskName, bool IsLogToDisk)
        {
            WriteTaskLog(Message, TaskName, IsLogToDisk, false);
        }
        /// <summary>
        /// 记录任务执行时  日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="TaskName"></param>
        /// <param name="IsLogToDisk"></param>
        /// <param name="IsEveryThreadNeedOne"></param>
        public static void WriteTaskLog(string Message, string TaskName, bool IsLogToDisk, bool IsEveryThreadNeedOne)
        {
            if (IsLogToDisk)
            {
                lock (Locker)
                {
                    StreamWriter writer = null;
                    StreamWriter writer2 = null;
                    try
                    {
                        string logFolder = AppPath.LogFolder;
                        if (!string.IsNullOrEmpty(logFolder))
                        {
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str2 = DateTime.Now.ToString("yyyyMMdd");
                            logFolder = Path.Combine(logFolder, str2);
                            if (!Directory.Exists(logFolder))
                            {
                                Directory.CreateDirectory(logFolder);
                            }
                            string str3 = "t_" + TaskName + "_" + str2 + "all.log";
                            writer = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                            writer.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId, Message));
                            writer.Flush();
                            if (IsEveryThreadNeedOne)
                            {
                                str3 = "cm.Thread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
                                writer2 = new StreamWriter(Path.Combine(logFolder, str3), true, Encoding.Unicode);
                                writer2.WriteLine(string.Format("{0}=>{1}", DateTime.Now.ToString(), Message));
                                writer2.Flush();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Close();
                        }
                        if (writer2 != null)
                        {
                            writer2.Close();
                        }
                    }
                }
            }
        }

        public static void WriteRawLog(string msg)
        {
            logobj.Info(msg);
        }

        public static void WriteRawLog(Exception ex)
        {
            logobj.Error(ex);
        }

        public static void WriteLog(LogEntity log)
        {
            LoggingData logdata = new LoggingData();
            logdata.ExStringOne = log.LogTitle;
            logdata.Message = log.LogContent;
            logdata.ExIntTwo = log.LogTypeID;
            logdata.ExIntOne = log.UserID;
            logdata.ExStringTwo = log.LogMeta;
            logdata.ExStringThree = log.ExInfo;
            logobj.Info(logdata);

#if DEBUG
            //System.Diagnostics.Debug.WriteLine(LogEntity.ToJsonString(log));
#endif
        }

        public static void Debug(string msg)
        {
            logobj.Debug(msg);
        }

    }

    /// <summary>
    /// 日志记录扩展类 
    /// 日志写入配置文件指定路径中
    /// </summary>
    public class LogUtilExpand
    {
        static readonly string LogDirKey = "Mail_LogDir";      
        static object[] Locker = new object[0];
        public static string EXStr = "::OP\r\nEX:{0}\r\nSource:{1}\r\n{2}\r\n{3}\r\n::ED";

        /// <summary>
        /// 默认在调试模式下记录到控制台，磁盘，并分线程记载分日志；在Release下只记录到磁盘
        /// </summary>
        /// <param name="Message"></param>
        public static void WriteLog(string Message)
        {
            WriteLog(Message, false,true);
        }    

        public static void WriteLog(Exception ex)
        {
            WriteLog(string.Format(EXStr, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite), false);
        }

        public static void WriteLog(Exception ex, bool IsEveryThreadNeedOne, bool IsLogToDisk=true)
        {
            WriteLog(string.Format(EXStr, ex.Message, ex.Source, ex.StackTrace, ex.TargetSite), IsEveryThreadNeedOne, IsLogToDisk);
        }           

        public static void WriteLog(string Message, bool IsEveryThreadNeedOne, bool IsLogToDisk=true)
        {
#if DEBUG
            Debug.WriteLine(Message);
#endif
            if (IsLogToDisk)
            {
                lock (Locker)
                {
                    System.IO.StreamWriter sw = null;
                    System.IO.StreamWriter sw_thread = null;
                    try
                    {
                        string path = ConfigurationManager.AppSettings[LogDirKey];
                        if (string.IsNullOrEmpty(path) == true)
                        {
                            return;
                        }
                        if (Directory.Exists(path) == false)
                            Directory.CreateDirectory(path);
                        string dir = DateTime.Now.ToString("yyyyMM");
                        string date = DateTime.Now.ToString("yyyyMMdd");
                        path = Path.Combine(path, dir, date);
                        if (Directory.Exists(path) == false)
                            Directory.CreateDirectory(path);

                        string file = "MarsEmailServiceLog.log";
                        sw = new StreamWriter(Path.Combine(path, file), true, Encoding.Unicode);
                        sw.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name:Thread.CurrentThread.ManagedThreadId.ToString(), Message));
                        sw.Flush();
                        if (IsEveryThreadNeedOne)
                        {
                            file = "MarsEmailServiceThread" + Thread.CurrentThread.ManagedThreadId.ToString() + ".log";
                            sw_thread = new StreamWriter(Path.Combine(path, file), true, Encoding.Unicode);
                            sw_thread.WriteLine(string.Format("{0}[{1}]=>{2}", DateTime.Now.ToString(), Thread.CurrentThread.Name != null ? Thread.CurrentThread.Name : Thread.CurrentThread.ManagedThreadId.ToString(), Message));
                            sw_thread.Flush();
                        }
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        Debug.WriteLine(e);
#endif
                    }
                    finally
                    {
                        if (sw != null)
                            sw.Close();
                        if (sw_thread != null)
                            sw_thread.Close();
                    }
                }
            }
        }
    }

    [Serializable]
    public class LogEntity
    {
        private int _LogID;
        public int LogID
        {
            get { return _LogID; }
            set { _LogID = value; }
        }
        private int _UserID;

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private string _LogTitle;

        public string LogTitle
        {
            get { return _LogTitle; }
            set { _LogTitle = value; }
        }
        private string _LogContent;

        public string LogContent
        {
            get { return _LogContent; }
            set { _LogContent = value; }
        }

        private DateTime _LogTime = DateTime.Now;
        public DateTime LogTime
        {
            get { return _LogTime; }
            set { _LogTime = value; }
        }

        private int _LogTypeID;

        public int LogTypeID
        {
            get { return _LogTypeID; }
            set { _LogTypeID = value; }
        }
        private string _LogMeta;

        public string LogMeta
        {
            get { return _LogMeta; }
            set { _LogMeta = value; }
        }
        private string _ExInfo;

        public string ExInfo
        {
            get { return _ExInfo; }
            set { _ExInfo = value; }
        }

        private Guid _UnqiueID;

        public Guid UnqiueID
        {
            get { return _UnqiueID; }
            set { _UnqiueID = value; }
        }

        public static string ToJsonString(LogEntity obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static LogEntity Parse(string jsonstring)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LogEntity>(jsonstring);
        }
    }
}

