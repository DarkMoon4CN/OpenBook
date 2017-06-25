using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;

namespace Log4Simple.Layout
{
    /// <summary>
    /// Interface implemented by layout objects
    /// </summary>
    /// <remarks>
    /// <para>
    /// An <see cref="ILayout"/> object is used to format a <see cref="LoggingData"/>
    /// as text. The <see cref="M:Format(TextWriter,LoggingEvent)"/> method is called by an
    /// appender to transform the <see cref="LoggingData"/> into a string.
    /// </para>
    /// <para>
    /// The layout can also supply <see cref="Header"/> and <see cref="Footer"/>
    /// text that is appender before any events and after all the events respectively.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public interface ILayout
    {
        /// <summary>
        /// Implement this method to create your own layout format.
        /// </summary>
        /// <param name="writer">The TextWriter to write the formatted event to</param>
        /// <param name="loggingEvent">The event to format</param>
        /// <remarks>
        /// <para>
        /// This method is called by an appender to format
        /// the <paramref name="loggingEvent"/> as text and output to a writer.
        /// </para>
        /// <para>
        /// If the caller does not have a <see cref="TextWriter"/> and prefers the
        /// event to be formatted as a <see cref="String"/> then the following
        /// code can be used to format the event into a <see cref="StringWriter"/>.
        /// </para>
        /// <code lang="C#">
        /// StringWriter writer = new StringWriter();
        /// Layout.Format(writer, loggingEvent);
        /// string formattedEvent = writer.ToString();
        /// </code>
        /// </remarks>
        void Format(TextWriter writer, LoggingData loggingEvent);

        /// <summary>
        /// The content type output by this layout. 
        /// </summary>
        /// <value>The content type</value>
        /// <remarks>
        /// <para>
        /// The content type output by this layout.
        /// </para>
        /// <para>
        /// This is a MIME type e.g. <c>"text/plain"</c>.
        /// </para>
        /// </remarks>
        string ContentType { get; }

        /// <summary>
        /// The header for the layout format.
        /// </summary>
        /// <value>the layout header</value>
        /// <remarks>
        /// <para>
        /// The Header text will be appended before any logging events
        /// are formatted and appended.
        /// </para>
        /// </remarks>
        string Header { get; }

        /// <summary>
        /// The footer for the layout format.
        /// </summary>
        /// <value>the layout footer</value>
        /// <remarks>
        /// <para>
        /// The Footer text will be appended after all the logging events
        /// have been formatted and appended.
        /// </para>
        /// </remarks>
        string Footer { get; }

        /// <summary>
        /// Flag indicating if this layout handle exceptions
        /// </summary>
        /// <value><c>false</c> if this layout handles exceptions</value>
        /// <remarks>
        /// <para>
        /// If this layout handles the exception object contained within
        /// <see cref="LoggingData"/>, then the layout should return
        /// <c>false</c>. Otherwise, if the layout ignores the exception
        /// object, then the layout should return <c>true</c>.
        /// </para>
        /// </remarks>
        bool IgnoresException { get; }
    }
}
