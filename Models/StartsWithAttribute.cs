using System.ComponentModel.DataAnnotations;

namespace SecureChild.Models
{
  

    public class StartsWithAttribute : ValidationAttribute
    {
        private readonly string _prefix;

        public StartsWithAttribute(string prefix)
        {
            _prefix = prefix;
        }

        public override bool IsValid(object value)
        {
            var stringValue = value as string;
            return stringValue != null && stringValue.StartsWith(_prefix);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must start with the area code '{_prefix}'.";
        }
    }

}