using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.SDK.Services.Implementation;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyCustomColumnsService"/>
    /// </summary>
    public class NfieldSurveyCustomColumnsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "SurveyId";

        [Fact]
        public void TestGetSurveyCustomColumnsAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyCustomColumnsService();
            Assert.ThrowsAsync<ArgumentNullException>(() => target.GetSurveyCustomColumnsAsync(null));
        }

        [Fact]
        public void TestGetSurveyCustomColumnsAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyCustomColumnsService();
            Assert.ThrowsAsync<ArgumentException>(() => target.GetSurveyCustomColumnsAsync(""));
        }

        [Fact]
        public async Task TestGetSurveyCustomColumnsAsync_ReturnsSurveyCustomColumns()
        {
            const string ColumnName0 = "Column1";
            const string ColumnName1 = "Column2";

            var expectedColumnNames = new string[] { ColumnName0, ColumnName1 };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/SurveyId/CustomColumns")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedColumnNames))));

            var target = new NfieldSurveyCustomColumnsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = (await target.GetSurveyCustomColumnsAsync(SurveyId)).ToList();

            Assert.Equal(ColumnName0, actual[0]);
            Assert.Equal(ColumnName1, actual[1]);
        }
    }
}
