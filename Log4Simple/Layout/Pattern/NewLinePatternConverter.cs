using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Writes a newline to the output
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes the system dependent line terminator to the output.
    /// This behavior can be overridden by setting the <see cref="PatternConverter.Option"/>:
    /// </para>
    /// <list type="definition">
    ///   <listheader>
    ///     <term>Option Value</term>
    ///     <description>Output</description>
    ///   </listheader>
    ///   <item>
    ///     <term>DOS</term>
    ///     <description>DOS or Windows line terminator <c>"\r\n"</c></description>
    ///   </item>
    ///   <item>
    ///     <term>UNIX</term>
    ///     <description>UNIX line terminator <c>"\n"</c></description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal sealed class NewLinePatternConverter : LiteralPatternConverter
    {
        #region Implementation of IOptionHandler

        /// <summary>
        /// Initialize the converter
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is part of the <see cref="IOptionHandler"/> delayed object
        /// activation scheme. The <see cref="ActivateOptions"/> method must 
        /// be called on this object after the configuration properties have
        /// been set. Until <see cref="ActivateOptions"/> is called this
        /// object is in an undefined state and must not be used. 
        /// </para>
        /// <para>
        /// If any of the configuration properties are modified then 
        /// <see cref="ActivateOptions"/> must be called again.
        /// </para>
        /// </remarks>
        override public void ActivateOptions()
        {

            Option = System.Environment.NewLine;
        }

        #endregion
    }
}
