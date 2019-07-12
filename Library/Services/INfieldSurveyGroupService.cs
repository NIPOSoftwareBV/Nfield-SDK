using Nfield.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.Services
{
    public interface INfieldSurveyGroupService
    {
        Task<IEnumerable<SurveyGroup>> GetAllAsync();

        Task<IEnumerable<Survey>> GetSurveysAsync(int surveyGroupId);
    }
}
