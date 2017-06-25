﻿using System;
using System.Collections.Generic;
using System.Text;
using Log4Simple.Util;

namespace Log4Simple.Core
{
    /// <summary>
    /// The <see cref="SecurityContextProvider"/> providers default <see cref="SecurityContext"/> instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A configured component that interacts with potentially protected system
    /// resources uses a <see cref="SecurityContext"/> to provide the elevated
    /// privileges required. If the <see cref="SecurityContext"/> object has
    /// been not been explicitly provided to the component then the component
    /// will request one from this <see cref="SecurityContextProvider"/>.
    /// </para>
    /// <para>
    /// By default the <see cref="SecurityContextProvider.DefaultProvider"/> is
    /// an instance of <see cref="SecurityContextProvider"/> which returns only
    /// <see cref="NullSecurityContext"/> objects. This is a reasonable default
    /// where the privileges required are not know by the system.
    /// </para>
    /// <para>
    /// This default behavior can be overridden by subclassing the <see cref="SecurityContextProvider"/>
    /// and overriding the <see cref="CreateSecurityContext"/> method to return
    /// the desired <see cref="SecurityContext"/> objects. The default provider
    /// can be replaced by programmatically setting the value of the 
    /// <see cref="SecurityContextProvider.DefaultProvider"/> property.
    /// </para>
    /// <para>
    /// An alternative is to use the <c>log4net.Config.SecurityContextProviderAttribute</c>
    /// This attribute can be applied to an assembly in the same way as the
    /// <c>log4net.Config.XmlConfiguratorAttribute"</c>. The attribute takes
    /// the type to use as the <see cref="SecurityContextProvider"/> as an argument.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    public class SecurityContextProvider
    {
        /// <summary>
        /// The default provider
        /// </summary>
        private static SecurityContextProvider s_defaultProvider = new SecurityContextProvider();

        /// <summary>
        /// Gets or sets the default SecurityContextProvider
        /// </summary>
        /// <value>
        /// The default SecurityContextProvider
        /// </value>
        /// <remarks>
        /// <para>
        /// The default provider is used by configured components that
        /// require a <see cref="SecurityContext"/> and have not had one
        /// given to them.
        /// </para>
        /// <para>
        /// By default this is an instance of <see cref="SecurityContextProvider"/>
        /// that returns <see cref="NullSecurityContext"/> objects.
        /// </para>
        /// <para>
        /// The default provider can be set programmatically by setting
        /// the value of this property to a sub class of <see cref="SecurityContextProvider"/>
        /// that has the desired behavior.
        /// </para>
        /// </remarks>
        public static SecurityContextProvider DefaultProvider
        {
            get { return s_defaultProvider; }
            set { s_defaultProvider = value; }
        }

        /// <summary>
        /// Protected default constructor to allow subclassing
        /// </summary>
        /// <remarks>
        /// <para>
        /// Protected default constructor to allow subclassing
        /// </para>
        /// </remarks>
        protected SecurityContextProvider()
        {
        }

        /// <summary>
        /// Create a SecurityContext for a consumer
        /// </summary>
        /// <param name="consumer">The consumer requesting the SecurityContext</param>
        /// <returns>An impersonation context</returns>
        /// <remarks>
        /// <para>
        /// The default implementation is to return a <see cref="NullSecurityContext"/>.
        /// </para>
        /// <para>
        /// Subclasses should override this method to provide their own
        /// behavior.
        /// </para>
        /// </remarks>
        public virtual SecurityContext CreateSecurityContext(object consumer)
        {
            return NullSecurityContext.Instance;
        }
    }
}
