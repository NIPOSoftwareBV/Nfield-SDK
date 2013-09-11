using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nfield.Models
{
    /// <summary>
    /// Holds all properties of a sampling point
    /// </summary>
    public class SamplingPoint
    {
        /// <summary>
        /// Gets or sets the sampling point unique identifier.
        /// </summary>
        public string SamplingPointId { get; set; }

        /// <summary>
        /// Gets or sets the name of the sampling point.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the instruction link, this is a link to a pdf blob storage.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// Gets or sets the associcated fieldwork office id.
        /// </summary>
        public string FieldworkOfficeId { get; set; }
    }
}
