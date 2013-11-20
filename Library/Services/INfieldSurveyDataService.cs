using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service for making a download data request.
    /// </summary>
    public interface INfieldSurveyDataService
    {
        /// <summary>
        /// Gets the download data url.
        /// </summary>
        Task<BackgroundTask> PostAsync(SurveyDownloadDataRequest surveyDownloadDataRequest);
    }
}
