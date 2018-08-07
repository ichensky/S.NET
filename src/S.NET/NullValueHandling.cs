using System;
using System.Collections.Generic;
using System.Text;

namespace S.NET
{
    /// <summary>
    /// Specifies null value handling options.
    /// </summary>
    public enum NullValueHandling
    {
        /// <summary>
        /// Include null values when serializing and deserializing objects.
        /// </summary>
        Include = 0,

        /// <summary>
        /// Ignore null values when serializing and deserializing objects.
        /// </summary>
        Ignore = 1
    }
}
