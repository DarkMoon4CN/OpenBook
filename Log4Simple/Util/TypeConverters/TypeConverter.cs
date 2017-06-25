using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Util.TypeConverters
{
    /// <summary>
    /// Supports conversion from string to <see cref="Type"/> type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Supports conversion from string to <see cref="Type"/> type.
    /// </para>
    /// </remarks>
    /// <seealso cref="ConverterRegistry"/>
    /// <seealso cref="IConvertFrom"/>
    /// <seealso cref="IConvertTo"/>
    /// <author>Nicko Cadell</author>
    internal class TypeConverter : IConvertFrom
    {
        #region Implementation of IConvertFrom

        /// <summary>
        /// Can the source type be converted to the type supported by this object
        /// </summary>
        /// <param name="sourceType">the type to convert</param>
        /// <returns>true if the conversion is possible</returns>
        /// <remarks>
        /// <para>
        /// Returns <c>true</c> if the <paramref name="sourceType"/> is
        /// the <see cref="String"/> type.
        /// </para>
        /// </remarks>
        public bool CanConvertFrom(Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>
        /// Overrides the ConvertFrom method of IConvertFrom.
        /// </summary>
        /// <param name="source">the object to convert to a Type</param>
        /// <returns>the Type</returns>
        /// <remarks>
        /// <para>
        /// Uses the <see cref="M:Type.GetType(string,bool)"/> method to convert the
        /// <see cref="String"/> argument to a <see cref="Type"/>.
        /// Additional effort is made to locate partially specified types
        /// by searching the loaded assemblies.
        /// </para>
        /// </remarks>
        /// <exception cref="ConversionNotSupportedException">
        /// The <paramref name="source"/> object cannot be converted to the
        /// target type. To check for this condition use the <see cref="CanConvertFrom"/>
        /// method.
        /// </exception>
        public object ConvertFrom(object source)
        {
            string str = source as string;
            if (str != null)
            {
                return SystemInfo.GetTypeFromString(str, true, true);
            }
            throw ConversionNotSupportedException.Create(typeof(Type), source);
        }

        #endregion
    }
}
