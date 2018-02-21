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
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    public class NfieldDeleteInterviewServiceTests : NfieldServiceTestsBase
    {
        private readonly string _surveyId = Guid.NewGuid().ToString();
        private readonly int _interviewId = new Random().Next(9999);
        private readonly NfieldDeleteInterviewService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldDeleteInterviewServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldDeleteInterviewService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

        }
      

        [Fact]
        public void DeleteAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DeleteAsync(null,_interviewId)));
        }

        [Fact]
        public void DeleteAsync_SurveyIdIsNotValid_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DeleteAsync("id", _interviewId)));
        }

    }
}
