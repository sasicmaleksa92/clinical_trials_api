using FluentValidation;

namespace ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand
{
    public class CreateClinicalTrialCommandValidator : AbstractValidator<CreateClinicalTrialCommand>
    {
        public CreateClinicalTrialCommandValidator()
        {
            // Validate file is not null
            RuleFor(x => x.ClinicalTrialFile)
                .NotNull().WithMessage("File is required.");

            // Validate file size (e.g., max 1 MB)
            RuleFor(x => x.ClinicalTrialFile.Length)
                .LessThanOrEqualTo(1 * 1024 * 1024) // 1 MB
                .WithMessage("File size exceeds the limit of 1 MB.");

            // Validate file extension
            RuleFor(x => x.ClinicalTrialFile.FileName)
                .Must(fileName => fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only .json files are allowed.");
        }
    }
}
