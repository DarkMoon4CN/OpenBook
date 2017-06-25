using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Util;

namespace Log4Simple.Appender.RemoteAppender
{

    /// <summary>
    /// Implement this interface for your own strategies for printing log statements.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Implementors should consider extending the <see cref="RemoteAppenderSkeleton"/>
    /// class which provides a default implementation of this interface.
    /// </para>
    /// <para>
    /// Appenders can also implement the <see cref="IOptionHandler"/> interface. Therefore
    /// they would require that the <see cref="M:IOptionHandler.ActivateOptions()"/> method
    /// be called after the appenders properties have been configured.
    /// </para>
    /// </remarks>
    /// <author>liuww</author>    
    public abstract class RemoteAppenderSkeleton : IAppender, IBulkAppender, IOptionHandler, ILogWrapper,ILog
    {


        public RemoteAppenderSkeleton()
        {

            m_errorHandler = new OnlyOnceErrorHandler(this.GetType().Name);
        }

        public virtual void Close()
        {
            
        }

        virtual protected bool FilterEvent(LoggingData loggingEvent)
        {
            return loggingEvent.Level >= m_Threshold;
        }

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        /// <param name="loggingEvent">The event to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        public virtual void DoAppend(Core.LoggingData loggingEvent)
        {
            DoAppend(new LoggingData[] {loggingEvent });
        }

        /// <summary>
        /// Log the array of logging events in Appender specific way.
        /// </summary>
        /// <param name="loggingEvents">The events to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log an array of events into this appender.
        /// </para>
        /// </remarks>
        public abstract void DoAppend(LoggingData[] loggingEvents);

       

        /// <summary>
        /// object name
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 日志对象
        /// </summary>
        public ILog Log
        {
            get
            {
                return m_Log;
            }
            set
            {
                m_Log = value;
            }
        }

        /// <summary>
        /// Gets or sets the threshold <see cref="Level"/> of this appender.
        /// </summary>
        /// <value>
        /// The threshold <see cref="Level"/> of the appender. 
        /// </value>
        /// <remarks>
        /// <para>
        /// All log events with lower level than the threshold level are ignored 
        /// by the appender.
        /// </para>
        /// <para>
        /// In configuration files this option is specified by setting the
        /// value of the <see cref="Threshold"/> option to a level
        /// string, such as "DEBUG", "INFO" and so on.
        /// </para>
        /// </remarks>
        virtual public Level Threshold
        {
            set { m_Threshold = value; }
            get { return m_Threshold; }
        }

        /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This allows an object to defer activation of its options until all
        /// options have been set. This is required for components which have
        /// related options that remain ambiguous until all are set.
        /// </para>
        /// <para>
        /// If a component implements this interface then this method must be called
        /// after its properties have been set before the component can be used.
        /// </para>
        /// </remarks>
        public virtual void ActivateOptions()
        {
            
        }      


        /// <summary>
        /// object name
        /// </summary>
        private string m_Name;

        /// <summary>
        /// message Level
        /// </summary>
        private Level m_Threshold = Level.ALL;


        protected ILog m_Log;


        /// <summary>
        /// It is assumed and enforced that errorHandler is never null.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is assumed and enforced that errorHandler is never null.
        /// </para>
        /// <para>
        /// See <see cref="ErrorHandler"/> for more information.
        /// </para>
        /// </remarks>
        private IErrorHandler m_errorHandler;

        /// <summary>
        /// The fully qualified type of the AppenderSkeleton class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(AppenderSkeleton);

        #region 日志

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void Debug(string message)
        {
            Debug(null, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void Debug(Exception exception)
        {
            Debug(null, exception);
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void Debug(string moudleName, string message)
        {
            if (m_Log != null)
                m_Log.Debug(moudleName, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void Debug(string moudleName, Exception exception)
        {            
            Debug(moudleName, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void DebugFormat(string format, params object[] args)
        {
            Debug(null, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void DebugFormatEx(string moudleName, string format, params object[] args)
        {
            Debug(moudleName, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Debug"/> level.</overloads>
        public void Debug(LoggingData logEvent)
        {
            if (m_Log != null)
                m_Log.Debug(logEvent);
        }

        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        public void Info(string message)
        {
            Info(null, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        public void Info(Exception exception)
        {
            Info(null, exception.Message);
        }

        public void Info(string moudleName, string message)
        {
            if (m_Log != null)
                m_Log.Info(moudleName, message);
        }


        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        public void Info(string moudleName, Exception exception)
        {
            Info(moudleName, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        public void InfoFormat(string format, params object[] args)
        {
            Info(null, string.Format(format, args));

        }

        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        public void InfoFormatEx(string moudleName, string format, params object[] args)
        {
            Info(moudleName, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Info"/> level.</overloads>
        /// 
        public void Info(LoggingData logEvent)
        {
            if (m_Log != null)
                m_Log.Info(logEvent);
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void Warn(string message)
        {
            Warn(null, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void Warn(Exception exception)
        {
            Warn(null, exception);
        }


        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void Warn(string moudleName, string message)
        {
            if (m_Log != null)
                m_Log.Warn(moudleName, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void Warn(string moudleName, Exception exception)
        {
            Warn(moudleName, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void WarnFormat(string format, params object[] args)
        {
            Warn(null, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void WarnFormatEx(string moudleName, string format, params object[] args)
        {
            Warn(moudleName, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Warn"/> level.</overloads>
        public void Warn(LoggingData logEvent)
        {
            if (m_Log != null)
                m_Log.Warn(logEvent);
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void Error(string message)
        {
            Error(null, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void Error(Exception exception)
        {
            Error(null, exception);
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void Error(string moudleName, string message)
        {
            if (m_Log != null)
                m_Log.Error(moudleName, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void Error(string moudleName, Exception exception)
        {
            Error(moudleName, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void ErrorFormat(string format, params object[] args)
        {
            Error(null, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void ErrorFormatEx(string moudleName, string format, params object[] args)
        {
            Error(moudleName, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Error"/> level.</overloads>
        public void Error(LoggingData logEvent)
        {
            if (m_Log != null)
                m_Log.Error(logEvent);
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void Fatal(string message)
        {
            Fatal(null, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void Fatal(Exception exception)
        {
            Fatal(null, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void Fatal(string moudleName, string message)
        {
            if (m_Log != null)
                m_Log.Fatal(moudleName, message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void Fatal(string moudleName, Exception exception)
        {
            Fatal(moudleName, exception.Message);
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void FatalFormat(string format, params object[] args)
        {
            Fatal(null, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void FatalFormatEx(string moudleName, string format, params object[] args)
        {
            Fatal(moudleName, string.Format(format, args));
        }

        /// <overloads>Log a message object with the <see cref="Level.Fatal"/> level.</overloads>
        public void Fatal(LoggingData logEvent)
        {
            if (m_Log != null)
                m_Log.Fatal(logEvent);
        }

        public bool IsDebugEnabled
        {
            get { return m_Log == null ? false :m_Log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return m_Log == null ? false : m_Log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return m_Log == null ? false : m_Log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return m_Log == null ? false : m_Log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return m_Log == null ? false : m_Log.IsFatalEnabled; }
        }

        ILogger ILoggerWrapper.Logger
        {
            get { return m_Log == null ? null : m_Log.Logger; }
        }

        #endregion
    }
}
