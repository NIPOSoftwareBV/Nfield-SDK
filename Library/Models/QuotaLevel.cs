using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfield.Models
{
    /// <summary>
    /// Class representing a quota level
    /// </summary>
    public class QuotaLevel
    {
        /// <summary>
        /// Unique Id of the QuotaLevel
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Description of the QuotaLevel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Child Attributes of the QuotaLevel
        /// </summary>
        public ICollection<QuotaAttribute> Attributes { get; set; }
    }
}
