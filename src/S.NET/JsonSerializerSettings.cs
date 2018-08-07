using System;
using System.Collections.Generic;
using System.Text;

namespace S.NET
{
    public class SerializerSettings
    {
        /// <summary>
        /// Gets or sets how null values are handled during serialization and deserialization.
        /// </summary>
        /// <value>Null value handling.</value>
        public NullValueHandling NullValueHandling { get; set; } = NullValueHandling.Include;

        /// <summary>
        /// Indicates how text output is formatted.
        /// </summary>
        public Formatting Formatting { get; set; } = Formatting.None;
    }
}
