using System.ComponentModel;

namespace ClinicalTrials.Domain.Common.Extensions
{
    public static class StringExtensions
    {
        public static T GetEnumByDescription<T>(this string description)
        {
            var type = typeof(T);

            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (string.Equals(attribute.Description, description, StringComparison.OrdinalIgnoreCase))
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description attribute");
        }
    }
}
