using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Converter to include event thread name
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes the <see cref="LoggingData.ThreadName"/> to the output.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal sealed class ThreadPatternConverter : PatternLayoutConverter
    {
        /// <summary>
        /// Write the ThreadName to the output
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="loggingEvent">the event being logged</param>
        /// <remarks>
        /// <para>
        /// Writes the <see cref="LoggingData.ThreadName"/> to the <paramref name="writer" />.
        /// </para>
        /// </remarks>
        override protected void Convert(TextWriter writer, LoggingData loggingEvent)
        {
            writer.Write(loggingEvent.ThreadName);
        }
    }
}
