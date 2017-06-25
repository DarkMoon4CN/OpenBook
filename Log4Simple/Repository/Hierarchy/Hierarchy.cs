using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using System.Xml;
using Log4Simple.Util;

namespace Log4Simple.Repository.Hierarchy
{
    public class Hierarchy : LoggerRepositorySkeleton, IXmlRepositoryConfigurator
    {
        

        #region Public Instance Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="Hierarchy" /> class.
        /// </para>
        /// </remarks>
        public Hierarchy()
            : this(new DefaultLoggerFactory())
        {
 
        }
        /// <summary>
        /// Construct with a logger factory
        /// </summary>
        /// <param name="loggerFactory">The factory to use to create new logger instances.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="Hierarchy" /> class with 
        /// the specified <see cref="ILoggerFactory" />.
        /// </para>
        /// </remarks>
        public Hierarchy(ILoggerFactory loggerfactory) 
        {
            if (loggerfactory == null)
                throw new Exception("loggerfactory");
            m_defaultFactory = loggerfactory;
            m_ht = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        }

        #endregion Public Instance Constructors

        /// <summary>
        /// Test if a logger exists
        /// </summary>
        /// <param name="name">The name of the logger to lookup</param>
        /// <returns>The Logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// Check if the named logger exists in the hierarchy. If so return
        /// its reference, otherwise returns <c>null</c>.
        /// </para>
        /// </remarks>
        public override Core.ILogger Exists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return (m_ht[name] as Logger);
        }

        /// <summary>
        /// Returns all the currently defined loggers in the hierarchy as an Array
        /// </summary>
        /// <returns>All the defined loggers</returns>
        /// <remarks>
        /// <para>
        /// Returns all the currently defined loggers in the hierarchy as an Array.
        /// The root logger is <b>not</b> included in the returned
        /// enumeration.
        /// </para>
        /// </remarks>
        public override Core.ILogger[] GetCurrentLoggers()
        {
            System.Collections.ArrayList loggers = new System.Collections.ArrayList(m_ht.Count);

            // Iterate through m_ht values
            foreach (object node in m_ht.Values)
            {
                if (node is Logger)
                {
                    loggers.Add(node);
                }
            }
            return (Logger[])loggers.ToArray(typeof(Logger));
        }


        /// <summary>
        /// Return a new logger instance named as the first parameter using
        /// the default factory.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Return a new logger instance named as the first parameter using
        /// the default factory.
        /// </para>
        /// <para>
        /// If a logger of that name already exists, then it will be
        /// returned.  Otherwise, a new logger will be instantiated and
        /// then linked with its existing ancestors as well as children.
        /// </para>
        /// </remarks>
        /// <param name="name">The name of the logger to retrieve</param>
        /// <returns>The logger object with the name specified</returns>
        public override Core.ILogger GetLogger(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return GetLogger(name, m_defaultFactory);
        }


        /// <summary>
        /// Return a new logger instance named as the first parameter using
        /// <paramref name="factory"/>.
        /// </summary>
        /// <param name="name">The name of the logger to retrieve</param>
        /// <param name="factory">The factory that will make the new logger instance</param>
        /// <returns>The logger object with the name specified</returns>
        /// <remarks>
        /// <para>
        /// If a logger of that name already exists, then it will be
        /// returned. Otherwise, a new logger will be instantiated by the
        /// <paramref name="factory"/> parameter and linked with its existing
        /// ancestors as well as children.
        /// </para>
        /// </remarks>
        public Logger GetLogger(string name, ILoggerFactory factory)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (factory == null)
                throw new ArgumentNullException("factory");

            lock (this)
            {
                Logger logger = null;

                object node = m_ht[name];
                if (node == null)
                {
                    logger = factory.CreateLogger(this, name);
                    logger.Hierarchy = this;
                    m_ht[name] = logger;
                    return logger;
                }
                return node as Logger;
            }
        }

        /// <summary>
        /// Shutting down a hierarchy will <i>safely</i> close and remove
        /// all appenders in all loggers including the root logger.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Shutting down a hierarchy will <i>safely</i> close and remove
        /// all appenders in all loggers including the root logger.
        /// </para>
        /// <para>
        /// Some appenders need to be closed before the
        /// application exists. Otherwise, pending logging events might be
        /// lost.
        /// </para>
        /// <para>
        /// The <c>Shutdown</c> method is careful to close nested
        /// appenders before closing regular appenders. This is allows
        /// configurations where a regular appender is attached to a logger
        /// and again to a nested appender.
        /// </para>
        /// </remarks>
        override public void Shutdown()
        {
            LogLog.Debug(declaringType, "Shutdown called on Hierarchy [" + this.Name + "]");

            try
            {
                base.Shutdown();
            }
            catch
            { }
            lock (m_ht)
            {
                
                foreach (Logger logger in GetCurrentLoggers())
                {
                    logger.RemoveAllAppenders();
                }
                m_ht.Clear();
            }

            
        }

        /// <summary>
        /// Initialize the log4net system using the specified config
        /// </summary>
        /// <param name="element">the element containing the root of the config</param>
        /// <remarks>
        /// <para>
        /// This method provides the same functionality as the 
        /// <see cref="M:IBasicRepositoryConfigurator.Configure(IAppender)"/> method implemented
        /// on this object, but it is protected and therefore can be called by subclasses.
        /// </para>
        /// </remarks>
        public void Configure(XmlElement element)
        {
            XmlHierarchyConfigurator config = new XmlHierarchyConfigurator(this);
            config.Configure(element);
        }

        /// <summary>
        /// Log the logEvent through this hierarchy.
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
        override public void Log(LoggingData logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            this.GetLogger(logEvent.LoggerName, m_defaultFactory).Log(logEvent);
        }

        /// <summary>
        /// Returns all the Appenders that are currently configured
        /// </summary>
        /// <returns>An array containing all the currently configured appenders</returns>
        /// <remarks>
        /// <para>
        /// Returns all the <see cref="log4net.Appender.IAppender"/> instances that are currently configured.
        /// All the loggers are searched for appenders. The appenders may also be containers
        /// for appenders and these are also searched for additional loggers.
        /// </para>
        /// <para>
        /// The list returned is unordered but does not contain duplicates.
        /// </para>
        /// </remarks>
        override public Appender.IAppender[] GetAppenders()
        {
            System.Collections.ArrayList appenderList = new System.Collections.ArrayList();           

            foreach (Logger logger in GetCurrentLoggers())
            {
                appenderList.AddRange(logger.Appenders);
            }

            return (Appender.IAppender[])appenderList.ToArray(typeof(Appender.IAppender));
        }


        private System.Collections.Hashtable m_ht;

        private ILoggerFactory m_defaultFactory = new DefaultLoggerFactory();

        /// <summary>
        /// The fully qualified type of the Hierarchy class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(Hierarchy);
    }
}
