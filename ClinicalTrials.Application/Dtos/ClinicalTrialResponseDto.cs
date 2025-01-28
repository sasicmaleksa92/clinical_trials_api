namespace ClinicalTrials.Application.Dtos
{
    public class ClinicalTrialResponseDto
    {
        public string TrialId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Participants { get; set; }

        public string Status { get; set; }

        public int? DurationInDays { get; set; } 
    }
}
