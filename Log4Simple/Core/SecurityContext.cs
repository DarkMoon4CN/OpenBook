using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Core
{
    /// <summary>
    /// A SecurityContext used by log4net when interacting with protected resources
    /// </summary>
    /// <remarks>
    /// <para>
    /// A SecurityContext used by log4net when interacting with protected resources
    /// for example with operating system services. This can be used to impersonate
    /// a principal that has been granted privileges on the system resources.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    public abstract class SecurityContext
    {
        /// <summary>
        /// Impersonate this SecurityContext
        /// </summary>
        /// <param name="state">State supplied by the caller</param>
        /// <returns>An <see cref="IDisposable"/> instance that will
        /// revoke the impersonation of this SecurityContext, or <c>null</c></returns>
        /// <remarks>
        /// <para>
        /// Impersonate this security context. Further calls on the current
        /// thread should now be made in the security context provided
        /// by this object. When the <see cref="IDisposable"/> result 
        /// <see cref="IDisposable.Dispose"/> method is called the security
        /// context of the thread should be reverted to the state it was in
        /// before <see cref="Impersonate"/> was called.
        /// </para>
        /// </remarks>
        public abstract IDisposable Impersonate(object state);
    }
}
