using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Layout.Pattern
{
    public class ExStringTwoPatternConverter : PatternLayoutConverter
    {

        protected override void Convert(System.IO.TextWriter writer, Core.LoggingData loggingEvent)
        {
            if (!string.IsNullOrEmpty(loggingEvent.ExStringTwo))
                writer.Write(loggingEvent.ExStringTwo);
        }
    }
}
