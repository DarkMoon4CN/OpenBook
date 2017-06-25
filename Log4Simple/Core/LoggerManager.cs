using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Log4Simple.Repository;

namespace Log4Simple.Core
{

    /// <summary>
    /// Static manager that controls the creation of repositories
    /// </summary>
    /// <remarks>
    /// <para>
    /// Static manager that controls the creation of repositories
    /// </para>
    /// <para>
    /// This class is used by the wrapper managers (e.g. <see cref="log4net.LogManager"/>)
    /// to provide access to the <see cref="ILogger"/> objects.
    /// </para>
    /// <para>
    /// This manager also holds the <see cref="IRepositorySelector"/> that is used to
    /// lookup and create repositories. The selector can be set either programmatically using
    /// the <see cref="RepositorySelector"/> property, or by setting the <c>log4net.RepositorySelector</c>
    /// AppSetting in the applications config file to the fully qualified type name of the
    /// selector to use. 
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public class LoggerManager
    {
        

        static LoggerManager()
        {
            RegisterAppDomainEvents();
            if (m_RepositorySelector == null)
                m_RepositorySelector = new DefaultRepositorySelector(typeof(Log4Simple.Repository.Hierarchy.Hierarchy));


        }

        /// <summary>
        /// Register for ProcessExit and DomainUnload events on the AppDomain
        /// </summary>
        /// <remarks>
        /// <para>
        /// This needs to be in a separate method because the events make
        /// a LinkDemand for the ControlAppDomain SecurityPermission. Because
        /// this is a LinkDemand it is demanded at JIT time. Therefore we cannot
        /// catch the exception in the method itself, we have to catch it in the
        /// caller.
        /// </para>
        /// </remarks>
        private static void RegisterAppDomainEvents()
        {
   
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(OnDomainUnload);  

        }

        /// <summary>
        /// Called when the <see cref="AppDomain.DomainUnload"/> event fires
        /// </summary>
        /// <param name="sender">the <see cref="AppDomain"/> that is exiting</param>
        /// <param name="e">null</param>
        /// <remarks>
        /// <para>
        /// Called when the <see cref="AppDomain.DomainUnload"/> event fires.
        /// </para>
        /// <para>
        /// When the event is triggered the log4net system is <see cref="M:Shutdown()"/>.
        /// </para>
        /// </remarks>
        private static void OnDomainUnload(object sender, EventArgs e)
        {
            Shutdown();
        }

        /// <summary>
        /// Called when the <see cref="AppDomain.ProcessExit"/> event fires
        /// </summary>
        /// <param name="sender">the <see cref="AppDomain"/> that is exiting</param>
        /// <param name="e">null</param>
        /// <remarks>
        /// <para>
        /// Called when the <see cref="AppDomain.ProcessExit"/> event fires.
        /// </para>
        /// <para>
        /// When the event is triggered the log4net system is <see cref="M:Shutdown()"/>.
        /// </para>
        /// </remarks>
        private static void OnProcessExit(object sender,EventArgs e)
        {
            Shutdown();
        }


        /// <summary>
        /// Retrieves or creates a named logger.
        /// </summary>
        /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        /// <remarks>
        /// <para>
        /// Retrieves a logger named as the <paramref name="name"/>
        /// parameter. If the named logger already exists, then the
        /// existing instance will be returned. Otherwise, a new instance is
        /// created.
        /// </para>
        /// <para>
        /// By default, loggers do not have a set level but inherit
        /// it from the hierarchy. This is one of the central features of
        /// log4net.
        /// </para>
        /// </remarks>
        public static ILogger GetLogger(Assembly repositoryassmebly, string name)
        {
            if (repositoryassmebly == null)
                throw new ArgumentNullException("repositoryAssmebly");

            if (name == null)
                throw new ArgumentNullException("name");
            return m_RepositorySelector.GetRepository(repositoryassmebly).GetLogger(name);
        }
      

        /// <summary>
        /// Shuts down the log4net system.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calling this method will <b>safely</b> close and remove all
        /// appenders in all the loggers including root contained in all the
        /// default repositories.
        /// </para>
        /// <para>
        /// Some appenders need to be closed before the application exists. 
        /// Otherwise, pending logging events might be lost.
        /// </para>
        /// <para>
        /// The <c>shutdown</c> method is careful to close nested
        /// appenders before closing regular appenders. This is allows
        /// configurations where a regular appender is attached to a logger
        /// and again to a nested appender.
        /// </para>
        /// </remarks>
        public static void Shutdown()
        {
            foreach (ILoggerRepository repository in GetAllRepositories())
            {
                repository.Shutdown();
            }
        }

        /// <summary>
        /// Gets an array of all currently defined repositories.
        /// </summary>
        /// <returns>An array of all the known <see cref="ILoggerRepository"/> objects.</returns>
        /// <remarks>
        /// <para>
        /// Gets an array of all currently defined repositories.
        /// </para>
        /// </remarks>
        public static ILoggerRepository[] GetAllRepositories()
        {
            return RepositorySelector.GetAllRepositories();
        }

        /// <summary>
        /// Gets or sets the repository selector used by the <see cref="LogManager" />.
        /// </summary>
        /// <value>
        /// The repository selector used by the <see cref="LogManager" />.
        /// </value>
        /// <remarks>
        /// <para>
        /// The repository selector (<see cref="IRepositorySelector"/>) is used by 
        /// the <see cref="LogManager"/> to create and select repositories 
        /// (<see cref="ILoggerRepository"/>).
        /// </para>
        /// <para>
        /// The caller to <see cref="LogManager"/> supplies either a string name 
        /// or an assembly (if not supplied the assembly is inferred using 
        /// <see cref="M:Assembly.GetCallingAssembly()"/>).
        /// </para>
        /// <para>
        /// This context is used by the selector to lookup a specific repository.
        /// </para>
        /// <para>
        /// For the full .NET Framework, the default repository is <c>DefaultRepositorySelector</c>;
        /// for the .NET Compact Framework <c>CompactRepositorySelector</c> is the default
        /// repository.
        /// </para>
        /// </remarks>
        public static IRepositorySelector RepositorySelector
        {
            get { return m_RepositorySelector; }
            set { m_RepositorySelector = value; }
        }

        /// <summary>
        /// Returns the default <see cref="ILoggerRepository"/> instance.
        /// </summary>
        /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
        /// <returns>The default <see cref="ILoggerRepository"/> instance.</returns>
        /// <remarks>
        /// <para>
        /// Returns the default <see cref="ILoggerRepository"/> instance.
        /// </para>
        /// </remarks>
        public static ILoggerRepository GetRepository(Assembly repositoryAssembly)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            return RepositorySelector.GetRepository(repositoryAssembly);
        }

        /// <summary>
        /// The fully qualified type of the LoggerManager class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(LoggerManager);

        /// <summary>
        /// Initialize the default repository selector
        /// </summary>
        private static IRepositorySelector m_RepositorySelector;
    }
}
