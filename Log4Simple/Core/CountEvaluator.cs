using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;
using System.IO;

namespace Log4Simple.Core
{

    /// <summary>
    /// over Buffer to Write Local File
    /// </summary>
    public class CountEvaluator : ITriggeringEventEvaluator
    {

        /// <summary>
        /// Test if an <see cref="LoggingData"/> triggers an action
        /// </summary>
        /// <remarks>
        /// <para>
        /// Implementations of this interface allow certain appenders to decide
        /// when to perform an appender specific action.
        /// </para>
        /// <para>
        /// The action or behavior triggered is defined by the implementation.
        /// </para>
        /// </remarks>
        /// <author>liuww</author>
        public bool IsTriggeringEvent(LoggingData loggingEvent, object para)
        {
            var cb = para as CyclicBuffer;

            if (cb == null)
                return false;
            return cb.Length == m_MaxBufferSize;
            
        }

        
        
        /// <summary>
        /// 最大缓存数量
        /// </summary>
        public int MaxBufferSize
        {
            get {return m_MaxBufferSize;}
            set { m_MaxBufferSize = (value <= 0 ?  10 : value);}
        }

        

        /// <summary>
        /// 最大缓存数量
        /// </summary>
        private int m_MaxBufferSize = 10;
    }
}
