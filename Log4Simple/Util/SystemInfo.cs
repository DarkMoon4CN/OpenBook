using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace Log4Simple.Util
{
    public sealed class SystemInfo
    {

        #region Private Constants

        private const string DEFAULT_NULL_TEXT = "(null)";
        private const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";

        #endregion

        #region Public Static Constructor

        /// <summary>
        /// Initialize default values for private static fields.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Only static methods are exposed from this type.
        /// </para>
        /// </remarks>
        static SystemInfo()
        {
            string nullText = DEFAULT_NULL_TEXT;
            string notAvailableText = DEFAULT_NOT_AVAILABLE_TEXT;

#if !NETCF
            // Look for log4net.NullText in AppSettings
            string nullTextAppSettingsKey = SystemInfo.GetAppSetting("log4net.NullText", true);
            if (nullTextAppSettingsKey != null && nullTextAppSettingsKey.Length > 0)
            {
                LogLog.Debug(declaringType, "Initializing NullText value to [" + nullTextAppSettingsKey + "].");
                nullText = nullTextAppSettingsKey;
            }

            // Look for log4net.NotAvailableText in AppSettings
            string notAvailableTextAppSettingsKey = SystemInfo.GetAppSetting("log4net.NotAvailableText", true);
            if (notAvailableTextAppSettingsKey != null && notAvailableTextAppSettingsKey.Length > 0)
            {
                LogLog.Debug(declaringType, "Initializing NotAvailableText value to [" + notAvailableTextAppSettingsKey + "].");
                notAvailableText = notAvailableTextAppSettingsKey;
            }
#endif
            s_notAvailableText = notAvailableText;
            s_nullText = nullText;
        }

        #endregion

        /// <summary>
        /// Gets the base directory for this <see cref="AppDomain"/>.
        /// </summary>
        /// <value>The base directory path for the current <see cref="AppDomain"/>.</value>
        /// <remarks>
        /// <para>
        /// Gets the base directory for this <see cref="AppDomain"/>.
        /// </para>
        /// <para>
        /// The value returned may be either a local file path or a URI.
        /// </para>
        /// </remarks>
        public static string ApplicationBaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// Loads the type specified in the type string.
        /// </summary>
        /// <param name="typeName">The name of the type to load.</param>
        /// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
        /// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>		
        /// <remarks>
        /// <para>
        /// If the type name is fully qualified, i.e. if contains an assembly name in 
        /// the type name, the type will be loaded from the system using 
        /// <see cref="M:Type.GetType(string,bool)"/>.
        /// </para>
        /// <para>
        /// If the type name is not fully qualified it will be loaded from the
        /// assembly that is directly calling this method. If the type is not found 
        /// in the assembly then all the loaded assemblies will be searched for the type.
        /// </para>
        /// </remarks>
        public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
        }

        /// <summary>
        /// Loads the type specified in the type string.
        /// </summary>
        /// <param name="relativeAssembly">An assembly to load the type from.</param>
        /// <param name="typeName">The name of the type to load.</param>
        /// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
        /// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>
        /// <remarks>
        /// <para>
        /// If the type name is fully qualified, i.e. if contains an assembly name in 
        /// the type name, the type will be loaded from the system using 
        /// <see cref="M:Type.GetType(string,bool)"/>.
        /// </para>
        /// <para>
        /// If the type name is not fully qualified it will be loaded from the specified
        /// assembly. If the type is not found in the assembly then all the loaded assemblies 
        /// will be searched for the type.
        /// </para>
        /// </remarks>
        public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
        {
            // Check if the type name specifies the assembly name
            if (typeName.IndexOf(',') == -1)
            {
                LogLog.Debug(declaringType, "SystemInfo: Loading type ["+typeName+"] from assembly ["+relativeAssembly.FullName+"]");
                // Attempt to lookup the type from the relativeAssembly
                Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
                if (type != null)
                {
                    // Found type in relative assembly
                    LogLog.Debug(declaringType, "SystemInfo: Loaded type ["+typeName+"] from assembly ["+relativeAssembly.FullName+"]");
                    return type;
                }

                Assembly[] loadedAssemblies = null;
                try
                {
                    loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (System.Security.SecurityException)
                {
                    // Insufficient permissions to get the list of loaded assemblies
                }

                if (loadedAssemblies != null)
                {
                    // Search the loaded assemblies for the type
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        type = assembly.GetType(typeName, false, ignoreCase);
                        if (type != null)
                        {
                            // Found type in loaded assembly
                            LogLog.Debug(declaringType, "Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
                            return type;
                        }
                    }
                }

                // Didn't find the type
                if (throwOnError)
                {
                    throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
                }
                return null;

            }
            else
            {
                // Includes explicit assembly name
                LogLog.Debug(declaringType, "SystemInfo: Loading type ["+typeName+"] from global Type");

                return Type.GetType(typeName, throwOnError, ignoreCase);

            }
        }

        /// <summary>
        /// Parse a string into an <see cref="Int32"/> value
        /// </summary>
        /// <param name="s">the string to parse</param>
        /// <param name="val">out param where the parsed value is placed</param>
        /// <returns><c>true</c> if the string was able to be parsed into an integer</returns>
        /// <remarks>
        /// <para>
        /// Attempts to parse the string into an integer. If the string cannot
        /// be parsed then this method returns <c>false</c>. The method does not throw an exception.
        /// </para>
        /// </remarks>
        public static bool TryParse(string s, out int val)
        {
            // Initialise out param
            val = 0;

            try
            {
                double doubleVal;
                if (Double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out doubleVal))
                {
                    val = Convert.ToInt32(doubleVal);
                    return true;
                }
            }
            catch
            {
                // Ignore exception, just return false
            }

            return false;
        }


        public static bool TryParse(string s, out long val)
        {
            // Initialise out param
            val = 0;

            try
            {
                double doubleVal;
                if (Double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out doubleVal))
                {
                    val = Convert.ToInt64(doubleVal);
                    return true;
                }
            }
            catch
            {
                // Ignore exception, just return false
            }

            return false;
        }

      
         /// <summary>
        /// Lookup an application setting
        /// </summary>
        /// <param name="key">the application settings key to lookup</param>
        /// <returns>the value for the key, or <c>null</c></returns>
        /// <remarks>
        /// <para>
        /// Configuration APIs are not supported under the Compact Framework
        /// </para>
        /// </remarks>
        public static string GetAppSetting(string key)
        {
            return GetAppSetting(key,true);
        }
        /// <summary>
        /// Lookup an application setting
        /// </summary>
        /// <param name="key">the application settings key to lookup</param>
        /// <returns>the value for the key, or <c>null</c></returns>
        /// <remarks>
        /// <para>
        /// Configuration APIs are not supported under the Compact Framework
        /// </para>
        /// </remarks>
        public static string GetAppSetting(string key,bool ignoreException)
        {
            try
            {
				return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                // If an exception is thrown here then it looks like the config file does not parse correctly.
                if (!ignoreException)
                    LogLog.Error(declaringType, "Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", ex);
            }
            return null;
        }

        /// <summary>
        /// Convert a path into a fully qualified local file path.
        /// </summary>
        /// <param name="path">The path to convert.</param>
        /// <returns>The fully qualified path.</returns>
        /// <remarks>
        /// <para>
        /// Converts the path specified to a fully
        /// qualified path. If the path is relative it is
        /// taken as relative from the application base 
        /// directory.
        /// </para>
        /// <para>
        /// The path specified must be a local file path, a URI is not supported.
        /// </para>
        /// </remarks>
        private static string _ConvertToFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string baseDirectory = "";
            try
            {
                string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                if (applicationBaseDirectory != null)
                {
                    // applicationBaseDirectory may be a URI not a local file path
                    Uri applicationBaseDirectoryUri = new Uri(applicationBaseDirectory);
                    if (applicationBaseDirectoryUri.IsFile && !string.IsNullOrEmpty(Path.GetExtension(path)))
                    {
                        baseDirectory = applicationBaseDirectoryUri.LocalPath;
                    }
                    else
                    {
                        if (!path.EndsWith("\\"))
                            path += '\\';
                    }
                }
            }
            catch
            {
                // Ignore URI exceptions & SecurityExceptions from SystemInfo.ApplicationBaseDirectory
            }

            if (baseDirectory != null && baseDirectory.Length > 0)
            {
                // Note that Path.Combine will return the second path if it is rooted
                return Path.GetFullPath(Path.Combine(baseDirectory, path));
            }
            return Path.GetFullPath(path);
        }

        public static string ConvertToFullPath(string path, bool isCreateDirectory)
        {
            string ret = _ConvertToFullPath(path);
            string directory = Path.GetDirectoryName(ret);
            if (isCreateDirectory && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return ret;
        }

        public static string ConvertToFullPath(string path)
        {
            return ConvertToFullPath(path, true);
        }


        /// <summary>
        /// Text to output when a <c>null</c> is encountered.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this value to indicate a <c>null</c> has been encountered while
        /// outputting a string representation of an item.
        /// </para>
        /// <para>
        /// The default value is <c>(null)</c>. This value can be overridden by specifying
        /// a value for the <c>log4net.NullText</c> appSetting in the application's
        /// .config file.
        /// </para>
        /// </remarks>
        public static string NullText
        {
            get { return s_nullText; }
            set { s_nullText = value; }
        }

        /// <summary>
        /// Text to output when an unsupported feature is requested.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this value when an unsupported feature is requested.
        /// </para>
        /// <para>
        /// The default value is <c>NOT AVAILABLE</c>. This value can be overridden by specifying
        /// a value for the <c>log4net.NotAvailableText</c> appSetting in the application's
        /// .config file.
        /// </para>
        /// </remarks>
        public static string NotAvailableText
        {
            get { return s_notAvailableText; }
            set { s_notAvailableText = value; }
        }


        public static string GetDT_DayString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string GetDT_DayAndHourString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH");
        }
        public static string GetDT_FullMiSecondString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
        }

        /// <summary>
        /// The fully qualified type of the SystemInfo class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(SystemInfo);

        /// <summary>
        /// Text to output when a <c>null</c> is encountered.
        /// </summary>
        private static string s_nullText;

        /// <summary>
        /// Text to output when an unsupported feature is requested.
        /// </summary>
        private static string s_notAvailableText;


        public static bool IsTermainated;

    }
}
