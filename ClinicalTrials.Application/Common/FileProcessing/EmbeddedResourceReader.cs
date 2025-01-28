using ClinicalTrials.Application.Interfaces;
using System.Reflection;

namespace ClinicalTrials.Application.Common.FileProcessing
{
    public class EmbeddedResourceReader : IFileReader
    {
        public string ReadFile(string resourceName)
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
