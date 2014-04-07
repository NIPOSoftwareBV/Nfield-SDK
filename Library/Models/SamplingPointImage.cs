using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfield.Models
{
    /// <summary>
    /// Object to hold downloaded sampling point image
    /// </summary>
    public class SamplingPointImage
    {
        /// <summary>
        /// The content of the file
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }
    }
}
