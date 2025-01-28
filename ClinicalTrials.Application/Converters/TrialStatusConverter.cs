using System.Text.Json.Serialization;
using System.Text.Json;
using ClinicalTrials.Domain.Enums;

namespace ClinicalTrialsApi.WebApi.Converters
{
    public class TrialStatusConverter : JsonConverter<ClinicalTrialStatusEnum>
    {
        public override ClinicalTrialStatusEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var statusString = reader.GetString();
            return statusString switch
            {
                "Not Started" => ClinicalTrialStatusEnum.NotStarted,
                "Ongoing" => ClinicalTrialStatusEnum.Ongoing,
                "Completed" => ClinicalTrialStatusEnum.Completed,
                _ => throw new JsonException($"Invalid value for TrialStatus: {statusString}")
            };
        }

        public override void Write(Utf8JsonWriter writer, ClinicalTrialStatusEnum value, JsonSerializerOptions options)
        {
            var statusString = value switch
            {
                ClinicalTrialStatusEnum.NotStarted => "Not Started",
                ClinicalTrialStatusEnum.Ongoing => "Ongoing",
                ClinicalTrialStatusEnum.Completed => "Completed",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

            writer.WriteStringValue(statusString);
        }
    }
}
