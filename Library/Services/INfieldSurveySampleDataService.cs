using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service to create a request to download sample data
    /// </summary>
    public interface INfieldSurveySampleDataService
    {
        /// <summary>
        /// Start a new download sample data task, gets the task
        /// </summary>
        Task<BackgroundTask> GetAsync(string surveyId, string fileName);
    }
}
