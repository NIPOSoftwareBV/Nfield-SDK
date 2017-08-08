using System.IO;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service for uploading images for a survey
    /// </summary>
    public interface INfieldSurveyInvitationImagesService
    {
        /// <summary>
        /// Uploads an image for the specified survey, so it can be used in invitation templates
        /// </summary>
        Task<AddInvitationImageResult> AddImage(string surveyId, string filename, Stream content);
    }
}
