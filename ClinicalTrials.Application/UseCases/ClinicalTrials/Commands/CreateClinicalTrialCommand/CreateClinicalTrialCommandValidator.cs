using ClinicalTrials.Domain.Configuration;
using FluentValidation;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand
{
    public class CreateClinicalTrialCommandValidator : AbstractValidator<CreateClinicalTrialCommand>
    {
        public CreateClinicalTrialCommandValidator()
        {
            RuleFor(x => x.ClinicalTrialFile)
                .NotNull().WithMessage("File is required.");

            RuleFor(x => x.ClinicalTrialFile.FileName)
                .Must(fileName => fileName.EndsWith(Configuration.AppSettings.UploadClinicalTrialFileAllowedExtensions, StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only .json files are allowed.");
        }
    }
}
