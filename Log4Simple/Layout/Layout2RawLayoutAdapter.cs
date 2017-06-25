using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;

namespace Log4Simple.Layout
{
    /// <summary>
    /// Adapts any <see cref="ILayout"/> to a <see cref="IRawLayout"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Where an <see cref="IRawLayout"/> is required this adapter
    /// allows a <see cref="ILayout"/> to be specified.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public class Layout2RawLayoutAdapter : IRawLayout
    {
        #region Member Variables

        /// <summary>
        /// The layout to adapt
        /// </summary>
        private ILayout m_layout;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new adapter
        /// </summary>
        /// <param name="layout">the layout to adapt</param>
        /// <remarks>
        /// <para>
        /// Create the adapter for the specified <paramref name="layout"/>.
        /// </para>
        /// </remarks>
        public Layout2RawLayoutAdapter(ILayout layout)
        {
            m_layout = layout;
        }

        #endregion

        #region Implementation of IRawLayout

        /// <summary>
        /// Format the logging event as an object.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>returns the formatted event</returns>
        /// <remarks>
        /// <para>
        /// Format the logging event as an object.
        /// </para>
        /// <para>
        /// Uses the <see cref="ILayout"/> object supplied to 
        /// the constructor to perform the formatting.
        /// </para>
        /// </remarks>
        virtual public object Format(LoggingData loggingEvent)
        {
            StringWriter writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
            m_layout.Format(writer, loggingEvent);
            return writer.ToString();
        }

        #endregion
    }
}
