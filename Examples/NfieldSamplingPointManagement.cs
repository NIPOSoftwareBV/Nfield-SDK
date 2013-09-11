using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nfield.Extensions;
using Nfield.Models;
using Nfield.Services;

namespace Nfield.SDK.Samples
{
    public class NfieldSamplingPointManagement
    {
        private readonly INfieldSurveysService _surveysService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NfieldSamplingPointManagement(INfieldSurveysService surveysService)
        {
            _surveysService = surveysService;
        }

        /// <summary>
        /// Performs query operation for available <see cref="Survey"/>s synchronously. 
        /// Note that this sample does not return the result, although your real class will do so.
        /// </summary>
        public void QueryForSurveys()
        {
            IEnumerable<Survey> allSurveys = _surveysService.Query().ToList();
        }

        /// <summary>
        /// Performs query operation for available <see cref="SamplingPoint"/>s synchronously. 
        /// Note that this sample does not return the result, although your real class will do so.
        /// </summary>
        public void QueryForSamplingPoints(string surveyId)
        {
            IEnumerable<SamplingPoint> allSamplingPoints = _surveysService.SamplingPointsQuery(surveyId).ToList();               
        }

        /// <summary>
        /// Performs query operation for SamplingPoint synchronously.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPointId"></param>
        public SamplingPoint QueryForSamplingPoint(string surveyId, string samplingPointId)
        {
            return _surveysService.SamplingPointQuery(surveyId, samplingPointId);
        }

        /// <summary>
        /// Adds a sampling point with a synchronous operation.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        public void AddSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            _surveysService.SamplingPointAdd(surveyId, samplingPoint);
        }

        /// <summary>
        /// Delete an existing <see cref="SamplingPoint"/> with a synchronous operation.
        /// </summary>
        public void DeleteSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            _surveysService.SamplingPointDelete(surveyId, samplingPoint);
        }

        /// <summary>
        /// Updates an existing <see cref="SamplingPoint"/> with a synchronous operation.
        /// </summary>
        public void UpdateSamplingPoint(string surveyId, SamplingPoint samplingPoint)
        {
            _surveysService.SamplingPointUpdate(surveyId, samplingPoint);
        }

    }
}
