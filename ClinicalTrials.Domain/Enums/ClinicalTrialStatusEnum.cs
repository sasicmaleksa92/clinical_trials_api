using System.ComponentModel;

namespace ClinicalTrials.Domain.Enums
{
    public enum ClinicalTrialStatusEnum
    {
        [Description("Not Started")]
        NotStarted = 1,

        [Description("Ongoing")]
        Ongoing = 2,

        [Description("Completed")]
        Completed = 3
    }
}
