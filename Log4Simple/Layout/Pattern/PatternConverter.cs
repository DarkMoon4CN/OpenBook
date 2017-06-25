using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Log4Simple.Layout.Pattern
{
    /// <summary>
    /// Abstract class that provides the formatting functionality that 
    /// derived classes need.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Conversion specifiers in a conversion patterns are parsed to
    /// individual PatternConverters. Each of which is responsible for
    /// converting a logging event in a converter specific manner.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public abstract class PatternConverter
    {
        #region Protected Instance Constructors

        /// <summary>
        /// Protected constructor
        /// </summary>
        /// <remarks>
        /// <para>
        /// Initializes a new instance of the <see cref="PatternConverter" /> class.
        /// </para>
        /// </remarks>
        protected PatternConverter()
        {
        }

        #endregion Protected Instance Constructors

        #region Public Instance Properties

        /// <summary>
        /// Get the next pattern converter in the chain
        /// </summary>
        /// <value>
        /// the next pattern converter in the chain
        /// </value>
        /// <remarks>
        /// <para>
        /// Get the next pattern converter in the chain
        /// </para>
        /// </remarks>
        public virtual PatternConverter Next
        {
            get { return m_next; }
        }

        
        /// <summary>
        /// Gets or sets the option value for this converter
        /// </summary>
        /// <summary>
        /// The option for this converter
        /// </summary>
        /// <remarks>
        /// <para>
        /// Gets or sets the option value for this converter
        /// </para>
        /// </remarks>
        public virtual string Option
        {
            get { return m_option; }
            set { m_option = value; }
        }

        #endregion Public Instance Properties

        #region Protected Abstract Methods

        /// <summary>
        /// Evaluate this pattern converter and write the output to a writer.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="state">The state object on which the pattern converter should be executed.</param>
        /// <remarks>
        /// <para>
        /// Derived pattern converters must override this method in order to
        /// convert conversion specifiers in the appropriate way.
        /// </para>
        /// </remarks>
        abstract protected void Convert(TextWriter writer, object state);

        #endregion Protected Abstract Methods

        #region Public Instance Methods

        /// <summary>
        /// Set the next pattern converter in the chains
        /// </summary>
        /// <param name="patternConverter">the pattern converter that should follow this converter in the chain</param>
        /// <returns>the next converter</returns>
        /// <remarks>
        /// <para>
        /// The PatternConverter can merge with its neighbor during this method (or a sub class).
        /// Therefore the return value may or may not be the value of the argument passed in.
        /// </para>
        /// </remarks>
        public virtual PatternConverter SetNext(PatternConverter patternConverter)
        {
            m_next = patternConverter;
            return m_next;
        }

        /// <summary>
        /// Write the pattern converter to the writer with appropriate formatting
        /// </summary>
        /// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
        /// <param name="state">The state object on which the pattern converter should be executed.</param>
        /// <remarks>
        /// <para>
        /// This method calls <see cref="Convert"/> to allow the subclass to perform
        /// appropriate conversion of the pattern converter. If formatting options have
        /// been specified via the <see cref="FormattingInfo"/> then this method will
        /// apply those formattings before writing the output.
        /// </para>
        /// </remarks>
        virtual public void Format(TextWriter writer, object state)
        {       
            // Formatting options are not in use
            Convert(writer, state);                
        }
             

        #endregion Public Instance Methods

        #region Private Instance Fields

        private PatternConverter m_next;
        
        /// <summary>
        /// The option string to the converter
        /// </summary>
        private string m_option = null;
        

        #endregion Private Instance Fields
    }
}
