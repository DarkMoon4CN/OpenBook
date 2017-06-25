using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Log4Simple.Repository;
using System.Collections;
using Log4Simple.Util;
using Log4Simple.Config;
using Log4Simple.Repository.Hierarchy;

namespace Log4Simple.Core
{

    /// <summary>
    /// The default implementation of the <see cref="IRepositorySelector"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Uses attributes defined on the calling assembly to determine how to
    /// configure the hierarchy for the repository.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public class DefaultRepositorySelector :IRepositorySelector
    {


        #region Public Events

        /// <summary>
        /// Event to notify that a logger repository has been created.
        /// </summary>
        /// <value>
        /// Event to notify that a logger repository has been created.
        /// </value>
        /// <remarks>
        /// <para>
        /// Event raised when a new repository is created.
        /// The event source will be this selector. The event args will
        /// be a <see cref="LoggerRepositoryCreationEventArgs"/> which
        /// holds the newly created <see cref="ILoggerRepository"/>.
        /// </para>
        /// </remarks>
        public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
        {
            add { m_loggerRepositoryCreatedEvent += value; }
            remove { m_loggerRepositoryCreatedEvent -= value; }
        }

        #endregion Public Events


        #region Public Instance Constructors

        /// <summary>
        /// Creates a new repository selector.
        /// </summary>
        /// <param name="defaultRepositoryType">The type of the repositories to create, must implement <see cref="ILoggerRepository"/></param>
        /// <remarks>
        /// <para>
        /// Create an new repository selector.
        /// The default type for repositories must be specified,
        /// an appropriate value would be <see cref="log4net.Repository.Hierarchy.Hierarchy"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="defaultRepositoryType"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="defaultRepositoryType"/> does not implement <see cref="ILoggerRepository"/>.</exception>
        public DefaultRepositorySelector(Type defaultRepositoryType)
        {
            if (defaultRepositoryType == null)
            {
                throw new ArgumentNullException("defaultRepositoryType");
            }

            // Check that the type is a repository
            if (!(typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType)))
            {
                throw  new ArgumentOutOfRangeException("defaultRepositoryType",defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
            }

            m_DefaultRepositoryType = defaultRepositoryType;

            LogLog.Debug(declaringType, "defaultRepositoryType [" + m_DefaultRepositoryType + "]");
        }

        #endregion Public Instance Constructors


        /// <summary>
        /// Gets the <see cref="ILoggerRepository"/> for the specified assembly.
        /// </summary>
        /// <param name="repositoryAssembly">The assembly use to lookup the <see cref="ILoggerRepository"/>.</param>
        /// <remarks>
        /// <para>
        /// The type of the <see cref="ILoggerRepository"/> created and the repository 
        /// to create can be overridden by specifying the <see cref="log4net.Config.RepositoryAttribute"/> 
        /// attribute on the <paramref name="repositoryAssembly"/>.
        /// </para>
        /// <para>
        /// The default values are to use the <see cref="log4net.Repository.Hierarchy.Hierarchy"/> 
        /// implementation of the <see cref="ILoggerRepository"/> interface and to use the
        /// <see cref="AssemblyName.Name"/> as the name of the repository.
        /// </para>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be automatically configured using 
        /// any <see cref="log4net.Config.ConfiguratorAttribute"/> attributes defined on
        /// the <paramref name="repositoryAssembly"/>.
        /// </para>
        /// </remarks>
        /// <returns>The <see cref="ILoggerRepository"/> for the assembly</returns>
        /// <exception cref="ArgumentNullException"><paramref name="repositoryAssembly"/> is <see langword="null" />.</exception>
        public ILoggerRepository GetRepository(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            return CreateRepository(assembly, m_DefaultRepositoryType);
        }


        /// <summary>
        /// Gets the <see cref="ILoggerRepository"/> for the specified repository.
        /// </summary>
        /// <param name="repositoryName">The repository to use to lookup the <see cref="ILoggerRepository"/>.</param>
        /// <returns>The <see cref="ILoggerRepository"/> for the specified repository.</returns>
        /// <remarks>
        /// <para>
        /// Returns the named repository. If <paramref name="repositoryName"/> is <c>null</c>
        /// a <see cref="ArgumentNullException"/> is thrown. If the repository 
        /// does not exist a <see cref="LogException"/> is thrown.
        /// </para>
        /// <para>
        /// Use <see cref="M:CreateRepository(string, Type)"/> to create a repository.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="repositoryName"/> is <see langword="null" />.</exception>
        /// <exception cref="LogException"><paramref name="repositoryName"/> does not exist.</exception>
        public ILoggerRepository GetRepository(string repositoryname)
        {
            if (string.IsNullOrEmpty(repositoryname))
                throw new ArgumentException("repositoryname");
            lock (this)
            {
                ILoggerRepository rep = m_Name2repositoryMap[repositoryname] as ILoggerRepository;
                if (rep == null)
                    throw new ApplicationException("Repository[" + repositoryname + "] is NOT defined.");
                return rep;
            }
        }

        /// <summary>
        /// Create a new repository for the assembly specified 
        /// </summary>
        /// <param name="repositoryAssembly">the assembly to use to create the repository to associate with the <see cref="ILoggerRepository"/>.</param>
        /// <param name="repositoryType">The type of repository to create, must implement <see cref="ILoggerRepository"/>.</param>
        /// <returns>The repository created.</returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be associated with the repository
        /// specified such that a call to <see cref="M:GetRepository(Assembly)"/> with the
        /// same assembly specified will return the same repository instance.
        /// </para>
        /// <para>
        /// The type of the <see cref="ILoggerRepository"/> created and
        /// the repository to create can be overridden by specifying the
        /// <see cref="log4net.Config.RepositoryAttribute"/> attribute on the 
        /// <paramref name="repositoryAssembly"/>.  The default values are to use the 
        /// <paramref name="repositoryType"/> implementation of the 
        /// <see cref="ILoggerRepository"/> interface and to use the
        /// <see cref="AssemblyName.Name"/> as the name of the repository.
        /// </para>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be automatically
        /// configured using any <see cref="log4net.Config.ConfiguratorAttribute"/> 
        /// attributes defined on the <paramref name="repositoryAssembly"/>.
        /// </para>
        /// <para>
        /// If a repository for the <paramref name="repositoryAssembly"/> already exists
        /// that repository will be returned. An error will not be raised and that 
        /// repository may be of a different type to that specified in <paramref name="repositoryType"/>.
        /// Also the <see cref="log4net.Config.RepositoryAttribute"/> attribute on the
        /// assembly may be used to override the repository type specified in 
        /// <paramref name="repositoryType"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="repositoryAssembly"/> is <see langword="null" />.</exception>
        public ILoggerRepository CreateRepository(Assembly assembly, Type repositorytype)
        {
            return CreateRepository(assembly, repositorytype, DefaultRepositoryName, true);
        }

        /// <summary>
        /// Creates a new repository for the specified repository.
        /// </summary>
        /// <param name="repositoryName">The repository to associate with the <see cref="ILoggerRepository"/>.</param>
        /// <param name="repositoryType">The type of repository to create, must implement <see cref="ILoggerRepository"/>.
        /// If this param is <see langword="null" /> then the default repository type is used.</param>
        /// <returns>The new repository.</returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be associated with the repository
        /// specified such that a call to <see cref="M:GetRepository(string)"/> with the
        /// same repository specified will return the same repository instance.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="repositoryName"/> is <see langword="null" />.</exception>
        /// <exception cref="LogException"><paramref name="repositoryName"/> already exists.</exception>
        public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (string.IsNullOrEmpty(repositoryName))
                throw new ArgumentNullException("repositoryname");

            if (repositoryType == null)
                repositoryType = m_DefaultRepositoryType;
            
            lock (this)
            {
                ILoggerRepository rep = null;
                rep = m_Name2repositoryMap[repositoryName] as ILoggerRepository;
                if (rep != null)
                {
                    throw new ApplicationException("Repository[" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                else
                {
                    LogLog.Debug(declaringType, "Creating repository [" + repositoryName + "] using type [" + repositoryType + "]");

                    rep = (ILoggerRepository)Activator.CreateInstance(repositoryType);
                    rep.Name = repositoryName;
                    m_Name2repositoryMap[repositoryName] = rep;                   
                }
                return rep;
            }
        }

        /// <summary>
        /// Test if a named repository exists
        /// </summary>
        /// <param name="repositoryName">the named repository to check</param>
        /// <returns><c>true</c> if the repository exists</returns>
        /// <remarks>
        /// <para>
        /// Test if a named repository exists. Use <see cref="M:CreateRepository(string, Type)"/>
        /// to create a new repository and <see cref="M:GetRepository(string)"/> to retrieve 
        /// a repository.
        /// </para>
        /// </remarks>
        public bool ExistsRepository(string repositoryname)
        {
            lock (this)
            {
                return m_Name2repositoryMap.ContainsKey(repositoryname);
            }
        }

        /// <summary>
        /// Gets a list of <see cref="ILoggerRepository"/> objects
        /// </summary>
        /// <returns>an array of all known <see cref="ILoggerRepository"/> objects</returns>
        /// <remarks>
        /// <para>
        /// Gets an array of all of the repositories created by this selector.
        /// </para>
        /// </remarks>
        public ILoggerRepository[] GetAllRepositories()
        {
            lock (this)
            {
                ICollection reps = m_Name2repositoryMap.Values;
                ILoggerRepository[] all = new ILoggerRepository[reps.Count];
                reps.CopyTo(all, 0);
                return all;
            }
        }




        /// <summary>
        /// Creates a new repository for the assembly specified.
        /// </summary>
        /// <param name="repositoryAssembly">the assembly to use to create the repository to associate with the <see cref="ILoggerRepository"/>.</param>
        /// <param name="repositoryType">The type of repository to create, must implement <see cref="ILoggerRepository"/>.</param>
        /// <param name="repositoryName">The name to assign to the created repository</param>
        /// <param name="readAssemblyAttributes">Set to <c>true</c> to read and apply the assembly attributes</param>
        /// <returns>The repository created.</returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be associated with the repository
        /// specified such that a call to <see cref="M:GetRepository(Assembly)"/> with the
        /// same assembly specified will return the same repository instance.
        /// </para>
        /// <para>
        /// The type of the <see cref="ILoggerRepository"/> created and
        /// the repository to create can be overridden by specifying the
        /// <see cref="log4net.Config.RepositoryAttribute"/> attribute on the 
        /// <paramref name="repositoryAssembly"/>.  The default values are to use the 
        /// <paramref name="repositoryType"/> implementation of the 
        /// <see cref="ILoggerRepository"/> interface and to use the
        /// <see cref="AssemblyName.Name"/> as the name of the repository.
        /// </para>
        /// <para>
        /// The <see cref="ILoggerRepository"/> created will be automatically
        /// configured using any <see cref="log4net.Config.ConfiguratorAttribute"/> 
        /// attributes defined on the <paramref name="repositoryAssembly"/>.
        /// </para>
        /// <para>
        /// If a repository for the <paramref name="repositoryAssembly"/> already exists
        /// that repository will be returned. An error will not be raised and that 
        /// repository may be of a different type to that specified in <paramref name="repositoryType"/>.
        /// Also the <see cref="log4net.Config.RepositoryAttribute"/> attribute on the
        /// assembly may be used to override the repository type specified in 
        /// <paramref name="repositoryType"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="repositoryAssembly"/> is <see langword="null" />.</exception>
        public ILoggerRepository CreateRepository(Assembly repositoryassembly, Type repositorytype, string repositoryname, bool readassemblyattributes)
        {
            if (repositoryassembly == null)
                throw new ArgumentNullException("repositoryassembly");

            if (repositorytype == null)
                repositorytype = m_DefaultRepositoryType;
            lock (this)
            {
                ILoggerRepository rep = m_Name2repositoryMap[repositoryname] as ILoggerRepository;
                if (rep == null)
                {
                    rep = CreateRepository(repositoryname, repositorytype);
                    if (readassemblyattributes)
                    {
                        try
                        {
                            ConfigureRepository(repositoryassembly, rep);
                        }
                        catch(Exception ex)
                        {
                            LogLog.Error(declaringType, ex.Message);
                        }
                    }                   
                }
                return rep;
            }
        }

        /// <summary>
        /// Configures the repository using information from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing <see cref="log4net.Config.ConfiguratorAttribute"/>
        /// attributes which define the configuration for the repository.</param>
        /// <param name="repository">The repository to configure.</param>
        /// <exception cref="ArgumentNullException">
        ///	<para><paramref name="assembly" /> is <see langword="null" />.</para>
        ///	<para>-or-</para>
        ///	<para><paramref name="repository" /> is <see langword="null" />.</para>
        /// </exception>
        private void ConfigureRepository(Assembly assembly, ILoggerRepository repository)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            object[] configAttributes = Attribute.GetCustomAttributes(assembly, typeof(Log4Simple.Config.ConfiguratorAttribute), false);
            if (configAttributes.Length == 0)
            {
                configAttributes = new Log4Simple.Config.ConfiguratorAttribute[] { new XmlConfiguratorAttribute() { ConfigFile = XmlHierarchyConfigurator.CONFIGURATION_TAG,ConfigFileExtension =  "xml" } };

            }
            if (configAttributes != null && configAttributes.Length > 0)
            {
                foreach (Log4Simple.Config.ConfiguratorAttribute attr in configAttributes)
                {
                    if (attr != null)
                    {
                        attr.Configure(assembly, repository);
                    }
                }
            }
        }

        /// <summary>
        /// The fully qualified type of the DefaultRepositorySelector class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(DefaultRepositorySelector);

        private const string DefaultRepositoryName = "log4simple-default-repository";

        private readonly Type m_DefaultRepositoryType;
        private readonly Hashtable m_Name2repositoryMap = new Hashtable();

        private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;
    }
}
