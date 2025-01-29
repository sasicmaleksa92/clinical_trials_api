using System.ComponentModel;

namespace ClinicalTrials.Domain.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            Type type = enumValue.GetType();
            string description = string.Empty;
            var memInfo = type.GetMember(type.GetEnumName(enumValue));
            var localizedAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (localizedAttributes.Length > 0)
            {
                description = ((DescriptionAttribute)localizedAttributes[0]).Description;
            }

            return !string.IsNullOrEmpty(description) ? description : enumValue.ToString();
        }
    }
}
