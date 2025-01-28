using ClinicalTrials.Domain.Enums;

namespace ClinicalTrials.Domain.Entities
{
    public class ClinicalTrial
    {
        public int Id { get; set; }
        public string TrialId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Participants { get; set; } 

        public ClinicalTrialStatusEnum Status { get; set; }

        public int? DurationInDays
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return (EndDate.Value - StartDate).Days;
                }
                return null;
            }
            set { }
        }

    }

}
