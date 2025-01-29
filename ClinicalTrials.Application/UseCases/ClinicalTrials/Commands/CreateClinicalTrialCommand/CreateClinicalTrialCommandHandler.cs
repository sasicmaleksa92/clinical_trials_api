using AutoMapper;
using ClinicalTrials.Application.Common.FileProcessing;
using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Dtos;
using ClinicalTrials.Application.Interfaces.Repositories;
using ClinicalTrials.Domain.Configuration;
using ClinicalTrials.Domain.Entities;
using MediatR;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand
{
    public class CreateClinicalTrialCommandHandler : IRequestHandler<CreateClinicalTrialCommand, Result<List<ClinicalTrialResponseDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IClinicalTrialRepository _repository;
        private readonly JsonFileProcessor<ClinicalTrialDto> _jsonFileProcessor;

        public CreateClinicalTrialCommandHandler(IMapper mapper, IClinicalTrialRepository repository, JsonFileProcessor<ClinicalTrialDto> jsonFileProcessor)
        {
            _mapper = mapper;
            _repository = repository;
            _jsonFileProcessor = jsonFileProcessor;
        }

        public async Task<Result<List<ClinicalTrialResponseDto>>> Handle(CreateClinicalTrialCommand request, CancellationToken cancellationToken)
        {

            var fileProcessingResult = await _jsonFileProcessor.ProcessFileAsync(request.ClinicalTrialFile.OpenReadStream(),
                Configuration.AppSettings.UploadClinicalTrialFileJsonSchemaFileName);

            if (!fileProcessingResult.IsSuccess)
            {
                return Result<List<ClinicalTrialResponseDto>>.Failure(fileProcessingResult.Error);
            }

            var clinicalTrials = fileProcessingResult.Value;
            // Check for duplicate TrialIds
            var trialIds = clinicalTrials.Select(t => t.TrialId).ToList();
            var existingTrials = await _repository.GetAsync(filter: x => trialIds.Contains(x.TrialId));

            if (existingTrials.Any())
            {
                var duplicateIds = string.Join(", ", existingTrials.Select(e => e.TrialId));
                return Result<List<ClinicalTrialResponseDto>>.Failure($"The following TrialIds already exist in the database: {duplicateIds}");
            }

            //Mapping
            var clinicalTrialEntities = _mapper.Map<List<ClinicalTrial>>(clinicalTrials);
            // Save to the database
            await _repository.AddManyAsync(clinicalTrialEntities, cancellationToken);

            var clinicalTrialRespone = _mapper.Map<List<ClinicalTrialResponseDto>>(clinicalTrialEntities);

            return Result<List<ClinicalTrialResponseDto>>.Success(clinicalTrialRespone);
        }

    }
}
