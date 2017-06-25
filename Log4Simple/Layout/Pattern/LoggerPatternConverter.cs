using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Converter for logger name
    /// </summary>
    /// <remarks>
    /// <para>
    /// Outputs the <see cref="LoggingData.LoggerName"/> of the event.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal sealed class LoggerPatternConverter : PatternLayoutConverter
    {
        /// <summary>
        /// Gets the fully qualified name of the logger
        /// </summary>
        /// <param name="loggingEvent">the event being logged</param>
        /// <returns>The fully qualified logger name</returns>
        /// <remarks>
        /// <para>
        /// Returns the <see cref="LoggingData.LoggerName"/> of the <paramref name="loggingEvent"/>.
        /// </para>
        /// </remarks>
        protected override void Convert(System.IO.TextWriter writer, Core.LoggingData loggingEvent)
        {
            writer.Write(loggingEvent.LoggerName);
        }
    }
}
