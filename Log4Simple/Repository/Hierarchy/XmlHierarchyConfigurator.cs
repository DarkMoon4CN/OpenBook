using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using Log4Simple.Appender;
using Log4Simple.Util;
using System.Reflection;
using System.Globalization;
using Log4Simple.Core;
using Log4Simple.Plugin;

namespace Log4Simple.Repository.Hierarchy
{


    public class XmlHierarchyConfigurator
    {
        Hierarchy m_hierarchy;       

        public XmlHierarchyConfigurator(Hierarchy hierarchy)
        {
            m_hierarchy = hierarchy;
            
        }


        /// <summary>
        /// Configure the hierarchy by parsing a DOM tree of XML elements.
        /// </summary>
        /// <param name="element">The root element to parse.</param>
        /// <remarks>
        /// <para>
        /// Configure the hierarchy by parsing a DOM tree of XML elements.
        /// </para>
        /// </remarks>
        public void Configure(XmlElement element) 
		{
            if (element == null || m_hierarchy == null)
                return;

            string rootelementname = element.LocalName;


            if (rootelementname != CONFIGURATION_TAG)
            {
                LogLog.Error(declaringType, "Xml element is - not a <" + CONFIGURATION_TAG + "> element.");
                return;
            }

            string quietModeAttribute = element.GetAttribute(QUIETMODE_ATTR);
            LogLog.Debug(declaringType, QUIETMODE_ATTR + " attribute [" + quietModeAttribute + "].");


            if (quietModeAttribute.Length > 0 && quietModeAttribute != "null")
            {
                LogLog.QuietMode = OptionConverter.ToBoolean(quietModeAttribute, false);
            }
            else
            {
                LogLog.Debug(declaringType, "Ignoring " + QUIETMODE_ATTR + " attribute.");
            }


            if (!LogLog.EmitInternalMessages)
            {
                // Look for a emitDebug attribute to enable internal debug
                string emitDebugAttribute = element.GetAttribute(EMIT_INTERNAL_DEBUG_ATTR);
                LogLog.Debug(declaringType, EMIT_INTERNAL_DEBUG_ATTR + " attribute [" + emitDebugAttribute + "].");

                if (emitDebugAttribute.Length > 0 && emitDebugAttribute != "null")
                {
                    LogLog.EmitInternalMessages = OptionConverter.ToBoolean(emitDebugAttribute, true);
                }
                else
                {
                    LogLog.Debug(declaringType, "Ignoring " + EMIT_INTERNAL_DEBUG_ATTR + " attribute.");
                }
            }

            if (!LogLog.InternalDebugging)
            {
                // Look for a debug attribute to enable internal debug
                string debugAttribute = element.GetAttribute(INTERNAL_DEBUG_ATTR);
                LogLog.Debug(declaringType, INTERNAL_DEBUG_ATTR + " attribute [" + debugAttribute + "].");

                if (debugAttribute.Length > 0 && debugAttribute != "null")
                {
                    LogLog.InternalDebugging = OptionConverter.ToBoolean(debugAttribute, true);
                }
                else
                {
                    LogLog.Debug(declaringType, "Ignoring " + INTERNAL_DEBUG_ATTR + " attribute.");
                }               
            }


            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                XmlElement xe = (XmlElement)node;
                if (xe.LocalName == LOGGER_TAG)
                {
                    ParseLogger(xe);
                }
                else if (xe.LocalName == APPENDER_TAG)
                {
                    //....
                }
                else if (xe.LocalName == PLUGINS)
                {
                    ParsePlugins(xe);                       
                }
                else
                    SetParameter(xe, m_hierarchy);



            }

            //if (rootelementname != 


        }

       

        /// <summary>
        /// Parses a logger element.
        /// </summary>
        /// <param name="loggerElement">The logger element.</param>
        /// <remarks>
        /// <para>
        /// Parse an XML element that represents a logger.
        /// </para>
        /// </remarks>
        private void ParseLogger(XmlElement element)
        {
            string loggername = element.GetAttribute(NAME_ATTR);
            Logger log = m_hierarchy.GetLogger(loggername) as Logger;

            lock (log)
            {
                ParseChildrenOfLoggerElement(element, log, false);
            }
        }

