using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace TemplateProject.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NameValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public static string CapsRegex = @"\b([a-z])(\w*)\b";

        public NameValidationAttribute() : base("The first letter of each word must be capitalized for field {0}")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            return !Regex.IsMatch(value.ToString(), CapsRegex);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientNameValidationRule("The first letter of each word must be capitalized for this field", CapsRegex);
            yield return rule;
        }
    }
}