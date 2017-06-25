using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Plugin;

namespace Log4Simple.Repository
{

    #region LoggerRepositoryShutdownEvent

    /// <summary>
    /// Delegate used to handle logger repository shutdown event notifications
    /// </summary>
    /// <param name="sender">The <see cref="ILoggerRepository"/> that is shutting down.</param>
    /// <param name="e">Empty event args</param>
    /// <remarks>
    /// <para>
    /// Delegate used to handle logger repository shutdown event notifications.
    /// </para>
    /// </remarks>
    public delegate void LoggerRepositoryShutdownEventHandler(object sender, EventArgs e);

    #endregion


    /// <summary>
    /// Interface implemented by logger repositories.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is implemented by logger repositories. e.g. 
    /// <see cref="Hierarchy"/>.
    /// </para>
    /// <para>
    /// This interface is used by the <see cref="LogManager"/>
    /// to obtain <see cref="ILog"/> interfaces.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public interface ILoggerRepository
    {

        /// <summary>
        /// The application no of the repository
        /// </summary>
        /// <value>
        /// The application no of the repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The application no of the repository.
        /// </para>
        /// </remarks>        
        int AppNo { set; get; }

        /// <summary>
        /// The name of the repository
        /// </summary>
        /// <value>
        /// The name of the repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The name of the repository.
        /// </para>
        /// </remarks>        
        string Name { set; get; }

        /// <summary>
        /// The threshold for all events in this repository
        /// </summary>
        /// <value>
        /// The threshold for all events in this repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The threshold for all events in this repository.
        /// </para>
        /// </remarks>        
        Level Threshold { set; get; }

        /// <summary>
        /// The plugin map for this repository.
        /// </summary>
        /// <value>
        /// The plugin map for this repository.
        /// </value>
        /// <remarks>
        /// <para>
        /// The plugin map holds the <see cref="IPlugin"/> instances
        /// that have been attached to this repository.
        /// </para>
        /// </remarks>
        PluginMap PluginMap { get; }
        

        /// <summary>
        /// Check if the named logger exists in the repository. If so return
        /// its reference, otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="name">The name of the logger to lookup</param>
        /// <returns>The Logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// If the names logger exists it is returned, otherwise
        /// <c>null</c> is returned.
        /// </para>
        /// </remarks>
        ILogger Exists(string name);

        /// <summary>
        /// Returns all the currently defined loggers as an Array.
        /// </summary>
        /// <returns>All the defined loggers</returns>
        /// <remarks>
        /// <para>
        /// Returns all the currently defined loggers as an Array.
        /// </para>
        /// </remarks>
        ILogger[] GetCurrentLoggers();        

        /// <summary>
        /// Returns a named logger instance
        /// </summary>
        /// <param name="name">The name of the logger to retrieve</param>
        /// <returns>The logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// Returns a named logger instance.
        /// </para>
        /// <para>
        /// If a logger of that name already exists, then it will be
        /// returned.  Otherwise, a new logger will be instantiated and
        /// then linked with its existing ancestors as well as children.
        /// </para>
        /// </remarks>
        ILogger GetLogger(string name);

        /// <summary>Shutdown the repository</summary>
        /// <remarks>
        /// <para>
        /// Shutting down a repository will <i>safely</i> close and remove
        /// all appenders in all loggers including the root logger.
        /// </para>
        /// <para>
        /// Some appenders need to be closed before the
        /// application exists. Otherwise, pending logging events might be
        /// lost.
        /// </para>
        /// <para>
        /// The <see cref="M:Shutdown()"/> method is careful to close nested
        /// appenders before closing regular appenders. This is allows
        /// configurations where a regular appender is attached to a logger
        /// and again to a nested appender.
        /// </para>
        /// </remarks>
        void Shutdown();

        /// <summary>
        /// Log the <see cref="LoggingData"/> through this repository.
        /// </summary>
        /// <param name="logEvent">the event to log</param>
        /// <remarks>
        /// <para>
        /// This method should not normally be used to log.
        /// The <see cref="ILog"/> interface should be used 
        /// for routine logging. This interface can be obtained
        /// using the <see cref="M:log4net.LogManager.GetLogger(string)"/> method.
        /// </para>
        /// <para>
        /// The <c>logEvent</c> is delivered to the appropriate logger and
        /// that logger is then responsible for logging the event.
        /// </para>
        /// </remarks>
        void Log(LoggingData logEvent);


        /// <summary>
        /// Returns all the Appenders that are configured as an Array.
        /// </summary>
        /// <returns>All the Appenders</returns>
        /// <remarks>
        /// <para>
        /// Returns all the Appenders that are configured as an Array.
        /// </para>
        /// </remarks>
        Log4Simple.Appender.IAppender[] GetAppenders();

    }
}
