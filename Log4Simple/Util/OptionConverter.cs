using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using Log4Simple.Util.TypeConverters;

namespace Log4Simple.Util
{
    public sealed class OptionConverter
    {

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or 
        /// more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="enumType">The type to convert to.</param>
        /// <param name="value">The enum string value.</param>
        /// <param name="ignoreCase">If <c>true</c>, ignore case; otherwise, regard case.</param>
        /// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
        private static object ParseEnum(System.Type enumtype, string value, bool ignorecase)
        {
            return Enum.Parse(enumtype, value, ignorecase);
        }




        /// <summary>
        /// Converts a string to an object.
        /// </summary>
        /// <param name="target">The target type to convert to.</param>
        /// <param name="txt">The string to convert to an object.</param>
        /// <returns>
        /// The object converted from a string or <c>null</c> when the 
        /// conversion failed.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Converts a string to an object. Uses the converter registry to try
        /// to convert the string value into the specified target type.
        /// </para>
        /// </remarks>
        public static object ConvertStringTo(Type target, string txt)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            // If we want a string we already have the correct type
            if (typeof(string) == target || typeof(object) == target)
            {
                return txt;
            }

            // First lets try to find a type converter
            IConvertFrom typeConverter = ConverterRegistry.GetConvertFrom(target);
            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)))
            {
                // Found appropriate converter
                return typeConverter.ConvertFrom(txt);
            }
            else
            {
                if (target.IsEnum)
                {
                    // Target type is an enum.

                    // Use the Enum.Parse(EnumType, string) method to get the enum value
                    return ParseEnum(target, txt, true);
                }
                else
                {
                    // We essentially make a guess that to convert from a string
                    // to an arbitrary type T there will be a static method defined on type T called Parse
                    // that will take an argument of type string. i.e. T.Parse(string)->T we call this
                    // method to convert the string to the type required by the property.
                    System.Reflection.MethodInfo meth = target.GetMethod("Parse", new Type[] { typeof(string) });
                    if (meth != null)
                    {
                        // Call the Parse method
                        return meth.Invoke(null, BindingFlags.InvokeMethod, null, new object[] { txt }, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        // No Parse() method found.
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if there is an appropriate type conversion from the source type to the target type.
        /// </summary>
        /// <param name="sourceType">The type to convert from.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <returns><c>true</c> if there is a conversion from the source type to the target type.</returns>
        /// <remarks>
        /// Checks if there is an appropriate type conversion from the source type to the target type.
        /// <para>
        /// </para>
        /// </remarks>
        public static bool CanConvertTypeTo(Type sourceType, Type targetType)
        {
            if (sourceType == null || targetType == null)
            {
                return false;
            }

            // Check if we can assign directly from the source type to the target type
            if (targetType.IsAssignableFrom(sourceType))
            {
                return true;
            }

            
            // Look for a From converter
            IConvertFrom tcTarget = ConverterRegistry.GetConvertFrom(targetType);
            if (tcTarget != null)
            {
                if (tcTarget.CanConvertFrom(sourceType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts an object to the target type.
        /// </summary>
        /// <param name="sourceInstance">The object to convert to the target type.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <returns>The converted object.</returns>
        /// <remarks>
        /// <para>
        /// Converts an object to the target type.
        /// </para>
        /// </remarks>
        public static object ConvertTypeTo(object sourceInstance, Type targetType)
        {
            Type sourceType = sourceInstance.GetType();

            // Check if we can assign directly from the source type to the target type
            if (targetType.IsAssignableFrom(sourceType))
            {
                return sourceInstance;
            }

            

            // Look for a FROM converter
            IConvertFrom tcTarget = ConverterRegistry.GetConvertFrom(targetType);
            if (tcTarget != null)
            {
                if (tcTarget.CanConvertFrom(sourceType))
                {
                    return tcTarget.ConvertFrom(sourceInstance);
                }
            }

            throw new ArgumentException("Cannot convert source object [" + sourceInstance.ToString() + "] to target type [" + targetType.Name + "]", "sourceInstance");
        }


        /// <summary>
        /// Parses a file size into a number.
        /// </summary>
        /// <param name="argValue">String to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The <see cref="long" /> value of <paramref name="argValue" />.</returns>
        /// <remarks>
        /// <para>
        /// Parses a file size of the form: number[KB|MB|GB] into a
        /// long value. It is scaled with the appropriate multiplier.
        /// </para>
        /// <para>
        /// <paramref name="defaultValue"/> is returned when <paramref name="argValue"/>
        /// cannot be converted to a <see cref="long" /> value.
        /// </para>
        /// </remarks>
        public static long ToFileSize(string argValue, long defaultValue)
        {
            if (argValue == null)
            {
                return defaultValue;
            }

            string s = argValue.Trim().ToUpper(CultureInfo.InvariantCulture);
            long multiplier = 1;
            int index;

            if ((index = s.IndexOf("KB")) != -1 || (index = s.IndexOf("K")) != -1)
            {
                multiplier = 1024;
                s = s.Substring(0, index);
            }
            else if ((index = s.IndexOf("MB")) != -1 || (index = s.IndexOf("M")) != -1)
            {
                multiplier = 1024 * 1024;
                s = s.Substring(0, index);
            }
            else if ((index = s.IndexOf("GB")) != -1 || (index = s.IndexOf("G")) != -1)
            {
                multiplier = 1024 * 1024 * 1024;
                s = s.Substring(0, index);
            }
            if (s != null)
            {
                // Try again to remove whitespace between the number and the size specifier
                s = s.Trim();

                long longVal;
                if (SystemInfo.TryParse(s, out longVal))
                {
                    return longVal * multiplier;
                }
                else
                {
                    throw new Exception("OptionConverter: [" + s + "] is not in the correct file size syntax.");
                }
            }
            return defaultValue;
        }


        /// <summary>
        /// Converts a string to a <see cref="bool" /> value.
        /// </summary>
        /// <param name="argValue">String to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The <see cref="bool" /> value of <paramref name="argValue" />.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="argValue"/> is "true", then <c>true</c> is returned. 
        /// If <paramref name="argValue"/> is "false", then <c>false</c> is returned. 
        /// Otherwise, <paramref name="defaultValue"/> is returned.
        /// </para>
        /// </remarks>
        public static bool ToBoolean(string argValue, bool defaultValue)
        {
            if (argValue != null && argValue.Length > 0)
            {
                try
                {
                    return bool.Parse(argValue);
                }
                catch (Exception e)
                {
                    LogLog.Error(declaringType, "[" + argValue + "] is not in proper bool form.", e);
                }
            }
            return defaultValue;
        }



        /// <summary>
        /// The fully qualified type of the OptionConverter class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(OptionConverter);

    }
}
