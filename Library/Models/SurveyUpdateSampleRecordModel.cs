using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// Model for updating a sample record
    /// </summary>
    public class SurveyUpdateSampleRecordModel
    {
        /// <summary>
        /// The unique id of the survey
        /// </summary>
        public int SampleRecordId { get; set; }

        /// <summary>
        /// Custom columns to be updated
        /// </summary>
        public IEnumerable<SampleColumnUpdate> ColumnUpdates { get; set; }
    }
}