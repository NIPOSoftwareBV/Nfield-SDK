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
using System.Net.Http;
using Newtonsoft.Json;
using Nfield.Exceptions;
using Nfield.Infrastructure.NipoSoftware.Nfield.Manager.Api.Helpers;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="HttpResponseMessage"/> class.
    /// </summary>
    internal static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Valides http response message and throws appropriate exceptions when errors have occured.
        /// E.g. a NfieldErrorException is thrown if the response contains a NfieldErrorCode.
        /// </summary>
        public static HttpResponseMessage ValidateResponse(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return response;

            Dictionary<string, object> httpErrorDictionary = null;
            if (response.Content != null)
            {
                var contentString = response.Content.ReadAsStringAsync().Result;
                try
                {
                    httpErrorDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                }
                catch (Exception)
                {
                    // it's not json
                }
            }

            object temp;
            if (httpErrorDictionary != null && httpErrorDictionary.TryGetValue("NfieldErrorCode", out temp))
            {
                NfieldErrorCode nfieldErrorCode = (NfieldErrorCode)Enum.Parse(typeof(NfieldErrorCode), temp.ToString());

                string message = httpErrorDictionary.TryGetValue("Message", out temp) ? temp.ToString() : response.ReasonPhrase;

                throw new NfieldErrorException(response.StatusCode, nfieldErrorCode, message);
            }

            throw new NfieldHttpResponseException(response.StatusCode, response.ReasonPhrase);
        }
    }
}