using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Layout;
using Log4Simple.Util;
using System.Collections;
using System.IO;

namespace Log4Simple.Appender
{
    /// <summary>
    /// Abstract base class implementation of <see cref="IAppender"/>. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class provides the code for common functionality, such 
    /// as support for threshold filtering and support for general filters.
    /// </para>
    /// <para>
    /// Appenders can also implement the <see cref="IOptionHandler"/> interface. Therefore
    /// they would require that the <see cref="M:IOptionHandler.ActivateOptions()"/> method
    /// be called after the appenders properties have been configured.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public abstract class AppenderSkeleton : IAppender, IOptionHandler
    {
        /// <summary>
        /// FileName
        /// </summary>
        protected string m_File;
        /// <summary>
        /// AppenderName
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// The layout of this appender.
        /// </summary>
        /// <remarks>
        /// See <see cref="Layout"/> for more information.
        /// </remarks>
        private ILayout m_layout;


        /// <summary>
        /// message Level
        /// </summary>
        private Level m_Threshold = Level.ALL;


        /// <summary>
        /// It is assumed and enforced that errorHandler is never null.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is assumed and enforced that errorHandler is never null.
        /// </para>
        /// <para>
        /// See <see cref="ErrorHandler"/> for more information.
        /// </para>
        /// </remarks>
        private IErrorHandler m_errorHandler;

        /// <summary>
        /// The security context to use for privileged calls
        /// </summary>
        private SecurityContext m_securityContext;


        /// <summary>
        /// Flag indicating if this appender is closed.
        /// </summary>
        /// <remarks>
        /// See <see cref="Close"/> for more information.
        /// </remarks>
        private bool m_closed = false;

        #region Protected Instance Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// <para>Empty default constructor</para>
        /// </remarks>
        protected AppenderSkeleton()
        {
            m_errorHandler = new OnlyOnceErrorHandler(this.GetType().Name);
        }

        #endregion Protected Instance Constructors

        #region Finalizer

        /// <summary>
        /// Finalizes this appender by calling the implementation's 
        /// <see cref="Close"/> method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this appender has not been closed then the <c>Finalize</c> method
        /// will call <see cref="Close"/>.
        /// </para>
        /// </remarks>
        ~AppenderSkeleton()
        {
            // An appender might be closed then garbage collected. 
            // There is no point in closing twice.
            if (!m_closed)
            {
                LogLog.Debug(declaringType, "Finalizing appender named [" + m_Name + "].");
                Close();
            }
        }

        #endregion Finalizer

       
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }


