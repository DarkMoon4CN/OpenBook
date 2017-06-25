using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Log4Simple.Core;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Write the event level to the output
    /// </summary>
    /// <remarks>
    /// <para>
    /// Writes the display name of the event <see cref="LoggingData.Level"/>
    /// to the writer.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    internal sealed class LevelPatternConverter : PatternLayoutConverter, IOptionHandler
    {

       

        /// <summary>
        ///  define get level format function
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        delegate string delFormat(Level level);

        /// <summary>
        /// return level 
        /// </summary>
        delFormat m_delFormat;



        /// <summary>
        /// get level by int
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        string GetLevelByInt(Level level)
        {
            return ((int)level).ToString();
        }

        /// <summary>
        /// get leve by string
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        string GetLevelByString(Level level)
        {
            return level.ToString();
        }


        /// <summary>
        /// Write the event level to the output
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="loggingEvent">the event being logged</param>
        /// <remarks>
        /// <para>
        /// Writes the <see cref="Level.DisplayName"/> of the <paramref name="loggingEvent"/> <see cref="LoggingData.Level"/>
        /// to the <paramref name="writer"/>.
        /// </para>
        /// </remarks>
        override protected void Convert(TextWriter writer, LoggingData loggingEvent)
        {
            writer.Write(m_delFormat(loggingEvent.Level));
        }


         

         /// <summary>
        /// Activate the options that were previously set with calls to properties.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This allows an object to defer activation of its options until all
        /// options have been set. This is required for components which have
        /// related options that remain ambiguous until all are set.
        /// </para>
        /// <para>
        /// If a component implements this interface then this method must be called
        /// after its properties have been set before the component can be used.
        /// </para>
        /// </remarks>
        public void ActivateOptions()
        {
            if (!string.IsNullOrEmpty(Option))
            {
                if (Option.ToLower() == "int")
                {
                    m_delFormat = GetLevelByInt;
                    return;
                }
            }    
            m_delFormat = GetLevelByString;
        }
    }
}
