using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;
using System.Collections;

namespace Log4Simple.Core
{
    /// <summary>
    /// provides method information without actually referencing a System.Reflection.MethodBase
    /// as that would require that the containing assembly is loaded.
    /// </summary>
    /// 
    [Serializable]
    public class MethodItem
    {
        #region Public Instance Constructors

        /// <summary>
        /// constructs a method item for an unknown method.
        /// </summary>
        public MethodItem()
        {
            m_name = NA;
            m_parameters = new string[0];
        }

        /// <summary>
        /// constructs a method item from the name of the method.
        /// </summary>
        /// <param name="name"></param>
        public MethodItem(string name)
            : this()
        {
            m_name = name;
        }

        /// <summary>
        /// constructs a method item from the name of the method and its parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public MethodItem(string name, string[] parameters)
            : this(name)
        {
            m_parameters = parameters;
        }

        /// <summary>
        /// constructs a method item from a method base by determining the method name and its parameters.
        /// </summary>
        /// <param name="methodBase"></param>
        public MethodItem(System.Reflection.MethodBase methodBase)
            : this(methodBase.Name, GetMethodParameterNames(methodBase))
        {
        }

        #endregion

        private static string[] GetMethodParameterNames(System.Reflection.MethodBase methodBase)
        {
            ArrayList methodParameterNames = new ArrayList();
            try
            {
                System.Reflection.ParameterInfo[] methodBaseGetParameters = methodBase.GetParameters();

                int methodBaseGetParametersCount = methodBaseGetParameters.GetUpperBound(0);

                for (int i = 0; i <= methodBaseGetParametersCount; i++)
                {
                    methodParameterNames.Add(methodBaseGetParameters[i].ParameterType + " " + methodBaseGetParameters[i].Name);
                }
            }
            catch (Exception ex)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method parameters.", ex);
            }

            return (string[])methodParameterNames.ToArray(typeof(string));
        }

        #region Public Instance Properties

        /// <summary>
        /// Gets the method name of the caller making the logging 
        /// request.
        /// </summary>
        /// <value>
        /// The method name of the caller making the logging 
        /// request.
        /// </value>
        /// <remarks>
        /// <para>
        /// Gets the method name of the caller making the logging 
        /// request.
        /// </para>
        /// </remarks>
        public string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// Gets the method parameters of the caller making
        /// the logging request.
        /// </summary>
        /// <value>
        /// The method parameters of the caller making
        /// the logging request
        /// </value>
        /// <remarks>
        /// <para>
        /// Gets the method parameters of the caller making
        /// the logging request.
        /// </para>
        /// </remarks>
        public string[] Parameters
        {
            get { return m_parameters; }
        }

        #endregion

        #region Private Instance Fields

        private readonly string m_name;
        private readonly string[] m_parameters;

        #endregion

        #region Private Static Fields

        /// <summary>
        /// The fully qualified type of the StackFrameItem class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(MethodItem);

        /// <summary>
        /// When location information is not available the constant
        /// <c>NA</c> is returned. Current value of this string
        /// constant is <b>?</b>.
        /// </summary>
        private const string NA = "?";

        #endregion Private Static Fields
    }
}
