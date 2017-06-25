using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Log4Simple.Core;
using Log4Simple;
using Log4Simple.Repository;

namespace Log4Simple
{
    public class LogManager
    {

        private static readonly WrapperMap s_wrapperMap = new WrapperMap(new WrapperCreationHandler(WrapperCreationHandler));

        private LogManager()
        { 
        }

        /// <summary>
        /// Create the <see cref="ILoggerWrapper"/> objects used by
        /// this manager.
        /// </summary>
        /// <param name="logger">The logger to wrap.</param>
        /// <returns>The wrapper for the logger specified.</returns>
        private static ILoggerWrapper WrapperCreationHandler(ILogger logger)
        {
            return new LogImpl(logger);
        }


        /// <summary>
        /// Looks up the wrapper object for the logger specified.
        /// </summary>
        /// <param name="logger">The logger to get the wrapper for.</param>
        /// <returns>The wrapper for the logger specified.</returns>
        private static ILog WrapLogger(ILogger logger)
        {
            return (ILog)s_wrapperMap.GetWrapper(logger);
        }


        /// <overloads>Returns the named logger if it exists.</overloads>
        /// <summary>
        /// Returns the named logger if it exists.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the named logger exists (in the default repository) then it
        /// returns a reference to the logger, otherwise it returns <c>null</c>.
        /// </para>
        /// </remarks>
        /// <param name="name">The fully qualified logger name to look for.</param>
        /// <returns>The logger found, or <c>null</c> if no logger could be found.</returns>
        public static ILog GetLogger(string name)
        {
            return GetLogger(Assembly.GetCallingAssembly(), name);
        }

        /// <summary>
        /// Shorthand for <see cref="M:LogManager.GetLogger(string)"/>.
        /// </summary>
        /// <remarks>
        /// Get the logger for the fully qualified name of the type specified.
        /// </remarks>
        /// <param name="type">The full name of <paramref name="type"/> will be used as the name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        public static ILog GetLogger(Type type)
        {
            return GetLogger(Assembly.GetCallingAssembly(), type.FullName);
        }

        /// <summary>
        /// Retrieves or creates a named logger.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Retrieve a logger named as the <paramref name="name"/>
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
        /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
        /// <param name="name">The name of the logger to retrieve.</param>
        /// <returns>The logger with the name specified.</returns>
        public static ILog GetLogger(Assembly repositoryassembly,string name)
        {
            return WrapLogger(LoggerManager.GetLogger(repositoryassembly, name));
        }



        /// <overloads>Get a logger repository.</overloads>
        /// <summary>
        /// Returns the default <see cref="ILoggerRepository"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gets the <see cref="ILoggerRepository"/> for the repository specified
        /// by the callers assembly (<see cref="M:Assembly.GetCallingAssembly()"/>).
        /// </para>
        /// </remarks>
        /// <returns>The <see cref="ILoggerRepository"/> instance for the default repository.</returns>
        public static ILoggerRepository GetRepository()
        {
            return GetRepository(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Returns the default <see cref="ILoggerRepository"/> instance.
        /// </summary>
        /// <returns>The default <see cref="ILoggerRepository"/> instance.</returns>
        /// <remarks>
        /// <para>
        /// Gets the <see cref="ILoggerRepository"/> for the repository specified
        /// by the <paramref name="repositoryAssembly"/> argument.
        /// </para>
        /// </remarks>
        /// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
        public static ILoggerRepository GetRepository(Assembly repositoryAssembly)
        {
            return LoggerManager.GetRepository(repositoryAssembly);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void ShutDown()
        {
            LoggerManager.Shutdown();
        }
    }
}
