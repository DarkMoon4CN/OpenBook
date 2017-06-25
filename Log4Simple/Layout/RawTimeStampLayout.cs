using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;

namespace Log4Simple.Layout
{
    /// <summary>
    /// Extract the date from the <see cref="LoggingData"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Extract the date from the <see cref="LoggingData"/>
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public class RawTimeStampLayout : IRawLayout
    {
        #region Constructors

        /// <summary>
        /// Constructs a RawTimeStampLayout
        /// </summary>
        public RawTimeStampLayout()
        {
        }

        #endregion

        #region Implementation of IRawLayout

        /// <summary>
        /// Gets the <see cref="LoggingData.TimeStamp"/> as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="loggingEvent">The event to format</param>
        /// <returns>returns the time stamp</returns>
        /// <remarks>
        /// <para>
        /// Gets the <see cref="LoggingData.TimeStamp"/> as a <see cref="DateTime"/>.
        /// </para>
        /// <para>
        /// The time stamp is in local time. To format the time stamp
        /// in universal time use <see cref="RawUtcTimeStampLayout"/>.
        /// </para>
        /// </remarks>
        public virtual object Format(LoggingData loggingEvent)
        {
            return loggingEvent.TimeStamp;
        }

        #endregion
    }
}
