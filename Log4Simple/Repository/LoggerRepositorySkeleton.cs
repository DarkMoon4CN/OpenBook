using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Plugin;

namespace Log4Simple.Repository
{


    /// <summary>
    /// Base implementation of <see cref="ILoggerRepository"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default abstract implementation of the <see cref="ILoggerRepository"/> interface.
    /// </para>
    /// <para>
    /// Skeleton implementation of the <see cref="ILoggerRepository"/> interface.
    /// All <see cref="ILoggerRepository"/> types can extend this type.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public abstract class LoggerRepositorySkeleton :ILoggerRepository
    {

        private string m_Name;

        private PluginMap m_pluginMap;

        private event LoggerRepositoryShutdownEventHandler m_shutdownEvent;

        protected Level m_Threshold;


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initializes the repository with default (empty) properties.
        /// </para>
        /// </remarks>
        protected LoggerRepositorySkeleton()           
        {
            m_pluginMap = new PluginMap(this);
            m_Threshold = Level.ALL;
        }

        /// <summary>
        /// The name of the repository
        /// </summary>
        /// <value>
        /// The string name of the repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The name of this repository. The name is
        /// used to store and lookup the repositories 
        /// stored by the <see cref="IRepositorySelector"/>.
        /// </para>
        /// </remarks>
        virtual public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value ;
            }
        }

        /// <summary>
        /// Test if logger exists
        /// </summary>
        /// <param name="name">The name of the logger to lookup</param>
        /// <returns>The Logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// Check if the named logger exists in the repository. If so return
        /// its reference, otherwise returns <c>null</c>.
        /// </para>
        /// </remarks>
        abstract public Core.ILogger Exists(string name);

        /// <summary>
        /// Returns all the currently defined loggers in the repository
        /// </summary>
        /// <returns>All the defined loggers</returns>
        /// <remarks>
        /// <para>
        /// Returns all the currently defined loggers in the repository as an Array.
        /// </para>
        /// </remarks>
        abstract public Core.ILogger[] GetCurrentLoggers();

        /// <summary>
        /// Return a new logger instance
        /// </summary>
        /// <param name="name">The name of the logger to retrieve</param>
        /// <returns>The logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// Return a new logger instance.
        /// </para>
        /// <para>
        /// If a logger of that name already exists, then it will be
        /// returned. Otherwise, a new logger will be instantiated and
        /// then linked with its existing ancestors as well as children.
        /// </para>
        /// </remarks>
        abstract public Core.ILogger GetLogger(string name);

        /// <summary>
        /// The threshold for all events in this repository
        /// </summary>
        /// <value>
        /// The threshold for all events in this repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The threshold for all events in this repository
        /// </para>
        /// </remarks>
        virtual public Core.Level Threshold
        {
            get
            {
                return m_Threshold;
            }
            set
            {
                m_Threshold = value;
            }
        }

        /// <summary>
        /// The Application no for all events in this repository
        /// </summary>
        /// <value>
        /// The Application no  for all events in this repository
        /// </value>
        /// <remarks>
        /// <para>
        /// The Application no  for all events in this repository
        /// </para>
        /// </remarks>
        public int AppNo
        {
            get;

            set;

        }

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
        virtual public PluginMap PluginMap
        {
            get { return m_pluginMap; }
        }

        /// <summary>
        /// Test if this hierarchy is disabled for the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level">The level to check against.</param>
        /// <returns>
        /// <c>true</c> if the repository is disabled for the level argument, <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If this hierarchy has not been configured then this method will
        /// always return <c>true</c>.
        /// </para>
        /// <para>
        /// This method will return <c>true</c> if this repository is
        /// disabled for <c>level</c> object passed as parameter and
        /// <c>false</c> otherwise.
        /// </para>
        /// <para>
        /// See also the <see cref="ILoggerRepository.Threshold"/> property.
        /// </para>
        /// </remarks>
        public bool IsDisabled(Level level) 
		{
            return level < Threshold;
		}

        /// <summary>
        /// Shutdown the repository
        /// </summary>
        /// <remarks>
        /// <para>
        /// Shutdown the repository. Can be overridden in a subclass.
        /// This base class implementation notifies the <see cref="ShutdownEvent"/>
        /// listeners and all attached plugins of the shutdown event.
        /// </para>
        /// </remarks>
        virtual public void Shutdown()
        {
            lock (PluginMap)
            {
                // Shutdown attached plugins
                foreach (IPlugin plugin in PluginMap.AllPlugins)
                {
                    plugin.Shutdown();
                }
                PluginMap.Clear();
            }

            // Notify listeners
            OnShutdown(null);
        }




        /// <summary>
        /// Log the logEvent through this repository.
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
        abstract public void Log(LoggingData logEvent);


        /// <summary>
        /// Returns all the Appenders that are configured as an Array.
        /// </summary>
        /// <returns>All the Appenders</returns>
        /// <remarks>
        /// <para>
        /// Returns all the Appenders that are configured as an Array.
        /// </para>
        /// </remarks>
        abstract public Appender.IAppender[] GetAppenders();



        /// <summary>
        /// Notify the registered listeners that the repository is shutting down
        /// </summary>
        /// <param name="e">Empty EventArgs</param>
        /// <remarks>
        /// <para>
        /// Notify any listeners that this repository is shutting down.
        /// </para>
        /// </remarks>
        protected virtual void OnShutdown(EventArgs e)
        {
            if (e == null)
            {
                e = EventArgs.Empty;
            }

            LoggerRepositoryShutdownEventHandler handler = m_shutdownEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Event to notify that the repository has been shutdown.
        /// </summary>
        /// <value>
        /// Event to notify that the repository has been shutdown.
        /// </value>
        /// <remarks>
        /// <para>
        /// Event raised when the repository has been shutdown.
        /// </para>
        /// </remarks>
        public event LoggerRepositoryShutdownEventHandler ShutdownEvent
        {
            add { m_shutdownEvent += value; }
            remove { m_shutdownEvent -= value; }
        }
    }
}
