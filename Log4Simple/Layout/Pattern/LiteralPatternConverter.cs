using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Pattern converter for literal string instances in the pattern
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes the literal string value specified in the 
    /// <see cref="log4net.Util.PatternConverter.Option"/> property to 
    /// the output.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal class LiteralPatternConverter : PatternConverter,IOptionHandler
    {        

        /// <summary>
        /// Write the literal to the output
        /// </summary>
        /// <param name="writer">the writer to write to</param>
        /// <param name="state">null, not set</param>
        /// <remarks>
        /// <para>
        /// Override the formatting behavior to ignore the FormattingInfo
        /// because we have a literal instead.
        /// </para>
        /// <para>
        /// Writes the value of <see cref="log4net.Util.PatternConverter.Option"/>
        /// to the output <paramref name="writer"/>.
        /// </para>
        /// </remarks>
        override public void Format(TextWriter writer, object state)
        {
            writer.Write(Option);
        }

        /// <summary>
        /// Convert this pattern into the rendered message
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="state">null, not set</param>
        /// <remarks>
        /// <para>
        /// This method is not used.
        /// </para>
        /// </remarks>
        override protected void Convert(TextWriter writer, object state)
        {
            throw new InvalidOperationException("Should never get here because of the overridden Format method");
        }

        virtual public void ActivateOptions()
        {
            
        }
    }
}
