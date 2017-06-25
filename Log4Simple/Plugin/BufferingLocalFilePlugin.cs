using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Util;
using System.Threading;
using Log4Simple.Appender;
using System.Collections;
using Log4Simple.Core;

namespace Log4Simple.Plugin
{
    /// <summary>
    /// 处理本发缓存日志插件
    /// </summary>
    public class BufferingLocalFilePlugin :PluginSkeleton
    {
       
        #region Public Instance Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="RemoteLoggingServerPlugin" /> class.
        /// </para>
        /// <para>
        /// The <see cref="SinkUri"/> property must be set.
        /// </para>
        /// </remarks>
        public BufferingLocalFilePlugin()
            : base("BufferingLocalFilePlugin:Local")
        {
        }
        
        #endregion


        /// <summary>
        /// 定时查找文件，发送至远程服务器
        /// </summary>
        private void SearchBufferingFile2Send()
        {
            while (!m_IsTermainated)
            {
                Thread.Sleep(1000);
                try
                {
                    if (DateTime.Now.Subtract(m_LastExecuteDt).TotalSeconds >= m_SearchInterval)
                    {
                        //查找文件
                        SearchBufferingFiles();
                        m_LastExecuteDt = DateTime.Now;
                    }
                   
                }
                catch(Exception ex)
                {
                    if (m_Log != null)
                        m_Log.Error(ex);
                }
            }
        }


        public void SearchBufferingFiles()
        {
            foreach (string mdir in m_LocalDirectorys)
            {
                FileInfo[] fis = new DirectoryInfo(mdir).GetFiles("*" + m_ExName);
                foreach (FileInfo fi in fis)
                {
                    if (m_IsTermainated)
                        return;
                    ProcessBufferingLocalFile(fi);
                }
            }
        }

        List<LoggingData> Bulks = new List<LoggingData>();


        private void DoAppend(bool isSend)
        {
            if (Bulks.Count >= m_BulkNum || isSend)
            {
                if (BulkAppender != null)
                {
                    BulkAppender.DoAppend(Bulks.ToArray());
                    Bulks.Clear();
                }
            }         
        }

        /// <summary>
        /// 读取文件，执行数据处理操作
        /// </summary>
        public void ProcessBufferingLocalFile(FileInfo fi)
        {
            StreamReader sreader = null;
            try
            {
                Bulks.Clear();
                sreader = new StreamReader(fi.FullName);
                while (!sreader.EndOfStream)
                {
                    if (m_IsTermainated)
                        return;
                    LoggingData le = string2LoggingEvent(sreader.ReadLine());
                    if (le != null)
                        Bulks.Add(le);
                    DoAppend(false);
                }
                DoAppend(true);

                sreader.Close();
                sreader.Dispose();
                sreader = null;

                fi.Delete();
            }
            finally
            {
                if (sreader != null)
                {
                    sreader.Close();
                    sreader.Dispose();
                }
            }                                      
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <returns></returns>
        private LoggingData string2LoggingEvent(string base64String)
        {
            try
            {
                return Util.FileSerializerByBase64.Deserialize<LoggingData>(base64String, true);
            }
            catch (Exception ex)
            {
                if (m_Log != null)
                    m_Log.Error("string2LoggingEvent: "  + ex.Message);
                return null;
            }
        }

      

        /// <summary>
        /// Is called when the plugin is to shutdown.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called to notify the plugin that 
        /// it should stop operating and should detach from
        /// the repository.
        /// </para>
        /// </remarks>
        public override void Shutdown()
        {

            m_IsTermainated = true;

            if (m_BulkAppender != null)
                m_BulkAppender.Close();

            base.Shutdown();
        }


        public override void ActivateOptions()
        {
            base.ActivateOptions();

            new Thread(new ThreadStart(SearchBufferingFile2Send)).Start();
            if (string.IsNullOrEmpty(m_LocalFileDirectory))
            {
                LogLog.Error(declaringType, "m_LocalFileDirectory 变量配置为空，服务没法上传日志");
            }
            else
            {
                m_LocalDirectorys = m_LocalFileDirectory.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < m_LocalDirectorys.Length; i++)
                {
                    m_LocalDirectorys[i] = SystemInfo.ConvertToFullPath(m_LocalDirectorys[i]);
                }
            }
            if (m_BulkAppender != null)
            {
                if (typeof(ILogWrapper).IsAssignableFrom(m_BulkAppender.GetType()))
                    ((ILogWrapper)m_BulkAppender).Log = m_Log;

            }
        }


        #region 变量定义
        /// <summary>
        /// the thread flag
        /// </summary>
        private bool m_IsTermainated = false;

        /// <summary>
        /// 本地文件目录
        /// </summary>
        private string m_LocalFileDirectory;// = SystemInfo.ApplicationBaseDirectory + "\\BufferLog\\";

        private string[] m_LocalDirectorys;

        /// <summary>
        /// 扫描时间间隔(秒)
        /// </summary>
        private int m_SearchInterval = 30;

        /// <summary>
        /// 上次执行时间
        /// </summary>
        private DateTime m_LastExecuteDt;

        /// <summary>
        /// 文件扩展名
        /// </summary>
        private string m_ExName = ".log";


        /// <summary>
        /// 日志处理
        /// </summary>
        private IBulkAppender m_BulkAppender;

        /// <summary>
        /// 批处理数量
        /// </summary>
        private int m_BulkNum = 20;

     

        /// <summary>
        /// The fully qualified type of the FileAppender class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(BufferingLocalFilePlugin);

        #endregion

        #region 对外属性

        /// <summary>
        /// 扫描时间间隔
        /// </summary>
        public int SearchInterval
        {
            get { return m_SearchInterval; }
            set { m_SearchInterval = value; }
        }

        public string LocalFileDirectory
        {
            //get { return m_LocalFileDirectory; }
            set { m_LocalFileDirectory = value; }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExName
        {
            get { return m_ExName; }
            set { m_ExName = value; }
        }

        /// <summary>
        /// 日志处理
        /// </summary>
        public IBulkAppender BulkAppender
        {
            get { return m_BulkAppender; }
            set { m_BulkAppender = value; }
        }


        public int BulkNum
        {
            get { return m_BulkNum; }
            set { m_BulkNum = value; }
        }

        #endregion

    }
}
