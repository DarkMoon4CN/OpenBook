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
        /// ģ������
        /// </summary>
        public string MouduleName;

        /// <summary>
        /// ��¼����Ϣ����
        /// </summary>
        /// <remarks>
        /// <para>
        /// The application supplied message of logging event.
        /// </para>
        /// </remarks>
        public string Message;

        /// <summary>
        /// ��ǰ�߳�����
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
        /// ��չ�����ֶ�1
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntOne;

        /// <summary>
        /// ��չ�����ֶ�2
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntTwo;

        /// <summary>
        /// ��չ�����ֶ�3
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntThree;

        /// <summary>
        /// ��չ�����ֶ�4
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public int ExIntFour;



        /// <summary>
        /// ��չ�ַ����ֶ�1
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringOne;

        /// <summary>
        /// ��չ�ַ����ֶ�2
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringTwo;

        /// <summary>
        /// ��չ�ַ����ֶ�3
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringThree;

        /// <summary>
        /// ��չ�ַ����ֶ�4
        /// </summary>
        /// <remarks>
        /// <para>        
        /// </para>
        /// </remarks>
        public string ExStringFour;

        /// <summary>
        /// Ψһ��ʶ
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

