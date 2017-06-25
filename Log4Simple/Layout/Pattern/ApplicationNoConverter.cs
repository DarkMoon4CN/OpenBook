using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Layout.Pattern
{
    public class ApplicationNoConverter :PatternLayoutConverter
    {

        protected override void Convert(System.IO.TextWriter writer, Core.LoggingData loggingEvent)
        {
            writer.Write(loggingEvent.AppNo);
        }
    }
}
