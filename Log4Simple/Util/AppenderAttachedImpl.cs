using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Core;
using Log4Simple.Appender;

namespace Log4Simple.Util
{

    /// <summary>
    /// A straightforward implementation of the <see cref="IAppenderAttachable"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the default implementation of the <see cref="IAppenderAttachable"/>
    /// interface. Implementors of the <see cref="IAppenderAttachable"/> interface
    /// should aggregate an instance of this type.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    /// <author>Gert Driesen</author>
    public class AppenderAttachedImpl : IAppenderAttachable
    {

        private List<IAppender> m_AppenderList = new List<IAppender>();
        

        public AppenderAttachedImpl()
        {
        }


        
        private IAppender Exists(string name)
        {           
            //public delegate bool Predicate<T>(T obj)

            return m_AppenderList.Find((appender) =>
                {
                    return appender.Name == name;
                });
        }

        /// <summary>
        /// Attaches an appender.
        /// </summary>
        /// <param name="newAppender">The appender to add.</param>
        /// <remarks>
        /// <para>
        /// If the appender is already in the list it won't be added again.
        /// </para>
        /// </remarks>
        public void AddAppender(Appender.IAppender appender)
        {
            if (appender == null)
                throw new ArgumentException("AddAppender the appender is null");

            if (Exists(appender.Name) == null)
            {
                m_AppenderList.Add(appender);
            }
        }

        /// <summary>
        /// Gets an attached appender with the specified name.
        /// </summary>
        /// <param name="name">The name of the appender to get.</param>
        /// <returns>
        /// The appender with the name specified, or <c>null</c> if no appender with the
        /// specified name is found.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Lookup an attached appender by name.
        /// </para>
        /// </remarks>
        public Appender.IAppender GetAppender(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("appender name is null");
            return Exists(name);            
            
        }

        /// <summary>
        /// Removes the specified appender from the list of attached appenders.
        /// </summary>
        /// <param name="appender">The appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// <para>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </para>
        /// </remarks>
        public Appender.IAppender RemoveAppender(Appender.IAppender appender)
        {
            if (appender == null)
                throw new ArgumentException("RemoveAppender the newappender is null");

            m_AppenderList.Remove(appender);
            return appender;

        }

        /// <summary>
        /// Removes the appender with the specified name from the list of appenders.
        /// </summary>
        /// <param name="name">The name of the appender to remove.</param>
        /// <returns>The appender removed from the list</returns>
        /// <remarks>
        /// <para>
        /// The appender removed is not closed.
        /// If you are discarding the appender you must call
        /// <see cref="IAppender.Close"/> on the appender removed.
        /// </para>
        /// </remarks>
        public Appender.IAppender RemoveAppender(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("RemoveAppender the name is null");

            IAppender ret = Exists(name);
            if (ret != null)
                m_AppenderList.Remove(ret);                                 

            return ret;
        }



        /// <summary>
        /// Gets all attached appenders.
        /// </summary>
        /// <returns>
        /// A collection of attached appenders, or <c>null</c> if there
        /// are no attached appenders.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The read only collection of all currently attached appenders.
        /// </para>
        /// </remarks>
        public ICollection Appenders
        {
            get { return m_AppenderList; }
        }


        /// <summary>
        /// Removes all attached appenders.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Removes and closes all attached appenders
        /// </para>
        /// </remarks>
        public void RemoveAllAppenders()
        {
            if (m_AppenderList != null)
            {
                foreach (IAppender appender in m_AppenderList)
                {
                    try
                    {
                        appender.Close();                         
                    }
                    catch (Exception ex)
                    {
                        LogLog.Error(declaringType, "Failed to Close appender [" + appender.Name + "]", ex);
                    }
                }
                m_AppenderList = null;
            }
        }


        /// <summary>
        /// Append on on all attached appenders.
        /// </summary>
        /// <param name="loggingEvent">The event being logged.</param>
        /// <returns>The number of appenders called.</returns>
        /// <remarks>
        /// <para>
        /// Calls the <see cref="IAppender.DoAppend" /> method on all 
        /// attached appenders.
        /// </para>
        /// </remarks>
        public int AppendLoopOnAppenders(LoggingData loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            // m_appenderList is null when empty
            if (m_AppenderList == null)
            {
                return 0;
            }


            foreach (IAppender appender in m_AppenderList)
            {
                try
                {
                    appender.DoAppend(loggingEvent);
                }
                catch (Exception ex)
                {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", ex);
                }
            }
            return m_AppenderList.Count;
        }


        /// <summary>
        /// The fully qualified type of the AppenderAttachedImpl class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(AppenderAttachedImpl);
       
    }
}
