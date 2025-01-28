using ClinicalTrials.Domain.Entities;
using ClinicalTrials.Domain.Enums;

namespace ClinicalTrialsApi.IntegrationTests.Common
{
    public class TestData
    {
        public const string ClinicalTrialsControllerEndpoint = "api/ClinicalTrials";


        public static List<ClinicalTrial> ClinicalTrialsTestList = new()
        {
            new ClinicalTrial
        {
            TrialId = "TR004",
            Title = "Diabetes Study",
            StartDate = new DateTime(2024, 1, 1),
            Status = ClinicalTrialStatusEnum.NotStarted,
            Participants = 3
        },
        new ClinicalTrial
        {
            TrialId = "TR005",
            Title = "Obesity Management Research",
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 2, 1),
            Status = ClinicalTrialStatusEnum.Completed,
            Participants = 300
        },
        new ClinicalTrial
        {
            TrialId = "TR006",
            Title = "Diabetes Study",
            StartDate = new DateTime(2024, 1, 1),
            Status = ClinicalTrialStatusEnum.Ongoing,
            Participants = 5
        },
        new ClinicalTrial
        {
            TrialId = "TR007",
            Title = "Diabetes Study",
            StartDate = new DateTime(2024, 3, 1),
            EndDate = new DateTime(2024, 6, 1),
            Status = ClinicalTrialStatusEnum.Completed,
            Participants = 500
        },
        };

    }
}