        /// <summary>
        /// Gets or sets the threshold <see cref="Level"/> of this appender.
        /// </summary>
        /// <value>
        /// The threshold <see cref="Level"/> of the appender. 
        /// </value>
        /// <remarks>
        /// <para>
        /// All log events with lower level than the threshold level are ignored 
        /// by the appender.
        /// </para>
        /// <para>
        /// In configuration files this option is specified by setting the
        /// value of the <see cref="Threshold"/> option to a level
        /// string, such as "DEBUG", "INFO" and so on.
        /// </para>
        /// </remarks>
        virtual public Level Threshold
        {
            set { m_Threshold = value; }
            get { return m_Threshold; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IErrorHandler"/> for this appender.
        /// </summary>
        /// <value>The <see cref="IErrorHandler"/> of the appender</value>
        /// <remarks>
        /// <para>
        /// The <see cref="AppenderSkeleton"/> provides a default 
        /// implementation for the <see cref="ErrorHandler"/> property. 
        /// </para>
        /// </remarks>
        virtual public IErrorHandler ErrorHandler
        {
            get { return this.m_errorHandler; }
            set
            {
                lock (this)
                {
                    if (value == null)
                    {
                        // We do not throw exception here since the cause is probably a
                        // bad config file.
                        LogLog.Warn(declaringType, "You have tried to set a null error-handler.");
                    }
                    else
                    {
                        m_errorHandler = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SecurityContext"/> used to write to the file.
        /// </summary>
        /// <value>
        /// The <see cref="SecurityContext"/> used to write to the file.
        /// </value>
        /// <remarks>
        /// <para>
        /// Unless a <see cref="SecurityContext"/> specified here for this appender
        /// the <see cref="SecurityContextProvider.DefaultProvider"/> is queried for the
        /// security context to use. The default behavior is to use the security context
        /// of the current thread.
        /// </para>
        /// </remarks>
        public SecurityContext SecurityContext
        {
            get { return m_securityContext; }
            set { m_securityContext = value; }
        }
        

        /// <summary>
		/// Test if the logging event should we output by this appender
		/// </summary>
		/// <param name="loggingEvent">the event to test</param>
		/// <returns><c>true</c> if the event should be output, <c>false</c> if the event should be ignored</returns>
		/// <remarks>
		/// <para>
		/// This method checks the logging event against the threshold level set
		/// on this appender and also against the filters specified on this
		/// appender.
		/// </para>
		/// <para>
		/// The implementation of this method is as follows:
		/// </para>
		/// <para>
		/// <list type="bullet">
		///		<item>
		///			<description>
		///			Checks that the severity of the <paramref name="loggingEvent"/>
		///			is greater than or equal to the <see cref="Threshold"/> of this
		///			appender.</description>
		///		</item>
		///		<item>
		///			<description>
		///			Checks that the <see cref="IFilter"/> chain accepts the 
		///			<paramref name="loggingEvent"/>.
		///			</description>
		///		</item>
		/// </list>
		/// </para>
		/// </remarks>
        virtual protected bool FilterEvent(LoggingData loggingEvent)
        {
            return loggingEvent.Level >= m_Threshold;
        }

        /// <summary>
        /// Called before <see cref="M:Append(LoggingEvent)"/> as a precondition.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called by <see cref="M:DoAppend(LoggingEvent)"/>
        /// before the call to the abstract <see cref="M:Append(LoggingEvent)"/> method.
        /// </para>
        /// <para>
        /// This method can be overridden in a subclass to extend the checks 
        /// made before the event is passed to the <see cref="M:Append(LoggingEvent)"/> method.
        /// </para>
        /// <para>
        /// A subclass should ensure that they delegate this call to
        /// this base class if it is overridden.
        /// </para>
        /// </remarks>
        /// <returns><c>true</c> if the call to <see cref="M:Append(LoggingEvent)"/> should proceed.</returns>
        virtual protected bool PreAppendCheck()
        {            
            return true;
        }

        /// <summary>
        /// Renders the <see cref="LoggingData"/> to a string.
        /// </summary>
        /// <param name="loggingEvent">The event to render.</param>
        /// <param name="writer">The TextWriter to write the formatted event to</param>
        /// <remarks>
        /// <para>
        /// Helper method to render a <see cref="LoggingData"/> to 
        /// a string. This appender must have a <see cref="Layout"/>
        /// set to render the <paramref name="loggingEvent"/> to 
        /// a string.
        /// </para>
        /// <para>If there is exception data in the logging event and 
        /// the layout does not process the exception, this method 
        /// will append the exception text to the rendered string.
        /// </para>
        /// <para>
        /// Use this method in preference to <see cref="M:RenderLoggingEvent(LoggingEvent)"/>
        /// where possible. If, however, the caller needs to render the event
        /// to a string then <see cref="M:RenderLoggingEvent(LoggingEvent)"/> does
        /// provide an efficient mechanism for doing so.
        /// </para>
        /// </remarks>
        protected void RenderLoggingEvent(TextWriter writer, LoggingData loggingEvent)
        {
            if (m_layout == null)
            {
                throw new InvalidOperationException("A layout must be set");
            }            
            // The layout will render the exception
            m_layout.Format(writer, loggingEvent);
            
        }

        /// <summary>
        /// Tests if this appender requires a <see cref="Layout"/> to be set.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In the rather exceptional case, where the appender 
        /// implementation admits a layout but can also work without it, 
        /// then the appender should return <c>true</c>.
        /// </para>
        /// <para>
        /// This default implementation always returns <c>false</c>.
        /// </para>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the appender requires a layout object, otherwise <c>false</c>.
        /// </returns>
        virtual protected bool RequiresLayout
        {
            get { return false; }
        }

        /// <summary>
        /// Performs threshold checks and invokes filters before 
        /// delegating actual logging to the subclasses specific 
        /// <see cref="M:Append(LoggingEvent)"/> method.
        /// </summary>
        /// <param name="loggingEvent">The event to log.</param>
        /// <remarks>
        /// <para>
        /// This method cannot be overridden by derived classes. A
        /// derived class should override the <see cref="M:Append(LoggingEvent)"/> method
        /// which is called by this method.
        /// </para>
        /// <para>
        /// The implementation of this method is as follows:
        /// </para>
        /// <para>
        /// <list type="bullet">
        ///		<item>
        ///			<description>
        ///			Checks that the severity of the <paramref name="loggingEvent"/>
        ///			is greater than or equal to the <see cref="Threshold"/> of this
        ///			appender.</description>
        ///		</item>
        ///		<item>
        ///			<description>
        ///			Checks that the <see cref="IFilter"/> chain accepts the 
        ///			<paramref name="loggingEvent"/>.
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<description>
        ///			Calls <see cref="M:PreAppendCheck()"/> and checks that 
        ///			it returns <c>true</c>.</description>
        ///		</item>
        /// </list>
        /// </para>
        /// <para>
        /// If all of the above steps succeed then the <paramref name="loggingEvent"/>
        /// will be passed to the abstract <see cref="M:Append(LoggingEvent)"/> method.
        /// </para>
        /// </remarks>
        public void DoAppend(Core.LoggingData loggingEvent)
        {
            if (FilterEvent(loggingEvent) && PreAppendCheck())
                Append(loggingEvent);
        }

        /// <summary>
        /// Subclasses of <see cref="AppenderSkeleton"/> should implement this method 
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent"/>.
        /// </para>
        /// <para>This method will be called by <see cref="M:DoAppend(LoggingEvent)"/>
        /// if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:PreAppendCheck()"/> method.
        /// </para>
        /// </remarks>
        abstract protected void Append(LoggingData loggingEvent);

        /// <summary>
        /// Append a bulk array of logging events.
        /// </summary>
        /// <param name="loggingEvents">the array of logging events</param>
        /// <remarks>
        /// <para>
        /// This base class implementation calls the <see cref="M:Append(LoggingEvent)"/>
        /// method for each element in the bulk array.
        /// </para>
        /// <para>
        /// A sub class that can better process a bulk array of events should
        /// override this method in addition to <see cref="M:Append(LoggingEvent)"/>.
        /// </para>
        /// </remarks>
        virtual protected void Append(LoggingData[] loggingEvents)
        {
            foreach (LoggingData loggingEvent in loggingEvents)
            {
                Append(loggingEvent);
            }
        }

        
       

        /// <summary>
        /// Closes the appender and release resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Release any resources allocated within the appender such as file handles, 
        /// network connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// <para>
        /// This method cannot be overridden by subclasses. This method 
        /// delegates the closing of the appender to the <see cref="OnClose"/>
        /// method which must be overridden in the subclass.
        /// </para>
        /// </remarks>     
        public void Close()
        {
            // This lock prevents the appender being closed while it is still appending
            lock (this)
            {
                if (!m_closed)
                {
                    OnClose();
                    m_closed = true;
                }
            }
        }


        /// <summary>
        /// Is called when the appender is closed. Derived classes should override 
        /// this method if resources need to be released.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Releases any resources allocated within the appender such as file handles, 
        /// network connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        virtual protected void OnClose()
        {
            // Do nothing by default
        }


        /// <summary>
        /// Gets or sets the <see cref="ILayout"/> for this appender.
        /// </summary>
        /// <value>The layout of the appender.</value>
        /// <remarks>
        /// <para>
        /// See <see cref="RequiresLayout"/> for more information.
        /// </para>
        /// </remarks>
        /// <seealso cref="RequiresLayout"/>
        virtual public ILayout Layout
        {
            get { return m_layout; }
            set { m_layout = value; }
        }

        virtual public void ActivateOptions()
        {
            
        }

        /// <summary>
        /// The fully qualified type of the AppenderSkeleton class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(AppenderSkeleton);

       
    }
}
