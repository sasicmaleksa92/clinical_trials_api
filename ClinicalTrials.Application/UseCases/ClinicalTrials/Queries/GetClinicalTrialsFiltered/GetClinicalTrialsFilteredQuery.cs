using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using MediatR;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialsFiltered
{
    public class GetFilteredClinicalTrialsQuery : IRequest<Result<List<ClinicalTrialResponseDto>>>
    {
        public string Status { get; init; }
        public string Title { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public int? Participants { get; init; }

    }
}
