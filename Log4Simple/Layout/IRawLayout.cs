using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Log4Simple.Core;

namespace Log4Simple.Layout
{
    /// <summary>
    /// Interface for raw layout objects
    /// </summary>
    /// <remarks>
    /// <para>
    /// Interface used to format a <see cref="LoggingData"/>
    /// to an object.
    /// </para>
    /// <para>
    /// This interface should not be confused with the
    /// <see cref="ILayout"/> interface. This interface is used in
    /// only certain specialized situations where a raw object is
    /// required rather than a formatted string. The <see cref="ILayout"/>
    /// is not generally useful than this interface.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    [TypeConverter(typeof(RawLayoutConverter))]
    public interface IRawLayout
    {
        /// <summary>
        /// Implement this method to create your own layout format.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>returns the formatted event</returns>
        /// <remarks>
        /// <para>
        /// Implement this method to create your own layout format.
        /// </para>
        /// </remarks>
        object Format(LoggingData loggingEvent);
    }
}
