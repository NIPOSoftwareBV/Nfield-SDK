﻿using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCryptographyModel
    {
        /// <summary>
        /// Gets or sets the iv.
        /// </summary>
        /// <value>
        /// The iv.
        /// </value>
        public string IV { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IDictionary<string, string> Data { get; set; }
    }
}
