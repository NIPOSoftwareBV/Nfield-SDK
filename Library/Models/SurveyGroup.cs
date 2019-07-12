using Newtonsoft.Json;
using System;

namespace Nfield.Models
{
    public class SurveyGroup
    {
        /// <summary>
        /// The Id of the survey group.
        /// </summary>
        [JsonProperty]
        public int SurveyGroupId { get; internal set; }

        /// <summary>
        /// The date on which the survey group was created (in UTC).
        /// </summary>
        [JsonProperty]
        public DateTime CreationDate { get; internal set; }

        /// <summary>
        /// The name of the survey group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the survey group.
        /// </summary>
        public string Description { get; set; }
    }
}
