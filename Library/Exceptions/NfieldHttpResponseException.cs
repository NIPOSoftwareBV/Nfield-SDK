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

namespace Nfield.Exceptions
{
    /// <summary>
    /// Base class from which will inherit all exceptions.
    /// </summary>
    [Serializable]
    public class NfieldHttpResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class.
        /// </summary>
        public NfieldHttpResponseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class with a specified error message and a http status code.
        /// </summary>
        /// <param name="httpStatusCode">The http status code that was sent by the server during the error.</param>
        /// <param name="message">A message that describes the error.</param>
        public NfieldHttpResponseException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public NfieldHttpResponseException(String message)
            : base(message) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class with a specified error message and a reference 
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference,
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public NfieldHttpResponseException(string message, Exception inner)
            : base(message, inner) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NfieldHttpResponseException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public NfieldHttpResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        /// <summary>
        /// The <see cref="HttpStatusCode" /> that was returned by the server
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; private set; }
    }

}