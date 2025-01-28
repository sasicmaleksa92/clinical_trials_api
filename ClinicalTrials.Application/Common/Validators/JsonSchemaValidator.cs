using ClinicalTrials.Application.Common.ResultPattern;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Reflection;
using ClinicalTrials.Application.Interfaces;

namespace ClinicalTrials.Application.Common.Validators
{
    public class JsonSchemaValidator
    {
        private readonly IFileReader _fileReader;

        public JsonSchemaValidator(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public Result<List<JObject>> ValidateList(string jsonString, string jsonSchemaFilePath)
        {
            try
            {
                var schema = JSchema.Parse(_fileReader.ReadFile(jsonSchemaFilePath));
                // Parse the JSON string into a JArray (list of JSON objects)
                var jsonArray = JArray.Parse(jsonString);

                // Prepare a list to store valid JSON objects
                var validObjects = new List<JObject>();
                var validationErrors = new List<string>();

                // Validate each JSON object in the array
                foreach (var token in jsonArray)
                {
                    if (token is JObject jsonObject)
                    {
                        if (!jsonObject.IsValid(schema, out IList<string> objectErrors))
                        {
                            validationErrors.AddRange(objectErrors.Select(e => $"Error: {e}"));
                        }
                        else
                        {
                            validObjects.Add(jsonObject);
                        }
                    }
                    else
                    {
                        validationErrors.Add($"Invalid JSON object: {token}");
                    }
                }

                // If there are validation errors, return failure
                if (validationErrors.Any())
                {
                    var errorMessage = $"JSON validation failed: {string.Join("; ", validationErrors)}";
                    return Result<List<JObject>>.Failure(errorMessage);
                }

                // Return success with the list of valid JSON objects
                return Result<List<JObject>>.Success(validObjects);
            }
            catch (JsonReaderException ex)
            {
                return Result<List<JObject>>.Failure($"Invalid JSON format: {ex.Message}");
            }
        }

    }
}
