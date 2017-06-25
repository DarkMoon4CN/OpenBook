using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Log4Simple.Core
{
    /// <summary>
    /// Exception base type for log4net.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type extends <see cref="ApplicationException"/>. It
    /// does not add any new functionality but does differentiate the
    /// type of exception being thrown.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
#if !NETCF
    [Serializable]
#endif
    public class LogException : ApplicationException
    {
        #region Public Instance Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="LogException" /> class.
        /// </para>
        /// </remarks>
        public LogException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message to include with the exception.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="LogException" /> class with
        /// the specified message.
        /// </para>
        /// </remarks>
        public LogException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">A message to include with the exception.</param>
        /// <param name="innerException">A nested exception to include.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="LogException" /> class
        /// with the specified message and inner exception.
        /// </para>
        /// </remarks>
        public LogException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion Public Instance Constructors

        #region Protected Instance Constructors

#if !NETCF
        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="LogException" /> class 
        /// with serialized data.
        /// </para>
        /// </remarks>
        protected LogException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        #endregion Protected Instance Constructors
    }
}
