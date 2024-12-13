using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Extensions
{
    public class CompareWithAttribute : ValidationAttribute
    {
        private readonly string otherProperty;

        public CompareWithAttribute(string otherProperty)
        {
            this.otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Property with name {otherProperty} not found.");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (!object.Equals(value, otherPropertyValue))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} don't match!");
            }

            return ValidationResult.Success;
        }
    }
}
