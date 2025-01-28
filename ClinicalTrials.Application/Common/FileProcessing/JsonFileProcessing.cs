using ClinicalTrials.Application.Common.ResultPattern;
using ClinicalTrials.Application.Common.Validators;
using System.Text.Json;

namespace ClinicalTrials.Application.Common.FileProcessing
{
    public class JsonFileProcessor<T>
    {
        private readonly JsonSchemaValidator _schemaValidator;

        public JsonFileProcessor(JsonSchemaValidator schemaValidator)
        {
            _schemaValidator = schemaValidator;
        }

        public async Task<Result<List<T>>> ProcessFileAsync(Stream fileStream, string schemaFilePath)
        {
            try
            {
                // Read file content
                using var streamReader = new StreamReader(fileStream);
                var jsonString = await streamReader.ReadToEndAsync();

                // Validate JSON schema
                var validationResult = _schemaValidator.ValidateList(jsonString, schemaFilePath);
                if (!validationResult.IsSuccess)
                {
                    return Result<List<T>>.Failure(validationResult.Error);
                }

                // Deserialize JSON into DTOs
                var objects = JsonSerializer.Deserialize<List<T>>(jsonString);

                if (objects == null)
                {
                    return Result<List<T>>.Failure("Invalid JSON format.");
                }

                return Result<List<T>>.Success(objects);
            }
            catch (JsonException ex)
            {
                return Result<List<T>>.Failure($"Error processing JSON file: {ex.Message}");
            }
        }
    }
}
