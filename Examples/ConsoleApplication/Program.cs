﻿//    This file is part of Nfield.SDK.
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

using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApplication
{

    /// <summary>
    /// Sample application for demonstrating Nfield SDK usage.
    /// </summary>
    class Program
    {
        static void Main()
        {
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
#endif
            const string serverUrl = "http://localhost:81/v1";

            // First step is to get an INfieldConnection which provides services used for data access and manipulation. 
            INfieldConnection connection = NfieldConnectionFactory.Create(new Uri(serverUrl));

            // User must sign in to the Nfield server with the appropriate credentials prior to using any of the services.

            connection.SignInAsync("testdomain", "user1", "password123").Wait();

            // Request the Interviewers service to manage interviewers.
            INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();

            // Request the Survey service and the sampling point managment
            INfieldSamplingPointsService samplingPointsService = connection.GetService<INfieldSamplingPointsService>();
            NfieldSamplingPointManagement samplingPointsManager = new NfieldSamplingPointManagement(samplingPointsService);

            // Example of performing operations on sampling points.
            samplingPointsManager.QueryForSamplingPoint("some surveyId", "some sampling pointId");

            //
            // Survey Management
            //
            INfieldSurveysService surveysService = connection.GetService<INfieldSurveysService>();
            // Create survey
            var createdSurvey = surveysService.AddAsync(new Survey(SurveyType.Advanced)
            {
                ClientName = "clientName",
                Description = "description",
                SurveyName = "abc"
            }).Result;

            // Update survey
            // Note SurveyId and SurveyType are not allowed to be changed
            createdSurvey.ClientName = "Nfield";
            surveysService.UpdateAsync(createdSurvey).Wait(); // We do nothing with the result here

            // Upload interviewer instructions file
            var fileContent = Encoding.Unicode.GetBytes("Interviewer Instructions");
            surveysService.UploadInterviewerFileInstructionsAsync(fileContent, "instructions.pdf",
                createdSurvey.SurveyId).Wait();

            // Query survey
            var query = surveysService.QueryAsync().Result.Where(s => s.ClientName == "Nfield");
            var survey = query.FirstOrDefault();

            // Delete survey
            surveysService.RemoveAsync(survey).Wait();

            // Get ODIN script for survey
            // Note: the survey with id 'surveyWithOdinScriptId' has a odin script uploaded
            var surveyScriptService = connection.GetService<INfieldSurveyScriptService>();
            var scriptModel = surveyScriptService.GetAsync("surveyWithOdinScriptId").Result;

            // Upload ODIN script for survey
            // Note: the survey with id 'surveyWithOdinScriptId' has a odin script uploaded
            var myScript = new SurveyScript
            {
                FileName = "myFileq.odin",
                Script = @"*QUESTION 1 *CODES 61L1
                           Do you watch TV?
                            1:Yes
                            2:No
                            *END"
            };
            var updatedScript = surveyScriptService.PostAsync("surveyWithOdinScriptId", myScript).Result;
            // Upload survey script with file path 
            var scriptFilePath = @"C:\SimpleQ.odin";
            var myUpdatedScript =
                surveyScriptService.PostAsync("surveyWithOdinScriptId", scriptFilePath).Result;

            // Create survey
            var newSurvey = surveysService.AddAsync(new Survey(SurveyType.Basic)
            {
                ClientName = "clientName",
                Description = "description",
                SurveyName = "abc"
            }).Result;

            surveyScriptService.PostAsync(newSurvey.SurveyId, myScript).Wait();

            var surveyFieldworkService = connection.GetService<INfieldSurveyFieldworkService>();
            //Get survey fieldwork status 
            var surveyFieldworkStatus = surveyFieldworkService.GetStatusAsync(newSurvey.SurveyId).Result; //Should be under construction

            // Start the fieldwork for the survey
            surveyFieldworkService.StartFieldworkAsync(newSurvey.SurveyId).Wait();
            surveyFieldworkStatus = surveyFieldworkService.GetStatusAsync(newSurvey.SurveyId).Result; //Should be started

            // Example of a download data request: filtering testdata collected today
            var surveyDataService = connection.GetService<INfieldSurveyDataService>();

            var surveyDataRequest = new SurveyDataRequest
            {
                FileName = "MyFileName",
                StartDate = DateTime.Today.ToUniversalTime(), // UTC time start of today
                EndDate = DateTime.Today.AddDays(1).ToUniversalTime(), // UTC time end of today
                SurveyVersion = "637242848690284790", // If the SurveyVersion (Etag) is specified only the surveys matching this version will be downloaded
                IncludeSuccessful = true,
                IncludeScreenOut = true,
                IncludeDroppedOut = true,
                IncludeRejected = true,
                IncludeTestData = true,
                IncludeClosedAnswers = true,
                IncludeOpenAnswers = true,
                IncludeParaData = true,
                IncludeCapturedMediaFiles = true,
                IncludeVarFile = true,
                IncludeQuestionnaireScript = true
            };

            var downloadUrl = surveyDataService.PrepareDownload(newSurvey.SurveyId, surveyDataRequest).Result;

            new WebClient().DownloadFile(downloadUrl, "mydownload");

            //
            // sample management
            //

            // sample is a csv file with headers
            const string sample = @"
RepondentKey, Email, Gender
rk1, jan@apekool.com, male
rk2, truus@apekool.com, female";

            var sampleManagement = new NfieldSampleManagement(connection.GetService<INfieldSurveySampleService>());

            // upload the sample
            sampleManagement.UploadSample(survey.SurveyId, sample);

            // download the sample
            sampleManagement.DownloadSample(survey.SurveyId);

            // delete a sample record
            sampleManagement.DeleteSample(survey.SurveyId, "rk1");
        }
    }
}