using ClinicalTrials.Domain.Enums;
using ClinicalTrialsApi.WebApi.Converters;
using System.Text.Json.Serialization;

namespace ClinicalTrials.Application.Dtos
{
    public class ClinicalTrialDto
    {
        [JsonPropertyName("trialId")]
        public string TrialId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("participants")]
        public int? Participants { get; set; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(TrialStatusConverter))]
        public ClinicalTrialStatusEnum Status { get; set; }
    }
}
