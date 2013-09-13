using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfield.Models
{
    /// <summary>
    /// Class representing a quota attribute
    /// </summary>
    public class QuotaAttribute
    {
        /// <summary>
        /// Name of the QuotaAttribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indication of whether this Attribute is mandatory or not
        /// When mandatory, a Level within this Attribute must be selected by the Interviewer
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Child Levels of the QuotaAttribute
        /// </summary>
        public ICollection<QuotaLevel> Levels { get; set; }
    }
}
