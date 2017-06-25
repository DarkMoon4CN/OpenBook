using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;
using Log4Simple.Util;

namespace Log4Simple.Layout.Pattern
{
    /// <para>
    /// The <see cref="LoggingEvent.TimeStamp"/> is in the local time zone and is rendered in that zone.
    /// To output the time in Universal time see <see cref="UtcDatePatternConverter"/>.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal class TimeStampPatternConverter : PatternLayoutConverter,IOptionHandler
    {

        /// <summary>
        /// Convert the pattern into the rendered message
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="loggingEvent">the event being logged</param>
        /// <remarks>
        /// <para>
        /// Pass the <see cref="LoggingData.TimeStamp"/> to the <see cref="IDateFormatter"/>
        /// for it to render it to the writer.
        /// </para>
        /// <para>
        /// The <see cref="LoggingData.TimeStamp"/> passed is in the local time zone.
        /// </para>
        /// </remarks>
        override protected void Convert(TextWriter writer, LoggingData loggingEvent)
        {
            try
            {
                writer.Write(loggingEvent.TimeStamp.ToString(Option));
              
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "Error occurred while converting date.", ex);
            }
        }

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the DatePatternConverter class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(TimeStampPatternConverter);

        #endregion Private Static Fields

        public void ActivateOptions()
        {
            if (string.IsNullOrEmpty(Option))
            {
                Option = "yyyy-MM-dd HH:mm:ss ffff";
            }    

        }
    }
}
