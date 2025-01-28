using ClinicalTrials.Application.Dtos;
using ClinicalTrialsApi.IntegrationTests.Common;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ClinicalTrialsApi.IntegrationTests
{
    public class ClinicalTrialsControllerTests : TestBase
    {
        public ClinicalTrialsControllerTests(TestServer server) : base(server)
        {
        }

        [Trait("Type", "Integration")]
        [Fact]
        public async Task GetByIdEndpoint_ReturnValidResult()
        {
            string trialId = "TR005";
            var response = await _client.SendAsync(GetHttpRequestMessageForGetByIdEndpoint(trialId));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ClinicalTrialResponseDto>();

            result?.Title.Should().Be("Obesity Management Research");
            result?.Status.Should().Be("Completed");
            result?.Participants.Should().Be(300);

        }

        [Trait("Type", "Integration")]
        [Fact]
        public async Task GetByIdEndpoint_ReturnNotFound()
        {
            string trialId = "TR030";
            var response = await _client.SendAsync(GetHttpRequestMessageForGetByIdEndpoint(trialId));
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }


        [Trait("Type", "Integration")]
        [Fact]
        public async Task GetFilteredEndpoint_ReturnValidResultsForStatusSearch()
        {
            string status = "Completed";

            var response = await _client.SendAsync(GetHttpRequestMessageForGetFilteredEndpoint(status: status));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<List<ClinicalTrialResponseDto>>();

            result.Should().NotBeEmpty();
            result.Should().HaveCount(2);
            result.All(x => x.Status == status).Should().BeTrue();
        }

        [Trait("Type", "Integration")]
        [Fact]
        public async Task UploadClinicalTrialJsonFileReturnValidResults()
        {

            var response = await _client.SendAsync(GetHttpRequestMessageForUploadJsonEndpoint());

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<List<ClinicalTrialResponseDto>>();

            result.Should().NotBeEmpty();
            result[0].TrialId.Should().Be("TR001");
            result[1].Title.Should().Be("Cardiovascular Health Study");
            result[2].Status.Should().Be("Ongoing");

        }

        #region Helper methods
        private HttpRequestMessage GetHttpRequestMessageForUploadJsonEndpoint()
        {
            var jsonContent = ReadEmbeddedResource("ClinicalTrialsApi.IntegrationTests.Resources.ClinicalTrial.json");
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(jsonContent));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var multipartContent = new MultipartFormDataContent
            {
                { fileContent, "clinicalTrialFile", "ClinicalTrial.json" }
            };

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://localhost/{TestData.ClinicalTrialsControllerEndpoint}/upload-clinical-trial"),
                Content = multipartContent
            };

            return requestMessage;
        }

        private HttpRequestMessage GetHttpRequestMessageForGetByIdEndpoint(string trialId)
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost/{TestData.ClinicalTrialsControllerEndpoint}/GetById/{trialId}")
            };

            return requestMessage;
        }

        private HttpRequestMessage GetHttpRequestMessageForGetFilteredEndpoint(string status = null,
            string title = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? participants = null)
        {
            // Build query parameters
            var queryParameters = new List<string>();

            if (!string.IsNullOrEmpty(status))
                queryParameters.Add($"status={Uri.EscapeDataString(status)}");

            if (!string.IsNullOrEmpty(title))
                queryParameters.Add($"title={Uri.EscapeDataString(title)}");

            if (startDate.HasValue)
                queryParameters.Add($"startDate={Uri.EscapeDataString(startDate.Value.ToString("o"))}");

            if (endDate.HasValue)
                queryParameters.Add($"endDate={Uri.EscapeDataString(endDate.Value.ToString("o"))}");

            if (participants.HasValue)
                queryParameters.Add($"participants={participants.Value}");

            var queryString = string.Join("&", queryParameters);

            var requestUri = $"https://localhost/{TestData.ClinicalTrialsControllerEndpoint}/GetFiltered";

            if (!string.IsNullOrEmpty(queryString))
                requestUri += $"?{queryString}";


            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri)
            };

            return requestMessage;
        }
        #endregion
    }
}