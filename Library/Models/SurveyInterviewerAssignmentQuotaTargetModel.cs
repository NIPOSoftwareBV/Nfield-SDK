namespace Nfield.SDK.Models
{
    /// <summary>
    /// Model for interviewers assignments quota targets and Successful counts at survey and interviewer level.
    /// </summary>
    public class SurveyInterviewerAssignmentQuotaTargetModel
    {
        /// <summary>
        /// Survey quota Level Id
        /// </summary>
        public string LevelId { get; set; }

        /// <summary>
        /// Survey global successful counts
        /// </summary>
        public int SurveySuccessful { get; set; }

        /// <summary>
        /// Interviewer total target
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Interviewer successful counts
        /// </summary>
        public int Successful { get; set; }
    }
}
