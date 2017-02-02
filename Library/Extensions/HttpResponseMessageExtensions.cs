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
            {
                return response;
            }

            var message = response.Content?.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(message))
            {
                message = response.ReasonPhrase;
            }
            else
            {
                try
                {
                    var httpErrorDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);
                    object temp;
                    if (httpErrorDictionary.TryGetValue("NfieldErrorCode", out temp))
                    {
                        var nfieldErrorCode = (NfieldErrorCode)Enum.Parse(typeof(NfieldErrorCode), temp.ToString());

                        var nfieldMessage = httpErrorDictionary.TryGetValue("Message", out temp)
                            ? temp.ToString()
                            : message;

                        throw new NfieldErrorException(response.StatusCode, nfieldErrorCode, nfieldMessage);
                    }
                }
                catch (JsonException)
                {
                    // it's not json
                }

            }

            throw new NfieldHttpResponseException(response.StatusCode, message);
        }
    }
}