        /// <summary>
        /// Parses the children of a logger element.
        /// </summary>
        /// <param name="catElement">The category element.</param>
        /// <param name="log">The logger instance.</param>
        /// <param name="isRoot">Flag to indicate if the logger is the root logger.</param>
        /// <remarks>
        /// <para>
        /// Parse the child elements of a &lt;logger&gt; element.
        /// </para>
        /// </remarks>
        protected void ParseChildrenOfLoggerElement(XmlElement xe, Logger log, bool isroot)
        {
            log.RemoveAllAppenders();
            foreach (XmlNode node in xe.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                XmlElement currxe = (XmlElement)node;
                if (node.LocalName == APPENDER_REF_TAG)                
                    log.AddAppender(FindAppenderByReference(currxe));                                    
                else
                    SetParameter(currxe, log);
            }
        }

        /// <summary>
        /// 查找ref对象
        /// </summary>
        /// <returns></returns>
        private XmlElement FindRefXmlElement(XmlDocument doc,string xmlname,string tag)
        {
            foreach (XmlElement currxe in doc.GetElementsByTagName(tag))
            {
                if (currxe.GetAttribute("name") == xmlname)
                {
                    return currxe;
                    
                }
            }
            return null;
        }

        /// <summary>
        /// Parse appenders by IDREF.
        /// </summary>
        /// <param name="appenderRef">The appender ref element.</param>
        /// <returns>The instance of the appender that the ref refers to.</returns>
        /// <remarks>
        /// <para>
        /// Parse an XML element that represents an appender and return 
        /// the appender.
        /// </para>
        /// </remarks>
        protected IAppender FindAppenderByReference(XmlElement appenderref)
        {
            string appendername = appenderref.GetAttribute(REF_ATTR);
            XmlElement element = null;
            if (string.IsNullOrEmpty(appendername))
                return null;

            element = FindRefXmlElement(appenderref.OwnerDocument, appendername,APPENDER_TAG);
            
            if (element == null)
                return null;
            return ParseAppender(element);
            
        }

       

        /// <summary>
        /// Parses an appender element.
        /// </summary>
        /// <param name="appenderElement">The appender element.</param>
        /// <returns>The appender instance or <c>null</c> when parsing failed.</returns>
        /// <remarks>
        /// <para>
        /// Parse an XML element that represents an appender and return
        /// the appender instance.
        /// </para>
        /// </remarks>
        protected IAppender ParseAppender(XmlElement appenderelement)
        {
            string appenderName = appenderelement.GetAttribute(NAME_ATTR);
            string typeName = appenderelement.GetAttribute(TYPE_ATTR);
            try
            {
                IAppender appender = (IAppender)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));
                appender.Name = appenderName;

                foreach (XmlNode currnode in appenderelement.ChildNodes)
                {
                    if (currnode.NodeType != XmlNodeType.Element)
                        continue;
                    if (currnode.LocalName == APPENDER_REF_TAG)
                    {
                        
                    }                   
                    else
                        SetParameter((XmlElement)currnode, appender);
                }
                IOptionHandler optionHandler = appender as IOptionHandler;
                if (optionHandler != null)
                {
                    optionHandler.ActivateOptions();
                }

