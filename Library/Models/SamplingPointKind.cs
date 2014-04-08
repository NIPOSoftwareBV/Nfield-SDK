using System;

namespace Nfield.Models
{
    /// <summary>
    /// Indicates the kind of the sampling point
    /// </summary>
    public enum SamplingPointKind
    {
        /// <summary>
        /// A regular sampling point that can be used for assignment
        /// </summary>
        Regular = 0,

        /// <summary>
        /// A spare sampling point that can be selected as spare to another sampling point
        /// </summary>
        Spare = 1

    }
}