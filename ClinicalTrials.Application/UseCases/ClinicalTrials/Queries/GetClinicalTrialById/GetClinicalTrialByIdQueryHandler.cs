using AutoMapper;
using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using ClinicalTrials.Application.Interfaces.Repositories;
using MediatR;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialById
{
    public class GetClinicalTrialByIdQueryHandler : IRequestHandler<GetClinicalTrialByIdQuery, Result<ClinicalTrialResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IClinicalTrialRepository _repository;

        public GetClinicalTrialByIdQueryHandler(IMapper mapper, IClinicalTrialRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<ClinicalTrialResponseDto>> Handle(GetClinicalTrialByIdQuery request, CancellationToken cancellationToken)
        {
            var clinicalTrialEntity = await _repository.GetFirstAsync(filter: x => x.TrialId == request.TrialId);
            if (clinicalTrialEntity == null)
            {
                return Result<ClinicalTrialResponseDto>.Failure("Clinical trial not found.");
            }
            var clinicalTrial = _mapper.Map<ClinicalTrialResponseDto>(clinicalTrialEntity);

            return Result<ClinicalTrialResponseDto>.Success(clinicalTrial);
        }
    }
}