                LogLog.Debug(declaringType, "Created Appender [" + appenderName + "]");
                return appender;
            }
            catch (Exception ex)
            {
                // Yes, it's ugly.  But all exceptions point to the same problem: we can't create an Appender

                LogLog.Error(declaringType, "Could not create Appender [" + appenderName + "] of type [" + typeName + "]. Reported error follows.", ex);
                return null;
            }

        }

       


        /// <summary>
        /// Test if an element has no attributes or child elements
        /// </summary>
        /// <param name="element">the element to inspect</param>
        /// <returns><c>true</c> if the element has any attributes or child elements, <c>false</c> otherwise</returns>
        private bool HasAttributesOrElements(XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Attribute || node.NodeType == XmlNodeType.Element)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test if a <see cref="Type"/> is constructible with <c>Activator.CreateInstance</c>.
        /// </summary>
        /// <param name="type">the type to inspect</param>
        /// <returns><c>true</c> if the type is creatable using a default constructor, <c>false</c> otherwise</returns>
        private static bool IsTypeConstructible(Type type)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                ConstructorInfo defaultConstructor = type.GetConstructor(new Type[0]);
                if (defaultConstructor != null && !defaultConstructor.IsAbstract && !defaultConstructor.IsPrivate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="element"></param>
        /// <param name="target"></param>
        private void SetParameter(XmlElement element, object target)
        {
            string name = element.GetAttribute(NAME_ATTR);
            if (element.LocalName != PARAM_TAG || string.IsNullOrEmpty(name))
                name = element.LocalName;

            Type targetType = target.GetType();

            Type propertyType = null;

            PropertyInfo propInfo = targetType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            MethodInfo methInfo = null;

            if (propInfo != null && propInfo.CanWrite)
                propertyType = propInfo.PropertyType;
            else
            {
                propInfo = null;
                methInfo = FindMethodInfo(targetType, name);
                if (methInfo != null)
                    propertyType = methInfo.GetParameters()[0].ParameterType;

            }
            if (propertyType == null)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Cannot find Property [" + name + "] to set object on [" + target.ToString() + "]");
                return;
            }


            string propvalue = null;

            if (element.GetAttributeNode(VALUE_ATTR) != null)
                propvalue = element.GetAttribute(VALUE_ATTR);

           
            if (propvalue != null)
            {
                object convertedValue = ConvertStringTo(propertyType, propvalue);

                try
                {
                    // Pass to the property
                    propInfo.SetValue(target, convertedValue, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                }
                catch (TargetInvocationException targetInvocationEx)
                {
                    LogLog.Error(declaringType, "Failed to set parameter [" + propInfo.Name + "] on object [" + target + "] using value [" + convertedValue + "]", targetInvocationEx.InnerException);
                }
            }
            else
            {
                object createdObject = null;
                if (propertyType == typeof(string) && !HasAttributesOrElements(element))
                {
                    // If the property is a string and the element is empty (no attributes
                    // or child elements) then we special case the object value to an empty string.
                    // This is necessary because while the String is a class it does not have
                    // a default constructor that creates an empty string, which is the behavior
                    // we are trying to simulate and would be expected from CreateObjectFromXml
                    createdObject = "";
                }
                else
                {
                    // No value specified
                    Type defaultObjectType = null;
                    if (IsTypeConstructible(propertyType))
                    {
                        defaultObjectType = propertyType;
                    }

                    createdObject = CreateObjectFromXml(element, defaultObjectType, propertyType);
                }
                if (createdObject == null)
                {
                    LogLog.Error(declaringType, "Failed to create object to set param: "+name);
                }
                else
                {
                    if (propInfo != null)
                    {
                        // Got a converted result
                        LogLog.Debug(declaringType, "Setting Property [" + propInfo.Name + "] to object [" + createdObject + "]");

                        try
                        {
                            // Pass to the property
                            propInfo.SetValue(target, createdObject, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException targetInvocationEx)
                        {
                            LogLog.Error(declaringType, "Failed to set parameter [" + propInfo.Name + "] on object [" + target + "] using value [" + createdObject + "]", targetInvocationEx.InnerException);
                        }
                    }
                    else if (methInfo != null)
                    {
                        // Got a converted result
                        LogLog.Debug(declaringType, "Setting Collection Property [" + methInfo.Name + "] to object [" + createdObject + "]");
                        try
                        {
                            // Pass to the property
                            methInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[] { createdObject }, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException targetInvocationEx)
                        {
                            LogLog.Error(declaringType, "Failed to set parameter [" + methInfo.Name + "] on object [" + target + "] using value [" + createdObject + "]", targetInvocationEx.InnerException);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Look for a method on the <paramref name="targetType"/> that matches the <paramref name="name"/> supplied
        /// </summary>
        /// <param name="targetType">the type that has the method</param>
        /// <param name="name">the name of the method</param>
        /// <returns>the method info found</returns>
        /// <remarks>
        /// <para>
        /// The method must be a public instance method on the <paramref name="targetType"/>.
        /// The method must be named <paramref name="name"/> or "Add" followed by <paramref name="name"/>.
        /// The method must take a single parameter.
        /// </para>
        /// </remarks>
        private MethodInfo FindMethodInfo(Type targetType, string name)
        {
            string requiredMethodNameA = name;
            string requiredMethodNameB = "Add" + name;

            MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo methInfo in methods)
            {
                if (!methInfo.IsStatic)
                {
                    if (string.Compare(methInfo.Name, requiredMethodNameA, true, System.Globalization.CultureInfo.InvariantCulture) == 0 ||
                        string.Compare(methInfo.Name, requiredMethodNameB, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        // Found matching method name

                        // Look for version with one arg only
                        System.Reflection.ParameterInfo[] methParams = methInfo.GetParameters();
                        if (methParams.Length == 1)
                        {
                            return methInfo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Converts a string value to a target type.
        /// </summary>
        /// <param name="type">The type of object to convert the string to.</param>
        /// <param name="value">The string value to use as the value of the object.</param>
        /// <returns>
        /// <para>
        /// An object of type <paramref name="type"/> with value <paramref name="value"/> or 
        /// <c>null</c> when the conversion could not be performed.
        /// </para>
        /// </returns>
        protected object ConvertStringTo(Type type, string value)
        {
            // Hack to allow use of Level in property
            //if (typeof(Level) == type)
            //{
            //    // Property wants a level
            //    Level levelValue = m_hierarchy.LevelMap[value];

            //    if (levelValue == null)
            //    {
            //        LogLog.Error(declaringType, "XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
            //    }

            //    return levelValue;
            //}
            return OptionConverter.ConvertStringTo(type, value);
        }

        /// <summary>
		/// Creates an object as specified in XML.
		/// </summary>
		/// <param name="element">The XML element that contains the definition of the object.</param>
		/// <param name="defaultTargetType">The object type to use if not explicitly specified.</param>
		/// <param name="typeConstraint">The type that the returned object must be or must inherit from.</param>
		/// <returns>The object or <c>null</c></returns>
		/// <remarks>
		/// <para>
		/// Parse an XML element and create an object instance based on the configuration
		/// data.
		/// </para>
		/// <para>
		/// The type of the instance may be specified in the XML. If not
		/// specified then the <paramref name="defaultTargetType"/> is used
		/// as the type. However the type is specified it must support the
		/// <paramref name="typeConstraint"/> type.
		/// </para>
		/// </remarks>
        protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
        {
            Type objectType = null;
            string objectTypeString = element.GetAttribute(TYPE_ATTR);
            if (objectTypeString == null || objectTypeString.Length == 0)
            {
                if (defaultTargetType == null)
                {
                    LogLog.Error(declaringType, "Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
                    return null;
                }
                else
                {
                    // Use the default object type
                    objectType = defaultTargetType;
                }
            }
            else
            {
                // Read the explicit object type
                try
                {
                    objectType = SystemInfo.GetTypeFromString(objectTypeString, true, true);
                }
                catch (Exception ex)
                {
                    LogLog.Error(declaringType, "Failed to find type [" + objectTypeString + "]", ex);
                    return null;
                }
            }

            bool requiresConversion = false;

            // Got the object type. Check that it meets the typeConstraint
            if (typeConstraint != null)
            {
                if (!typeConstraint.IsAssignableFrom(objectType))
                {
                    // Check if there is an appropriate type converter
                    if (OptionConverter.CanConvertTypeTo(objectType, typeConstraint))
                    {
                        requiresConversion = true;
                    }
                    else
                    {
                        LogLog.Error(declaringType, "Object type [" + objectType.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
                        return null;
                    }
                }
            }

            // Create using the default constructor
            object createdObject = null;
            try
            {
                createdObject = Activator.CreateInstance(objectType);
            }
            catch (Exception createInstanceEx)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Failed to construct object of type [" + objectType.FullName + "] Exception: " + createInstanceEx.ToString());
            }

            // Set any params on object
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    SetParameter((XmlElement)currentNode, createdObject);
                }
            }

            // Check if we need to call ActivateOptions
            IOptionHandler optionHandler = createdObject as IOptionHandler;
            if (optionHandler != null)
            {
                optionHandler.ActivateOptions();
            }


            // Ok object should be initialized

            if (requiresConversion)
            {
                // Convert the object type
                return OptionConverter.ConvertTypeTo(createdObject, typeConstraint);
            }
            else
            {
                // The object is of the correct type
                return createdObject;
            }

        }

        /// <summary>
        /// create plugins
        /// </summary>
        /// <param name="element"></param>
        private void ParsePlugins(XmlElement xe)
        {
            lock (m_hierarchy.PluginMap)
            {
                foreach (XmlNode node in xe.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Element)
                        continue;
                    XmlElement currxe = (XmlElement)node;
                    if (node.LocalName == PLUGIN)
                        m_hierarchy.PluginMap.Add(ParsePlugin(currxe));                 
                }
            }
        }

        /// <summary>
        /// create plugin
        /// </summary>
        private IPlugin ParsePlugin(XmlElement pluginElement)
        {
            string appenderName = pluginElement.GetAttribute(NAME_ATTR);
            string typeName = pluginElement.GetAttribute(TYPE_ATTR);
            try
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));


                foreach (XmlNode currnode in pluginElement.ChildNodes)
                {
                    if (currnode.NodeType != XmlNodeType.Element)
                        continue;
                    SetParameter((XmlElement)currnode, plugin);
                }
                IOptionHandler optionHandler = plugin as IOptionHandler;
                if (optionHandler != null)
                {
                    optionHandler.ActivateOptions();
                }

                LogLog.Debug(declaringType, "Created Appender [" + appenderName + "]");
                return plugin;
            }
            catch (Exception ex)
            {
                // Yes, it's ugly.  But all exceptions point to the same problem: we can't create an Appender

                LogLog.Error(declaringType, "Could not create Plugin [" + appenderName + "] of type [" + typeName + "]. Reported error follows.", ex);
                return null;
            }
        }




        /// <summary>
        /// The fully qualified type of the XmlHierarchyConfigurator class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(XmlHierarchyConfigurator);


        #region

        public const string CONFIGURATION_TAG        = "log4simple";
        private const string RENDERER_TAG             = "renderer";
        private const string APPENDER_TAG             = "appender";
        private const string APPENDER_REF_TAG         = "appender-ref";

        private const string FILESTRATEGY             = "filestrategy";
        private const string FILESTRATEGY_REF_TAG     = "filestrategy-ref";

        private const string PARAM_TAG                = "param";
        
        
        private const string LOGGER_TAG               = "logger";
        private const string NAME_ATTR                = "name";
        private const string TYPE_ATTR                = "type";
        private const string VALUE_ATTR               = "value";
        private const string LEVEL_TAG                = "level";


        private const string PLUGINS                 = "plugins";
        private const string PLUGIN                  = "plugin";

        private const string REF_ATTR                 = "ref";
        private const string THRESHOLD_ATTR           = "threshold";

        private const string INTERNAL_DEBUG_ATTR      = "debug";
        private const string EMIT_INTERNAL_DEBUG_ATTR = "emitdebug";
        private const string QUIETMODE_ATTR            = "quietmode";


        




        #endregion



    }
}
