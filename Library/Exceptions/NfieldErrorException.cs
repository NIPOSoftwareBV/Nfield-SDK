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
using System.Net;
using System.Runtime.Serialization;
using Nfield.Infrastructure.NipoSoftware.Nfield.Manager.Api.Helpers;

namespace Nfield.Exceptions
{
    /// <summary>
    /// Exception indicating an error on the server.
    /// </summary>
    [Serializable]
    public class NfieldErrorException : NfieldHttpResponseException
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="NfieldErrorException"/> class.
        /// </summary>
        public NfieldErrorException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class with a http status code, a nfield error code and an error message.
        /// </summary>
        /// <param name="httpStatusCode">The http status code that was sent by the server during the error.</param>
        /// <param name="nfieldErrorCode">The error Nfield specific error code.</param>
        /// <param name="message">A message that describes the error.</param>
        public NfieldErrorException(HttpStatusCode httpStatusCode, NfieldErrorCode nfieldErrorCode, string message)
            : base(httpStatusCode, message)
        {
            NfieldErrorCode = nfieldErrorCode;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldErrorException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public NfieldErrorException(String message)
            : base(message) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldErrorException"/> class with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public NfieldErrorException(string message, Exception inner)
            : base(message, inner) {}


        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldErrorException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public NfieldErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        /// <summary>
        /// The error Nfield specific error code, <see cref="NfieldErrorCode"/>.
        /// </summary>
        public NfieldErrorCode NfieldErrorCode { get; private set; }
    }

}