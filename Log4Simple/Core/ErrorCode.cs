using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Core
{
    /// <summary>
    /// Defined error codes that can be passed to the <see cref="M:IErrorHandler.Error(string, Exception, ErrorCode)"/> method.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Values passed to the <see cref="M:IErrorHandler.Error(string, Exception, ErrorCode)"/> method.
    /// </para>
    /// </remarks>
    /// <author>Nicko Cadell</author>
    public enum ErrorCode : int
    {
        /// <summary>
        /// A general error
        /// </summary>
        GenericFailure = 0,

        /// <summary>
        /// Error while writing output
        /// </summary>
        WriteFailure,

        /// <summary>
        /// Failed to flush file
        /// </summary>
        FlushFailure,

        /// <summary>
        /// Failed to close file
        /// </summary>
        CloseFailure,

        /// <summary>
        /// Unable to open output file
        /// </summary>
        FileOpenFailure,

        /// <summary>
        /// No layout specified
        /// </summary>
        MissingLayout,

        /// <summary>
        /// Failed to parse address
        /// </summary>
        AddressParseFailure
    }
}
