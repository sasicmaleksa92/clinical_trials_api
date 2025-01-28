using AutoMapper;
using ClinicalTrials.Application.Common.PredicateBuilder;
using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using ClinicalTrials.Application.Interfaces.Repositories;
using ClinicalTrials.Domain.Entities;
using ClinicalTrials.Domain.Enums;
using MediatR;
using System.Linq.Expressions;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Queries.GetClinicalTrialsFiltered
{
    public class GetClinicalTrialsFilteredQueryHandler : IRequestHandler<GetFilteredClinicalTrialsQuery, Result<List<ClinicalTrialResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IClinicalTrialRepository _repository;

        public GetClinicalTrialsFilteredQueryHandler(IClinicalTrialRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<List<ClinicalTrialResponseDto>>> Handle(GetFilteredClinicalTrialsQuery request, CancellationToken cancellationToken)
        {
            
            if (CheckIfAllPropertiesAreNull(request))
            {
                return Result<List<ClinicalTrialResponseDto>>.Failure("At least one filter must be provided.");
            }

            var filter = BuildFilterExpression(request);
            var clinicalTrialEntities = await _repository.GetAsync(filter: filter); 

            if (!clinicalTrialEntities.Any())
            {
                return Result<List<ClinicalTrialResponseDto>>.Failure("Clinical trials not found.");
            }

            var clinicalTrials = _mapper.Map<List<ClinicalTrialResponseDto>>(clinicalTrialEntities);

            return Result<List<ClinicalTrialResponseDto>>.Success(clinicalTrials);

        }

        private bool CheckIfAllPropertiesAreNull(GetFilteredClinicalTrialsQuery request)
        {
            // Use reflection to check if all properties of the request are null
            return request.GetType()
                .GetProperties()
                .Select(prop => prop.GetValue(request))
                .All(value => value == null);
        }

        private Expression<Func<ClinicalTrial, bool>> BuildFilterExpression(GetFilteredClinicalTrialsQuery request)
        {
            // Initialize a filter expression
            Expression<Func<ClinicalTrial, bool>> filter = x => true;

            // Dynamically build the filter expression based on the input
            if(request.Title != null)
                filter = filter.And(x => x.Title == request.Title);

            if (request.Status != null && Enum.TryParse(request.Status, out ClinicalTrialStatusEnum status))
                filter = x => x.Status == status;

            if (request.StartDate != null)
                filter = filter.And(x => x.StartDate == request.StartDate);

            if (request.EndDate != null)
                filter = filter.And(x => x.EndDate == request.EndDate);

            if (request.Participants != null)
                filter = filter.And(x => x.Participants == request.Participants);

            return filter;
        }
    }
}
