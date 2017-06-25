using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Repository.Hierarchy;
using Log4Simple.Repository;
using Log4Simple.Core;
using System.Globalization;

namespace Log4Simple.Core
    
{
    /// <summary>
	/// Implementation of <see cref="ILog"/> wrapper interface.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This implementation of the <see cref="ILog"/> interface
	/// forwards to the <see cref="ILogger"/> held by the base class.
	/// </para>
	/// <para>
	/// This logger has methods to allow the caller to log at the following
	/// levels:
	/// </para>
	/// <list type="definition">
	///   <item>
	///     <term>DEBUG</term>
	///     <description>
	///     The <see cref="M:Debug(object)"/> and <see cref="M:DebugFormat(string, object[])"/> methods log messages
	///     at the <c>DEBUG</c> level. That is the level with that name defined in the
	///     repositories <see cref="ILoggerRepository.LevelMap"/>. The default value
	///     for this level is <see cref="Level.Debug"/>. The <see cref="IsDebugEnabled"/>
	///     property tests if this level is enabled for logging.
	///     </description>
	///   </item>
	///   <item>
	///     <term>INFO</term>
	///     <description>
	///     The <see cref="M:Info(object)"/> and <see cref="M:InfoFormat(string, object[])"/> methods log messages
	///     at the <c>INFO</c> level. That is the level with that name defined in the
	///     repositories <see cref="ILoggerRepository.LevelMap"/>. The default value
	///     for this level is <see cref="Level.Info"/>. The <see cref="IsInfoEnabled"/>
	///     property tests if this level is enabled for logging.
	///     </description>
	///   </item>
	///   <item>
	///     <term>WARN</term>
	///     <description>
	///     The <see cref="M:Warn(object)"/> and <see cref="M:WarnFormat(string, object[])"/> methods log messages
	///     at the <c>WARN</c> level. That is the level with that name defined in the
	///     repositories <see cref="ILoggerRepository.LevelMap"/>. The default value
	///     for this level is <see cref="Level.Warn"/>. The <see cref="IsWarnEnabled"/>
	///     property tests if this level is enabled for logging.
	///     </description>
	///   </item>
	///   <item>
	///     <term>ERROR</term>
	///     <description>
	///     The <see cref="M:Error(object)"/> and <see cref="M:ErrorFormat(string, object[])"/> methods log messages
	///     at the <c>ERROR</c> level. That is the level with that name defined in the
	///     repositories <see cref="ILoggerRepository.LevelMap"/>. The default value
	///     for this level is <see cref="Level.Error"/>. The <see cref="IsErrorEnabled"/>
	///     property tests if this level is enabled for logging.
	///     </description>
	///   </item>
	///   <item>
	///     <term>FATAL</term>
	///     <description>
	///     The <see cref="M:Fatal(object)"/> and <see cref="M:FatalFormat(string, object[])"/> methods log messages
	///     at the <c>FATAL</c> level. That is the level with that name defined in the
	///     repositories <see cref="ILoggerRepository.LevelMap"/>. The default value
	///     for this level is <see cref="Level.Fatal"/>. The <see cref="IsFatalEnabled"/>
	///     property tests if this level is enabled for logging.
	///     </description>
	///   </item>
	/// </list>
	/// <para>
	/// The values for these levels and their semantic meanings can be changed by 
	/// configuring the <see cref="ILoggerRepository.LevelMap"/> for the repository.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class LogImpl : LoggerWrapperImpl, ILog
	{
		#region Public Instance Constructors

		/// <summary>
		/// Construct a new wrapper for the specified logger.
		/// </summary>
		/// <param name="logger">The logger to wrap.</param>
		/// <remarks>
		/// <para>
		/// Construct a new wrapper for the specified logger.
		/// </para>
		/// </remarks>
		public LogImpl(ILogger logger) : base(logger)
		{
			// Listen for changes to the repository
			//logger.Repository.ConfigurationChanged += new LoggerRepositoryConfigurationChangedEventHandler(LoggerRepositoryConfigurationChanged);

		
		}

		#endregion Public Instance Constructors
		

		#region Implementation of ILog

		/// <summary>
		/// Logs a message object with the <c>DEBUG</c> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <remarks>
		/// <para>
		/// This method first checks if this logger is <c>DEBUG</c>
		/// enabled by comparing the level of this logger with the 
		/// <c>DEBUG</c> level. If this logger is
		/// <c>DEBUG</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="log4net.ObjectRenderer.IObjectRenderer"/>. It then 
		/// proceeds to call all the registered appenders in this logger 
		/// and also higher in the hierarchy depending on the value of the 
		/// additivity flag.
		/// </para>
		/// <para>
		/// <b>WARNING</b> Note that passing an <see cref="Exception"/> 
		/// to this method will print the name of the <see cref="Exception"/> 
		/// but no stack trace. To print a stack trace use the 
		/// <see cref="M:Debug(object,Exception)"/> form instead.
		/// </para>
		/// </remarks>
		virtual public void Debug(string message) 
		{
            Debug(null, message);
		}

        /// <summary>
        /// Logs a message object with the <c>DEBUG</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>DEBUG</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> passed
        /// as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Debug(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)"/>
        virtual public void Debug(Exception exception)
        {
            Debug(null, exception.Message);
        }

        /// <summary>
        /// Logs a message object with the <c>DEBUG</c> level
        /// </summary>
        /// <param name="moudleName">The moudleName to log.</param>
        /// <param name="message">The message to log</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>DEBUG</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> passed
        /// as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Debug(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)"/>
        virtual public void Debug(string moudleName, string message)
        {
            Logger.Log(m_levelDebug, moudleName, message);
        }

		/// <summary>
		/// Logs a message object with the <c>DEBUG</c> level
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Logs a message object with the <c>DEBUG</c> level including
		/// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> passed
		/// as a parameter.
		/// </para>
		/// <para>
		/// See the <see cref="M:Debug(object)"/> form for more detailed information.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:Debug(object)"/>
		virtual public void Debug(string moudleName, Exception exception) 
		{
            Debug(moudleName, exception.Message);
		}

		/// <summary>
		/// Logs a formatted message string with the <c>DEBUG</c> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// The string is formatted using the <see cref="CultureInfo.InvariantCulture"/>
		/// format provider. To specify a localized provider use the
		/// <see cref="M:DebugFormat(IFormatProvider,string,object[])"/> method.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Debug(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void DebugFormat(string format, params object[] args) 
		{
            Debug(null, string.Format(format, args));
		}

		

		/// <summary>
		/// Logs a formatted message string with the <c>DEBUG</c> level.
		/// </summary>
		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Debug(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>        
        virtual public void DebugFormatEx(string moudleName, string format, params object[] args) 
		{
            Debug(moudleName, string.Format(CultureInfo.InvariantCulture, format, args));            			
		}

        /// <summary>
        /// Logs a formatted message string with the <see cref="Level.Debug"/> level.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>      
        /// <seealso cref="M:Debug(object)"/>
        /// <seealso cref="IsDebugEnabled"/>
        public void Debug(LoggingData logEvent)
        {
            if (IsDebugEnabled)
            {                
                logEvent.Level = m_levelDebug;
                Logger.Log(logEvent);
            }
        }

		/// <summary>
		/// Logs a message object with the <c>INFO</c> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <remarks>
		/// <para>
		/// This method first checks if this logger is <c>INFO</c>
		/// enabled by comparing the level of this logger with the 
		/// <c>INFO</c> level. If this logger is
		/// <c>INFO</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="log4net.ObjectRenderer.IObjectRenderer"/>. It then 
		/// proceeds to call all the registered appenders in this logger 
		/// and also higher in the hierarchy depending on the value of 
		/// the additivity flag.
		/// </para>
		/// <para>
		/// <b>WARNING</b> Note that passing an <see cref="Exception"/> 
		/// to this method will print the name of the <see cref="Exception"/> 
		/// but no stack trace. To print a stack trace use the 
		/// <see cref="M:Info(object,Exception)"/> form instead.
		/// </para>
		/// </remarks>
		virtual public void Info(string message) 
		{
			Info(null, message);
		}

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>INFO</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Info(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)"/>
        virtual public void Info(Exception exception)
        {
            Info(null, exception.Message);
        }

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="moudleName">The moudleName to log.</param>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>INFO</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Info(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)"/>
        virtual public void Info(string moudleName, string message)
        {
            Logger.Log(m_levelInfo, moudleName, message);
        }

		/// <summary>
		/// Logs a message object with the <c>INFO</c> level.
		/// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Logs a message object with the <c>INFO</c> level including
		/// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
		/// passed as a parameter.
		/// </para>
		/// <para>
		/// See the <see cref="M:Info(object)"/> form for more detailed information.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:Info(object)"/>
		virtual public void Info(string moudleName, Exception exception) 
		{
            Info(moudleName, exception.Message);
		}

		/// <summary>
		/// Logs a formatted message string with the <c>INFO</c> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// The string is formatted using the <see cref="CultureInfo.InvariantCulture"/>
		/// format provider. To specify a localized provider use the
		/// <see cref="M:InfoFormat(IFormatProvider,string,object[])"/> method.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Info(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void InfoFormat(string format, params object[] args) 
		{
            Info(null, string.Format(format, args));
			
		}		

		/// <summary>
		/// Logs a formatted message string with the <c>INFO</c> level.
		/// </summary>
        /// <param name="moudleName">An <see cref="IFormatProvider"/> the moudle name</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Info(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void InfoFormatEx(string moudleName, string format, params object[] args) 
		{
            Info(moudleName, string.Format(format, args));			
		}

        /// <summary>
        /// Logs a formatted message string with the <see cref="Level.Info"/> level.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>      
        /// <seealso cref="M:Debug(object)"/>
        /// <seealso cref="IsDebugEnabled"/>
        public void Info(LoggingData logEvent)
        {
            if (IsInfoEnabled)
            {                
                logEvent.Level = m_levelInfo;
                Logger.Log(logEvent);
            }
        }

		/// <summary>
		/// Logs a message object with the <c>WARN</c> level.
		/// </summary>
		/// <param name="message">the message to log</param>
		/// <remarks>
		/// <para>
		/// This method first checks if this logger is <c>WARN</c>
		/// enabled by comparing the level of this logger with the 
		/// <c>WARN</c> level. If this logger is
		/// <c>WARN</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="log4net.ObjectRenderer.IObjectRenderer"/>. It then 
		/// proceeds to call all the registered appenders in this logger and 
		/// also higher in the hierarchy depending on the value of the 
		/// additivity flag.
		/// </para>
		/// <para>
		/// <b>WARNING</b> Note that passing an <see cref="Exception"/> to this
		/// method will print the name of the <see cref="Exception"/> but no
		/// stack trace. To print a stack trace use the 
		/// <see cref="M:Warn(object,Exception)"/> form instead.
		/// </para>
		/// </remarks>
		virtual public void Warn(string message) 
		{
            Warn(null, message);
		}

        /// <summary>
        /// Logs a message object with the <c>WARN</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>WARN</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Warn(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object)"/>
        virtual public void Warn(Exception exception)
        {
            Warn(null, exception.Message);
        }


        /// <summary>
        /// Logs a message object with the <c>WARN</c> level
        /// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>WARN</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Warn(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object)"/>
        virtual public void Warn(string moudleName, string message)
        {
            Logger.Log(m_levelWarn, moudleName, message);
        }

		/// <summary>
		/// Logs a message object with the <c>WARN</c> level
		/// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Logs a message object with the <c>WARN</c> level including
		/// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
		/// passed as a parameter.
		/// </para>
		/// <para>
		/// See the <see cref="M:Warn(object)"/> form for more detailed information.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:Warn(object)"/>
		virtual public void Warn(string moudleName, Exception exception) 
		{
            Warn(moudleName, exception.Message);
		}

		/// <summary>
		/// Logs a formatted message string with the <c>WARN</c> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// The string is formatted using the <see cref="CultureInfo.InvariantCulture"/>
		/// format provider. To specify a localized provider use the
		/// <see cref="M:WarnFormat(IFormatProvider,string,object[])"/> method.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Warn(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void WarnFormat(string format, params object[] args) 
		{						                
            Warn(null, string.Format(CultureInfo.InvariantCulture, format, args));			
		}

		/// <summary>
		/// Logs a formatted message string with the <c>WARN</c> level.
		/// </summary>
        /// <param name="moudleName">An <see cref="IFormatProvider"/> the moudle name</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Warn(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
        virtual public void WarnFormatEx(string moudleName, string format, params object[] args) 
		{
            Warn(moudleName, string.Format(format, args));			
		}

        /// <summary>
        /// Logs a formatted message string with the <see cref="Level.Warn"/> level.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>            
        public void Warn(LoggingData logEvent)
        {
            if (IsWarnEnabled)
            {               
                logEvent.Level = m_levelWarn;
                Logger.Log(logEvent);
            }
        }

		/// <summary>
		/// Logs a message object with the <c>ERROR</c> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <remarks>
		/// <para>
		/// This method first checks if this logger is <c>ERROR</c>
		/// enabled by comparing the level of this logger with the 
		/// <c>ERROR</c> level. If this logger is
		/// <c>ERROR</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="log4net.ObjectRenderer.IObjectRenderer"/>. It then 
		/// proceeds to call all the registered appenders in this logger and 
		/// also higher in the hierarchy depending on the value of the 
		/// additivity flag.
		/// </para>
		/// <para>
		/// <b>WARNING</b> Note that passing an <see cref="Exception"/> to this
		/// method will print the name of the <see cref="Exception"/> but no
		/// stack trace. To print a stack trace use the 
		/// <see cref="M:Error(object,Exception)"/> form instead.
		/// </para>
		/// </remarks>
		virtual public void Error(string message) 
		{
            Error(null, message);
		}

        /// <summary>
        /// Logs a message object with the <c>ERROR</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>ERROR</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Error(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)"/>
        virtual public void Error(Exception exception)
        {
            Error(null, exception.Message);
        }

        /// <summary>
        /// Logs a message object with the <c>ERROR</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>ERROR</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Error(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)"/>
        virtual public void Error(string moudleName, string message)
        {
            Logger.Log(m_levelError, moudleName, message);
        }

		/// <summary>
		/// Logs a message object with the <c>ERROR</c> level
		/// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Logs a message object with the <c>ERROR</c> level including
		/// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
		/// passed as a parameter.
		/// </para>
		/// <para>
		/// See the <see cref="M:Error(object)"/> form for more detailed information.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:Error(object)"/>
		virtual public void Error(string moudleName, Exception exception) 
		{
            Error(moudleName, exception.Message);
		}

		/// <summary>
		/// Logs a formatted message string with the <c>ERROR</c> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// The string is formatted using the <see cref="CultureInfo.InvariantCulture"/>
		/// format provider. To specify a localized provider use the
		/// <see cref="M:ErrorFormat(IFormatProvider,string,object[])"/> method.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Error(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void ErrorFormat(string format, params object[] args) 
		{
			Error(null, string.Format(CultureInfo.InvariantCulture, format, args));		
		}		

		/// <summary>
		/// Logs a formatted message string with the <c>ERROR</c> level.
		/// </summary>
        /// <param name="moudleName">An <see cref="IFormatProvider"/> the moudle Name</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Error(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
        virtual public void ErrorFormatEx(string moudleName, string format, params object[] args) 
		{
            Error(moudleName, string.Format(format, args));			
		}

        /// <summary>
        /// Logs a formatted message string with the <see cref="Level.Error"/> level.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>            
        public void Error(LoggingData logEvent)
        {
            if (IsErrorEnabled)
            {
                logEvent.Level = m_levelError;
                Logger.Log(logEvent);
            }
        }

		/// <summary>
		/// Logs a message object with the <c>FATAL</c> level.
		/// </summary>
		/// <param name="message">The message object to log.</param>
		/// <remarks>
		/// <para>
		/// This method first checks if this logger is <c>FATAL</c>
		/// enabled by comparing the level of this logger with the 
		/// <c>FATAL</c> level. If this logger is
		/// <c>FATAL</c> enabled, then it converts the message object
		/// (passed as parameter) to a string by invoking the appropriate
		/// <see cref="log4net.ObjectRenderer.IObjectRenderer"/>. It then 
		/// proceeds to call all the registered appenders in this logger and 
		/// also higher in the hierarchy depending on the value of the 
		/// additivity flag.
		/// </para>
		/// <para>
		/// <b>WARNING</b> Note that passing an <see cref="Exception"/> to this
		/// method will print the name of the <see cref="Exception"/> but no
		/// stack trace. To print a stack trace use the 
		/// <see cref="M:Fatal(object,Exception)"/> form instead.
		/// </para>
		/// </remarks>
		virtual public void Fatal(string message) 
		{
			Fatal(null, message);
		}

        /// <summary>
        /// Logs a message object with the <c>FATAL</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>FATAL</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Fatal(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)"/>
        virtual public void Fatal(Exception exception)
        {
            Fatal(null, exception.Message);
        }

        /// <summary>
        /// Logs a message object with the <c>FATAL</c> level
        /// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// <para>
        /// Logs a message object with the <c>FATAL</c> level including
        /// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
        /// passed as a parameter.
        /// </para>
        /// <para>
        /// See the <see cref="M:Fatal(object)"/> form for more detailed information.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)"/>
        virtual public void Fatal(string moudleName, string message)
        {
            Logger.Log(m_levelFatal, moudleName, message);
        }

		/// <summary>
		/// Logs a message object with the <c>FATAL</c> level
		/// </summary>
        /// <param name="moudleName">The moudleName object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Logs a message object with the <c>FATAL</c> level including
		/// the stack trace of the <see cref="Exception"/> <paramref name="exception"/> 
		/// passed as a parameter.
		/// </para>
		/// <para>
		/// See the <see cref="M:Fatal(object)"/> form for more detailed information.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:Fatal(object)"/>
		virtual public void Fatal(string moudleName, Exception exception) 
		{
            Fatal(moudleName, exception.Message);
		}

		/// <summary>
		/// Logs a formatted message string with the <c>FATAL</c> level.
		/// </summary>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// The string is formatted using the <see cref="CultureInfo.InvariantCulture"/>
		/// format provider. To specify a localized provider use the
		/// <see cref="M:FatalFormat(IFormatProvider,string,object[])"/> method.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Fatal(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
		virtual public void FatalFormat(string format, params object[] args) 
		{
			Fatal(null, string.Format(CultureInfo.InvariantCulture, format, args));			
		}		

		/// <summary>
		/// Logs a formatted message string with the <c>FATAL</c> level.
		/// </summary>
        /// <param name="moudleName">An <see cref="IFormatProvider"/> moudleName</param>
		/// <param name="format">A String containing zero or more format items</param>
		/// <param name="args">An Object array containing zero or more objects to format</param>
		/// <remarks>
		/// <para>
		/// The message is formatted using the <see cref="M:String.Format(IFormatProvider, string, object[])"/> method. See
		/// <c>String.Format</c> for details of the syntax of the format string and the behavior
		/// of the formatting.
		/// </para>
		/// <para>
		/// This method does not take an <see cref="Exception"/> object to include in the
		/// log event. To pass an <see cref="Exception"/> use one of the <see cref="M:Fatal(object)"/>
		/// methods instead.
		/// </para>
		/// </remarks>
        virtual public void FatalFormatEx(string moudleName, string format, params object[] args) 
		{
            Fatal(moudleName, string.Format(format, args));	
		}

        /// <summary>
        /// Logs a formatted message string with the <see cref="Level.Fatal"/> level.
        /// </summary>
        /// <param name="logEvent">The event being logged.</param>            
        public void Fatal(LoggingData logEvent)
        {
            if (IsFatalEnabled)
            {
                logEvent.Level = m_levelFatal;
                Logger.Log(logEvent);
            }
        }

		/// <summary>
		/// Checks if this logger is enabled for the <c>DEBUG</c>
		/// level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for <c>DEBUG</c> events,
		/// <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// This function is intended to lessen the computational cost of
		/// disabled log debug statements.
		/// </para>
		/// <para>
		/// For some <c>log</c> Logger object, when you write:
		/// </para>
		/// <code lang="C#">
		/// log.Debug("This is entry number: " + i );
		/// </code>
		/// <para>
		/// You incur the cost constructing the message, concatenation in
		/// this case, regardless of whether the message is logged or not.
		/// </para>
		/// <para>
		/// If you are worried about speed, then you should write:
		/// </para>
		/// <code lang="C#">
		/// if (log.IsDebugEnabled())
		/// { 
		///	 log.Debug("This is entry number: " + i );
		/// }
		/// </code>
		/// <para>
		/// This way you will not incur the cost of parameter
		/// construction if debugging is disabled for <c>log</c>. On
		/// the other hand, if the <c>log</c> is debug enabled, you
		/// will incur the cost of evaluating whether the logger is debug
		/// enabled twice. Once in <c>IsDebugEnabled</c> and once in
		/// the <c>Debug</c>.  This is an insignificant overhead
		/// since evaluating a logger takes about 1% of the time it
		/// takes to actually log.
		/// </para>
		/// </remarks>
		virtual public bool IsDebugEnabled
		{
			get { return Logger.IsEnabledFor(m_levelDebug); }
		}
  
		/// <summary>
		/// Checks if this logger is enabled for the <c>INFO</c> level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for <c>INFO</c> events,
		/// <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// See <see cref="IsDebugEnabled"/> for more information and examples 
		/// of using this method.
		/// </para>
		/// </remarks>
		/// <seealso cref="LogImpl.IsDebugEnabled"/>
		virtual public bool IsInfoEnabled
		{
			get { return Logger.IsEnabledFor(m_levelInfo); }
		}

		/// <summary>
		/// Checks if this logger is enabled for the <c>WARN</c> level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for <c>WARN</c> events,
		/// <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// See <see cref="IsDebugEnabled"/> for more information and examples 
		/// of using this method.
		/// </para>
		/// </remarks>
		/// <seealso cref="ILog.IsDebugEnabled"/>
		virtual public bool IsWarnEnabled
		{
			get { return Logger.IsEnabledFor(m_levelWarn); }
		}

		/// <summary>
		/// Checks if this logger is enabled for the <c>ERROR</c> level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for <c>ERROR</c> events,
		/// <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// See <see cref="IsDebugEnabled"/> for more information and examples of using this method.
		/// </para>
		/// </remarks>
		/// <seealso cref="ILog.IsDebugEnabled"/>
		virtual public bool IsErrorEnabled
		{
			get { return Logger.IsEnabledFor(m_levelError); }
		}

		/// <summary>
		/// Checks if this logger is enabled for the <c>FATAL</c> level.
		/// </summary>
		/// <value>
		/// <c>true</c> if this logger is enabled for <c>FATAL</c> events,
		/// <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// See <see cref="IsDebugEnabled"/> for more information and examples of using this method.
		/// </para>
		/// </remarks>
		/// <seealso cref="ILog.IsDebugEnabled"/>
		virtual public bool IsFatalEnabled
		{
			get { return Logger.IsEnabledFor(m_levelFatal); }
		}

		#endregion Implementation of ILog

		#region Private Methods

		
		#endregion

		#region Private Static Instance Fields

		/// <summary>
		/// The fully qualified name of this declaring type not the type of any subclass.
		/// </summary>
		private readonly static Type ThisDeclaringType = typeof(LogImpl);

		#endregion Private Static Instance Fields

		

       


        #region Private Fields

        private Level m_levelDebug = Level.DEBUG;
        private Level m_levelInfo = Level.INFO;
        private Level m_levelWarn = Level.WARN;
        private Level m_levelError = Level.ERROR;
        private Level m_levelFatal = Level.Fatal;

        #endregion

    }
}
