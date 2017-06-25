using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Log4Simple.Layout.Pattern;
using System.IO;
using Log4Simple.Core;
using Log4Simple.Util;

namespace Log4Simple.Layout
{
    /// A similar pattern except that the relative time is
    /// right padded if less than 6 digits, thread name is right padded if
    /// less than 15 characters and truncated if longer and the logger
    /// name is left padded if shorter than 30 characters and truncated if
    /// longer.
    /// <code><b>%-6timestamp [%15.15thread] %-5level %30.30logger %ndc - %message%newline</b></code>
    /// </example>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    /// <author>Douglas de la Torre</author>
    /// <author>Daniel Cazzulino</author>
    public class PatternLayout : LayoutSkeleton
    {
        #region Constants

        /// <summary>
        /// Default pattern string for log output. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Default pattern string for log output. 
        /// Currently set to the string <b>"%message%newline"</b> 
        /// which just prints the application supplied message. 
        /// </para>
        /// </remarks>
        public const string DefaultConversionPattern = "%message%newline";

        /// <summary>
        /// A detailed conversion pattern
        /// </summary>
        /// <remarks>
        /// <para>
        /// A conversion pattern which includes Time, Thread, Logger, and Nested Context.
        /// Current value is <b>%timestamp [%thread] %level %logger %ndc - %message%newline</b>.
        /// </para>
        /// </remarks>
        public const string DetailConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";

        #endregion

        #region Static Fields

        

        #endregion Static Fields

        #region Member Variables

        /// <summary>
        /// the pattern
        /// </summary>
        private string m_pattern;

        /// <summary>
        /// the head of the pattern converter chain
        /// </summary>
        private PatternConverter m_head;

        /// <summary>
        /// patterns defined on this PatternLayout only
        /// </summary>
        private Hashtable m_instanceRulesRegistry = new Hashtable();

        #endregion

       
        #region Constructors

        /// <summary>
        /// Constructs a PatternLayout using the DefaultConversionPattern
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default pattern just produces the application supplied message.
        /// </para>
        /// <para>
        /// Note to Inheritors: This constructor calls the virtual method
        /// <see cref="CreatePatternParser"/>. If you override this method be
        /// aware that it will be called before your is called constructor.
        /// </para>
        /// <para>
        /// As per the <see cref="IOptionHandler"/> contract the <see cref="ActivateOptions"/>
        /// method must be called after the properties on this object have been
        /// configured.
        /// </para>
        /// </remarks>
        //public PatternLayout()
        //    : this(DefaultConversionPattern)
        //{
        //}

        /// <summary>
        /// Constructs a PatternLayout using the supplied conversion pattern
        /// </summary>
        /// <param name="pattern">the pattern to use</param>
        /// <remarks>
        /// <para>
        /// Note to Inheritors: This constructor calls the virtual method
        /// <see cref="CreatePatternParser"/>. If you override this method be
        /// aware that it will be called before your is called constructor.
        /// </para>
        /// <para>
        /// When using this constructor the <see cref="ActivateOptions"/> method 
        /// need not be called. This may not be the case when using a subclass.
        /// </para>
        /// </remarks>
        //public PatternLayout(string pattern)
        //{
        //    // By default we do not process the exception
        //    IgnoresException = true;

        //    m_pattern = pattern;
        //    if (m_pattern == null)
        //    {
        //        m_pattern = DefaultConversionPattern;
        //    }

        //    ActivateOptions();
        //}

        #endregion

        /// <summary>
        /// The pattern formatting string
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <b>ConversionPattern</b> option. This is the string which
        /// controls formatting and consists of a mix of literal content and
        /// conversion specifiers.
        /// </para>
        /// </remarks>
        public string ConversionPattern
        {
            get { return m_pattern; }
            set { m_pattern = value; }
        }

        /// <summary>
        /// Create the pattern parser instance
        /// </summary>
        /// <param name="pattern">the pattern to parse</param>
        /// <returns>The <see cref="PatternParser"/> that will format the event</returns>
        /// <remarks>
        /// <para>
        /// Creates the <see cref="PatternParser"/> used to parse the conversion string. Sets the
        /// global and instance rules on the <see cref="PatternParser"/>.
        /// </para>
        /// </remarks>
        virtual protected PatternParser CreatePatternParser(string pattern)
        {
            PatternParser patternParser = new PatternParser(pattern);

            

            return patternParser;
        }

        #region Implementation of IOptionHandler

        /// <summary>
        /// Initialize layout options
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
            m_head = CreatePatternParser(m_pattern).Parse();

            PatternConverter curConverter = m_head;
            while (curConverter != null)
            {
                PatternLayoutConverter layoutConverter = curConverter as PatternLayoutConverter;
                if (layoutConverter != null)
                {
                    if (!layoutConverter.IgnoresException)
                    {
                        // Found converter that handles the exception
                        this.IgnoresException = false;

                        break;
                    }
                }
                curConverter = curConverter.Next;
            }
        }

        #endregion

        #region Override implementation of LayoutSkeleton

        /// <summary>
        /// Produces a formatted string as specified by the conversion pattern.
        /// </summary>
        /// <param name="loggingEvent">the event being logged</param>
        /// <param name="writer">The TextWriter to write the formatted event to</param>
        /// <remarks>
        /// <para>
        /// Parse the <see cref="LoggingData"/> using the patter format
        /// specified in the <see cref="ConversionPattern"/> property.
        /// </para>
        /// </remarks>
        override public void Format(TextWriter writer, LoggingData loggingEvent)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            PatternConverter c = m_head;

            // loop through the chain of pattern converters
            while (c != null)
            {
                c.Format(writer, loggingEvent);
                c = c.Next;
            }
        }

        #endregion

               
    }
}
