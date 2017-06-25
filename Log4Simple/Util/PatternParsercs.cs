using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Layout.Pattern;
using Log4Simple.Core;
using System.Collections;

namespace Log4Simple.Util
{
    /// <summary>
    /// Most of the work of the <see cref="PatternLayout"/> class
    /// is delegated to the PatternParser class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>PatternParser</c> processes a pattern string and
    /// returns a chain of <see cref="PatternConverter"/> objects.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public sealed class PatternParser
    {

         #region Static Constructor

        /// <summary>
        /// Initialize the global registry
        /// </summary>
        /// <remarks>
        /// <para>
        /// Defines the builtin global rules.
        /// </para>
        /// </remarks>
        static PatternParser()
        {
            //s_globalRulesRegistry = new Hashtable(45);

            s_globalRulesRegistry["literal"] = typeof(LiteralPatternConverter);

            s_globalRulesRegistry["newline"] = typeof(NewLinePatternConverter);      

            s_globalRulesRegistry["logger"] = typeof(LoggerPatternConverter);
                 
            s_globalRulesRegistry["date"] = typeof(TimeStampPatternConverter);
                            
            s_globalRulesRegistry["message"] = typeof(MessagePatternConverter);
            
            s_globalRulesRegistry["moudlename"] = typeof(MoudleNamePatternConverter);
            
            s_globalRulesRegistry["level"] = typeof(LevelPatternConverter);
                           
            s_globalRulesRegistry["thread"] = typeof(ThreadPatternConverter);

            s_globalRulesRegistry["appno"] = typeof(ApplicationNoConverter);

            s_globalRulesRegistry["unique"] = typeof(UniquePatternConverter);



            s_globalRulesRegistry["exint"] = typeof(ExIntOnePatternConverter);
            s_globalRulesRegistry["exintone"] = typeof(ExIntOnePatternConverter);
            s_globalRulesRegistry["exinttwo"] = typeof(ExIntTwoPatternConverter);
            s_globalRulesRegistry["exintthree"] = typeof(ExIntThreePatternConverter);
            s_globalRulesRegistry["exintfour"] = typeof(ExIntFourPatternConverter);


            s_globalRulesRegistry["exstring"] = typeof(ExStringOnePatternConverter);
            s_globalRulesRegistry["exstringone"] = typeof(ExStringOnePatternConverter);
            s_globalRulesRegistry["exstringtwo"] = typeof(ExStringTwoPatternConverter);
            s_globalRulesRegistry["exstringthree"] = typeof(ExStringThreePatternConverter);
            s_globalRulesRegistry["exstringfour"] = typeof(ExStringFourPatternConverter);




            //s_globalRulesRegistry.Add("u", typeof(IdentityPatternConverter));
            //s_globalRulesRegistry.Add("identity", typeof(IdentityPatternConverter));

        }

        #endregion Static Constructor


        #region Public Instance Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">The pattern to parse.</param>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="PatternParser" /> class 
        /// with the specified pattern string.
        /// </para>
        /// </remarks>
        public PatternParser(string pattern)
        {
            m_pattern = pattern;
        }

        #endregion Public Instance Constructors

        #region Public Instance Methods

        /// <summary>
        /// Parses the pattern into a chain of pattern converters.
        /// </summary>
        /// <returns>The head of a chain of pattern converters.</returns>
        /// <remarks>
        /// <para>
        /// Parses the pattern into a chain of pattern converters.
        /// </para>
        /// </remarks>
        public PatternConverter Parse()
        {            

            ParseInternal(m_pattern);

            return m_head;
        }
        

        #endregion Public Instance Methods

       /// <summary>
		/// Internal method to parse the specified pattern to find specified matches
		/// </summary>
		/// <param name="pattern">the pattern to parse</param>
		/// <param name="matches">the converter names to match in the pattern</param>
		/// <remarks>
		/// <para>
		/// The matches param must be sorted such that longer strings come before shorter ones.
		/// </para>
		/// </remarks>
		private void ParseInternal(string pattern)
		{          

            int index = 0;
            int index_b = -1;
            int index_o = -1;
            int index_option = -1;

            while (index < pattern.Length)
            {
                //Trace.WriteLine(pattern[index]);

                if (pattern[index] == '%')
                {
                    if (index_b == -1)
                        index_b = index;
                    else
                    {
                        ProcessConverter(pattern.Substring(index_b, index - index_b));
                        index_b = index;
                    }

                    if (index_o > -1 && index > index_o)
                    {
                        ProcessConverter(pattern.Substring(index_o, index - index_o));
                        index_o = -1;
                    }
                }
                else if (IsEnd(pattern[index]))
                {
                    if (pattern[index] == '{')
                    {
                        index_option = index;
                    }
                    else
                        if ((index_option == -1 || pattern[index] == '}') && index_b > -1)
                        {

                            ProcessConverter(pattern.Substring(index_b, index - index_b + (pattern[index] == '}' ? 1 : 0)));
                            index_b = -1;
                            index_o = index;
                            if (pattern[index] == '}')
                                index_o = index + 1;
                            index_option = -1;
                        }
                }
                index++;
            }
            if (index_b > -1)
            {
                ProcessConverter(pattern.Substring(index_b, index - index_b));
            }
            if (index_o > -1 && index_o < pattern.Length)
                ProcessConverter(pattern.Substring(index_o, index - index_o));
        }
        /// <summary>
        /// check for end by char ascii
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private bool IsEnd(char chr)
        {
            int ascii = (int)chr;
            return !((ascii > 96 && ascii < 123) || (ascii > 64 && ascii < 91));
        }


        private void ParseOption(string converterNameSrc, out string converterName, out string Option)
        {
            int index = 0;
            converterName = null;
            Option = "";
            while (index < converterNameSrc.Length)
            {
                if (converterNameSrc[index] == '{')
                {
                    converterName = converterNameSrc.Substring(0, index);
                    Option = converterNameSrc.Substring(index + 1, converterNameSrc.Length - index - 2);
                    break;
                }
                index++;
            }
            if (converterName == null)
                converterName = converterNameSrc;
        }

        /// <summary>
        /// Process a parsed converter pattern
        /// </summary>
        /// <param name="converterName">the name of the converter</param>
        /// <param name="option">the optional option for the converter</param>
        /// <param name="formattingInfo">the formatting info for the converter</param>
        private void ProcessConverter(string converterNameSrc)
        {

            string option, converterName;


            ParseOption(converterNameSrc, out converterName, out option);

            converterName = converterName.TrimStart(new char[] { ESCAPE_CHAR });
            LogLog.Debug(declaringType, "Converter [" + converterName + "] Option [" + option + "]");

            // Lookup the converter type
            Type type = (Type)s_globalRulesRegistry[converterName];
            if (type == null)
            {
                type = (Type)s_globalRulesRegistry["literal"];
                option = converterName;
            }

            if (type == null)
            {
                LogLog.Error(declaringType, "Unknown converter name [" + converterName + "] in conversion pattern.");
            }
            else
            {
                // Create the pattern converter
                PatternConverter pc = null;
                try
                {
                    pc = (PatternConverter)Activator.CreateInstance(type);
                }
                catch (Exception createInstanceEx)
                {
                    LogLog.Error(declaringType, "Failed to create instance of Type [" + type.FullName + "] using default constructor. Exception: " + createInstanceEx.ToString());
                }
                pc.Option = option;
                IOptionHandler optionHandler = pc as IOptionHandler;
                if (optionHandler != null)
                {
                    optionHandler.ActivateOptions();
                }

                AddConverter(pc);
            }
        }

        /// <summary>
        /// Resets the internal state of the parser and adds the specified pattern converter 
        /// to the chain.
        /// </summary>
        /// <param name="pc">The pattern converter to add.</param>
        private void AddConverter(PatternConverter pc)
        {
            // Add the pattern converter to the list.

            if (m_head == null)
            {
                m_head = m_tail = pc;
            }
            else
            {
                // Set the next converter on the tail
                // Update the tail reference
                // note that a converter may combine the 'next' into itself
                // and therefore the tail would not change!
                m_tail = m_tail.SetNext(pc);
            }
        }

      

        #region Private Constants

        private const char ESCAPE_CHAR = '%';

        /// <summary>
        /// Internal map of converter identifiers to converter types.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This static map is overridden by the m_converterRegistry instance map
        /// </para>
        /// </remarks>
        private static Hashtable s_globalRulesRegistry = new Hashtable();


        #endregion Private Constants

        #region Private Instance Fields

        /// <summary>
        /// The first pattern converter in the chain
        /// </summary>
        private PatternConverter m_head;

        /// <summary>
        ///  the last pattern converter in the chain
        /// </summary>
        private PatternConverter m_tail;

        /// <summary>
        /// The pattern
        /// </summary>
        private string m_pattern;

       
        #endregion Private Instance Fields

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the PatternParser class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(PatternParser);

        #endregion Private Static Fields
    }
}
