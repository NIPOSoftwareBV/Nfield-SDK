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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services;
using Ninject;

namespace Nfield.SDK.Samples
{

    /// <summary>
    /// Sample application for demonstrating Nfield SDK usage.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Example of using the Nfield SDK with a user defined IoC container.
            // In most cases the IoC container is created and managed through the application. 
            using(IKernel kernel = new StandardKernel())
            {
                InitializeNfield(kernel);

                const string serverUrl = "http://localhost:81/v1";
                                                             
                // First step is to get an INfieldConnection which provides services used for data access and manipulation. 
                INfieldConnection connection = NfieldConnectionFactory.Create(new Uri(serverUrl));

                // User must sign in to the Nfield server with the appropriate credentials prior to using any of the services.

                connection.SignInAsync("testdomain", "user1", "password123").Wait();
                
                // Request the Interviewers service to manage interviewers.
                INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();

                // Create a new manager to perform the operations on the service.
                NfieldInterviewersManagement interviewersManager = new NfieldInterviewersManagement(interviewersService);

                // This sample shows various ways of performing synchronous and asynchronous operations on Interviewers.
                var t1 = interviewersManager.AddInterviewerAsync();
                var interviewer2 = interviewersManager.AddInterviewer();

                // Update the interviewer name asynchronously
                interviewer2.FirstName = "Harry";
                var t2 = interviewersManager.UpdateInterviewerAsync(interviewer2);

                // Wait for all pending tasks to finish
                Task.WaitAll(t1, t2);

                // Extract the results from the asynchronous tasks
                interviewer2 = t2.Result;
                var interviewer1 = t1.Result;

                // Update interviewer name synchronous
                interviewer1.EmailAddress = interviewer1.EmailAddress + "changed";
                interviewer1.FirstName = "Bob";
                interviewer1 = interviewersManager.UpdateInterviewer(interviewer1);

                // Change password for interviewer, asynchronously and synchronously
                var t3 = interviewersManager.ChangePasswordAsync(interviewer2, "ab12345");
                interviewersManager.ChangePassword(interviewer1, "12345ab");

                t3.Wait();

                interviewersManager.QueryForInterviewers();
                interviewersManager.QueryForInterviewersAsync();

                interviewersManager.RemoveInterviewerAsync(interviewer1).Wait();
                interviewersManager.RemoveInterviewer(interviewer2);
                
                // Request the Survey service and the sampling point managment
                INfieldSurveysService surveysService = connection.GetService<INfieldSurveysService>();
                NfieldSamplingPointManagement samplingPointsManager = new NfieldSamplingPointManagement(surveysService);

                // Example of performing operations on sampling points.
                samplingPointsManager.QueryForSamplingPoints("some surveyId");
                
                //
                // Survey Management
                //

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

                var myRequest = new SurveyDownloadDataRequest
                {
                    DownloadSuccessfulLiveInterviewData = false,
                    DownloadNotSuccessfulLiveInterviewData = false,
                    DownloadOpenAnswerData = true,
                    DownloadClosedAnswerData = true,
                    DownloadSuspendedLiveInterviewData = false,
                    DownloadCapturedMedia = false,
                    DownloadParaData = false,
                    DownloadTestInterviewData = true,
                    DownloadFileName = "MyFileName",
                    StartDate = DateTime.Today.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), // UTC time start of today
                    EndDate = DateTime.Today.AddDays(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), // UTC time end of today
                    SurveyId = "SomeSurveyId"
                };
                
                var task = surveyDataService.PostAsync(myRequest).Result;

                // request the background tasks service 
                var backgroundTasksService = connection.GetService<INfieldBackgroundTasksService>();

                // Example of performing operations on background tasks.
                var backgroundTaskQuery = backgroundTasksService.QueryAsync().Result.Where(s => s.Id == task.Id);
                var mybackgroundTask = backgroundTaskQuery.FirstOrDefault();

                if (mybackgroundTask != null)
                {
                    var status = mybackgroundTask.Status;
                }

                // Example of creating a new translation
                const string surveyName = "Language";
                const string languageName = "Dutch";
                const string translationName = "ButtonNext";
                const string translationText = "Volgende";
                // First create the survey if it does not exist
                var languageSurvey = surveysService.QueryAsync().Result
                        .SingleOrDefault(s => s.SurveyName == surveyName);
                if (languageSurvey == null)
                    languageSurvey = surveysService.AddAsync(
                        new Survey(SurveyType.Basic)
                        {
                            SurveyName = surveyName
                        }).Result;
                // Then find the language we wish to change a translation for
                var languageService = connection.GetService<INfieldLanguagesService>();
                var language = languageService.QueryAsync(languageSurvey.SurveyId)
                    .Result.SingleOrDefault(l => l.Name == languageName);
                if (language == null)
                {
                    language = languageService.AddAsync(languageSurvey.SurveyId,
                            new Language { Name = languageName }).Result;
                }
                // Now add or update a translation
                var translationsService = connection.GetService<INfieldTranslationsService>();
                var translation = translationsService.QueryAsync(languageSurvey.SurveyId,
                        language.Id).Result.SingleOrDefault(t => t.Name == translationName);
                if (translation == null)
                {
                    translationsService.AddAsync(languageSurvey.SurveyId,
                        language.Id, new Translation
                        {
                            Name = translationName,
                            Text = translationText
                        }).Wait();
                }
                else
                {
                    translation.Text = translationText;
                    translationsService.UpdateAsync(languageSurvey.SurveyId,
                        language.Id, translation).Wait();
                }

            }
        }

        /// <summary>
        /// Example of initializing the SDK with Ninject as the IoC container.
        /// </summary>
        private static void InitializeNfield(IKernel kernel)
        {
            DependencyResolver.Register(type => kernel.Get(type), type => kernel.GetAll(type));

            NfieldSdkInitializer.Initialize((bind, resolve) => kernel.Bind(bind).To(resolve).InTransientScope(),
                                            (bind, resolve) => kernel.Bind(bind).To(resolve).InSingletonScope(),
                                            (bind, resolve) => kernel.Bind(bind).ToConstant(resolve));
        }
    }
}