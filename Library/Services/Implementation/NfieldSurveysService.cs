//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveysService"/>
    /// </summary>
    internal class NfieldSurveysService : INfieldSurveysService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveysService

        /// <summary>
        /// See <see cref="INfieldSurveysService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Survey>> QueryAsync()
        {
            return Client.GetAsync(SurveysApi.AbsoluteUri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Survey>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.AddAsync"/>
        /// </summary>
        public Task<Survey> AddAsync(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException("survey");
            }

            return Client.PostAsJsonAsync(SurveysApi.AbsoluteUri, survey)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<Survey>(task.Result).Result)
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException("survey");
            }

            return
                Client.DeleteAsync(SurveysApi.AbsoluteUri + survey.SurveyId)
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.UpdateAsync"/>
        /// </summary>
        public Task<Survey> UpdateAsync(Survey survey)
        {
            if (survey == null)
            {
                throw new ArgumentNullException("survey");
            }

            var updatedSurvey = new UpdateSurvey
            {
                ClientName = survey.ClientName,
                Description = survey.Description,
                SurveyName = survey.SurveyName,
                InterviewerInstruction = survey.InterviewerInstruction
            };

            return Client.PatchAsJsonAsync(SurveysApi + survey.SurveyId, updatedSurvey)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask => JsonConvert.DeserializeObjectAsync<Survey>(stringTask.Result).Result)
             .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.UploadInterviewerFileInstructionsAsync(string,string)"/>
        /// </summary>
        public Task UploadInterviewerFileInstructionsAsync(string filePath, string surveyId)
        {
            var fileName = Path.GetFileName(filePath);

            if(!File.Exists(filePath))
                throw new FileNotFoundException(fileName);

            var uri = GetInterviewerInstructionUri(surveyId, fileName);
            
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return Client.PostAsync(uri, byteArrayContent).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.UploadInterviewerFileInstructionsAsync(string, string)"/>
        /// </summary>
        public Task UploadInterviewerFileInstructionsAsync(byte[] fileContent, string fileName, string surveyId)
        {
            var uri = GetInterviewerInstructionUri(surveyId, fileName);
            
            var byteArrayContent = new ByteArrayContent(fileContent);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return Client.PostAsync(uri, byteArrayContent).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.DownloadInterviewerFileInstructionsAsync(string)"/>
        /// </summary>
        public Task<InterviewerInstruction> DownloadInterviewerFileInstructionsAsync(string surveyId)
        {
            var uri = GetInterviewerInstructionUri(surveyId, null);

            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => new InterviewerInstruction
                        {
                            Content = responseMessageTask.Result.Content.ReadAsByteArrayAsync().Result,
                            FileName = responseMessageTask.Result.Content.Headers.ContentDisposition.FileName
                        })
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.DeleteInterviewerFileInstructionsAsync(string)"/>
        /// </summary>
        public Task DeleteInterviewerFileInstructionsAsync(string surveyId)
        {
            var uri = GetInterviewerInstructionUri(surveyId, null);
            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.QuotaQueryAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task<QuotaLevel> QuotaQueryAsync(string surveyId)
        {
            string uri = string.Format(@"{0}{1}/{2}", SurveysApi.AbsoluteUri, surveyId, QuotaControllerName);

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<QuotaLevel>(stringTask.Result))
                         .FlattenExceptions();    
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.CreateOrUpdateQuotaAsync"/>
        /// </summary>
        public Task<QuotaLevel> CreateOrUpdateQuotaAsync(string surveyId, QuotaLevel quota)
        {
            string uri = string.Format(@"{0}{1}/{2}", SurveysApi.AbsoluteUri, surveyId, QuotaControllerName);

            return Client.PutAsJsonAsync(uri, quota)
                         .ContinueWith(
                            responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                            stringTask => JsonConvert.DeserializeObject<QuotaLevel>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointsQueryAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPoint>> SamplingPointsQueryAsync(string surveyId)
        {
            string uri = string.Format(@"{0}{1}/{2}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName);
            
            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SamplingPoint>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointQueryAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointQueryAsync(string surveyId, string samplingPointId)
        {
            string uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPointId);
            
            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
                         .FlattenExceptions();            
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointUpdateAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointUpdateAsync(string surveyId, SamplingPoint samplingPoint)
        {
            if (samplingPoint == null)
            {
                throw new ArgumentNullException("samplingPoint");
            }

            var updatedSamplingPoint = new UpdateSamplingPoint
            {
                Name = samplingPoint.Name,
                Description = samplingPoint.Description,
                FieldworkOfficeId = samplingPoint.FieldworkOfficeId,
                GroupId = samplingPoint.GroupId,
                Stratum = samplingPoint.Stratum
            };

            string uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPoint.SamplingPointId);

            return Client.PatchAsJsonAsync(uri, updatedSamplingPoint)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObjectAsync<SamplingPoint>(stringTask.Result).Result)
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointAddAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointAddAsync(string surveyId, SamplingPoint samplingPoint)
        {
            string uri = string.Format(@"{0}{1}/{2}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName);
            return Client.PostAsJsonAsync(uri, samplingPoint)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<SamplingPoint>(task.Result).Result)
                         .FlattenExceptions();            
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointDeleteAsync"/>
        /// </summary>
        public Task SamplingPointDeleteAsync(string surveyId, SamplingPoint samplingPoint)
        {
            if (samplingPoint == null)
            {
                throw new ArgumentNullException("samplingPoint");
            }
            string uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPoint.SamplingPointId);
            return Client.DeleteAsync(uri)
                        .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointQuotaTargetsQueryAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPointQuotaTarget>> SamplingPointQuotaTargetsQueryAsync(string surveyId, string samplingPointId)
        {
            string uri = string.Format(@"{0}{1}/{2}/{3}/{4}", SurveysApi.AbsoluteUri, surveyId,
                SamplingPointsControllerName, samplingPointId, SamplingPointsQuotaControllerName);

            return Client.GetAsync(uri)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<List<SamplingPointQuotaTarget>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointQuotaTargetQueryAsync"/>
        /// </summary>
        public Task<SamplingPointQuotaTarget> SamplingPointQuotaTargetQueryAsync(string surveyId, string samplingPointId, string levelId)
        {
            string uri = string.Format(@"{0}{1}/{2}/{3}/{4}/{5}", SurveysApi.AbsoluteUri, surveyId,
                SamplingPointsControllerName, samplingPointId, SamplingPointsQuotaControllerName, levelId);

            return Client.GetAsync(uri)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<SamplingPointQuotaTarget>(stringTask.Result))
             .FlattenExceptions();  
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointQuotaTargetUpdateAsync"/>
        /// </summary>
        public Task<SamplingPointQuotaTarget> SamplingPointQuotaTargetUpdateAsync(string surveyId, string samplingPointId, SamplingPointQuotaTarget samplingPointQuotaTarget)
        {
            if (samplingPointQuotaTarget == null)
            {
                throw new ArgumentNullException("samplingPointQuotaTarget");
            }

            var updatedSamplingPointQuotaTarget = new UpdateSamplingPointQuotaTarget
            {
                Target = samplingPointQuotaTarget.Target
            };

            string uri = string.Format(@"{0}{1}/{2}/{3}/{4}/{5}", SurveysApi.AbsoluteUri, surveyId,
                SamplingPointsControllerName, samplingPointId, SamplingPointsQuotaControllerName,
                samplingPointQuotaTarget.LevelId);

            return Client.PatchAsJsonAsync(uri, updatedSamplingPointQuotaTarget)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask => JsonConvert.DeserializeObjectAsync<SamplingPointQuotaTarget>(stringTask.Result).Result)
             .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.SamplingPointImageAddAsync(string, string, string)"/>
        /// </summary>
        /// <returns>image file name</returns>
        public Task<string> SamplingPointImageAddAsync(string surveyId, string samplingPointId, string filePath)
        {
            if(!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
           
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            return SamplingPointImageAddAsync(surveyId, samplingPointId, Path.GetFileName(filePath), byteArrayContent);
        }

        /// <summary>
        /// <see cref="INfieldSurveysService.SamplingPointImageAddAsync(string, string, string, byte[])"/>
        /// </summary>
        /// <returns>image file name</returns>
        public Task<string> SamplingPointImageAddAsync(string surveyId, string samplingPointId, string filename, byte[] content)
        {
            var byteArrayContent = new ByteArrayContent(content);
            return SamplingPointImageAddAsync(surveyId, samplingPointId, filename, byteArrayContent);
        }

        private Task<string> SamplingPointImageAddAsync(string surveyId, string samplingPointId, string filename, ByteArrayContent byteArrayContent)
        {
            var uri = GetSamplingPointImageUri(surveyId, samplingPointId, filename);

            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            byteArrayContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };

            return Client.PostAsync(uri, byteArrayContent)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringTask => stringTask.Result.Substring(1, stringTask.Result.Length - 2))
                .FlattenExceptions();
        }

        public Task<SamplingPointImage> SamplingPointImageGetAsync(string surveyId, string samplingPointId)
        {
            var uri = GetSamplingPointImageUri(surveyId, samplingPointId, null);

            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => new SamplingPointImage
                    {
                        Content = responseMessageTask.Result.Content.ReadAsByteArrayAsync().Result,
                        FileName = responseMessageTask.Result.Content.Headers.ContentDisposition.FileName
                    })
                .FlattenExceptions();
        }

        public Task SamplingPointImageDeleteAsync(string surveyId, string samplingPointId)
        {
            var uri = GetSamplingPointImageUri(surveyId, samplingPointId, null);

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private static string SamplingPointsControllerName
        {
            get { return "samplingpoints";  }
        }

        private static string QuotaControllerName
        {
            get { return "quota"; }
        }
        private static string SamplingPointsQuotaControllerName
        {
            get { return "quotatargets"; }
        }

        private Uri SurveysApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "surveys/"); }
        }

        private static string SurveyInterviewerInstructionsControllerName
        {
            get { return "InterviewerInstructions"; }
        }

        private static string SamplingPointImageControllerName
        {
            get { return "Surveys"; }
        }

        /// <summary>
        /// Returns the URI to upload the interviewer instructions 
        /// based on the provided <paramref name="surveyId"/> and <paramref name="fileName"/>
        /// </summary>
        private string GetInterviewerInstructionUri(string surveyId, string fileName)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(@"Surveys/{0}/{1}/"
                , surveyId, SurveyInterviewerInstructionsControllerName);
            if (!string.IsNullOrEmpty(fileName))
                result.AppendFormat(@"/{0}", fileName);
            return result.ToString();
        }

        /// <summary>
        /// Returns the URI to upload the image associated with a sampling point
        /// <paramref name="surveyId"/>
        /// <paramref name="samplingPointId"/>
        /// <paramref name="fileName"/>
        /// </summary>
        private string GetSamplingPointImageUri(string surveyId, string samplingPointId, string fileName)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"{0}/{1}/SamplingPoint/{2}/Image/{3}",
                                        SamplingPointImageControllerName,
                                        surveyId,
                                        Uri.EscapeUriString(samplingPointId),
                                        !string.IsNullOrEmpty(fileName) ? Uri.EscapeUriString(fileName) : "");

            return result.ToString();
        }
    }

    /// <summary>
    /// Update model for a sampling point
    /// Instruction is not allowed to be updated (because this is a link to a pdf in blob storage)
    /// </summary>
    internal class UpdateSamplingPoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FieldworkOfficeId { get; set; }
        public string GroupId { get; set; }
        public string Stratum { get; set; }
    }

    /// <summary>
    /// Update model for a sampling point's qouta target
    /// </summary>
    internal class UpdateSamplingPointQuotaTarget
    {
        public int? Target { get; set; }
    }

    /// <summary>
    /// Update model for a survey
    /// </summary>
    internal class UpdateSurvey
    {
        /// <summary>
        /// Name of the survey
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// Name of the survey client
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The description of the survey
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The default interviewer instruction of a survey
        /// </summary>
        public string InterviewerInstruction { get; set; }
    }

}