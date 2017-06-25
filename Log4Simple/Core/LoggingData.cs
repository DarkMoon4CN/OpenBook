using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;
using Log4Simple.Repository;
using System.IO;

namespace Log4Simple.Core
{

    /// <summary>
    /// define message level
    /// </summary>
    public enum Level : int
    {
        ALL = 0,
        DEBUG = 1,
        INFO = 5,
        WARN = 9,
        ERROR = 13,
        Fatal = 17,
        OFF = 18
    }

    //UserId,LogTitle,LogContext,LogMeta,LogExInfo,LogTypeId,LogTime,Unique

    /// <summary>
    /// Portable data structure used by <see cref="LoggingData"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Portable data structure used by <see cref="LoggingData"/>
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    [Serializable]
    sealed public class LoggingData
    {
        public LoggingData()
        {
 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level">message level</param>
        /// <param name="mouduleName">moudle name,can use class name</param>
        /// <param name="message">message content</param>
        public LoggingData(Level level, string mouduleName, string message)
        {

            MouduleName = mouduleName;

            Message = message;
           
            Level = level;
                                 
        }


        #region Public Instance Fields

        /// <summary>
        /// The logger name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logger name.
        /// </para>
        /// </remarks>
        public string LoggerName;

        /// <summary>
        /// Level of logging event.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Level of logging event. Level cannot be Serializable
        /// because it is a flyweight.  Due to its special serialization it
        /// cannot be declared final either.
        /// </para>
        /// </remarks>
        public Level Level;        

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MouduleName;

        /// <summary>
        /// 记录的消息内容
        /// </summary>
        /// <remarks>
        /// <para>
        /// The application supplied message of logging event.
        /// </para>
        /// </remarks>
        public string Message;

        /// <summary>
        /// 当前线程名称
        /// </summary>
        /// <remarks>
        /// <para>
        /// The name of thread in which this logging event was generated
        /// </para>
        /// </remarks>
        public string ThreadName;

        /// <summary>
        /// The time the event was logged
        /// </summary>
        /// <remarks>
        /// <para>
        /// The TimeStamp is stored in the local time zone for this computer.
        /// </para>
        /// </remarks>
        public DateTime TimeStamp;       

        /// <summary>
        /// 拆展整数字段1
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntOne;

        /// <summary>
        /// 拆展整数字段2
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntTwo;

        /// <summary>
        /// 拆展整数字段3
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntThree;

        /// <summary>
        /// 拆展整数字段4
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntFour;



        /// <summary>
        /// 拆展字符串字段1
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringOne;

        /// <summary>
        /// 拆展字符串字段2
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringTwo;

        /// <summary>
        /// 拆展字符串字段3
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringThree;

        /// <summary>
        /// 拆展字符串字段4
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringFour;

        /// <summary>
        /// 唯一标识
        /// </summary>
        /// <remarks>
        /// <para>
        /// String representation of the current thread's principal identity.
        /// </para>
        /// </remarks>
        public string Unique;
              

        /// <summary>
        /// application No
        /// </summary>
        public int AppNo;
       

        #endregion Public Instance Fields
    }
    
}

