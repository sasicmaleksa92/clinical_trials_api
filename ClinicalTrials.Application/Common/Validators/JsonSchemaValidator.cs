using ClinicalTrials.Application.Common.ResultPattern;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using ClinicalTrials.Application.Interfaces;

namespace ClinicalTrials.Application.Common.Validators
{
    /// <summary>
    /// Provides functionality to validate a list of JSON objects against a specified JSON schema.
    /// </summary>
    public class JsonSchemaValidator
    {
        private readonly IFileReader _fileReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSchemaValidator"/> class.
        /// </summary>
        /// <param name="fileReader">An instance of <see cref="IFileReader"/> used to read the JSON schema file.</param>
        public JsonSchemaValidator(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        /// <summary>
        /// Validates a JSON array string against a JSON schema file.
        /// </summary>
        /// <param name="jsonString">The JSON string containing an array of objects to be validated.</param>
        /// <param name="jsonSchemaFilePath">The file path of the JSON schema used for validation.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> object containing either a list of valid JSON objects
        /// or an error message if validation fails.
        /// </returns>
        public Result<List<JObject>> ValidateList(string jsonString, string jsonSchemaFilePath)
        {
            try
            {
                // Read and parse the JSON schema from the file
                var schema = JSchema.Parse(_fileReader.ReadFile(jsonSchemaFilePath));
                var jsonArray = JArray.Parse(jsonString);

                var validObjects = new List<JObject>();
                var validationErrors = new List<string>();

                // Iterate through each item in the JSON array
                foreach (var token in jsonArray)
                {
                    if (token is JObject jsonObject)
                    {
                        // Validate the JSON object against the schema
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

                // If validation errors exist, return failure result with error messages
                if (validationErrors.Any())
                {
                    var errorMessage = $"JSON validation failed: {string.Join("; ", validationErrors)}";
                    return Result<List<JObject>>.Failure(errorMessage);
                }

                // Return success result with valid JSON objects
                return Result<List<JObject>>.Success(validObjects);
            }
            catch (JsonReaderException ex)
            {
                return Result<List<JObject>>.Failure($"Invalid JSON format: {ex.Message}");
            }
        }
    }
}
