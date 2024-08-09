using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Models;
using Nfield.Services;

namespace ConsoleApplication
{
    public class NfieldSamplingPointManagement
    {
        private readonly INfieldSamplingPointsService _surveysSamplingPointsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NfieldSamplingPointManagement(INfieldSamplingPointsService surveysSamplingPointsService)
        {
            _surveysSamplingPointsService = surveysSamplingPointsService;
        }

        /// <summary>
        /// Performs query operation for SamplingPoint synchronously.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPointId"></param>
        public async Task<SamplingPoint> QueryForSamplingPoint(string surveyId, string samplingPointId)
        {
            return (await _surveysSamplingPointsService.QueryAsync(surveyId)).SingleOrDefault(sp => sp.SamplingPointId == samplingPointId);
        }

        /// <summary>
        /// Adds a sampling point with a synchronous operation.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        public async void AddSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            await _surveysSamplingPointsService.CreateAsync(surveyId, samplingPoint);
        }

        /// <summary>
        /// Delete an existing <see cref="SamplingPoint"/> with a synchronous operation.
        /// </summary>
        public async void DeleteSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            await _surveysSamplingPointsService.RemoveAsync(surveyId, samplingPoint.SamplingPointId);
        }

        /// <summary>
        /// Updates an existing <see cref="SamplingPoint"/> with a synchronous operation.
        /// </summary>
        public async void UpdateSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            await _surveysSamplingPointsService.UpdateAsync(surveyId, samplingPoint);
        }

    }
}
