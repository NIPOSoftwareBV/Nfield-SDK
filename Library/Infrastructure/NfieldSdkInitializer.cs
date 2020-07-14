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

using Nfield.SDK.Services.Implementation;
using Nfield.Services;
using Nfield.Services.Implementation;
using Nfield.Utilities;
using System;
using System.Collections.Generic;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Used to register the types into the user-defined IoC container.
    /// </summary>
    public static class NfieldSdkInitializer
    {
        internal static Dictionary<Type, Type> TypeMap = new Dictionary<Type, Type>()
        {
            { typeof(NfieldConnection), typeof(NfieldConnection) },
            { typeof(INfieldInterviewersService), typeof(NfieldInterviewersService) },
            { typeof(INfieldInterviewsService), typeof(NfieldInterviewsService) },
            { typeof(INfieldInterviewQualityService), typeof(NfieldInterviewQualityService) },
            { typeof(INfieldSurveysService), typeof(NfieldSurveysService) },
            { typeof(INfieldSurveyResourcesService), typeof(NfieldSurveyResourcesService) },
            { typeof(INfieldRespondentDataEncryptService), typeof(NfieldRespondentDataEncryptService) },
            { typeof(INfieldSurveyDataService), typeof(NfieldSurveyDataService) },
            { typeof(INfieldBackgroundTasksService), typeof(NfieldBackgroundTasksService) },
            { typeof(INfieldSurveyScriptService), typeof(NfieldSurveyScriptService) },
            { typeof(INfieldSurveyScriptFragmentService), typeof(NfieldSurveyScriptFragmentService) },
            { typeof(INfieldFieldworkOfficesService), typeof(NfieldFieldworkOfficesService) },
            { typeof(INfieldMediaFilesService), typeof(NfieldMediaFilesService) },
            { typeof(INfieldLanguagesService), typeof(NfieldLanguagesService) },
            { typeof(INfieldTranslationsService), typeof(NfieldTranslationsService) },
            { typeof(INfieldDomainEmailSettingsService), typeof(NfieldDomainEmailSettingsService) },
            { typeof(INfieldDomainSearchFieldsSettingService), typeof(NfieldDomainSearchFieldsSettingService) },
            { typeof(INfieldSurveyEmailSettingsService), typeof(NfieldSurveyEmailSettingsService) },
            { typeof(INfieldSurveyInvitationImagesService), typeof(NfieldSurveyInvitationImagesService) },
            { typeof(INfieldSurveyInvitationTemplatesService), typeof(NfieldSurveyInvitationTemplatesService) },
            { typeof(INfieldSurveySettingsService), typeof(NfieldSurveySettingsService) },
            { typeof(INfieldSurveyResponseCodesService), typeof(NfieldSurveyResponseCodesService) },
            { typeof(INfieldSurveyRelocationsService), typeof(NfieldSurveyRelocationsService) },
            { typeof(INfieldSurveyPublicIdsService), typeof(NfieldSurveyPublicIdsService) },
            { typeof(INfieldSurveyInterviewersService), typeof(NfieldSurveyInterviewersService) },
            { typeof(INfieldSamplingPointInterviewerAssignmentsService), typeof(NfieldSamplingPointInterviewerAssignmentsService) },
            { typeof(INfieldSurveyInterviewerAssignmentsService), typeof(NfieldSurveyInterviewerAssignmentsService) },
            { typeof(INfieldSurveyInterviewerWorkpackageDistributionService), typeof(NfieldSurveyInterviewerWorkpackageDistributionService) },
            { typeof(INfieldSurveyFieldworkService), typeof(NfieldSurveyFieldworkService) },
            { typeof(INfieldAddressesService), typeof(NfieldAddressesService) },
            { typeof(INfieldSurveyPackageService), typeof(NfieldSurveyPackageService) },
            { typeof(INfieldSurveyPublishService), typeof(NfieldSurveyPublishService) },
            { typeof(INfieldSurveySampleDataService), typeof(NfieldSurveySampleDataService) },
            { typeof(INfieldSurveySampleService), typeof(NfieldSurveySampleService) },
            { typeof(INfieldSurveyGroupService), typeof(NfieldSurveyGroupService) },
            { typeof(INfieldSurveyGroupAssignmentsService), typeof(NfieldSurveyGroupAssignmentsService) },
            { typeof(INfieldHttpClient), typeof(DefaultNfieldHttpClient) },
            { typeof(IFileSystem), typeof(FileSystem) },
            { typeof(INfieldEncryptionUtility), typeof(NfieldEncryptionUtility) },
            { typeof(IAesManagedWrapper), typeof(AesManagedWrapper) },
            { typeof(INfieldSurveyInviteRespondentsService), typeof(NfieldSurveyInviteRespondentsService) },
            { typeof(INfieldSurveyVarFileService), typeof(NfieldSurveyVarFileService) },
            { typeof(INfieldSurveysSearchService), typeof(NfieldSurveysSearchService) },
            { typeof(INfieldExternalApisService), typeof(NfieldExternalApisService) },
            { typeof(INfieldExternalApisLogService), typeof(NfieldExternalApisLogService) },
            { typeof(INfieldLocalUserService), typeof(NfieldLocalUserService) },
            { typeof(INfieldCatiInterviewersService), typeof(NfieldCatiInterviewersService) },
            { typeof(INfieldSurveyVersionsService), typeof(NfieldSurveyVersionsService) },
            { typeof(INfieldQuotaService), typeof(NfieldQuotaService) }
        };

        /// <summary>
        /// Method that registers all known types by calling the delegates provided.
        /// This method must be called before using the SDK.
        /// </summary>
        /// <param name="registerTransient">Method that registers a Transient type.</param>
        /// <param name="registerSingleton">Method that registers a Singleton.</param>
        /// <param name="registerInstance">Method that registers an instance.</param>
        [Obsolete("Dependency injection for Nfield Services is no longer supported. Please use NfieldConnection.GetService instead.")]
        public static void Initialize(Action<Type, Type> registerTransient,
                                      Action<Type, Type> registerSingleton,
                                      Action<Type, object> registerInstance)
        {
            foreach (var type in TypeMap.Keys)
            {
                registerTransient(type, TypeMap[type]);
            }
        }

    }
}