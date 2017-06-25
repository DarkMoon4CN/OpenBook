using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;

namespace Log4Simple.Appender
{
    
        /// <summary>
        /// Implement this interface for your own strategies for printing log statements.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Implementors should consider extending the <see cref="AppenderSkeleton"/>
        /// class which provides a default implementation of this interface.
        /// </para>
        /// <para>
        /// Appenders can also implement the <see cref="IOptionHandler"/> interface. Therefore
        /// they would require that the <see cref="M:IOptionHandler.ActivateOptions()"/> method
        /// be called after the appenders properties have been configured.
        /// </para>
        /// </remarks>
        /// <author>Nicko Cadell</author>
        /// <author>Gert Driesen</author>
    public interface IAppender
    {
        /// <summary>
        /// Closes the appender and releases resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Releases any resources allocated within the appender such as file handles, 
        /// network connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        void Close();

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        /// <param name="loggingEvent">The event to log</param>
        /// <remarks>
        /// <para>
        /// This method is called to log a message into this appender.
        /// </para>
        /// </remarks>
        void DoAppend(LoggingData loggingEvent);

        /// <summary>
        /// Gets or sets the name of this appender.
        /// </summary>
        /// <value>The name of the appender.</value>
        /// <remarks>
        /// <para>The name uniquely identifies the appender.</para>
        /// </remarks>
        string Name { get; set; }

      
    }
    
}
