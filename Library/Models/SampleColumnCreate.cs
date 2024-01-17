using System;
using System.Collections.Generic;
using System.Text;

namespace Nfield.SDK.Models
{
    public class SampleColumnCreate
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// The value that the column
        /// </summary>
        public object Value { get; set; }
    }
}
