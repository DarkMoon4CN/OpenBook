using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Appender;
using Log4Simple.Util;
using System.Collections;

namespace Log4Simple.Repository.Hierarchy
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> used by <see cref="Hierarchy"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Internal class used to provide implementation of <see cref="ILogger"/>
    /// interface. Applications should use <see cref="LogManager"/> to get
    /// logger instances.
    /// </para>
    /// <para>
    /// This is one of the central classes in the log4net implementation. One of the
    /// distinctive features of log4net are hierarchical loggers and their
    /// evaluation. The <see cref="Hierarchy"/> organizes the <see cref="Logger"/>
    /// instances into a rooted tree hierarchy.
    /// </para>
    /// <para>
    /// The <see cref="Logger"/> class is abstract. Only concrete subclasses of
    /// <see cref="Logger"/> can be created. The <see cref="ILoggerFactory"/>
    /// is used to create instances of this type for the <see cref="Hierarchy"/>.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    /// <author>Aspi Havewala</author>
    /// <author>Douglas de la Torre</author>
    public abstract class Logger : IAppenderAttachable, ILogger
    {
        #region Protected Instance Constructors

        /// <summary>
        /// This constructor created a new <see cref="Logger" /> instance and
        /// sets its name.
        /// </summary>
        /// <param name="name">The name of the <see cref="Logger" />.</param>
        /// <remarks>
        /// <para>
        /// This constructor is protected and designed to be used by
        /// a subclass that is not abstract.
        /// </para>
        /// <para>
        /// Loggers are constructed by <see cref="ILoggerFactory"/> 
        /// objects. See <see cref="DefaultLoggerFactory"/> for the default
        /// logger creator.
        /// </para>
        /// </remarks>
        protected Logger(string name)
        {
            m_name = string.Intern(name);
        }

        #endregion Protected Instance Constructors

        #region Public Instance Properties
       
              

        /// <summary>
        /// Gets or sets the <see cref="Hierarchy"/> where this 
        /// <c>Logger</c> instance is attached to.
        /// </summary>
        /// <value>The hierarchy that this logger belongs to.</value>
        /// <remarks>
        /// <para>
        /// This logger must be attached to a single <see cref="Hierarchy"/>.
        /// </para>
        /// </remarks>
        virtual public Hierarchy Hierarchy
        {
            get { return m_hierarchy; }
            set { m_hierarchy = value; }
        }

        /// <summary>
        /// Gets or sets the assigned <see cref="Level"/>, if any, for this Logger.  
        /// </summary>
        /// <value>
        /// The <see cref="Level"/> of this logger.
        /// </value>
        /// <remarks>
        /// <para>
        /// The assigned <see cref="Level"/> can be <c>null</c>.
        /// </para>
        /// </remarks>
        virtual public Level Level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        #endregion Public Instance Properties

        #region Implementation of IAppenderAttachable

        /// <summary>
        /// Add <paramref name="newAppender"/> to the list of appenders of this
        /// Logger instance.
        /// </summary>
        /// <param name="newAppender">An appender to add to this logger</param>
        /// <remarks>
        /// <para>
        /// Add <paramref name="newAppender"/> to the list of appenders of this
        /// Logger instance.
        /// </para>
        /// <para>
        /// If <paramref name="newAppender"/> is already in the list of
        /// appenders, then it won't be added again.
        /// </para>
        /// </remarks>
        virtual public void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }

            m_appenderLock.AcquireWriterLock();
            try
            {
                if (m_appenderAttachedImpl == null)
                {
                    m_appenderAttachedImpl = new Log4Simple.Util.AppenderAttachedImpl();
                }
                m_appenderAttachedImpl.AddAppender(newAppender);
            }
            finally
            {
                m_appenderLock.ReleaseWriterLock();
            }
        }

       

        /// <summary>
        /// Look for the appender named as <c>name</c>
        /// </summary>
        /// <param name="name">The name of the appender to lookup</param>
        /// <returns>The appender with the name specified, or <c>null</c>.</returns>
        /// <remarks>
        /// <para>
        /// Returns the named appender, or null if the appender is not found.
        /// </para>
        /// </remarks>
        virtual public IAppender GetAppender(string name)
        {
            m_appenderLock.AcquireReaderLock();
            try
            {
                if (m_appenderAttachedImpl == null || name == null)
                {
                    return null;
                }

                return m_appenderAttachedImpl.GetAppender(name);
            }
            finally
            {
                m_appenderLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Remove all previously added appenders from this Logger instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Remove all previously added appenders from this Logger instance.
        /// </para>
        /// <para>
        /// This is useful when re-reading configuration information.
        /// </para>
        /// </remarks>
        virtual public void RemoveAllAppenders()
        {
            m_appenderLock.AcquireWriterLock();
            try
            {
                if (m_appenderAttachedImpl != null)
                {
                    m_appenderAttachedImpl.RemoveAllAppenders();
                    m_appenderAttachedImpl = null;
                }
            }
            finally
            {
                m_appenderLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Remove the appender passed as parameter form the list of appenders.
        /// </summary>
        /// <param name="appender">The appender to remove</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// <para>
        /// Remove the appender passed as parameter form the list of appenders.
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </para>
        /// </remarks>
        virtual public IAppender RemoveAppender(IAppender appender)
        {
            m_appenderLock.AcquireWriterLock();
            try
            {
                if (appender != null && m_appenderAttachedImpl != null)
                {
                    return m_appenderAttachedImpl.RemoveAppender(appender);
                }
            }
            finally
            {
                m_appenderLock.ReleaseWriterLock();
            }
            return null;
        }

        /// <summary>
        /// Remove the appender passed as parameter form the list of appenders.
        /// </summary>
        /// <param name="name">The name of the appender to remove</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// <para>
        /// Remove the named appender passed as parameter form the list of appenders.
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </para>
        /// </remarks>
        virtual public IAppender RemoveAppender(string name)
        {
            m_appenderLock.AcquireWriterLock();
            try
            {
                if (name != null && m_appenderAttachedImpl != null)
                {
                    return m_appenderAttachedImpl.RemoveAppender(name);
                }
            }
            finally
            {
                m_appenderLock.ReleaseWriterLock();
            }
            return null;
        }

        #endregion

        #region Implementation of ILogger

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        /// <value>
        /// The name of the logger.
        /// </value>
        /// <remarks>
        /// <para>
        /// The name of this logger
        /// </para>
        /// </remarks>
        virtual public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// This generic form is intended to be used by wrappers.
        /// </summary>
        /// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
        /// the stack boundary into the logging system for this call.</param>
        /// <param name="level">The level of the message to be logged.</param>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Generate a logging event for the specified <paramref name="level"/> using
        /// the <paramref name="message"/> and <paramref name="exception"/>.
        /// </para>
        /// <para>
        /// This method must not throw any exception to the caller.
        /// </para>
        /// </remarks>
        virtual public void Log(Level level, string moudleName, Exception exception)
        {
            try
            {
                if (IsEnabledFor(level))
                {
                    ForcedLog(level, moudleName, exception.Message);
                }
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "Exception while logging", ex);
            }
        }

        /// <summary>
        /// This generic form is intended to be used by wrappers.
        /// </summary>
        /// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
        /// the stack boundary into the logging system for this call.</param>
        /// <param name="level">The level of the message to be logged.</param>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">the exception to log, including its stack trace. Pass <c>null</c> to not log an exception.</param>
        /// <remarks>
        /// <para>
        /// Generates a logging event for the specified <paramref name="level"/> using
        /// the <paramref name="message"/> and <paramref name="exception"/>.
        /// </para>
        /// </remarks>
        public void Log(Level level, string moudleName, string message)
        {
            try
            {
                if (IsEnabledFor(level))
                {
                    ForcedLog(level, moudleName, message);
                }
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "Exception while logging", ex);
            }
        }

        /// <summary>
        /// This is the most generic printing method that is intended to be used 
        /// by wrappers.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>
        /// <remarks>
        /// <para>
        /// Logs the specified logging event through this logger.
        /// </para>
        /// <para>
        /// This method must not throw any exception to the caller.
        /// </para>
        /// </remarks>
        virtual public void Log(LoggingData logEvent)
        {
            try
            {
                if (logEvent != null)
                {
                    if (IsEnabledFor(logEvent.Level))
                    {                       
                        ForcedLog(logEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "Exception while logging", ex);
            }
        }

        /// <summary>
        /// Checks if this logger is enabled for a given <see cref="Level"/> passed as parameter.
        /// </summary>
        /// <param name="level">The level to check.</param>
        /// <returns>
        /// <c>true</c> if this logger is enabled for <c>level</c>, otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Test if this logger is going to log events of the specified <paramref name="level"/>.
        /// </para>
        /// <para>
        /// This method must not throw any exception to the caller.
        /// </para>
        /// </remarks>
        virtual public bool IsEnabledFor(Level level)
        {
            try
            {               
                if (m_hierarchy.IsDisabled(level))
                {
                    return false;
                }
                return level >= m_level;
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "Exception while logging", ex);
            }

            return false;
        }

        /// <summary>
        /// Gets the <see cref="ILoggerRepository"/> where this 
        /// <c>Logger</c> instance is attached to.
        /// </summary>
        /// <value>
        /// The <see cref="ILoggerRepository" /> that this logger belongs to.
        /// </value>
        /// <remarks>
        /// <para>
        /// Gets the <see cref="ILoggerRepository"/> where this 
        /// <c>Logger</c> instance is attached to.
        /// </para>
        /// </remarks>
        public ILoggerRepository Repository
        {
            get { return m_hierarchy; }
        }

        #endregion Implementation of ILogger

        /// <summary>
        /// Deliver the <see cref="LoggingData"/> to the attached appenders.
        /// </summary>
        /// <param name="loggingEvent">The event to log.</param>
        /// <remarks>
        /// <para>
        /// Call the appenders in the hierarchy starting at
        /// <c>this</c>. If no appenders could be found, emit a
        /// warning.
        /// </para>
        /// <para>
        /// This method calls all the appenders inherited from the
        /// hierarchy circumventing any evaluation of whether to log or not
        /// to log the particular log request.
        /// </para>
        /// </remarks>
        virtual protected void CallAppenders(LoggingData loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            loggingEvent.LoggerName = this.m_name;
            loggingEvent.TimeStamp = DateTime.Now;
            loggingEvent.AppNo = m_hierarchy.AppNo;
            loggingEvent.Unique = Guid.NewGuid().ToString();
            loggingEvent.ThreadName = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            m_appenderLock.AcquireReaderLock();
            try
            {
                if (m_appenderAttachedImpl != null)
                {
                    m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
                }
            }
            finally
            {
                m_appenderLock.ReleaseReaderLock();
            }             
                             
        }

        /// <summary>
        /// Creates a new logging event and logs the event without further checks.
        /// </summary>
        /// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
        /// the stack boundary into the logging system for this call.</param>
        /// <param name="level">The level of the message to be logged.</param>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Generates a logging event and delivers it to the attached
        /// appenders.
        /// </para>
        /// </remarks>
        virtual protected void ForcedLog(Level level, string moudleName, string message)
        {
            CallAppenders(new LoggingData(level, moudleName, message));
        }

        /// <summary>
        /// Creates a new logging event and logs the event without further checks.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>
        /// <remarks>
        /// <para>
        /// Delivers the logging event to the attached appenders.
        /// </para>
        /// </remarks>
        virtual protected void ForcedLog(LoggingData logEvent)
        {

            CallAppenders(logEvent);
        }

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the Logger class.
        /// </summary>
        private readonly static Type declaringType = typeof(Logger);

        #endregion Private Static Fields

        #region Private Instance Fields

        /// <summary>
        /// The name of this logger.
        /// </summary>
        private readonly string m_name;

        /// <summary>
        /// The assigned level of this logger. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <c>level</c> variable need not be 
        /// assigned a value in which case it is inherited 
        /// form the hierarchy.
        /// </para>
        /// </remarks>
        private Level m_level;

       

        /// <summary>
        /// Loggers need to know what Hierarchy they are in.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Loggers need to know what Hierarchy they are in.
        /// The hierarchy that this logger is a member of is stored
        /// here.
        /// </para>
        /// </remarks>
        private Hierarchy m_hierarchy;

        /// <summary>
        /// Helper implementation of the <see cref="IAppenderAttachable"/> interface
        /// </summary>
        private Log4Simple.Util.AppenderAttachedImpl m_appenderAttachedImpl;
        

        /// <summary>
        /// Lock to protect AppenderAttachedImpl variable m_appenderAttachedImpl
        /// </summary>
        private readonly ReaderWriterLock m_appenderLock = new ReaderWriterLock();

        #endregion



        public ICollection Appenders
        {
            get { return m_appenderAttachedImpl != null ? m_appenderAttachedImpl.Appenders : null; }
        }

        
    }
}
