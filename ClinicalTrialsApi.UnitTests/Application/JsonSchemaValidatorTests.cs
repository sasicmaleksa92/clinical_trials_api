namespace ClinicalTrialsApi.UnitTests.Application
{
    using System.IO;
    using System.Reflection;
    using ClinicalTrials.Application.Common.Validators;
    using ClinicalTrials.Application.Interfaces;
    using Moq;
    using Xunit;

    public class JsonSchemaValidatorTests
    {
        private const string RESOURCES_FILE_PATH = "ClinicalTrialsApi.UnitTests.Application.Resources";

        [Fact]
        public void ValidateList_ValidJsonAndSchema_ReturnsSuccess()
        {
            // Arrange
            var jsonString = ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.test-data.json");
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                          .Returns(ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.test-schema.json"));
            var validator = new JsonSchemaValidator(mockFileReader.Object);
            // Act
            var result = validator.ValidateList(jsonString, $"{RESOURCES_FILE_PATH}.test-schema.json");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public void ValidateList_InvalidJsonData_ReturnsFailure()
        {
            // Arrange
            var jsonString = ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.invalid-test-data.json");
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                          .Returns(ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.test-schema.json"));
            var validator = new JsonSchemaValidator(mockFileReader.Object);

            // Act
            var result = validator.ValidateList(jsonString, $"{RESOURCES_FILE_PATH}.test-schema.json");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("JSON validation failed", result.Error);
        }

        [Fact]
        public void ValidateList_InvalidJsonFormat_ReturnsFailure()
        {
            // Arrange
            var jsonString = ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.invalid-json-format.json");
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                          .Returns(ReadEmbeddedResource($"{RESOURCES_FILE_PATH}.test-schema.json"));
            var validator = new JsonSchemaValidator(mockFileReader.Object);

            // Act
            var result = validator.ValidateList(jsonString, $"{RESOURCES_FILE_PATH}.test-schema.json");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid JSON format", result.Error);
        }


        private string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }

}