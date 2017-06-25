using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Layout.Pattern
{
    public class ExStringFourPatternConverter : PatternLayoutConverter
    {

        protected override void Convert(System.IO.TextWriter writer, Core.LoggingData loggingEvent)
        {
            if (!string.IsNullOrEmpty(loggingEvent.ExStringFour))
                writer.Write(loggingEvent.ExStringFour);
        }
    }
}
