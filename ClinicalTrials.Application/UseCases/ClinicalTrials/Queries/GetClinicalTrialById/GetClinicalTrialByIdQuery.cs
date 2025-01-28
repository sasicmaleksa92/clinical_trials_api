using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using MediatR;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialById
{
    public class GetClinicalTrialByIdQuery : IRequest<Result<ClinicalTrialResponseDto>>
    {
        public string TrialId { get; set; }
    }
}
