using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;

namespace Log4Simple.Core
{
    /// <summary>
    /// Implementation of the <see cref="ILoggerWrapper"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be used as the base for all wrapper implementations.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public abstract class LoggerWrapperImpl : ILoggerWrapper
    {
        #region Protected Instance Constructors

        /// <summary>
        /// Constructs a new wrapper for the specified logger.
        /// </summary>
        /// <param name="logger">The logger to wrap.</param>
        /// <remarks>
        /// <para>
        /// Constructs a new wrapper for the specified logger.
        /// </para>
        /// </remarks>
        protected LoggerWrapperImpl(ILogger logger)
        {
            m_logger = logger;
        }

        #endregion Public Instance Constructors

        #region Implementation of ILoggerWrapper

        /// <summary>
        /// Gets the implementation behind this wrapper object.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> object that this object is implementing.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <c>Logger</c> object may not be the same object as this object 
        /// because of logger decorators.
        /// </para>
        /// <para>
        /// This gets the actual underlying objects that is used to process
        /// the log events.
        /// </para>
        /// </remarks>
        virtual public ILogger Logger
        {
            get { return m_logger; }
        }

        #endregion

        #region Private Instance Fields

        /// <summary>
        /// The logger that this object is wrapping
        /// </summary>
        private readonly ILogger m_logger;

        #endregion Private Instance Fields
    }
}
