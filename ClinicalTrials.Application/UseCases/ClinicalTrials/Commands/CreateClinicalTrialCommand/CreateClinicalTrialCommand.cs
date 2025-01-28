using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand
{
    public record CreateClinicalTrialCommand(IFormFile ClinicalTrialFile) : IRequest<Result<List<ClinicalTrialResponseDto>>>
    {
    }
}
