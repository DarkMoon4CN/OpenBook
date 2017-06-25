using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;
using Log4Simple.Core;
using System.IO;
using System.Threading;

namespace Log4Simple.Appender
{
    /// <summary>
    /// buffer LoggingEventData[] 2 Local File
    /// </summary>
    public class BufferingAppender2LocalFile :BufferingAppenderSkeleton
    {

        protected override void OnClose()
        {
            m_IsTermainated = true;

            base.OnClose();
        }

        public override void ActivateOptions()
        {
            if (SecurityContext == null)
            {
                SecurityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            if (m_LogStayTimeOut > 0)
                new Thread(CheckLoggingEventTimeOut).Start();

            base.ActivateOptions();
        }


        private void CheckLoggingEventTimeOut()
        {
            while (!m_IsTermainated)
            {
                try
                {
                    if (DateTime.Now.Subtract(m_LastCheckDt).TotalSeconds >= m_SpaceCheck)
                    {
                        if (m_cb.Length > 0 && DateTime.Now.Subtract(m_LastAppendLoggingEventDt).TotalSeconds >= m_LogStayTimeOut)
                        {
                            Flush();
                        }

                        m_LastCheckDt = DateTime.Now;
                    }
                }
                catch {
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 序列化到本地文件
        /// </summary>
        /// <param name="events"></param>
        protected override void SendBuffer(LoggingData[] events)
        {
            string filename = SystemInfo.ConvertToFullPath(m_FileSaveDirectory + this.m_Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".temp");
            SaveBuffer2File(filename, events);
            using (SecurityContext.Impersonate(this))
            {
                File.Move(filename, filename.Replace(Path.GetExtension(filename), m_ExName));
            }
        }        

        /// <summary>
        /// 保存缓存到本地文件
        /// </summary>
        /// <param name="filename"></param>
        private void SaveBuffer2File(string filename, LoggingData[] events)
        {
            using (SecurityContext.Impersonate(this))
            {
                using (var sw = new StreamWriter(filename))
                {
                    sw.Write(Util.FileSerializerByBase64.Serializer(events));
                    sw.Flush();
                    sw.Close();
                }
            }
        }




        #region 字段定义

        /// <summary>
        /// the thread flag
        /// </summary>
        private bool m_IsTermainated = false;

        private DateTime m_LastCheckDt;
        private int m_SpaceCheck = 30;

        /// <summary>
        /// 文件保存目录
        /// </summary>
        private string m_FileSaveDirectory = SystemInfo.ApplicationBaseDirectory + "\\BufferLog\\";

        /// <summary>
        /// 文件扩展名
        /// </summary>
        private string m_ExName = ".log";

        /// <summary>
        /// 是否启动自动检查
        /// </summary>
        private int m_LogStayTimeOut = 30;

        #endregion


        #region 属性定义

        /// <summary>
        /// 文件保存目录
        /// </summary>
        public string FileSaveDirectory
        {
            get { return m_FileSaveDirectory; }
            set { m_FileSaveDirectory = value; }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExName
        {
            get { return m_ExName; }
            set { m_ExName = value; }
        }


        public int LogStayTimeOut
        {
            get { return m_LogStayTimeOut; }
            set { m_LogStayTimeOut = value; }
        }
        #endregion

    }
}
